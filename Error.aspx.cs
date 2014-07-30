using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Error : JBBasePageSub 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.PageTitle = "Page Error";
        this.PageLabel = "An error has occurred";

        try
        {
            //            if (Request.QueryString["ErrorID"] != null)
            {
                Exception ex = null;

                Server.ClearError();

                if (Session["CurrentError"] != null)
                {
                    ex = (Exception)Session["CurrentError"];
                    clsPublic.LogError(ex);
                }

                Session["CurrentError"] = null;

                switch (Request.QueryString["ErrorID"])
                {
                    case "404":
                        Response.Clear();
                        Response.StatusCode = 404;

                        lblMessage.Text = "Sorry, that page was not found.";

                        break;
                    case "1000":

                        Response.Clear();
                        Response.StatusCode = 200;

                        lblMessage.Text = "Sorry, but HTML tags, (&lt;?&gt;) or (&lt;/?&gt;) values, are not allowed as input.";
                        break;

                    default:
                        Response.Clear();
                        Response.StatusCode = 200;

                        if (ex != null)
                            lblMessage.Text = string.Format("An error has occured.<br/>{0}", ex.Message);
                        else
                        {
                            lblMessage.Text = "An unknown error has occurred";
                            clsPublic.LogError(new Exception("Unknown error occurred"));
                        }


                        break;

                }
            }


        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
        }

    }
}
