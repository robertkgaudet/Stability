using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

public partial class CaseManagement_EditHome : BaseOrganizationWebForm
{
	public string _userId;

	protected void Page_Load(object sender, EventArgs e)
	{
		this.Master.PageTitle			= "Stability - Edit Home Information";
		this.Master.PageDescription		= "";
		this.Master.FbDescription		= "";
		this.Master.FbImage				= "Images/HurricaneMichael.jpg";
		this.Master.FbImageType			= "image/jpg";
		this.Master.FbSite_name			= "Stability - Edit Home Information";
		this.Master.FbURL				= Request.Url.AbsoluteUri;

		string addressId = Request.QueryString["addressId"];
		_userId = Request.QueryString["userId"];

		if (!IsPostBack)
		{
			LoadRadioButtonLists(); //ActiveStatus - Stage
			LoadCollaborators(); //Non-profits working together.
			if (!String.IsNullOrEmpty(addressId))
			{
				chkIsOnMap.Checked = true;

				CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

				var address = (from a in dc.Addresses
										  where a.AddressId == new Guid(addressId)
										  select a).SingleOrDefault();

				lblLocationAddress.InnerHtml = address.StreetNumber + " " + address.StreetName +"<br>" + address.City + ", " + address.State + " " + address.Zip;
			  
				var profileAddress = (from lp in dc.ProfileAddresses
										where lp.AddressId == new Guid(addressId)
										select lp).SingleOrDefault();

				if(profileAddress != null)
				{
					//Load the info.
					chkIsOnMap.Checked = profileAddress.ShowOnAgencyMap;
					chkIsOnCleanupMap.Checked = profileAddress.ShowOnCleanupMap;
					if (profileAddress.HomeRelationshipOwnRentTypeId != null)
					{ rblOwnRent.SelectedValue = profileAddress.HomeRelationshipOwnRentTypeId.ToString(); }

					if (profileAddress.HomeTypeId != null)
					{ rblHomeType.SelectedValue = profileAddress.HomeTypeId.ToString(); }

					cbMultistory.Checked = profileAddress.IsMultistory;
					cbBasement.Checked = profileAddress.HasBasement;
					cbCarport.Checked = profileAddress.HasCarport;
					cbCrawlspace.Checked = profileAddress.HasCrawlspace;
					cbGarage.Checked = profileAddress.HasGarage;
					chkPrimaryResidence.Checked = profileAddress.IsPrimaryResidence;

					if(profileAddress.Bathrooms != null)
						txtBedrooms.Value = profileAddress.Bathrooms.ToString();

					if (profileAddress.Bedrooms != null)
						txtBathrooms.Value = profileAddress.Bedrooms.ToString();

					if (profileAddress.SquareFeet != null)
						txtSquareFeet.Value = profileAddress.SquareFeet.ToString();

				}
			}
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		string addressId = Request.QueryString["addressId"];
		string squareFeet = txtSquareFeet.Value;
		string bathrooms = txtBathrooms.Value;
		string bedrooms = txtBedrooms.Value;
		string isActive = !String.IsNullOrEmpty(rblIsActive.SelectedValue) ? rblIsActive.SelectedValue : "0";
		string rebuildStatusId = rblRebuildStatus.SelectedValue;
		string rebuildDescription = txtRebuildDescription.Text;
		string targetStartDate = hidTargetedStartDate.Value;
		Guid rebuildId = Guid.NewGuid();
		Guid rebuildRebuildStatusId = Guid.NewGuid(); //First entry into the rebuild status tracking table.

		var existingRebuild = (from r in dc.Rebuilds
							   where r.RebuildAddressId == new Guid(addressId)
							   select r).SingleOrDefault();

		if(existingRebuild == null)
		{
			//Add new rebuild, unless one exists for this address, then we need to just update it.
			Rebuild rebuild = new Rebuild();
			rebuild.CreatedBy = userId;
			rebuild.CreatedOn = DateTime.Now;
			rebuild.IsHidden = false;
			rebuild.IsOnHold = Convert.ToBoolean(Int32.Parse(isActive));
			rebuild.RebuildAddressId = new Guid(addressId);
			rebuild.RebuildDescription = rebuildDescription.Replace("\r", "<br\\>").Replace("\n", "<br\\>").Replace("\r\n", "<br\\>");
			rebuild.RebuildId = rebuildId;
			if (!String.IsNullOrEmpty(targetStartDate))
			{
				DateTime startDate = Convert.ToDateTime(targetStartDate);
				rebuild.RebuildStartDate = startDate;
			}
			if (!String.IsNullOrEmpty(hidTaskType.Value))
			{
				rebuild.Difficulty = Int32.Parse(hidTaskType.Value);
			}
			rebuild.SurvivorId = new Guid(_userId);
			dc.Rebuilds.InsertOnSubmit(rebuild);
		}
		else
		{
			//Update rebuild information.
			rebuildId = existingRebuild.RebuildId;
			existingRebuild.IsOnHold = Convert.ToBoolean(Int32.Parse(isActive));
			existingRebuild.RebuildDescription = rebuildDescription.Replace("\r", "<br\\>").Replace("\n", "<br\\>").Replace("\r\n", "<br\\>");
			if (!String.IsNullOrEmpty(targetStartDate))
			{
				DateTime startDate = Convert.ToDateTime(targetStartDate);
				existingRebuild.RebuildStartDate = startDate;
			}
			if (!String.IsNullOrEmpty(hidTaskType.Value))
			{
				existingRebuild.Difficulty = Int32.Parse(hidTaskType.Value);
			}
			dc.SubmitChanges();
		}

		//Update the profile/Address
		var profileAddress = (from lp in dc.ProfileAddresses
								where lp.AddressId == new Guid(addressId)
								select lp).SingleOrDefault();

		profileAddress.ShowOnAgencyMap = chkIsOnMap.Checked;
		profileAddress.ShowOnCleanupMap = chkIsOnCleanupMap.Checked;
		profileAddress.IsMultistory = cbMultistory.Checked;
		profileAddress.HasBasement = cbBasement.Checked;
		profileAddress.HasCarport = cbCarport.Checked;
		profileAddress.HasCrawlspace = cbCrawlspace.Checked;
		profileAddress.HasGarage = cbGarage.Checked;
		profileAddress.IsPrimaryResidence = chkPrimaryResidence.Checked;
		string ownRentId = rblOwnRent.SelectedValue;
		string homeTypeId = rblHomeType.SelectedValue;
		profileAddress.HomeRelationshipOwnRentTypeId = new Guid(ownRentId);
		profileAddress.HomeTypeId = new Guid(homeTypeId);
		if (!String.IsNullOrEmpty(squareFeet))
		{ 
			profileAddress.SquareFeet = Int32.Parse(squareFeet);
		}
		if (!String.IsNullOrEmpty(bathrooms))
		{
			profileAddress.Bathrooms = Int32.Parse(bathrooms);
		}
		if (!String.IsNullOrEmpty(bedrooms))
		{
			profileAddress.Bedrooms = Int32.Parse(bedrooms);
		}

		dc.SubmitChanges();

		//Get profile number
		var profile = (from p in dc.Profiles
					  where p.UserId == profileAddress.ProfileId
					  select p).SingleOrDefault();

		int profileNumber = profile.ProfileNumber;

		//Update selected partners.

		//Tracking where we are in the rebuild process.
		if (!String.IsNullOrEmpty(rebuildStatusId))
		{
			RebuildRebuildStatus rebuildRebuildStatus = new RebuildRebuildStatus();
			rebuildRebuildStatus.CreatedBy = userId;
			rebuildRebuildStatus.CreatedOn = DateTime.Now;
			rebuildRebuildStatus.RebuildRebuildStatusId = rebuildRebuildStatusId;
			rebuildRebuildStatus.RebuildId = rebuildId;
			rebuildRebuildStatus.RebuildStatusId = new Guid(rebuildStatusId);
			dc.RebuildRebuildStatus.InsertOnSubmit(rebuildRebuildStatus);
			dc.SubmitChanges();
		}

		Response.Redirect("/case/" + profileNumber + "/" + profile.Firstname + "_" + profile.Lastname);

		////How do we get the organization associated with this person?
		////Show the organization that the case manager works for on their case page.
		////Add a way to track ALL organizations collaborating on this case.

		//var userOrganization = (from uo in dc.UserOrganizations
		//						where uo.UserId == userId
		//						select new { uo.OrganizationId }).Take(1).SingleOrDefault();

		////TODO: Need to create a way for the user to be associated with only organization for case management.
		//OrganizationRebuild organizationRebuild = new OrganizationRebuild();
		//organizationRebuild.CreatedOn = DateTime.Now;
		//organizationRebuild.IsPrimaryOrganization = true;
		//organizationRebuild.OrganizationId = organizationId;
		//organizationRebuild.OrganizationRebuildId = userOrganization.OrganizationId;
		//organizationRebuild.RebuildId = rebuildId;
		//dc.OrganizationRebuilds.InsertOnSubmit(organizationRebuild);
		//dc.SubmitChanges();

		//foreach (ListItem listItem in dllSelectCollaborators.Items)
		//{
		//	if (listItem.Selected == true)
		//	{
		//		Guid selectedOrganizationId = new Guid(listItem.Value);

		//		if (organizationId != selectedOrganizationId)
		//		{
		//			OrganizationRebuild collaboratingOrganizationRebuild = new OrganizationRebuild();
		//			collaboratingOrganizationRebuild.CreatedOn = DateTime.Now;
		//			collaboratingOrganizationRebuild.IsPrimaryOrganization = false;
		//			collaboratingOrganizationRebuild.OrganizationId = selectedOrganizationId;
		//			collaboratingOrganizationRebuild.OrganizationRebuildId = Guid.NewGuid();
		//			collaboratingOrganizationRebuild.RebuildId = rebuildId;
		//			dc.OrganizationRebuilds.InsertOnSubmit(collaboratingOrganizationRebuild);
		//			dc.SubmitChanges();
		//		}
		//	}
		//}
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/CaseManagement/Survivor.aspx?userId=" + _userId);
	}

	protected void LoadCollaborators()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var collaboratingOrganizations = from o in dc.Organizations
										 where o.IsActive ==  true
										 orderby o.Name
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


		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var homeType = from rs in dc.RebuildStatus
							where rs.StatusType == 4
							orderby rs.OrderBy
							select new { rs.Status, rs.RebuildStatusId };
		rblHomeType.DataSource = homeType;
		rblHomeType.DataBind();

		var ownRent = from rs in dc.RebuildStatus
							 where rs.StatusType == 6
							 orderby rs.OrderBy
							 select new { rs.Status, rs.RebuildStatusId };

		rblOwnRent.DataSource = ownRent;
		rblOwnRent.DataBind();
	}
}