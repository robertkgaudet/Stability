using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : BaseOrganizationWebForm
{
	public string disasterDropDown;
	protected void Page_Load(object sender, EventArgs e)
	{
		//disasterDropDown = GetsDisastersForDropDown();

		//CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		//var homeCount = from c in dc.Rebuilds
		//				select c;

		//litHomesAdded.Text = homeCount.Count().ToString();

		//var volunteerCount = from uir in dc.aspnet_UsersInRoles
		//					 join r in dc.aspnet_Roles on uir.RoleId equals r.RoleId
		//					 where r.LoweredRoleName == "volunteer" || r.LoweredRoleName == "helper"
		//					 select uir;

		//litVolunteerCount.Text = volunteerCount.Count().ToString();

		//var survivorCount = from uir in dc.aspnet_UsersInRoles
		//					select uir;

		//litSurvivorCount.Text = survivorCount.Count().ToString();

		//var nonProfitCount = from pe in dc.OrganizationEvents
		//					 join ev in dc.Events on pe.EventId equals ev.EventId
		//					 select pe;


		//litNonprofitCount.Text = nonProfitCount.Count().ToString();

		if(User.Identity.IsAuthenticated)
		{
			//Send to their team activity page.
			//Get the org they are associated with

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var userOrganization = (from uo in dc.UserOrganizations
									where uo.UserId == userId
									select uo).Take(1).SingleOrDefault();

			if(userOrganization != null && !User.IsInRole("Survivor"))
			{
				Response.Redirect("/V1/NonProfit/Default.aspx?organizationId=" + userOrganization.OrganizationId);
			}
			else
			{
				//IF IN SURVIVOR ROLE, SEND TO TICKET PAGE.
				if(User.IsInRole("Survivor"))
				{
					//See if they have a ticket and send to their ticket page, if not, send to the ticket page.
					var userTicket = (from ue in dc.UserEvents
									 join p in dc.Profiles on ue.UserId equals p.UserId
									 where ue.UserId == userId
									 && ue.IsVictim == true
									 select p).Take(1).SingleOrDefault();

					if(userTicket != null )
					{ 
						Response.Redirect("/ticket/" + userTicket.ProfileNumber + "/" + userTicket.Firstname + "-" + userTicket.Lastname);
					}
					else
					{
						// No tickets yet, send to create a ticket.
						Response.Redirect("/V1/Victim/Createticket.aspx?victimId=" + userId);
					}
				}
				else
				{
					//For anyone who does not yet have a team have them choose a team.
					Response.Redirect("/V1/Administration/NonProfitList.aspx");
				}
			}
		}
	}
}