using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Profile_Resources : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var events = from c in dc.Resources
									orderby c.Type, c.Name
									select new {name = " - " + c.Name + " (" + c.Type + ")", c.ResourceId };
			
			chkBoxListResources.DataSource = events;
			chkBoxListResources.DataBind();

			var userResources = from us in dc.UserResources
							where us.UserId == userId
							select us;
			
			//Preselect the orgs for this user
			if(userResources.Count() > 0 )
			{
				foreach(var userEvent in userResources)
				{
					for (int i = 0; i < chkBoxListResources.Items.Count; i++)
					{
						if(userEvent.ResourceId.ToString() == chkBoxListResources.Items[i].Value)
						{
							chkBoxListResources.Items[i].Selected = true;
						}
					}
				}
			}
		}
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Member/Default.aspx");
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
	    divMessage.Visible = true;
		lblMessage.Text = "Your resources have been updated.";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		foreach (ListItem item in chkBoxListResources.Items)
		{
			if (item.Selected)
			{
				var userCheck = from p in dc.UserResources
								where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
								&& p.ResourceId == new Guid(item.Value)
								select p;

				if (userCheck.Count() == 0)
				{
					UserResource userResource = new UserResource();
					userResource.ResourceId = new Guid(item.Value);
					userResource.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
					userResource.UserResourceId = Guid.NewGuid();
					dc.UserResources.InsertOnSubmit(userResource);
					dc.SubmitChanges();
				}
			}
			else
			{
				//If item is selected then unselect it.
				var userChecks = from p in dc.UserResources
								where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
								&& p.ResourceId == new Guid(item.Value)
								select p;
				
				//Delete any checked records
				if (userChecks.Count() > 0)
				{
					//Item is selected.
					foreach(var userCheck in userChecks)
					{
						dc.UserResources.DeleteOnSubmit(userCheck);
						dc.SubmitChanges();
					}
				}
			}
		}
        var userOrg = (from org in dc.UserOrganizations
                       where org.UserId == userId
                       select org).FirstOrDefault();
		
       
			Response.Redirect("/V1/Member/Default.aspx");
        
    }
}