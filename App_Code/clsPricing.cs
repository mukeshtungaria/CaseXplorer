// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 05-21-2012
// ***********************************************************************
// <copyright file="clsPricing.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using JuryData.Entities;
using JuryData.Data;

/// <summary>
/// Price Calculation Class
/// </summary>
public class clsPricing
{
    /// <summary>
    /// Initializes a new instance of the <see cref="clsPricing" /> class.
    /// </summary>
    public clsPricing()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Gets the type of the pricing by item type
    /// </summary>
    /// <param name="intPriceType">Type of the int price.</param>
    /// <returns>System.Decimal.</returns>
    public static decimal GetPricingByType(Int16 intPriceType)
    {
        ResearchPricing entResearchPricing = DataRepository.ResearchPricingProvider.GetByGuiPricingUserIDIntPricingType(new Guid(), intPriceType);

        if (entResearchPricing != null)
            return entResearchPricing.CurPricingAmount;
        else
            return 0;
    }

    /// <summary>
    /// Updates pricing for a study - by Research ID
    /// </summary>
    /// <param name="intResearchID">Research ID</param>
    public static void UpdateQuestionPricing(int intResearchID)
    {
        System.Data.Common.DbCommand cmd = clsUtilities.GetDBCommand("usp_paramsel_ResearchQuestionCount");
        clsUtilities.AddParameter(ref cmd, "@ResearchID", ParameterDirection.Input, DbType.Int32, intResearchID);

        DataSet ds = clsUtilities.SQLExecuteDataSet(cmd);

        Int16 intQuestionCount = 0;

        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
                intQuestionCount = Convert.ToInt16(ds.Tables[0].Rows[0]["QuestionCount"]);
        }

        clsPricing.CreateResearchRespondent(intResearchID, 2, intQuestionCount);

    }

    /// <summary>
    /// Applies a promo code to a study
    /// </summary>
    /// <param name="intResearchID">Research ID</param>
    /// <param name="strPromoCode">Promo Code</param>
    /// <returns>Promo Code</returns>
    public static PromoCodes ApplyPromoCode(int intResearchID, string strPromoCode)
    {
        try
        {
            System.Data.Common.DbCommand cmdApplyPromoCode = clsUtilities.GetDBCommand("_up_paramsel_ApplyPromoCode");

            clsUtilities.AddParameter(ref cmdApplyPromoCode, "@PromoCode", ParameterDirection.Input, DbType.String, strPromoCode);
            clsUtilities.AddParameter(ref cmdApplyPromoCode, "@ResearchID", ParameterDirection.Input, DbType.Int32, intResearchID);

            DataSet ds = clsUtilities.SQLExecuteDataSet(cmdApplyPromoCode);

            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                string strPromoType = dr["PromoType"].ToString();

                string loginID = clsPublic.GetAppSetting("authorize.net-Login");
                string transactionKey = clsPublic.GetAppSetting("authorize.net-Key");
                var seq = AuthorizeNet.Crypto.GenerateSequence();
                var stamp = AuthorizeNet.Crypto.GenerateTimestamp();

                if (strPromoType != "-1")
                {
                    decimal decPromoAmount = 0;
                    decimal decTotalOff = 0;
                    decimal decTotalAmount = 0;
                    DateTime datPromoExpires;
                    int intPromoType = 0;
                    int intPromoOtherUnit = 0;

                    decimal.TryParse(dr["PromoAmount"].ToString(), out decPromoAmount);
                    decimal.TryParse(dr["TotalAmount"].ToString(), out decTotalAmount);
                    decimal.TryParse(dr["TotalOff"].ToString(), out decTotalOff);
                    int.TryParse(dr["PromoOtherUnit"].ToString(), out intPromoOtherUnit);
                    int.TryParse(strPromoType, out intPromoType);
                    DateTime.TryParse(dr["PromoExpires"].ToString(), out datPromoExpires);

                    PromoCodes PromoCodesTemp = new PromoCodes();
                    PromoCodesTemp.PromoDesc = dr["PromoDesc"].ToString();
                    PromoCodesTemp.PromoCode = dr["PromoCode"].ToString();

                    PromoCodesTemp.PromoAmount = decPromoAmount;
                    PromoCodesTemp.TotalOff = decTotalOff;
                    PromoCodesTemp.TotalOriginal = decTotalAmount;
                    PromoCodesTemp.PromoExpire = datPromoExpires;

                    PromoCodesTemp.PromoType = intPromoType;
                    PromoCodesTemp.PromoUnit = intPromoOtherUnit;



                    var fingerPrint = AuthorizeNet.Crypto.GenerateFingerprint(transactionKey, loginID, PromoCodesTemp.NewTotal, seq, stamp.ToString());

                    PromoCodesTemp.Seq = seq;
                    PromoCodesTemp.Stamp = stamp.ToString();
                    PromoCodesTemp.FingerPrint = fingerPrint;

                    return PromoCodesTemp;

                }
                else
                {
                    PromoCodes PromoCodesTemp = new PromoCodes();
                    PromoCodesTemp.PromoType = -1;

                    decimal decTotalAmount = 0;
                    decimal.TryParse(dr["TotalAmount"].ToString(), out decTotalAmount);

                    var fingerPrint = AuthorizeNet.Crypto.GenerateFingerprint(transactionKey, loginID, PromoCodesTemp.NewTotal, seq, stamp.ToString());

                    PromoCodesTemp.Seq = seq;
                    PromoCodesTemp.Stamp = stamp.ToString();
                    PromoCodesTemp.FingerPrint = fingerPrint;

                    return PromoCodesTemp;
                }
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return null;
        }


    }


    /// <summary>
    /// Creates the research respondent record for a study
    /// - specified the number of respondents
    /// - specified the type of respondent (Nationwide, By County, By Zip Code)
    /// -- different pricing is based upon the respondent type
    /// </summary>
    /// <param name="intResearchID">Research ID</param>
    /// <param name="intRespondentType">Respondent Type </param>
    /// <param name="intItemCount">Number of respondents</param>
    public static void CreateResearchRespondent(int intResearchID, int intRespondentType, Int16 intItemCount)
    {
        ResearchResponders entResearchResponders = DataRepository.ResearchRespondersProvider.GetByIntRespondersResearchIDIntRespondersType(intResearchID, intRespondentType);

        if (entResearchResponders == null)
            entResearchResponders = new ResearchResponders();

        entResearchResponders.IntRespondersResearchID = intResearchID;
        entResearchResponders.IntRespondersType = intRespondentType;
        entResearchResponders.IntRespondersCount = intItemCount;
        entResearchResponders.CurRespondersCost = clsPricing.GetPricingByType(Convert.ToInt16(intRespondentType));
        entResearchResponders.CurRespondersTotCost = Convert.ToDecimal(intItemCount) * entResearchResponders.CurRespondersCost;

        DataRepository.ResearchRespondersProvider.Save(entResearchResponders);
    }
}
