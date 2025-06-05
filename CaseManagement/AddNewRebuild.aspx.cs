using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net;
using System.Xml.Linq;

public partial class Account_Organization_AddNewRebuild : BaseOrganizationWebForm
{
	Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
	Guid organizationId = new Guid("79305F85-3816-46A8-911F-0D7E3E227C32");
	string eventId = HttpContext.Current.Request.QueryString["eventId"];
	public string headerColor = string.Empty;
	bool userIsAssociatedWithNonProfit = false;
	public string disasterDropDown = string.Empty;
	public string preselectedDisasterJQuery = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			
		//If user is not associated with an organization hide the organization selector.
		var userOrganizationCheck = from uo in dc.UserOrganizations
									where uo.UserId == userId && (uo.Status == (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
                                    select uo;
		
		divNonProfitForm.Visible = false;
		if(userOrganizationCheck.Count() > 0)
		{
			//divNonProfitForm.Visible = true;
			//userIsAssociatedWithNonProfit = true;
		}

		if(!IsPostBack)
		{
			if(userIsAssociatedWithNonProfit)
			{
				LoadRadioButtonLists(); //ActiveStatus - Stage
				LoadCollaborators(); //Non-profits working together.
			}

			LoadDisasters();
			LoadQualifiers();
			//hidTargetedStartDate.Value = DateTime.Today.ToShortDateString();


			//Get the event selected.
		}
	}

	public void LoadDisasters()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disasters = from d in dc.Events
						orderby d.BeginDate descending
						select new {d };
		
		litEventName.Text = "Add A New Home Rebuild Project";

		int idNumber = 0;
		foreach(var disaster in disasters)
		{
			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			disasterDropDown = disasterDropDown + "<li id=\"" + disaster.d.EventId + "\"><a href=\"#\">" + disaster.d.Name + " - " + disasterDate +  "</a></li>" + Environment.NewLine;
			idNumber = idNumber + 1;
		}
		if(!String.IsNullOrEmpty(eventId))
		{
			//Hide the Dropdown and show the selected disaster
			var disaster = (from d in dc.Events
							where d.EventId == new Guid(eventId)
							orderby d.BeginDate descending
							select new {d}).SingleOrDefault();
			
			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			preselectedDisasterJQuery = "$(\"#btn-dropdown.disasterEvent\").html('" + disaster.d.Name + " - " + disasterDate + "');";
			hidEventId.Value = eventId;
			
			var eventDetails = (from ev in dc.Events
						   where ev.EventId == new Guid(eventId)
						   select ev).SingleOrDefault();

			if(eventDetails != null)
			{
				litEventName.Text = "New " + eventDetails.Name + " Home Rebuild Project";
				headerColor = "h" + CrowdRelief.Tools.GetColor(eventDetails.Color);
			}
		}
	}

	protected void LoadQualifiers()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var qualifiers = from o in dc.RebuildStatus
						 where o.StatusType == 2
						 orderby o.OrderBy
						 select o;

