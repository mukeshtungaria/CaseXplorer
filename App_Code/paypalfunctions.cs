using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;

/// <summary>
/// Summary description for NVPAPICaller
/// </summary>
public class NVPAPICaller
{
    //private static readonly ILog log = LogManager.GetLogger(typeof(NVPAPICaller));
	
    private string pendpointurl = "https://api-3t.paypal.com/nvp";
    private const string CVV2 = "CVV2";

    //Flag that determines the PayPal environment (live or sandbox)

    private const string SIGNATURE = "SIGNATURE";
    private const string PWD = "PWD";
    private const string ACCT = "ACCT";

	private string BNCode = "PP-ECWizard";

    //HttpWebRequest Timeout specified in milliseconds 
    private const int Timeout = 50000;
    private static readonly string[] SECURED_NVPS = new string[] { ACCT, CVV2, SIGNATURE, PWD };

    private string m_strAPIUserName = string.Empty;
    private string m_strAPIPassword = string.Empty;
    private string m_strAPISignature = string.Empty;
    private string m_strSubject = string.Empty;
    private bool m_boolIsSandBox = false;

    public bool IsSandBox
    {
        get
        {
            string strResult = clsPublic.GetProgramSetting("keyPPSandBox").ToString(); // ConfigurationManager.AppSettings["PPSandBox"].ToString();

            if (clsPublic.GetHost().ToUpper() == "LOCALHOST")
                strResult = "TRUE";
            else
                strResult = "FALSE";

            if (strResult.ToUpper() == "TRUE")
                m_boolIsSandBox = true;
            else
                m_boolIsSandBox = false;

            return m_boolIsSandBox;
        }
        set
        {
            m_boolIsSandBox = value;
        }
    }

    public string APIUserName
    {
        get
        {
            if (m_strAPIUserName.Length == 0)
            {
                if (this.IsSandBox == true)
                {
                    m_strAPIUserName = clsPublic.GetProgramSetting("keyPPAPIUserNameSandBox").ToString(); // ConfigurationManager.AppSettings["PPAPIUserNameSandBox"].ToString();
                }
                else
                {
                    m_strAPIUserName = clsPublic.GetProgramSetting("keyPPAPIUserName").ToString(); // ConfigurationManager.AppSettings["PPAPIUserName"].ToString();
                }
            }

            return m_strAPIUserName;
        }
        set
        {
            m_strAPIUserName = value;
        }
    }

    public string APIPassword
    {
        get
        {
            if (m_strAPIPassword.Length == 0)
            {
                if (this.IsSandBox)
                {
                    m_strAPIPassword = clsPublic.GetProgramSetting("keyPPAPIPasswordSandBox").ToString(); // ConfigurationManager.AppSettings["PPAPIPasswordSandBox"].ToString();
                }
                else
                {
                    m_strAPIPassword = clsPublic.GetProgramSetting("keyPPAPIPassword").ToString(); // ConfigurationManager.AppSettings["PPAPIPassword"].ToString();
                }
            }
            return m_strAPIPassword;
        }
        set
        {
            m_strAPIPassword = value;
        }
    }

    public string APISignature
    {
        get
        {
            if (m_strAPISignature.Length == 0)
            {
                if (this.IsSandBox)
                {
                    m_strAPISignature = clsPublic.GetProgramSetting("keyPPAPISignatureSandBox").ToString(); // ConfigurationManager.AppSettings["PPAPISignatureSandBox"].ToString();
                }
                else
                {
                    m_strAPISignature = clsPublic.GetProgramSetting("keyPPAPISignature").ToString(); // ConfigurationManager.AppSettings["PPAPISignature"].ToString();
                }
            }
            return m_strAPISignature;
        }
        set
        {
            m_strAPISignature = value;
        }
    }

