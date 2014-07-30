using System;
using System.Collections.Generic;
using System.Web;

namespace ProjectHandler
{
    /// <summary>
    /// Summary description for phpHandler
    /// </summary>
    public class phpHandler : IHttpHandler
    {
        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        public phpHandler()
        {
            //
            // TODO: Add constructor logic here.
            //
        }


        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["CaseID"] != null)
            {
                string strCaseID = context.Request.QueryString["CaseID"];

                context.Response.Redirect(string.Format("http://www.decisionquest.com/DQ-Keypage.php?CaseID={0}", strCaseID));
            }
        }
    }

}