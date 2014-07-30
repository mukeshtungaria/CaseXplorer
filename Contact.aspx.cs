using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public partial class Contact : JBBasePageSub 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.PageTitle = "Request Contact";
            this.PageLabel = "Request Contact";

            this.LoadPageText(litPageText, 1);
            this.LoadPageText(litPageText2, 2);

        }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        try
        {
            System.Data.Common.DbCommand cmd = clsPublic.GetDBCommand("up_paramupd_FeedbackJB");

            string strIPAddress = Request.UserHostAddress;

            pnlMain.Visible = false;

            string strSendTo = clsPublic.GetProgramSetting("keyContactSendTo");
            string strSMTP = clsPublic.GetProgramSetting("keySMTP");
            string strPort = clsPublic.GetProgramSetting("keySMTPPort");
            int intPort = 25;

            int.TryParse(strPort, out intPort);

            if (intPort <= 0) intPort = 25;

              
            string strUser = clsPublic.GetProgramSetting("keySMTPUser");
            string strPassword = clsPublic.GetProgramSetting("keySMTPPassword");
            string strFrom = clsPublic.GetProgramSetting("keyContactSentToFrom");
            string strCC = clsPublic.GetProgramSetting("keyContactSendToCC");

            if (strSendTo.Length > 0)
            {

                MailMessage mm = new MailMessage();
                mm.To.Add(strSendTo);
                mm.From = new MailAddress(strFrom);
                mm.CC.Add(new MailAddress(strCC));

                mm.Subject = clsPublic.GetProgramSetting("keyContactSubject");

                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("From:  {0}", txtName.Text);
                sb.Append(Environment.NewLine);

                sb.AppendFormat("Firm:  {0}", txtFirm.Text);
                sb.Append(Environment.NewLine);

                sb.AppendFormat("Title:  {0}", txtTitle.Text);
                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("Email:  {0}", txtEmail.Text);
                sb.Append(Environment.NewLine);

                sb.AppendFormat("Phone:  {0}", txtPhone.Text);
                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("Comment / Message:  {0}", txtComment.Text);
                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("Page Parameter:  {0}", Request.QueryString.ToString().Length == 0 ? "None" : Request.QueryString.ToString());
                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("Referring Page:  {0}", Request.UrlReferrer.ToString());
                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("Agent:  {0}", Request.UserAgent.ToString());
                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("IP:  {0}", Request.UserHostAddress.ToString());
                sb.AppendFormat(Environment.NewLine);

                clsPublic.AddParameter(ref cmd, "@strJBFeedbackIP", ParameterDirection.Input, DbType.String, strIPAddress);
                clsPublic.AddParameter(ref cmd, "@strJBFeedbackEmail", ParameterDirection.Input, DbType.String, txtEmail.Text);
                clsPublic.AddParameter(ref cmd, "@strJBFeedbackName", ParameterDirection.Input, DbType.String, txtName.Text);
                clsPublic.AddParameter(ref cmd, "@memJBFeedbackNote", ParameterDirection.Input, DbType.String, sb.ToString());

                clsPublic.AddParameter(ref cmd, "@strJBFeedbackType", ParameterDirection.Input, DbType.String, -1);

                clsPublic.SQLExecuteNonQuery(cmd);

                mm.Body = sb.ToString();

                SmtpClient smtp = new SmtpClient(strSMTP, intPort);
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(strUser, strPassword);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = SMTPUserInfo;

                smtp.Send(mm);

                Response.Redirect("~/ThankYou.aspx");
            }

        }
        catch(Exception ex)
        {
            string str = ex.Message;
        }
    }
}