    public string Subject
    {
        get
        {
            m_strSubject = string.Empty;

            if (m_strSubject.Length == 0)
            {
                if (this.IsSandBox)
                {
                    if (clsPublic.GetProgramSetting("keyPPSubjectSandBox") != null)
                        m_strSubject = clsPublic.GetProgramSetting("keyPPSubjectSandBox").ToString(); // ConfigurationManager.AppSettings["PPSubjectSandBox"].ToString();
                }
                else
                {
                    if (clsPublic.GetProgramSetting("keyPPSubject") != null)
                        m_strSubject = clsPublic.GetProgramSetting("keyPPSubject").ToString(); // ConfigurationManager.AppSettings["PPSubject"].ToString();
                }
            }
            return m_strSubject;
        }
        set
        {
            m_strSubject = value;
        }
    }

    /// <summary>
    /// Sets the API Credentials
    /// </summary>
    /// <param name="Userid"></param>
    /// <param name="Pwd"></param>
    /// <param name="Signature"></param>
    /// <returns></returns>
    public void SetCredentials(string Userid, string Pwd, string Signature)
    {
        this.APIUserName = Userid;
        this.APIPassword = Pwd;
        this.APISignature = Signature;
    }

    /// <summary>
    /// ShortcutExpressCheckout: The method that calls SetExpressCheckout API
    /// </summary>
    /// <param name="amt"></param>
    /// <param ref name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool ShortcutExpressCheckout(string amt, ref string token, ref string retMsg)
    {
        try
        {
            string host = "www.paypal.com";
            string returnURL = string.Empty;
            string cancelURL = string.Empty;

            if (this.IsSandBox)
            {
                pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
                host = "www.sandbox.paypal.com";
                returnURL = clsPublic.GetProgramSetting("keySandBoxPPReturnURLSetCheckout"); // ConfigurationManager.AppSettings["PPReturnURLSetCheckout"];
                cancelURL = clsPublic.GetProgramSetting("keySandBoxPPCancelURLSetCheckout"); // ConfigurationManager.AppSettings["PPCancelURLSetCheckout"];
            }
            else
            {
                returnURL = clsPublic.GetProgramSetting("keyPPReturnURLSetCheckout"); // ConfigurationManager.AppSettings["PPReturnURLSetCheckout"];
                cancelURL = clsPublic.GetProgramSetting("keyPPCancelURLSetCheckout"); // ConfigurationManager.AppSettings["PPCancelURLSetCheckout"];
            }

            NVPCodec encoder = new NVPCodec();
            encoder["METHOD"] = "SetExpressCheckout";
            encoder["RETURNURL"] = returnURL;
            encoder["CANCELURL"] = cancelURL;
            encoder["AMT"] = amt;
            encoder["PAYMENTACTION"] = "Sale";
            encoder["CURRENCYCODE"] = "USD";

            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = HttpCall(pStrrequestforNvp);

            NVPCodec decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();
            if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
            {
                token = decoder["TOKEN"];

                string ECURL = "https://" + host + "/cgi-bin/webscr?cmd=_express-checkout" + "&token=" + token;

                retMsg = ECURL;
                return true;
            }
            else
            {
                retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                    "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                    "Desc2=" + decoder["L_LONGMESSAGE0"];

                return false;
            }
        }
        catch (Exception ex)
        {
            retMsg = string.Format("ErrorCode={0}", ex.Message);
            return false;
        }
    }

