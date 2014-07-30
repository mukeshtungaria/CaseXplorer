using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Admin_UserList : JBBasePageSub
{
    private void Page_PreRender()
    {

        Users.DataSource = Membership.GetAllUsers();
//        Users.DataSource = Membership.FindUsersByName(Alphalinks.Letter + "%");
        Users.DataBind();
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}