		 ddlQualifiers.DataSource = qualifiers;
		ddlQualifiers.DataBind();
		ddlQualifiers.Attributes.Add("multiple", "");
		ddlQualifiers.Attributes.Add("style", "width:100%;");
	}

	protected void LoadCollaborators()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var collaboratingOrganizations =  from o in dc.Organizations
										 //from oo in dc.OrganizationOrganizations
										 //where
										 //((o.OrganizationId == oo.OrganizationId1) || (o.OrganizationId == oo.OrganizationId2))
										 //&&
										 //((oo.OrganizationId1 == organizationId) || (oo.OrganizationId2 == organizationId))
										 select new { o.Name, o.OrganizationId };

		dllSelectCollaborators.DataSource = collaboratingOrganizations;
		dllSelectCollaborators.DataBind();
		dllSelectCollaborators.Attributes.Add("multiple", "multiple");
		dllSelectCollaborators.Attributes.Add("style", "width:100%;");
	}

	protected void LoadRadioButtonLists()
	{
		rblIsActive.RepeatDirection = RepeatDirection.Horizontal;
		ListItem isActive = new ListItem();
		isActive.Text = "Is Active";
		isActive.Attributes.Add("class", "radio radio-success");
		isActive.Value = "1";
		isActive.Selected = true;
		rblIsActive.Items.Add(isActive);

		ListItem isNoGo = new ListItem();
		isNoGo.Attributes.Add("class", "radio radio-danger");
		isNoGo.Text = "Is No Go";
		isNoGo.Value = "0";
		rblIsActive.Items.Add(isNoGo);


		rblRebuildStatus.RepeatDirection = RepeatDirection.Horizontal;
		ListItem iceBox = new ListItem();
		iceBox.Text = "Ice Box";
		iceBox.Attributes.Add("class", "radio radio-info");
		iceBox.Value = "7D166B51-6326-4F0D-A80D-7CB497F602FD";
		iceBox.Selected = true;
		rblRebuildStatus.Items.Add(iceBox);

		ListItem onDeck = new ListItem();
		onDeck.Text = "On Deck";
		onDeck.Attributes.Add("class", "radio radio-warning	");
		onDeck.Value = "E41D550E-FE7C-49B7-99CE-C479D0218FB3";
		rblRebuildStatus.Items.Add(onDeck);

		rblRebuildStatus.RepeatDirection = RepeatDirection.Horizontal;
		ListItem active = new ListItem();
		active.Text = "Active";
		active.Attributes.Add("class", "radio radio-success");
		active.Value = "75F946C8-AEFD-4068-A1DC-4C410F123B15";
		rblRebuildStatus.Items.Add(active);
		rblRebuildStatus.RepeatDirection = RepeatDirection.Horizontal;

		ListItem complete = new ListItem();
		complete.Text = "Complete";
		complete.Attributes.Add("class", "radio radio-primary	");
		complete.Value = "CABC8F7B-70C5-48FC-B9F9-55C7F78CD1CF";
		rblRebuildStatus.Items.Add(complete);



	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		Guid organizationId = new Guid("79305F85-3816-46A8-911F-0D7E3E227C32");
		string firstname = txtFirstname.Value;
		string lastname = txtLastName.Value;
		string age = txtAge.Value;
		string bathrooms = txtBathrooms.Value;
		string bedrooms = txtBedrooms.Value;
		string city = txtCity.Value;
		string email = txtEmail.Value;
		string lossAddress = txtLossAddress.Value;
		string squareFeet = txtSquareFeet.Value;
		string telephone = txtTelephone.Value;
		string zip = txtZip.Value;
		string state = ddlState.Value;
		string isActive = !String.IsNullOrEmpty(rblIsActive.SelectedValue) ? rblIsActive.SelectedValue : "0";
		string rebuildStatusIdText = rblRebuildStatus.SelectedValue;
		string rebuildDescription = txtRebuildDescription.Value;
		string targetStartDate = hidTargetedStartDate.Value;
		string latitude = string.Empty;
		string longitude = string.Empty;
		string latLonMessage = string.Empty;

		if(!String.IsNullOrEmpty(rebuildDescription))
		{
			rebuildDescription = rebuildDescription.Replace("\n", "<br/>").Replace("\r","<br/>");
		}

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		Guid newRebuildUserId = Guid.Empty;
		string newUserNameCheck = Membership.GetUserNameByEmail(email); // Get the user object

		if (String.IsNullOrEmpty(newUserNameCheck))
		{
			string username = CreateRandomUsername(firstname) + "-" + CreateRandomUsername(lastname); //Create a username
					
			Membership.CreateUser(username, "password", email); //Create the new user
			MembershipUser newRebuildUser = Membership.GetUser(username); // Get the user object
			newRebuildUserId = new Guid(newRebuildUser.ProviderUserKey.ToString()); //Get the users new userId

			Profile profile = new Profile();
			profile.Firstname = firstname;
			profile.Address = lossAddress;
			profile.City = city;
			profile.Lastname = lastname;
			profile.PhoneNumber = telephone;
			profile.ProfileId = newRebuildUserId;
			profile.State = state;
			profile.UserId = newRebuildUserId;
			profile.Zip = zip;
			profile.VettingComplete = false;
			if(!String.IsNullOrEmpty(age))
				profile.Age = Int32.Parse(age);

			dc.Profiles.InsertOnSubmit(profile);
			dc.SubmitChanges();

			Roles.AddUserToRole(username, "Survivor");
			Roles.AddUserToRole(username, "Member");
		}
		else
		{
			//user already exists get their userid
			MembershipUser newRebuildUser = Membership.GetUser(newUserNameCheck); // Get the user object
			newRebuildUserId = new Guid(newRebuildUser.ProviderUserKey.ToString()); //Get the users new userId


			//Should probably update the profile informaiton here.
		}
		
		Guid rebuildAddressId = Guid.NewGuid();
		Guid rebuildId = Guid.NewGuid();
		Guid rebuildSurveyId = Guid.NewGuid();
		Guid rebuildRebuildStatusId = Guid.NewGuid();
		Guid rebuildStatusId = Guid.Empty;
		if(userIsAssociatedWithNonProfit && !String.IsNullOrEmpty(rebuildStatusIdText))
		{
			rebuildStatusId = new Guid(rebuildStatusIdText);
		}
		Guid organizationRebuildId = Guid.NewGuid();
		Guid rebuildPostId = Guid.NewGuid();

		string fullAddress = lossAddress + " " + city + " " + state;

		if (GetLatitudeLongitude(fullAddress, out latitude, out longitude, out latLonMessage))
		{}
		else
		{
			//Error getting lat and lon...
			//Invalide address perhaps?
			//Store this information in a log file.
		}

		Address address = new Address();
		address.Address1 = lossAddress;
		address.AddressId = rebuildAddressId;
		address.City = city;
		address.CreatedBy = userId;
		address.CreatedOn = DateTime.Now;
		address.IsActive = Convert.ToBoolean(Int32.Parse(isActive));
		address.State = state;
		address.Zip = zip;
		if(!String.IsNullOrEmpty(latitude) && !String.IsNullOrEmpty(longitude))
		{ 
			address.Latitude = latitude;
			address.Longitude = longitude;
		}
		dc.Addresses.InsertOnSubmit(address);
		dc.SubmitChanges();
		
		if(userIsAssociatedWithNonProfit)
		{
			isActive = "1";
		}

		Rebuild rebuild = new Rebuild();
		rebuild.CreatedBy = userId;
		rebuild.EventId = new Guid(hidEventId.Value);
		rebuild.CreatedOn = DateTime.Now;
		rebuild.IsHidden = false;
		rebuild.IsOnHold = Convert.ToBoolean(Int32.Parse(isActive));
		rebuild.RebuildAddressId = rebuildAddressId;
		rebuild.RebuildDescription = rebuildDescription.Replace("\r","<br\\>").Replace("\n", "<br\\>").Replace("\r\n", "<br\\>");
		rebuild.RebuildId = rebuildId;
		if(!String.IsNullOrEmpty(targetStartDate))
		{
			DateTime startDate = Convert.ToDateTime(targetStartDate);
			rebuild.RebuildStartDate = startDate;
		}
		if(!String.IsNullOrEmpty(hidTaskType.Value))
		{
			rebuild.Difficulty = Int32.Parse(hidTaskType.Value);
		}
		rebuild.SurvivorId = newRebuildUserId;
		dc.Rebuilds.InsertOnSubmit(rebuild);

		RebuildSurvey rebuildSurvey = new RebuildSurvey();
		if(!String.IsNullOrEmpty(bathrooms))
		{
			rebuildSurvey.Bathrooms = Int32.Parse(bathrooms);

		}
		if (!String.IsNullOrEmpty(bedrooms))
		{
			rebuildSurvey.Bedrooms = Int32.Parse(bedrooms);
		}
		if (!String.IsNullOrEmpty(squareFeet))
		{
			rebuildSurvey.SquareFeet = Int32.Parse(squareFeet);
		}
		rebuildSurvey.CreatedBy = userId;
		rebuildSurvey.AddressId = rebuildAddressId;
		rebuildSurvey.CreatedOn = DateTime.Now;
		rebuildSurvey.NumberOfPeopleAffected = 0;
		rebuildSurvey.RebuildSurveyId = rebuildSurveyId;
		dc.RebuildSurveys.InsertOnSubmit(rebuildSurvey);
		dc.SubmitChanges();
		
		if(userIsAssociatedWithNonProfit)
		{
			RebuildRebuildStatus rebuildRebuildStatus = new RebuildRebuildStatus();
			rebuildRebuildStatus.CreatedBy = userId;
			rebuildRebuildStatus.CreatedOn = DateTime.Now;
			rebuildRebuildStatus.RebuildRebuildStatusId = rebuildRebuildStatusId;
			rebuildRebuildStatus.RebuildId = rebuildId;
			rebuildRebuildStatus.RebuildStatusId = rebuildStatusId;
			dc.RebuildRebuildStatus.InsertOnSubmit(rebuildRebuildStatus);
			dc.SubmitChanges();

			OrganizationRebuild organizationRebuild = new OrganizationRebuild();
			organizationRebuild.CreatedOn = DateTime.Now;
			organizationRebuild.IsPrimaryOrganization = true;
			organizationRebuild.OrganizationId = organizationId;
			organizationRebuild.OrganizationRebuildId = organizationRebuildId;
			organizationRebuild.RebuildId = rebuildId;
			dc.OrganizationRebuilds.InsertOnSubmit(organizationRebuild);
			dc.SubmitChanges();

			foreach (ListItem listItem in dllSelectCollaborators.Items)
			{
				if (listItem.Selected == true)
				{
					Guid selectedOrganizationId = new Guid(listItem.Value);

					if (organizationId != selectedOrganizationId)
					{
						OrganizationRebuild collaboratingOrganizationRebuild = new OrganizationRebuild();
						collaboratingOrganizationRebuild.CreatedOn = DateTime.Now;
						collaboratingOrganizationRebuild.IsPrimaryOrganization = false;
						collaboratingOrganizationRebuild.OrganizationId = selectedOrganizationId;
						collaboratingOrganizationRebuild.OrganizationRebuildId = Guid.NewGuid();
						collaboratingOrganizationRebuild.RebuildId = rebuildId;
						dc.OrganizationRebuilds.InsertOnSubmit(collaboratingOrganizationRebuild);
						dc.SubmitChanges();
					}
				}
			}
		}


		foreach (ListItem listItem in ddlQualifiers.Items)
		{
			if (listItem.Selected == true)
			{
				Guid qualifierId = new Guid(listItem.Value);

				RebuildQualifier rebuildQualifier = new RebuildQualifier();
				rebuildQualifier.QualifierId = qualifierId;
				rebuildQualifier.RebuildId = rebuildId;
				rebuildQualifier.RebuildQualifiersId = Guid.NewGuid();
				dc.RebuildQualifiers.InsertOnSubmit(rebuildQualifier);
				dc.SubmitChanges();
			}
		}

		RebuildPost rebuildPost = new RebuildPost();
		rebuildPost.RebuildPostId = rebuildPostId;
		rebuildPost.CreatedOn = DateTime.Now;
		rebuildPost.IsVisible = true;
		rebuildPost.Post = "Just created a new rebuild in " + city + ", " + state + " for " + firstname + " " + lastname + ". The targeted start date is <i>" + targetStartDate + "</i>.";
		rebuildPost.RebuildId = rebuildId;
		rebuildPost.UserId = userId;
		dc.RebuildPosts.InsertOnSubmit(rebuildPost);
		dc.SubmitChanges();

		Response.Redirect("Rebuild.aspx?rebuildId=" + rebuildId.ToString());
	}

	protected bool GetLatitudeLongitude(string address, out string latitude, out string longitude, out string message)
	{
		message		= string.Empty;
		latitude	= string.Empty;
		longitude	= string.Empty;
		bool status = true;
		try
		{
			status = true;
			//var address = "123 something st, somewhere";
			var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));

			var request = WebRequest.Create(requestUri);
			var response = request.GetResponse();
			var xdoc = XDocument.Load(response.GetResponseStream());

			var result = xdoc.Element("GeocodeResponse").Element("result");
			var locationElement = result.Element("geometry").Element("location");

			latitude = locationElement.Element("lat").Value;
			longitude = locationElement.Element("lng").Value;
			message = "success";
		}
		catch (Exception ex)
		{
			status = false;
			message = ex.Message;
		}
		return status;//return false if failed.
	}

	private string CreateRandomUsername(string allowedCharacters)
	{
		Random random = new Random();
		
		char[] chars = new char[allowedCharacters.Length];

		for (int i = 0; i < allowedCharacters.Length; i++)
		{
			chars[i] = allowedCharacters[random.Next(0, allowedCharacters.Length)];
		}

		return new string(chars);
	}
}