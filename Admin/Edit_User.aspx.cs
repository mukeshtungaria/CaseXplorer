using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Admin_Edit_User : JBBasePageSub
{
	string username;
	
	MembershipUser user;
	
	private void Page_Load()
	{
		username = Request.QueryString["username"];
		if (username == null || username == "")
		{
			Response.Redirect("UserList.aspx");
		}
		user = Membership.GetUser(username);
		UserUpdateMessage.Text = "";
	}

	protected void UserInfo_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
	{
		//Need to handle the update manually because MembershipUser does not have a
		//parameterless constructor  

		user.Email = (string)e.NewValues[0];
		user.Comment = (string)e.NewValues[1];
		user.IsApproved = (bool)e.NewValues[2];

		try
		{
			// Update user info:
			Membership.UpdateUser(user);
			
			// Update user roles:
			UpdateUserRoles();
			
			UserUpdateMessage.Text = "Update Successful.";
			
			e.Cancel = true;
			UserInfo.ChangeMode(DetailsViewMode.ReadOnly);
		}
		catch (Exception ex)
		{
			UserUpdateMessage.Text = "Update Failed: " + ex.Message;

			e.Cancel = true;
			UserInfo.ChangeMode(DetailsViewMode.ReadOnly);
		}
	}

	private void Page_PreRender()
	{
		// Load the User Roles into checkboxes.
		UserRoles.DataSource = Roles.GetAllRoles();
		UserRoles.DataBind();

		// Disable checkboxes if appropriate:
		if (UserInfo.CurrentMode != DetailsViewMode.Edit)
		{
			foreach (ListItem checkbox in UserRoles.Items)
			{
				checkbox.Enabled = false;
			}
		}
		
		// Bind these checkboxes to the User's own set of roles.
		string[] userRoles = Roles.GetRolesForUser(username);
		foreach (string role in userRoles)
		{
			ListItem checkbox = UserRoles.Items.FindByValue(role);
			checkbox.Selected = true;
		}
	}
	
	private void UpdateUserRoles()
	{
		foreach (ListItem rolebox in UserRoles.Items)
		{
			if (rolebox.Selected)
			{
				if (!Roles.IsUserInRole(username, rolebox.Text))
				{
					Roles.AddUserToRole(username, rolebox.Text);
				}
			}
			else
			{
				if (Roles.IsUserInRole(username, rolebox.Text))
				{
					Roles.RemoveUserFromRole(username, rolebox.Text);
				}
			}
		}
	}

	protected void UnlockUser(object sender, EventArgs e)
	{
		// Dan Clem, added 5/30/2007 post-live upgrade.
		
		// Unlock the user.
		user.UnlockUser();
		
		// DataBind the GridView to reflect same.
		UserInfo.DataBind();
	}
    protected void DeleteUser(object sender, EventArgs e)
    {
		//Membership.DeleteUser(username, false); // DC: My apps will NEVER delete the related data.
		Membership.DeleteUser(username, true); // DC: except during testing, of course!
		Response.Redirect("UserList.aspx");

    }
}