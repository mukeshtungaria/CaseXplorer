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

public partial class FeedBack : JBBasePageSub 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.PageTitle = "Feedback";
            this.PageLabel = "Feedback";

            this.LoadPageText(litTop, 1);

            clsPublic.PopulateDDL(ddlReason, 17, false, "", "0");
        }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        try
        {
            System.Data.Common.DbCommand cmd = clsPublic.GetDBCommand("up_paramupd_FeedbackJB");

            string strIPAddress = Request.UserHostAddress;

            clsPublic.AddParameter(ref cmd, "@strJBFeedbackIP", ParameterDirection.Input, DbType.String, strIPAddress);
            clsPublic.AddParameter(ref cmd, "@strJBFeedbackEmail", ParameterDirection.Input, DbType.String, txtEmail.Text);
            clsPublic.AddParameter(ref cmd, "@strJBFeedbackName", ParameterDirection.Input, DbType.String, txtName.Text);
            clsPublic.AddParameter(ref cmd, "@memJBFeedbackNote", ParameterDirection.Input, DbType.String, txtFeedback.Text);

            clsPublic.AddParameter(ref cmd, "@strJBFeedbackType", ParameterDirection.Input, DbType.String, ddlReason.SelectedItem.Text);

            clsPublic.SQLExecuteNonQuery(cmd);

            pnlMain.Visible = false;
            pnlResult.Visible = true;

            string strSendTo = ConfigurationManager.AppSettings["FeedbackEmail"].ToString();

            MailMessage mm = new MailMessage();
            mm.To.Add(strSendTo);
            mm.From = new MailAddress(strSendTo);

            mm.Subject = "Jury Feedback";

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("From:  {0}", txtName.Text);
            sb.Append(Environment.NewLine);

            sb.AppendFormat("Email:  {0}", txtEmail.Text);
            sb.Append(Environment.NewLine);

            sb.AppendFormat("Reason:  {0}", ddlReason.SelectedItem.Text);
            sb.AppendFormat(Environment.NewLine);

            sb.AppendFormat("Feedback:  {0}", txtFeedback.Text);
            sb.AppendFormat(Environment.NewLine);

            mm.Body = sb.ToString();

            SmtpClient smtp = new SmtpClient();
            smtp.Send(mm);

        }
        catch
        {

        }
    }
}
