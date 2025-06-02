using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Profile_Skills : BaseOrganizationWebForm
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            string searchTerm = Request.QueryString["searchTerm"];

            IQueryable<dynamic> skills;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                skills = from c in dc.Skills
                         where c.Name.Contains(searchTerm) 
                         orderby c.Name
                         select new { Name = c.Name, c.SkillId };
            }
            else
            {
                skills = from c in dc.Skills
                         orderby c.Name
                         select new { Name = c.Name, c.SkillId };
            }

            rptSkills.DataSource = skills;
            rptSkills.DataBind();

            var userSkills = from us in dc.UserSkills
                             where us.UserId == userId
                             select us.SkillId;

            hfSelectedSkills.Value = string.Join(",", userSkills.Select(id => id.ToString().ToLowerInvariant()));
        }
    }


    protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Member/Default.aspx");
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
        string register = Request.QueryString["register"];
        divMessage.Visible = true;
		lblMessage.Text = "Your skills have been updated.";

		var selectedSkills = hfSelectedSkills.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
												   .Select(Guid.Parse)
												   .ToList();

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

		// Get current user skills
		var currentSkills = dc.UserSkills.Where(us => us.UserId == userId).ToList();

		// Add new skills
		foreach (var skillId in selectedSkills)
		{
			if (!currentSkills.Any(us => us.SkillId == skillId))
			{
				dc.UserSkills.InsertOnSubmit(new UserSkill
				{
					UserSkillId = Guid.NewGuid(),
					UserId = userId,
					SkillId = skillId
				});
			}
		}

		// Remove unselected skills
		foreach (var userSkill in currentSkills)
		{
			if (!selectedSkills.Contains(userSkill.SkillId))
			{
				dc.UserSkills.DeleteOnSubmit(userSkill);
			}
		}

		dc.SubmitChanges();
		if (!String.IsNullOrEmpty(register))
		{
            Response.Redirect("/V1/Profile/EditResources.aspx?register=true");
        }
		else
		{
			Response.Redirect("/V1/Member/Default.aspx");
		}
	}
}