    /// <summary>
    /// MarkExpressCheckout: The method that calls SetExpressCheckout API, invoked from the 
    /// Billing Page EC placement
    /// </summary>
    /// <param name="amt"></param>
    /// <param ref name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool MarkExpressCheckout(string amt, 
                        string shipToName, string shipToStreet, string shipToStreet2,
                        string shipToCity, string shipToState, string shipToZip, 
                        string shipToCountryCode,ref string token, ref string retMsg)
    {
        try
        {

            string host = "www.paypal.com";
            string returnURL = string.Empty;
            string cancelURL = string.Empty;

            if (this.IsSandBox)
            {
                pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
                host = "www.sandbox.paypal.com";
                returnURL = clsPublic.GetProgramSetting("keySandBoxPPReturnURLSetCheckout"); // ConfigurationManager.AppSettings["PPReturnURLSetCheckout"];
                cancelURL = clsPublic.GetProgramSetting("keySandBoxPPCancelURLSetCheckout"); // ConfigurationManager.AppSettings["PPCancelURLSetCheckout"];
            }
            else
            {
                returnURL = clsPublic.GetProgramSetting("keyPPReturnURLSetCheckout"); // ConfigurationManager.AppSettings["PPReturnURLSetCheckout"];
                cancelURL = clsPublic.GetProgramSetting("keyPPCancelURLSetCheckout"); // ConfigurationManager.AppSettings["PPCancelURLSetCheckout"];
            }


            NVPCodec encoder = new NVPCodec();
            encoder["METHOD"] = "SetExpressCheckout";
            encoder["RETURNURL"] = returnURL;
            encoder["CANCELURL"] = cancelURL;
            encoder["AMT"] = amt;
            encoder["PAYMENTACTION"] = "Sale";
            encoder["CURRENCYCODE"] = "USD";

            //Optional Shipping Address entered on the merchant site
            encoder["SHIPTONAME"] = shipToName;
            encoder["SHIPTOSTREET"] = shipToStreet;
            encoder["SHIPTOSTREET2"] = shipToStreet2;
            encoder["SHIPTOCITY"] = shipToCity;
            encoder["SHIPTOSTATE"] = shipToState;
            encoder["SHIPTOZIP"] = shipToZip;
            encoder["SHIPTOCOUNTRYCODE"] = shipToCountryCode;


            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = HttpCall(pStrrequestforNvp);

            NVPCodec decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();
            if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
            {
                token = decoder["TOKEN"];

                string ECURL = "https://" + host + "/cgi-bin/webscr?cmd=_express-checkout" + "&token=" + token;

                retMsg = ECURL;
                return true;
            }
            else
            {
                retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                    "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                    "Desc2=" + decoder["L_LONGMESSAGE0"];

                return false;
            }
        }
        catch (Exception ex)
        {
            retMsg = string.Format("ErrorCode={0}", ex.Message);
            return false;
        }
    }


    /// <summary>
    /// GetShippingDetails: The method that calls SetExpressCheckout API, invoked from the 
    /// Billing Page EC placement
    /// </summary>
    /// <param name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool GetCheckoutDetails(string token, ref clsPaypalData PayPalData, ref string retMsg)
    {
        try
        {
            if (this.IsSandBox)
            {
                pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
            }

            NVPCodec encoder = new NVPCodec();
            encoder["METHOD"] = "GetExpressCheckoutDetails";
            encoder["TOKEN"] = token;

            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = HttpCall(pStrrequestforNvp);

            NVPCodec decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();
            if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
            {
                if (PayPalData == null)
                    PayPalData = new clsPaypalData();

                if (PayPalData != null)
                {
                    PayPalData.Firstname = decoder["FIRSTNAME"];
                    PayPalData.Lastname = decoder["LASTNAME"];
                    PayPalData.Middlename = decoder["MIDDLENAME"];
                    PayPalData.Salutation = decoder["SALUTATION"];
                    PayPalData.Suffix = decoder["SUFFIX"];

                    PayPalData.AddressStatus = decoder["ADDRESSSTATUS"];
                    PayPalData.ShipToName = decoder["SHIPTONAME"];
                    PayPalData.ShipToStreet = decoder["SHIPTOSTREET"];
                    PayPalData.ShipToStreet2 = decoder["SHIPTOSTREET2"];
                    PayPalData.ShipToCity = decoder["SHIPTOCITY"];
                    PayPalData.ShipToState = decoder["SHIPTOSTATE"];
                    PayPalData.ShipToZip = decoder["SHIPTOZIP"];

                    PayPalData.Email = decoder["EMAIL"];
                    PayPalData.PayerID = decoder["PAYERID"];
                    PayPalData.PayerStatus = decoder["PAYERSTATUS"];
                    PayPalData.CountryCode = decoder["COUNTRYCODE"];
                    PayPalData.BusinessName = decoder["BUSINESS"];

                    PayPalData.Token = token;
                    PayPalData.Custom = decoder["CUSTOM"];
                    PayPalData.InvNumber = decoder["INVNUM"];
                    PayPalData.PhoneNumber = decoder["PHONEUM"];
                    PayPalData.Note = decoder["NOTE"];

                    PayPalData.Amount = decoder["AMOUNT"];


                }
                return true;
            }
            else
            {
                retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                    "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                    "Desc2=" + decoder["L_LONGMESSAGE0"];

                return false;
            }
        }
        catch (Exception ex)
        {
            retMsg = string.Format("ErrorCode={0}", ex.Message);
            return false;
        }
    }

    /// <summary>
    /// GetShippingDetails: The method that calls SetExpressCheckout API, invoked from the 
    /// Billing Page EC placement
    /// </summary>
    /// <param name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool GetShippingDetails(string token, ref string PayerId, ref string ShippingAddress, ref string retMsg)
    {
        try
        {
            if (this.IsSandBox)
            {
                pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
            }

            NVPCodec encoder = new NVPCodec();
            encoder["METHOD"] = "GetExpressCheckoutDetails";
            encoder["TOKEN"] = token;

            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = HttpCall(pStrrequestforNvp);

            NVPCodec decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();
            if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
            {
                ShippingAddress = "<table><tr>";
                ShippingAddress += "<td> First Name </td><td>" + decoder["FIRSTNAME"] + "</td></tr>";
                ShippingAddress += "<td> Last Name </td><td>" + decoder["LASTNAME"] + "</td></tr>";
                ShippingAddress += "<td colspan='2'> Shipping Address</td></tr>";
                ShippingAddress += "<td> Name </td><td>" + decoder["SHIPTONAME"] + "</td></tr>";
                ShippingAddress += "<td> Street1 </td><td>" + decoder["SHIPTOSTREET"] + "</td></tr>";
                ShippingAddress += "<td> Street2 </td><td>" + decoder["SHIPTOSTREET2"] + "</td></tr>";
                ShippingAddress += "<td> City </td><td>" + decoder["SHIPTOCITY"] + "</td></tr>";
                ShippingAddress += "<td> State </td><td>" + decoder["SHIPTOSTATE"] + "</td></tr>";
                ShippingAddress += "<td> Zip </td><td>" + decoder["SHIPTOZIP"] + "</td>";
                ShippingAddress += "</tr>";

                return true;
            }
            else
            {
                retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                    "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                    "Desc2=" + decoder["L_LONGMESSAGE0"];

                return false;
            }
        }
        catch (Exception ex)
        {
            retMsg = string.Format("ErrorCode={0}", ex.Message);
            return false;
        }

    }


    /// <summary>
    /// ConfirmPayment: The method that calls SetExpressCheckout API, invoked from the 
    /// Billing Page EC placement
    /// </summary>
    /// <param name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool ConfirmPayment(string finalPaymentAmount, string token, string PayerId, ref NVPCodec decoder, ref string retMsg )
    {
        try
        {
            if (this.IsSandBox)
            {
                pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
            }

            NVPCodec encoder = new NVPCodec();
            encoder["METHOD"] = "DoExpressCheckoutPayment";
            encoder["TOKEN"] = token;
            encoder["PAYMENTACTION"] = "Sale";
            encoder["PAYERID"] = PayerId;
            encoder["AMT"] = finalPaymentAmount;

            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = HttpCall(pStrrequestforNvp);

            decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();
            if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
            {
                return true;
            }
            else
            {
                retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                    "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                    "Desc2=" + decoder["L_LONGMESSAGE0"];

                return false;
            }
        }
        catch (Exception ex)
        {
            retMsg = string.Format("ErrorCode={0}", ex.Message);
            return false;
        }
    }


    /// <summary>
    /// HttpCall: The main method that is used for all API calls
    /// </summary>
    /// <param name="NvpRequest"></param>
    /// <returns></returns>
    public string HttpCall(string NvpRequest) //CallNvpServer
    {
        string url = pendpointurl;

        //To Add the credentials from the profile
        string strPost = NvpRequest + "&" + buildCredentialsNVPString();
		strPost = strPost + "&BUTTONSOURCE=" + HttpUtility.UrlEncode ( BNCode );

        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
        objRequest.Timeout = Timeout;
        objRequest.Method = "POST";
        objRequest.ContentLength = strPost.Length;

        try
        {
            using (StreamWriter myWriter = new StreamWriter(objRequest.GetRequestStream()))
            {
                myWriter.Write(strPost);
            }
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            /*
            if (log.IsFatalEnabled)
            {
                log.Fatal(e.Message, this);
            }*/
        }

        //Retrieve the Response returned from the NVP API call to PayPal
        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        string result;
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            result = sr.ReadToEnd();
        }

        //Logging the response of the transaction
        /* if (log.IsInfoEnabled)
         {
             log.Info("Result :" +
                       " Elapsed Time : " + (DateTime.Now - startDate).Milliseconds + " ms" +
                      result);
         }
         */
        return result;
    }

    /// <summary>
    /// Credentials added to the NVP string
    /// </summary>
    /// <param name="profile"></param>
    /// <returns></returns>
    private string buildCredentialsNVPString()
    {
        NVPCodec codec = new NVPCodec();

        if (!IsEmpty(this.APIUserName  ))
            codec["USER"] = this.APIUserName;

        if (!IsEmpty(APIPassword))
            codec[PWD] = APIPassword;

        if (!IsEmpty(APISignature))
            codec[SIGNATURE] = APISignature;

        if (!IsEmpty(Subject))
            codec["SUBJECT"] = Subject;

        codec["VERSION"] = "2.3";

        return codec.Encode();
    }

    /// <summary>
    /// Returns if a string is empty or null
    /// </summary>
    /// <param name="s">the string</param>
    /// <returns>true if the string is not null and is not empty or just whitespace</returns>
    public static bool IsEmpty(string s)
    {
        return s == null || s.Trim() == string.Empty;
    }
}


