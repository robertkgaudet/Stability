using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Profile_EditDisasters : BaseOrganizationWebForm
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string searchTerm = Request.QueryString["searchTerm"];

            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            var eventsQuery = dc.ExecuteQuery<SearchResponse.PortalResult>(
    "EXEC SearchFillter {0}, {1}",
    "Portals",
    string.IsNullOrWhiteSpace(searchTerm) ? "" : searchTerm
    );
            if (!string.IsNullOrEmpty(searchTerm))
            {
                eventsQuery = eventsQuery
                    .Where(ev => ev.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            chkBoxListDisasters.DataSource = eventsQuery;
            chkBoxListDisasters.DataBind();

            var userEvents = from ue in dc.UserEvents
                             where ue.UserId == userId
                             select ue;

            if (userEvents.Any())
            {
                foreach (var userEvent in userEvents)
                {
                    for (int i = 0; i < chkBoxListDisasters.Items.Count; i++)
                    {
                        if (userEvent.EventId.ToString() == chkBoxListDisasters.Items[i].Value)
                        {
                            chkBoxListDisasters.Items[i].Selected = true;
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
		lblMessage.Text = "Your Community Portals have been updated.";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		foreach (ListItem item in chkBoxListDisasters.Items)
		{
			if (item.Selected)
			{
				var userCheck = from p in dc.UserEvents
						where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
						&& p.EventId == new Guid(item.Value)
						select p;

				if (userCheck.Count() == 0)
				{ 
					UserEvent userEvent = new UserEvent();
					userEvent.EventId = new Guid(item.Value);
					userEvent.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
					userEvent.UserEventId = Guid.NewGuid();
					dc.UserEvents.InsertOnSubmit(userEvent);
					dc.SubmitChanges();
				}
			}
			else
			{
				//If item is selected then unselect it.
				var userChecks = from p in dc.UserEvents
								where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
								&& p.EventId == new Guid(item.Value)
								select p;
				
				//Delete any checked records
				if (userChecks.Count() > 0)
				{
					//Item is selected.
					foreach(var userCheck in userChecks)
					{
						dc.UserEvents.DeleteOnSubmit(userCheck);
						dc.SubmitChanges();
					}
				}
			}
		}
		Response.Redirect("/V1/Member/Default.aspx");
	}
}