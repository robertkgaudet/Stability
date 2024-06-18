using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Profile_Programs : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var events = from c in dc.Skills
									orderby c.Name
									select new {name = " - " + c.Name, c.SkillId };
			
			chkBoxListSkills.DataSource = events;
			chkBoxListSkills.DataBind();

			var userSkills = from us in dc.UserSkills
							where us.UserId == userId
							select us;
			
			//Preselect the orgs for this user
			if(userSkills.Count() > 0 )
			{
				foreach(var userEvent in userSkills)
				{
					for (int i = 0; i < chkBoxListSkills.Items.Count; i++)
					{
						if(userEvent.SkillId.ToString() == chkBoxListSkills.Items[i].Value)
						{
							chkBoxListSkills.Items[i].Selected = true;
						}
					}
				}
			}
		}
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("Profile.aspx");
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		divMessage.Visible = true;
		lblMessage.Text = "Your skills have been updated.";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		foreach (ListItem item in chkBoxListSkills.Items)
		{
			if (item.Selected)
			{
				var userCheck = from p in dc.UserSkills
								where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
								&& p.SkillId == new Guid(item.Value)
								select p;

				if (userCheck.Count() == 0)
				{
					UserSkill userSkill = new UserSkill();
					userSkill.SkillId = new Guid(item.Value);
					userSkill.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
					userSkill.UserSkillId = Guid.NewGuid();
					dc.UserSkills.InsertOnSubmit(userSkill);
					dc.SubmitChanges();
				}
			}
			else
			{
				//If item is selected then unselect it.
				var userChecks = from p in dc.UserSkills
								where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
								&& p.SkillId == new Guid(item.Value)
								select p;
				
				//Delete any checked records
				if (userChecks.Count() > 0)
				{
					//Item is selected.
					foreach(var userCheck in userChecks)
					{
						dc.UserSkills.DeleteOnSubmit(userCheck);
						dc.SubmitChanges();
					}
				}
			}
		}
	}
}