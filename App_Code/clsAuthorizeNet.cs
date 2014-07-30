// ***********************************************************************
// Assembly         : C:\juryresearch\
// Author           : dennis
// Created          : 08-29-2011
//
// Last Modified By : Dennis Sebenick
// Last Modified On : 05-22-2012
// ***********************************************************************
// <copyright file="clsAuthorizeNet.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections.Specialized;

using JuryData.Entities;
using JuryData.Data;

/// <summary>
/// Summary description for clsAuthorizeNet
/// </summary>
public class clsAuthorizeNet
{
    /// <summary>
    /// 
    /// </summary>
    NameValueCollection _nvc;

    /// <summary>
    /// Initializes a new instance of the <see cref="clsAuthorizeNet" /> class.
    /// </summary>
    /// <param name="nvc">The NVC.</param>
    public clsAuthorizeNet(NameValueCollection nvc)
	{
        _nvc = nvc;
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="strKey">The STR key.</param>
    /// <returns>System.String.</returns>
    protected string GetValue(string strKey)
    {
        if (_nvc != null)
        {
            if (_nvc[strKey] != null)
                return _nvc[strKey].ToString();
            else
                return string.Empty;
        }
        else
            return string.Empty;

    }


    /// <summary>
    /// Gets the response_ code.
    /// </summary>
    /// <value>The response_ code.</value>
    public string Response_Code
    {
        get { return this.GetValue("x_response_code"); }
    }

    /// <summary>
    /// 
    /// </summary>
    private int m_intResearchID = 0;

    /// <summary>
    /// Gets the research ID.
    /// </summary>
    /// <value>The research ID.</value>
    public int ResearchID
    {
        get { return m_intResearchID; }

    }

    /// <summary>
    /// Gets the response_ reason_ text.
    /// </summary>
    /// <value>The response_ reason_ text.</value>
    public string Response_Reason_Text
    {
        get { return this.GetValue("x_response_reason_text"); }
    }

    /// <summary>
    /// Gets the authorization code.
    /// </summary>
    /// <value>The authorization code.</value>
    public string Authorization_Code
    {
        get { return this.GetValue("x_auth_code"); }
    }

    /// <summary>
    /// Gets the AVS code.
    /// </summary>
    /// <value>The AVS code.</value>
    public string AVS_Code
    {
        get { return this.GetValue("x_avs_code"); }
    }

    /// <summary>
    /// Gets the transaction ID.
    /// </summary>
    /// <value>The transaction ID.</value>
    public string Transaction_ID
    {
        get { return this.GetValue("x_trans_id"); }
    }

    /// <summary>
    /// Gets the invoice_number.
    /// </summary>
    /// <value>The invoice number.</value>
    public string Invoice_Number
    {
        get { return this.GetValue("x_invoice_num"); }
    }

    /// <summary>
    /// Gets the PO number.
    /// </summary>
    /// <value>The PO number.</value>
    public string PO_Number
    {
        get { return this.GetValue("x_po_num"); }
    }

    /// <summary>
    /// Gets the amount.
    /// </summary>
    /// <value>The amount.</value>
    public string Amount
    {
        get { return this.GetValue("x_amount"); }
    }

    /// <summary>
    /// Gets the type of the card.
    /// </summary>
    /// <value>The type of the card.</value>
    public string Card_Type
    {
        get { return this.GetValue("x_card_type"); }
    }

    /// <summary>
    /// Gets the customer_ ID.
    /// </summary>
    /// <value>The customer_ ID.</value>
    public string Customer_ID
    {
        get { return this.GetValue("x_cust_id"); }
    }


    /// <summary>
    /// Gets the name of the billing first name.
    /// </summary>
    /// <value>The name of the billing first name.</value>
    public string Billing_First_Name
    {
        get { return this.GetValue("x_first_name"); }
    }

    /// <summary>
    /// Gets the name of the billing last name.
    /// </summary>
    /// <value>The name of the billing last name.</value>
    public string Billing_Last_Name
    {
        get { return this.GetValue("x_last_name"); }
    }

    /// <summary>
    /// Gets the billing company.
    /// </summary>
    /// <value>The billing company.</value>
    public string Billing_Company
    {
        get { return this.GetValue("x_company"); }
    }

    /// <summary>
    /// Gets the billing address.
    /// </summary>
    /// <value>The billing address.</value>
    public string Billing_Address
    {
        get { return this.GetValue("x_address"); }
    }

    /// <summary>
    /// Gets the billing city.
    /// </summary>
    /// <value>The billing city.</value>
    public string Billing_City
    {
        get { return this.GetValue("x_city"); }
    }

    /// <summary>
    /// Gets the state of the billing.
    /// </summary>
    /// <value>The state of the billing.</value>
    public string Billing_State
    {
        get { return this.GetValue("x_state"); }
    }

    /// <summary>
    /// Gets the billing zip.
    /// </summary>
    /// <value>The billing zip.</value>
    public string Billing_Zip
    {
        get { return this.GetValue("x_zip"); }
    }

    /// <summary>
    /// Gets the billing country.
    /// </summary>
    /// <value>The billing country.</value>
    public string Billing_Country
    {
        get { return this.GetValue("x_country"); }
    }

    /// <summary>
    /// Gets the billing phone.
    /// </summary>
    /// <value>The billing phone.</value>
    public string Billing_Phone
    {
        get { return this.GetValue("x_phone"); }
    }

    /// <summary>
    /// Gets the billing full address.
    /// </summary>
    /// <value>The billing full address.</value>
    public string Billing_FullAddress
    {
        get
        {
            string strFullAddress = string.Empty;

            strFullAddress = string.Format("{0}{1}{2}, {3}  {4}", this.Billing_Address, "<br/>", this.Billing_City, this.Billing_State, this.Billing_Zip).Replace("  ", " ").Trim();

            return strFullAddress;
        }
    }


    /// <summary>
    /// Gets the billing fax.
    /// </summary>
    /// <value>The billing fax.</value>
    public string Billing_Fax
    {
        get { return this.GetValue("x_fax"); }
    }

    /// <summary>
    /// Gets the billing email.
    /// </summary>
    /// <value>The billing email.</value>
    public string Billing_Email
    {
        get { return this.GetValue("x_email"); }
    }

    /// <summary>
    /// Gets the full name of the billing.
    /// </summary>
    /// <value>The full name of the billing.</value>
    public string Billing_FullName
    {
        get
        {
            string strResult = this.Billing_First_Name + ' ' + this.Billing_Last_Name;

            return strResult.Trim();

        }
    }

    /// <summary>
    /// Save the authorize.net data
    /// </summary>
    public void Save()
    {
        try
        {
            int intOrderID = 0;

            int.TryParse(this.PO_Number, out intOrderID);

            clsPublic.SendMessage(string.Format("Save Order ID:  {0}", intOrderID));

            if (intOrderID > 0)
            {
                ResearchOrders entResearchOrders = DataRepository.ResearchOrdersProvider.GetByAutoOrderID(intOrderID);

                if (entResearchOrders != null)
                {
                    entResearchOrders.StrOrderPaypalTransactionID =  this.Transaction_ID;
                    entResearchOrders.StrOrderPaypalBusiness = clsPublic.Left(this.Billing_Company, 50);
                    entResearchOrders.StrOrderPaypalFirstName = clsPublic.Left(this.Billing_First_Name, 25);
                    entResearchOrders.StrOrderPaypalLastName = clsPublic.Left(this.Billing_Last_Name, 25);
                    entResearchOrders.StrOrderPaypalCountry = string.Empty;
                    entResearchOrders.StrOrderPaypalStreet1 = clsPublic.Left(this.Billing_Address, 100);
                    entResearchOrders.StrOrderPaypalCity = clsPublic.Left(this.Billing_City, 40);
                    entResearchOrders.StrOrderPaypalState = clsPublic.Left(this.Billing_State, 40);
                    entResearchOrders.StrOrderPaypalZip = clsPublic.Left(this.Billing_Zip, 20);
                    entResearchOrders.StrOrderPaypalToken = clsPublic.Left(this.Authorization_Code, 25);
                    entResearchOrders.StrOrderPaypalTransactionType = clsPublic.Left(this.Card_Type, 15);
                    entResearchOrders.StrOrderPaypalEmail = clsPublic.Left(this.Billing_Email, 127);
                    entResearchOrders.StrOrderPaypalPhone = clsPublic.Left(this.Billing_Phone, 20);

                    decimal decCharged = 0;

                    decimal.TryParse(this.Amount, out decCharged);

                    entResearchOrders.CurOrderTotalCharged = decCharged;

                    m_intResearchID = entResearchOrders.IntOrderResearchID;

                    DataRepository.ResearchOrdersProvider.Save(entResearchOrders);

                    ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(entResearchOrders.IntOrderResearchID);

                    if (entResearchMain != null)
                    {
                        entResearchMain.DatResearchSubmitted = DateTime.Now;
                        DataRepository.ResearchMainProvider.Save(entResearchMain);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);

        }
    }

}