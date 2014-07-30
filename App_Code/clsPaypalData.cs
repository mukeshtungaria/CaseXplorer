// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 08-29-2011
// ***********************************************************************
// <copyright file="clsPaypalData.cs" company="DGCC.COM">
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

/// <summary>
/// Paypal Class
/// </summary>
[Serializable()]
public class clsPaypalData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="clsPaypalData" /> class.
    /// </summary>
	public clsPaypalData()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// 
    /// </summary>
    private string m_strCustom = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strInvNumber = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strPhoneNumber = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strNote = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strEmail = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strBusinessName = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strCountryCode = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strPayerID = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strPayerStatus = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strSalutation = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strFirstname = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strLastname = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strMiddlename = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strSuffix = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strAddressStatus = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strShipToName = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strShipToStreet = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strShipToStreet2 = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strShipToCity = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strShipToState = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strShipToZip = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strShipToCountryCode = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strAmount = string.Empty;


    /// <summary>
    /// 
    /// </summary>
    private string m_strToken = string.Empty;

    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    /// <value>The token.</value>
    public string Token
    {
        get { return m_strToken; }
        set { m_strToken = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the custom.
    /// </summary>
    /// <value>The custom.</value>
    public string Custom
    {
        get { return m_strCustom; }
        set { m_strCustom = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the inv number.
    /// </summary>
    /// <value>The inv number.</value>
    public string InvNumber
    {
        get { return m_strInvNumber; }
        set { m_strInvNumber = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    /// <value>The phone number.</value>
    public string PhoneNumber
    {
        get { return m_strPhoneNumber; }
        set { m_strPhoneNumber = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the note.
    /// </summary>
    /// <value>The note.</value>
    public string Note
    {
        get { return m_strNote; }
        set { m_strNote = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    public string Email
    {
        get { return m_strEmail; }
        set { m_strEmail = value == null ? string.Empty : value; }
    }


    /// <summary>
    /// Gets or sets the payer ID.
    /// </summary>
    /// <value>The payer ID.</value>
    public string PayerID
    {
        get { return m_strPayerID; }
        set { m_strPayerID = value == null ? string.Empty : value; }
    }


    /// <summary>
    /// Gets or sets the payer status.
    /// </summary>
    /// <value>The payer status.</value>
    public string PayerStatus
    {
        get { return m_strPayerStatus; }
        set { m_strPayerStatus = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the country code.
    /// </summary>
    /// <value>The country code.</value>
    public string CountryCode
    {
        get { return m_strCountryCode; }
        set { m_strCountryCode = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the name of the business.
    /// </summary>
    /// <value>The name of the business.</value>
    public string BusinessName
    {
        get { return m_strBusinessName; }
        set { m_strBusinessName = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the salutation.
    /// </summary>
    /// <value>The salutation.</value>
    public string Salutation
    {
        get { return m_strSalutation; }
        set { m_strSalutation = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the firstname.
    /// </summary>
    /// <value>The firstname.</value>
    public string Firstname
    {
        get { return m_strFirstname; }
        set { m_strFirstname = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the lastname.
    /// </summary>
    /// <value>The lastname.</value>
    public string Lastname
    {
        get { return m_strLastname; }
        set { m_strLastname = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the middlename.
    /// </summary>
    /// <value>The middlename.</value>
    public string Middlename
    {
        get { return m_strMiddlename; }
        set { m_strMiddlename = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the suffix.
    /// </summary>
    /// <value>The suffix.</value>
    public string Suffix
    {
        get { return m_strSuffix; }
        set { m_strSuffix = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets the full address.
    /// </summary>
    /// <value>The full address.</value>
    public string FullAddress
    {
        get
        {
            string strFullAddress = string.Empty;

            if (this.ShipToStreet.Length > 0 && this.ShipToStreet2.Length > 0)
                strFullAddress = string.Format("{0}{1}{2}{3}{4}, {5}  {6}", this.ShipToStreet, "<br/>", this.ShipToStreet2, "<br/>", this.ShipToCity, this.ShipToState, this.ShipToZip).Replace("  ", " ").Trim();
            else
                strFullAddress = string.Format("{0}{1}{2}, {3}  {4}", this.ShipToStreet, "<br/>", this.ShipToCity, this.ShipToState, this.ShipToZip).Replace("  ", " ").Trim();

            return strFullAddress;
        }
    }

    /// <summary>
    /// Gets the full name.
    /// </summary>
    /// <value>The full name.</value>
    public string FullName
    {
        get
        {
            string strFullName = string.Format("{0} {1} {2} {3} {4}", this.Salutation, this.Firstname, this.Middlename, this.Lastname, this.Suffix).Replace("  ", " ").Trim();

            return strFullName;
        }

    }

    /// <summary>
    /// Gets or sets the address status.
    /// </summary>
    /// <value>The address status.</value>
    public string AddressStatus
    {
        get { return m_strAddressStatus; }
        set { m_strAddressStatus = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the name of the ship to.
    /// </summary>
    /// <value>The name of the ship to.</value>
    public string ShipToName
    {
        get { return m_strShipToName; }
        set { m_strShipToName = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the ship to street.
    /// </summary>
    /// <value>The ship to street.</value>
    public string ShipToStreet
    {
        get { return m_strShipToStreet; }
        set { m_strShipToStreet = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the ship to street2.
    /// </summary>
    /// <value>The ship to street2.</value>
    public string ShipToStreet2
    {
        get { return m_strShipToStreet2; }
        set { m_strShipToStreet2 = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the ship to city.
    /// </summary>
    /// <value>The ship to city.</value>
    public string ShipToCity
    {
        get { return m_strShipToCity; }
        set { m_strShipToCity = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the state of the ship to.
    /// </summary>
    /// <value>The state of the ship to.</value>
    public string ShipToState
    {
        get { return m_strShipToState; }
        set { m_strShipToState = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the ship to zip.
    /// </summary>
    /// <value>The ship to zip.</value>
    public string ShipToZip
    {
        get { return m_strShipToZip; }
        set { m_strShipToZip = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the ship to country code.
    /// </summary>
    /// <value>The ship to country code.</value>
    public string ShipToCountryCode
    {
        get { return m_strShipToCountryCode; }
        set { m_strShipToCountryCode = value == null ? string.Empty : value; }
    }

    /// <summary>
    /// Gets or sets the amount.
    /// </summary>
    /// <value>The amount.</value>
    public string Amount
    {
        get { return m_strAmount; }
        set { m_strAmount = value == null ? string.Empty : value; }
    }


}
