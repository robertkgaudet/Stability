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
        if (!IsPostBack)
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            var resources = from c in dc.Resources
                            orderby c.Name, c.Type
							select new { Name = c.Name + " (" + c.Type + ")", c.ResourceId };

            rptResources.DataSource = resources;
            rptResources.DataBind();

            var userResources = from us in dc.UserResources
                                where us.UserId == userId
                                select us.ResourceId;

            // Output user resource IDs as comma-separated, lowercase string for JS
            hfSelectedResources.Value = string.Join(",", userResources.Select(id => id.ToString().ToLowerInvariant()));
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

        // Read selected resource IDs from the hidden field
        var selectedResources = hfSelectedResources.Value
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => new Guid(s))
            .ToList();

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

        // Get current user resources
        var currentResources = dc.UserResources.Where(ur => ur.UserId == userId).ToList();

        // Add new resources
        foreach (var resourceId in selectedResources)
        {
            if (!currentResources.Any(ur => ur.ResourceId == resourceId))
            {
                dc.UserResources.InsertOnSubmit(new UserResource
                {
                    UserResourceId = Guid.NewGuid(),
                    UserId = userId,
                    ResourceId = resourceId
                });
            }
        }

        // Remove unselected resources
        foreach (var userResource in currentResources)
        {
            if (!selectedResources.Contains(userResource.ResourceId))
            {
                dc.UserResources.DeleteOnSubmit(userResource);
            }
        }

        dc.SubmitChanges();

        Response.Redirect("/V1/Member/Default.aspx");
    }
}