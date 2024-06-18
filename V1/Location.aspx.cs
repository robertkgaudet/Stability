using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class V1_Location : BaseOrganizationWebForm
{
	public string googlePlacesId = string.Empty;
	public string mapDomain = ConfigurationManager.AppSettings["mapDomain"].ToString();
	public string mapApiKey = ConfigurationManager.AppSettings["mapApiKey"].ToString();
	public string addressId = string.Empty;
	public string locationProfileId = string.Empty;
	public string facebookURL = string.Empty;
	public string youTubeURL = string.Empty;
	public string websiteURL = string.Empty;
	public string twitterUsername = string.Empty;
	public string instagramUsername = string.Empty;
	public string donationURL = string.Empty;
	public string activityStatus = string.Empty;
	public string eventList = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		string pageTitle = string.Empty;
		if (User.IsInRole("Administrator") || User.IsInRole("LocationAdministrator") || User.IsInRole("LocationManager"))
		{
			iEditLocationProfile.Visible = true;
			iEditLocationRelationships.Visible = true;
			divAdministrationButtons.Visible = true;
			iAddLocation.Visible = true;
		}

		locationProfileId = Request.QueryString["locationProfileId"];

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var events = from ev in dc.Events
					 join lpe in dc.LocationProfileEvents on ev.EventId equals lpe.EventId
					 join et in dc.EventTypes on ev.EventTypeId equals et.EventTypeId
					 where lpe.LocationProfileId == new Guid(locationProfileId)
					 orderby ev.BeginDate descending
					 select new { lpe, ev, et };

		if (events.Count() > 0)
		{
			hypEvent.Text = events.Take(1).SingleOrDefault().ev.Name + " " + events.Take(1).SingleOrDefault().ev.BeginDate.Value.ToShortDateString() + " (" + events.Take(1).SingleOrDefault().et.Name + ")";
			hypEvent.NavigateUrl = "/" + events.Take(1).SingleOrDefault().ev.URLFriendlyName;
			litEvent.Text = events.Take(1).SingleOrDefault().ev.Name;
			litEventType.Text = events.Take(1).SingleOrDefault().et.Name;
			pageTitle = events.Take(1).SingleOrDefault().ev.Name + ": ";
			litEventBeginDate.Text = events.Take(1).SingleOrDefault().ev.BeginDate.Value.ToLongDateString();
			if (events.Take(1).SingleOrDefault().ev.EndDate != null)
			{
				litEventEndDate.Text = events.Take(1).SingleOrDefault().ev.EndDate.Value.ToLongDateString();
			}
			else
			{
				litEventEndDate.Text = "N/A";
			}

			foreach (var eventItem in events)
			{
				eventList += "<div class=\"chat-message\">" +
				   eventItem.ev.Icon +
				   "<div class=\"message\">" +
					   "<a class=\"message-author\" style=\"text-decoration: underline;\" href=\"/Disaster/" + eventItem.ev.URLFriendlyName + "\">" + eventItem.ev.Name + "</a>" +
					   "<span class=\"message-date\"> " + eventItem.ev.BeginDate.Value.ToLongDateString() + " </span>" +
							   "<span class=\"message-content\">" +
							   eventItem.ev.Description +
							   "</span>" +
					   "<div class=\"m-t-md hidden\">" +
						   "<a class=\"btn btn-xs btn-default\"><i class=\"fa fa-thumbs-up\"></i> Thank</a>" +
						   "<a class=\"btn btn-xs btn-default\"><i class=\"fa fa-heart\"></i> Donate</a>" +
						   "<a class=\"btn btn-xs btn-default\"><i class=\"fa fa-handshake-o\"></i> Offer Help</a>" +
					   "</div>" +
				   "</div>" +
			   "</div>";
			}
		}
		else
		{
			dtEvent.Visible = false;
		}

		//List all disasters responded to.

		var location = (from lp in dc.LocationProfiles
						join a in dc.Addresses on lp.AddressId equals a.AddressId
						where lp.LocationProfileId == new Guid(locationProfileId)
						select new { lp, a }).SingleOrDefault();

		var servicesProvided = from llt in dc.LocationLocationTypes
							   join lt in dc.LocationTypes on llt.LocationTypeId equals lt.LocationTypeId
							   where llt.AddressId == location.a.AddressId
							   orderby lt.Name
							   select new { lt.Name };

		var locationParentType = (from lpt in dc.LocationParentTypes
								 join lp in dc.LocationProfiles on lpt.LocationParentTypeId equals lp.LocationParentTypeId
								 where lp.AddressId == location.a.AddressId
								 select new { lpt.Name }).SingleOrDefault();
		litLocationType.Text = "Unknown";
		if (locationParentType != null)
		{
			litLocationType.Text = locationParentType.Name;
		}

		if (servicesProvided.Count() > 0)
		{
			string locationServicesProvided = string.Empty;
			foreach (var servicesProvidedData in servicesProvided)
			{
				locationServicesProvided += servicesProvidedData.Name + ", ";
			}
			litServicesProvided.Text = locationServicesProvided.Substring(0, locationServicesProvided.Length - 2);
		}

		if (!location.lp.IsOnMap)
		{
			divMap.Visible = false;
			divHiddenMap.Visible = true;
		}

		int? capacity = location.lp.Capacity;
		LoadLocationNotes(location.a.AddressId);

		addressId = location.a.AddressId.ToString();
		litLatitude.Text = location.a.Latitude;
		litLongitude.Text = location.a.Longitude;
		litName.Text = location.lp.Name;
		litLocationName.Text = location.lp.Name;
		litDescription.Text = location.lp.Description;
		litLocationDescription.Text = location.lp.Description;
		lblStreetAddress.Text = location.a.StreetNumber + " " + location.a.StreetName;
		lblCityStateZip.Text = location.a.City + ", " + location.a.State + " " + location.a.Zip;
		lblCountry.Text = location.a.Country;
		lblCounty.Text = location.a.County;
		litCapacity.Text = ((capacity == null) || (capacity == 0)) ? "N/A" : location.lp.Capacity.ToString();

		donationURL = location.lp.DonationURL;
		liDonation.Visible = location.lp.DonationURL == null ? false : true;

		facebookURL = location.lp.FacebookURL;
		iFacebook.Visible = location.lp.FacebookURL == null ? false : true;

		youTubeURL = location.lp.YouTubeURL;
		iYouTube.Visible = location.lp.YouTubeURL == null ? false : true;

		websiteURL = location.lp.WebsiteURL;
		iGlobe.Visible = location.lp.WebsiteURL == null ? false : true;

		twitterUsername = location.lp.TwitterUsername;
		iTwitter.Visible = location.lp.TwitterUsername == null ? false : true;

		instagramUsername = location.lp.InstagramUsername;
		iInstagram.Visible = location.lp.InstagramUsername == null ? false : true;

		litUpdatedBy.Text = location.lp.UpdatedBy == null ? "N/A" : GetFullNameFromUserId(location.lp.UpdatedBy);
		litCreatedBy.Text = location.lp.CreatedBy == null ? "N/A" : GetFullNameFromUserId(location.lp.CreatedBy);
		litUpdatedOn.Text = location.lp.UpdatedOn == null ? "N/A" : location.lp.UpdatedOn.Value.ToShortDateString();
		litCreatedOn.Text = location.lp.CreatedOn == null ? "N/A" : location.lp.CreatedOn.ToShortDateString();
		litDateOpened.Text = location.lp.DateOpened == null ? "N/A" : location.lp.DateOpened.Value.ToLongDateString();
		litDateClosed.Text = location.lp.DateClosed == null ? "N/A" : location.lp.DateClosed.Value.ToLongDateString();
		litIsActve.Text = location.lp.IsActive ? "<label class='fa fa-clock-o text-success'> VISIBLE</label>" : "<label class='fa fa-ban text-danger'> HIDDEN</label>";
		litPets.Text = location.lp.AllowsPets ? "<label class='fa fa-check text-success'> ALLOWS PETS</label>" : "<label class='fa fa-ban text-danger'> NO PETS</label>";
		litMedical.Text = location.lp.ProvidesMedicalHelp ? "<label class='fa fa-ambulance text-success'> MEDICAL HELP AVAILABLE</label>" : "<label class='fa fa-ban text-danger'> NO MEDICAL HELP AVAILABLE</label>";
		litVolunteers.Text = location.lp.SeekingVolunteers == null ? "N/A" : (Convert.ToBoolean(location.lp.SeekingVolunteers) ? "Seeking Volunteers" : "No Volunteers At This Time");
		var locationActivityStatus = from lls in dc.LocationLocationStatus
									 join ls in dc.LocationStatus on lls.LocationStatusId equals ls.LocationStatusId
									 where lls.AddressId == new Guid(addressId)
									 orderby lls.UpdatedOn descending
									 select new { ls.Name, lls.UpdatedBy, lls.UpdatedOn };

		if (locationActivityStatus.Count() > 0)
		{
			litActivity.Text = locationActivityStatus.Take(1).SingleOrDefault().Name;

			foreach (var locationActivityStatusItem in locationActivityStatus)
			{
				activityStatus += "<tr><td>" + locationActivityStatusItem.Name + "</td><td><small>" + GetFullNameFromUserId(locationActivityStatusItem.UpdatedBy) + "</small></td><td><small>" + locationActivityStatusItem.UpdatedOn + "</small></td></tr>";
			}
		}

		if (!String.IsNullOrEmpty(location.lp.PointOfContactName))
		{
			lblPointOfContactName.Text = location.lp.PointOfContactName;
		}
		else
		{
			lblPointOfContactName.Text = "No Point of Contact";
		}

		if (!String.IsNullOrEmpty(location.lp.EmailAddress))
		{
			hypEmail.Text = location.lp.EmailAddress;
			hypEmail.NavigateUrl = "mailto:" + location.lp.EmailAddress;
		}
		else
		{
			hypEmail.Text = "Email NA";
		}

		if (!String.IsNullOrEmpty(location.lp.PhoneNumber))
		{
			hypPhone.Text = Regex.Replace(location.lp.PhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
			hypPhone.NavigateUrl = "tel:" + location.lp.PhoneNumber;
		}
		else
		{
			hypPhone.Text = "Phone NA";
		}

		if (!String.IsNullOrEmpty(location.lp.PointOfContactEmail))
		{
			hypPointOfContactEamil.Text = location.lp.PointOfContactEmail;
			hypPointOfContactEamil.NavigateUrl = "mailto:" + location.lp.PointOfContactEmail;
		}
		else
		{
			hypPointOfContactEamil.Text = "Email NA";
		}
		if (!String.IsNullOrEmpty(location.lp.PointOfContactPhoneNumber))
		{
			hypPointOfContactPhone.Text = Regex.Replace(location.lp.PointOfContactPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
			hypPointOfContactPhone.NavigateUrl = "tel:" + location.lp.PointOfContactPhoneNumber;
		}
		else
		{
			hypPointOfContactPhone.Text = "Phone NA";
		}
		googlePlacesId = location.a.GooglePlaceId;


		this.Master.PageTitle = pageTitle + location.lp.Name + " - Stability Location";
		this.Master.FbSite_name = pageTitle + location.lp.Name + " - Stability Location";
		this.Master.PageDescription = location.lp.Description;
		this.Master.FbDescription = location.lp.Description;
		this.Master.FbImage = "Images/HurricaneMichael.jpg";
		this.Master.FbImageType = "image/jpg";
		this.Master.FbURL = Request.Url.AbsoluteUri;
	}

	protected void btnAddProfileNote_Click(object sender, EventArgs e)
	{
		string post = txtPost.Text;
		if (!string.IsNullOrEmpty(post))
		{
			InsertLocationNote(post, new Guid(addressId), userId);
			LoadLocationNotes(new Guid(addressId));
			txtPost.Text = string.Empty;
		}
	}

	public void LoadLocationNotes(Guid addressId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var posts = from n in dc.Notes
					join ln in dc.LocationNotes on n.NoteId equals ln.NoteId
					join p in dc.Profiles on n.CreatedBy equals p.UserId
					where ln.AddressId == addressId
					orderby n.CreatedOn descending
					select new { p.ProfileId, n.Note1, n.CreatedOn, p.ZelloName, p.UserId, fullname = "<b>" + p.Firstname + " " + p.Lastname + "</b>", ProfilePhoto = (p.Photo == null ? "Avatar.png" : p.Photo) };

		rptPosts.DataSource = posts;
		rptPosts.DataBind();
	}

	protected string GetFullNameFromUserId(Guid? userId)
	{
		string fullname = "N/A";
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var userProfile = (from p in dc.Profiles
						   where p.UserId == userId
						   select new { FullName = p.Firstname + " " + p.Lastname }).SingleOrDefault();


		if (userProfile != null)
		{
			fullname = userProfile.FullName;
		}
		return fullname;
	}

	protected void rptPosts_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
			String fullname = (String)DataBinder.Eval(dataItem.DataItem, "fullname");
			String zelloName = (String)DataBinder.Eval(dataItem.DataItem, "ZelloName");
			DateTime createdOn = (DateTime)DataBinder.Eval(dataItem.DataItem, "createdOn");

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var roles = from ur in dc.aspnet_UsersInRoles
					   join r in dc.aspnet_Roles on ur.RoleId equals r.RoleId
					   where ur.UserId == userId && (r.RoleName.ToLower() == "survivor" || r.RoleName.ToLower() == "helper" || r.RoleName.ToLower() == "volunteer")
					   select new { r.RoleName };

			string rolesDisplay = string.Empty;
			foreach (var role in roles)
			{
				switch(role.RoleName.ToLower())
				{
					case "helper":
						{
							rolesDisplay = "Volunteer/Helper";
							break;
						}
					case "volunteer":
						{
							rolesDisplay = "Volunteer/Helper";
							break;
						}
					case "survivor":
						{
							rolesDisplay = "Survivor";
							break;
						}
				}
			}

			string profileURL = "/V1/Profile/Profile.aspx";
			if (roles.Count() > 0)
			{
				profileURL = "/S1/Profile/Default.aspx";
			}

			var profilePhoto = (from p in dc.ProfilePhotos
					join ph in dc.Photos on p.PhotoId equals ph.PhotoId
					where p.UserId == userId && p.IsCurrrent == true
					select new { ph.FilenameCropped }).Take(1).SingleOrDefault();
			
			HyperLink hypFullname = (HyperLink)e.Item.FindControl("hypFullname");
			Image imgProfilePhoto = (Image)e.Item.FindControl("imgProfilePhoto");
			Literal litCreatedOn = (Literal)e.Item.FindControl("litCreatedOn");

			litCreatedOn.Text = GetElapsedTime(createdOn);
			hypFullname.Text = fullname + " (" + rolesDisplay + ") posted a comment.";
			hypFullname.NavigateUrl = profileURL;

			imgProfilePhoto.ImageUrl = "/V1/Images/icons8-customer-64.png";
		}
	}
}