public sealed class NVPCodec : NameValueCollection
{
    private const string AMPERSAND = "&";
    private const string EQUALS = "=";
    private static readonly char[] AMPERSAND_CHAR_ARRAY = AMPERSAND.ToCharArray();
    private static readonly char[] EQUALS_CHAR_ARRAY = EQUALS.ToCharArray();

    /// <summary>
    /// Returns the built NVP string of all name/value pairs in the Hashtable
    /// </summary>
    /// <returns></returns>
    public string Encode()
    {
        StringBuilder sb = new StringBuilder();
        bool firstPair = true;
        foreach (string kv in AllKeys)
        {
            string name = UrlEncode(kv);
            string value = UrlEncode(this[kv]);
            if (!firstPair)
            {
                sb.Append(AMPERSAND);
            }
            sb.Append(name).Append(EQUALS).Append(value);
            firstPair = false;
        }
        return sb.ToString();
    }

    /// <summary>
    /// Decoding the string
    /// </summary>
    /// <param name="nvpstring"></param>
    public void Decode(string nvpstring)
    {
        Clear();
        foreach (string nvp in nvpstring.Split(AMPERSAND_CHAR_ARRAY))
        {
            string[] tokens = nvp.Split(EQUALS_CHAR_ARRAY);
            if (tokens.Length >= 2)
            {
                string name = UrlDecode(tokens[0]);
                string value = UrlDecode(tokens[1]);
                Add(name, value);
            }
        }
    }

    private static string UrlDecode(string s) { return HttpUtility.UrlDecode(s); }
    private static string UrlEncode(string s) { return HttpUtility.UrlEncode(s); }

    #region Array methods
    public void Add(string name, string value, int index)
    {
        this.Add(GetArrayName(index, name), value);
    }

    public void Remove(string arrayName, int index)
    {
        this.Remove(GetArrayName(index, arrayName));
    }

    /// <summary>
    /// 
    /// </summary>
    public string this[string name, int index]
    {
        get
        {
            return this[GetArrayName(index, name)];
        }
        set
        {
            this[GetArrayName(index, name)] = value;
        }
    }

    private static string GetArrayName(int index, string name)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException("index", "index can not be negative : " + index);
        }
        return name + index;
    }
    #endregion
}