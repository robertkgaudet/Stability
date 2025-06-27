using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class S1_Profile_Survey_BasicNeeds : System.Web.UI.Page
{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack && User.Identity.IsAuthenticated)
			{
				Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
				CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
				//Does this user already have a survey? If so send them to their profile page.

				var duplicateSurveyCheck = from bns in dc.BasicNeedsSurveys
										   where bns.CreatedBy == userId
										   select bns;

				if(duplicateSurveyCheck.Count() > 0)
				{
					//Redirect, they already have a survey.
					//Response.Redirect("/Account/SurvivorProfile.aspx");
				}

				var organization = from uo in dc.UserOrganizations
								   join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
								   where uo.UserId == userId && (uo.Status == (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
                                   select o;

				if (organization.Count() > 0)
				{
					if (organization.Count() == 1)
					{
						lblOrganization.Text = organization.SingleOrDefault().Name;
						hidOrganization.Value = organization.SingleOrDefault().OrganizationId.ToString();
						ddlOrganization.Visible = false;
					}
					else
					{
						lblOrganization.Visible = false;
						ddlOrganization.DataSource = organization;
						ddlOrganization.DataBind();
						ddlOrganization.Items.Insert(0, new ListItem("-- Select --", "-- Select --"));
					}
				}
				else
				{
					//Have them select the Cajun Relief Foundation.
					var defaultOrganization =	(from o in dc.Organizations
											   where o.OrganizationId == new Guid("79305F85-3816-46A8-911F-0D7E3E227C32")
											   select o).SingleOrDefault();

					lblOrganization.Text = defaultOrganization.Name;
					hidOrganization.Value = defaultOrganization.OrganizationId.ToString();
					ddlOrganization.Visible = false;
				}
			}
		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{
			if (User.Identity.IsAuthenticated)
			{
				Guid userId				= new Guid(Membership.GetUser().ProviderUserKey.ToString());
				DateTime requestTime	= DateTime.Now;
				Guid basicNeedsSurveyId = Guid.NewGuid();

				CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
				
				string story = txtStory.Text;
				if (!String.IsNullOrEmpty(story))
				{
					story = story.Replace(System.Environment.NewLine, "<br>");
				}

				string help = txtHelp.Text;
				if (!String.IsNullOrEmpty(help))
				{
					help = help.Replace(System.Environment.NewLine, "<br>");
				}

				//Add BasicNeedsSurvey Info
				bool isHousingNeeded = false;
				string housingNeeded = rblHousingNeed.SelectedValue;
				if(housingNeeded == "Yes")
				{
					isHousingNeeded = true;
				}


				bool isRebuildRepairNeeded = false;
				string rebuildRepairNeeded = rblRebuildRepair.SelectedValue;
				if (housingNeeded == "Yes")
				{
					isRebuildRepairNeeded = true;
				}

				bool isSupportSystemProvided = false;
				string supportSystemProvided = rblSupportSystem.SelectedValue;
				if (supportSystemProvided == "Yes")
				{
					isSupportSystemProvided = true;
				}

				string organizationId = string.Empty;

				if(!String.IsNullOrEmpty(hidOrganization.Value))
				{
					organizationId = hidOrganization.Value;
				}
				else
				{
					organizationId = ddlOrganization.SelectedValue;
				}

				Guid organizationGuid = new Guid(organizationId);

				BasicNeedsSurvey basicNeedsSurvey = new BasicNeedsSurvey();
				basicNeedsSurvey.CreatedBy = userId;
				basicNeedsSurvey.CreatedOn = DateTime.Now;
				basicNeedsSurvey.EmailAddress = txtEmail.Text;

				if(!string.IsNullOrEmpty(txtFemaNumber.Text))
					basicNeedsSurvey.FEMANumber = txtFemaNumber.Text;

				basicNeedsSurvey.OrganizationId = organizationGuid;
				basicNeedsSurvey.HelpDetails = help;
				basicNeedsSurvey.Story = story;
				basicNeedsSurvey.Hidden = false;
				basicNeedsSurvey.FirstName = txtFirstname.Text;
				basicNeedsSurvey.LastName = txtLastname.Text;
				basicNeedsSurvey.HomeLossAddress = txtLossAddress.Text;
				basicNeedsSurvey.HomeLossCity = txtLossCity.Text;
				basicNeedsSurvey.HomeLossParishCounty = txtLossCountyParish.Text;
				basicNeedsSurvey.HomeLossState = ddlLossState.SelectedValue;
				basicNeedsSurvey.HomeLossZipCode = txtZipCode.Text;
				if(!String.IsNullOrEmpty(txtBathrooms.Text))
				{ 
					basicNeedsSurvey.NumberOfBathrooms = Convert.ToInt32(txtBathrooms.Text);
				}
				if (!String.IsNullOrEmpty(txtBedrooms.Text))
				{
					basicNeedsSurvey.NumberOfBedrooms = Convert.ToInt32(txtBedrooms.Text);
				}
				if (!String.IsNullOrEmpty(txtPeopleInHome.Text))
				{
					basicNeedsSurvey.NumberOfPeopleAffected = Convert.ToInt32(txtPeopleInHome.Text);
				}
				basicNeedsSurvey.PhoneNumber = txtPhoneNumber.Text;

				basicNeedsSurvey.ImmediateHousingNeeded = isHousingNeeded;
				basicNeedsSurvey.RebuildRepairNeeded = isRebuildRepairNeeded;
				basicNeedsSurvey.SupportAlreadyBeingProvided = isSupportSystemProvided;

				basicNeedsSurvey.SurveyId = basicNeedsSurveyId;
				dc.BasicNeedsSurveys.InsertOnSubmit(basicNeedsSurvey);
				dc.SubmitChanges();

				if(cbClothing.Checked)
				{
					Guid clothingNeededGuid = new Guid(cbClothing.Attributes["guid"].ToString());

					Guid basicNeedsSurveyItemId = Guid.NewGuid();
					// If the item is selected, add the value to the list.
					BasicNeedsSurveyItem basicNeedsSurveyItem = new BasicNeedsSurveyItem();
					basicNeedsSurveyItem.BasicSurveyItemId = basicNeedsSurveyItemId;
					basicNeedsSurveyItem.SurveyId = basicNeedsSurveyId;
					basicNeedsSurveyItem.Count = 1;
					basicNeedsSurveyItem.ItemId = clothingNeededGuid;
					dc.BasicNeedsSurveyItems.InsertOnSubmit(basicNeedsSurveyItem);
					dc.SubmitChanges();
				}
				
				//INSERT INTO THE BASICSURVEYITEMS TABLE
				//Text Boxes
				UpdateCountedItems(txtSheetRock.Attributes["guid"],		txtSheetRock.Text, dc, basicNeedsSurveyId);
				UpdateCountedItems(txtInsulation.Attributes["guid"],	txtInsulation.Text, dc, basicNeedsSurveyId);
				UpdateCountedItems(txtSingleBeds.Attributes["guid"],	txtSingleBeds.Text, dc, basicNeedsSurveyId);
				UpdateCountedItems(txtDoubleBeds.Attributes["guid"],	txtDoubleBeds.Text, dc, basicNeedsSurveyId);
				UpdateCountedItems(txtQueenBeds.Attributes["guid"],		txtQueenBeds.Text, dc, basicNeedsSurveyId);
				UpdateCountedItems(txtDressers.Attributes["guid"],		txtDressers.Text, dc, basicNeedsSurveyId);
				UpdateCountedItems(txtBedFrames.Attributes["guid"],		txtBedFrames.Text, dc, basicNeedsSurveyId);

				//Radio Button Lists
				//GET AND INSERT each of the radio items checked.
				UpdateRadioItems(rblGroupWaterHeater.SelectedValue, dc, basicNeedsSurveyId);
				UpdateRadioItems(rblDryer.SelectedValue, dc, basicNeedsSurveyId);
				UpdateRadioItems(rblStoveTop.SelectedValue, dc, basicNeedsSurveyId); 

				//Check Box Lists
				//GET AND INSERT each of the items checked.
				UpdateCheckedItems(cblAppliances, dc, basicNeedsSurveyId);
				UpdateCheckedItems(cblFurniture, dc, basicNeedsSurveyId);
				UpdateCheckedItems(cblLinen, dc, basicNeedsSurveyId);
				UpdateCheckedItems(cblUtensils, dc, basicNeedsSurveyId); 
				UpdateCheckedItems(cblWindowTreatments, dc, basicNeedsSurveyId); 

				 //GET THE SURVEY NUMBER.
				 var basicNeedsSurveyQuery = (from bns in dc.BasicNeedsSurveys
								 where bns.SurveyId == basicNeedsSurveyId
								 select bns).SingleOrDefault();

				lblNumber.Text = basicNeedsSurveyQuery.SurveyNumber.ToString();

				divFormFields.Visible = false;
				divResults.Visible = true;

				var profile = (from p in dc.Profiles
							   where p.UserId == userId
							   select p).SingleOrDefault();

				lblTimeName.Text = "Requested by: " + profile.Firstname + " " + profile.Lastname + " on " + requestTime;

			}
		}

		protected void UpdateCountedItems(string itemGuid, string itemCount, CrowdReliefDBDataContext dc, Guid basicNeedsSurveyId)
		{
			int result;
			if (!String.IsNullOrEmpty(itemGuid) && int.TryParse(itemCount, out result))
			{
				// The string was a valid integer => use result here
				Guid basicNeedsSurveyItemId = Guid.NewGuid();
				Guid itemId = new Guid(itemGuid);
				BasicNeedsSurveyItem basicNeedsSurveyItem = new BasicNeedsSurveyItem();
				basicNeedsSurveyItem.BasicSurveyItemId = basicNeedsSurveyItemId;
				basicNeedsSurveyItem.SurveyId = basicNeedsSurveyId;
				basicNeedsSurveyItem.Count = int.Parse(itemCount);
				basicNeedsSurveyItem.ItemId = itemId;
				dc.BasicNeedsSurveyItems.InsertOnSubmit(basicNeedsSurveyItem);
				dc.SubmitChanges();
			}
		}

		protected void UpdateRadioItems(string selectedItemValue,  CrowdReliefDBDataContext dc, Guid basicNeedsSurveyId)
		{
			if(!string.IsNullOrEmpty(selectedItemValue))
			{
				//Update the db.
				Guid basicNeedsSurveyItemId = Guid.NewGuid();
				Guid itemId = new Guid(selectedItemValue);
				BasicNeedsSurveyItem basicNeedsSurveyItem = new BasicNeedsSurveyItem();
				basicNeedsSurveyItem.BasicSurveyItemId = basicNeedsSurveyItemId;
				basicNeedsSurveyItem.SurveyId = basicNeedsSurveyId;
				basicNeedsSurveyItem.Count = 1;
				basicNeedsSurveyItem.ItemId = itemId;
				dc.BasicNeedsSurveyItems.InsertOnSubmit(basicNeedsSurveyItem);
				dc.SubmitChanges();
			}
		}

		//Insert the checked items.
		protected void UpdateCheckedItems(CheckBoxList cbl, CrowdReliefDBDataContext dc, Guid basicNeedsSurveyId)
		{
			List<String> cblList = new List<string>();
			foreach (ListItem item in cbl.Items)
			{
				if (item.Selected)
				{
					Guid basicNeedsSurveyItemId = Guid.NewGuid();
					Guid itemId = new Guid(item.Value);
					// If the item is selected, add the value to the list.
					//cblList.Add(item.Value);
					BasicNeedsSurveyItem basicNeedsSurveyItem = new BasicNeedsSurveyItem();
					basicNeedsSurveyItem.BasicSurveyItemId = basicNeedsSurveyItemId;
					basicNeedsSurveyItem.SurveyId = basicNeedsSurveyId;
					basicNeedsSurveyItem.Count = 1;
					basicNeedsSurveyItem.ItemId = itemId;
					dc.BasicNeedsSurveyItems.InsertOnSubmit(basicNeedsSurveyItem);
					dc.SubmitChanges();
				}
			}
		}
	}