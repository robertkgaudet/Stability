using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;

public partial class CaseManagement_Default : BaseOrganizationWebForm
{
	public string disasterDropDown = string.Empty;
	public string eventId = string.Empty;
	public string eventName = string.Empty;
	public string preselectedDisasterJQuery = string.Empty;

	public string mapDomain = ConfigurationManager.AppSettings["mapDomain"].ToString();
	public string mapApiKey = ConfigurationManager.AppSettings["mapApiKey"].ToString();
	public string mapURL = string.Empty;
	public string latitude = string.Empty;
	public string longitude = string.Empty;
	public string zoom = string.Empty;
	public string host = HttpContext.Current.Request.Url.Host; //HttpContext.Current.Request.Url.Host;

	protected void Page_Load(object sender, EventArgs e)
	{
		Event disaster;
		if (host == "localhost")
		{
			host = "http://" + host + ":" + HttpContext.Current.Request.Url.Port;
		}
		else
		{
			host = "https://" + host;
		}
		userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
		//Load page from an event.
		if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["eventName"]))
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			disaster = (from ev in dc.Events
							where ev.URLFriendlyName == HttpContext.Current.Request.QueryString["eventName"]
							select ev).SingleOrDefault();
		}
		else
		{
			//Choose the latest disaster this user is associated with
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			disaster = (from ev in dc.Events
							join oe in dc.OrganizationEvents on ev.EventId equals oe.EventId
							where oe.OrganizationId == userOrganizationId
							orderby ev.BeginDate descending
							select ev).Take(1).SingleOrDefault();
		}

		if(disaster != null)
		{ 
			eventId = disaster.EventId.ToString();
			eventName = disaster.Name;

			if (!string.IsNullOrEmpty(disaster.Latitude))
			{
				latitude = disaster.Latitude;
				longitude = disaster.Longitude;
				zoom = disaster.Zoom.ToString();
			}
			litEventTimeLapse.Text = GetMonthsYearsElapsed(Convert.ToDateTime(disaster.BeginDate));
			litEventActive.Text = disaster.IsActive ? "Active" : "Not Active";
			litEventDate1.Text = disaster.BeginDate.Value.ToLongDateString();
			litEventDate2.Text = disaster.BeginDate.Value.ToLongDateString();
			litEventDate3.Text = disaster.BeginDate.Value.ToLongDateString();
		}
		else
		{
			//Organization has not created a campawriteign yet.
			//TODO
			Response.Redirect("/HurricaneIan/NonProfit");
		}
		LoadSurvivors(eventId);
		LoadDisasters();
		hidEventId.Value = eventId.ToString();
	}

    protected void btnNewSurveySubmit_Click(object sender, EventArgs e)
    {
		Response.Redirect("Screening.aspx");
    }
    protected void btnNewClientIntakeSubmit_Click(object sender, EventArgs e)
	{
		Response.Redirect("AddCase.aspx");
	}

	protected void btnViewClients_Click(object sender, EventArgs e)
	{
		Response.Redirect("CaseList.aspx");
	}
	public void LoadDisasters()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disasters = from d in dc.Events
						join oe in dc.OrganizationEvents on d.EventId equals oe.EventId
						where oe.OrganizationId == userOrganizationId
						orderby d.BeginDate descending
						select new { d };

		int idNumber = 0;
		foreach (var disaster in disasters)
		{
			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			disasterDropDown = disasterDropDown + "<li id=\"" + disaster.d.EventId + "\" urlName=\"" + disaster.d.URLFriendlyName + "\"><a href=\"#\">" + disaster.d.Name + " - " + disasterDate + "</a></li>" + Environment.NewLine;
			idNumber = idNumber + 1;
		}
		if (!String.IsNullOrEmpty(eventId))
		{
			//Hide the Dropdown and show the selected disaster
			var disaster = (from d in dc.Events
							where d.EventId == new Guid(eventId)
							orderby d.BeginDate descending
							select new { d }).SingleOrDefault();

			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			preselectedDisasterJQuery = "$(\"#btn-dropdown.disasterEvent\").html('" + disaster.d.Name + " - " + disasterDate + "');";
			hidEventId.Value = eventId;

			var eventDetails = (from ev in dc.Events
								where ev.EventId == new Guid(eventId)
								select ev).SingleOrDefault();
		}
	}

	protected void LoadSurvivors(string eventId)
	{
		//Show all survivors for this disaster.
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var survivors = from ue in dc.UserEvents
						join p in dc.Profiles on ue.UserId equals p.UserId //Gets the Survivors information.
						join aspUser in dc.aspnet_Memberships on p.UserId equals aspUser.UserId
						join ur in dc.aspnet_UsersInRoles on ue.UserId equals ur.UserId
						join r in dc.aspnet_Roles on ur.RoleId equals r.RoleId
						where ue.EventId == new Guid(eventId)
						&& r.LoweredRoleName == "survivor"
						orderby aspUser.CreateDate descending
						select new { p.Firstname, p.Lastname, createdBy = ue.CreatedBy, p.PhoneNumber, p.City, p.State, aspUser.Email, survivorId = p.UserId, p.ProfileNumber };

		if (survivors.Count() > 0)
		{
			litTotalCases.Text = survivors.Count().ToString();
			divSurvivorList.Visible = true;
			divAddSurvivorButton.Visible = false;

			rpSurvivorListTable.DataSource = survivors;
			rpSurvivorListTable.DataBind();
		}
		else
		{
			//Show a button to add a new survivor.
			divSurvivorList.Visible = false;
			divAddSurvivorButton.Visible = true;
		}
	}

	protected void rpSurvivorListTable_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litLivingSituation = (Literal)e.Item.FindControl("litLivingSituation");
			Literal litRecoveryStage = (Literal)e.Item.FindControl("litRecoveryStage"); 
			HyperLink hypAddress = (HyperLink)e.Item.FindControl("hypAddress"); 
			HyperLink hypName = (HyperLink)e.Item.FindControl("hypName");
			HyperLink hypPhone = (HyperLink)e.Item.FindControl("hypPhone");
			HyperLink hypEmailAddress = (HyperLink)e.Item.FindControl("hypEmailAddress");
			Literal litProfileNumber = (Literal)e.Item.FindControl("litProfileNumber");
			Literal litSurvivorQualifiers = (Literal)e.Item.FindControl("litSurvivorQualifiers");
			Literal litNote = (Literal)e.Item.FindControl("litNote");
			Literal litAddedBy = (Literal)e.Item.FindControl("litAddedBy"); 

			 String Firstname = (string)DataBinder.Eval(dataItem.DataItem, "Firstname");
			String Lastname = (string)DataBinder.Eval(dataItem.DataItem, "Lastname");
			int profileNumber = (int)DataBinder.Eval(dataItem.DataItem, "profileNumber");
			String email = (string)DataBinder.Eval(dataItem.DataItem, "Email");
			String phoneNumber = (string)DataBinder.Eval(dataItem.DataItem, "PhoneNumber");
			Guid survivorId = (Guid)DataBinder.Eval(dataItem.DataItem, "survivorId");
			profileNumber = (int)DataBinder.Eval(dataItem.DataItem, "ProfileNumber");
			if(DataBinder.Eval(dataItem.DataItem, "CreatedBy") != null)
			{ 
				Guid createdBy = (Guid)DataBinder.Eval(dataItem.DataItem, "CreatedBy");

				var createdByProfile = (from p in dc.Profiles
										where p.UserId == createdBy
										select p).SingleOrDefault();

				litAddedBy.Text = createdByProfile.Firstname + " " + createdByProfile.Lastname;
			}
			litProfileNumber.Text = profileNumber.ToString();

			//Get userQualifiers/RebuildStatus

			var userQualifiers = from uq in dc.UserQualifiers
								join rs in dc.RebuildStatus on uq.QualifierId equals rs.RebuildStatusId
								where uq.UserId == survivorId
								select new { rs.Status };
			litSurvivorQualifiers.Text = "<b>Vulnerabilities:</b> ";
			foreach (var userQualifier in userQualifiers)
			{
				litSurvivorQualifiers.Text += userQualifier.Status + ", ";
			}
			var userBasicNeeds = from bn in dc.UserBasicNeeds
								 join rs in dc.RebuildStatus on bn.RebuildStatusId equals rs.RebuildStatusId
								 where bn.UserId == survivorId
								 select new { rs.Status };
			litSurvivorQualifiers.Text = litSurvivorQualifiers.Text.TrimEnd(' ').TrimEnd(',');
			litSurvivorQualifiers.Text += "<br><b>Basic Needs:</b> ";
			foreach (var userBasicNeed in userBasicNeeds)
			{
				litSurvivorQualifiers.Text += userBasicNeed.Status + ", ";
			}
			litSurvivorQualifiers.Text = litSurvivorQualifiers.Text.TrimEnd(' ').TrimEnd(',');

			//Does user have an entry in profile address table?
			var profileAddress = (from pa in dc.ProfileAddresses
								  join a in dc.Addresses on pa.AddressId equals a.AddressId
								  where pa.ProfileId == survivorId
								  select new { a }).Take(1).SingleOrDefault();

			if(profileAddress != null)
			{
				hypAddress.Text = profileAddress.a.Address1 + " " + profileAddress.a.City + ", " + profileAddress.a.State + "</br>" + profileAddress.a.County;
				hypAddress.NavigateUrl = "EditHome.aspx?userId=" + survivorId + "&addressId=" + profileAddress.a.AddressId;
			}
			else
			{
				hypAddress.Text = "Create a Home Record";
				hypAddress.NavigateUrl = "AddLocation.aspx?userId=" + survivorId;
				hypAddress.CssClass = "btn btn-default btn-xs btn-mini text-muted";
			}

			//Get the latest housing situation.
			var housing = (from h in dc.Housings
						   join uh in dc.UserHousings on h.HousingId equals uh.HousingId
						   where uh.UserId == survivorId
						   orderby uh.CreatedOn descending
						   select new { h.Housing1, h.Sentence }).Take(1).SingleOrDefault();

			if (housing != null)
			{
				litLivingSituation.Text = housing.Sentence + " " + housing.Housing1;
			}

			//Get case notes.
			var post = (from n in dc.Notes
						join pn in dc.ProfileNotes on n.NoteId equals pn.NoteId
						join p in dc.Profiles on pn.UserId equals p.UserId
						where pn.UserId == survivorId
						orderby n.CreatedOn descending
						select new { n.Note1, n.CreatedOn, fullname = p.Firstname + " " + p.Lastname }).Take(1).SingleOrDefault();
			if(post != null)
			{ 
				litNote.Text = post.Note1 + "<small class='text-muted'>By: " + post.fullname + " " + post.CreatedOn + "</small>";
			}
			hypName.NavigateUrl = "/case/" + profileNumber + "\\" + Firstname + "-" + Lastname;
			hypName.Text = Firstname + " " + Lastname;

			hypEmailAddress.NavigateUrl = "mailto:" + email;
			hypEmailAddress.Text = email;

			hypPhone.NavigateUrl = "tel:" + phoneNumber;
			hypPhone.Text = phoneNumber;
		}
	}

}