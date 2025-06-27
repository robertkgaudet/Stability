using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_CountyInfo : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string stateCode = Request.QueryString["stateCode"];
		string countyId = Request.QueryString["countyId"];
		string eventId = Request.QueryString["eventId"];

		if (!IsPostBack)
		{
			if(User.IsInRole("Administrator"))
			{
				hypEditCounty.NavigateUrl = "/V1/Administration/EmergencyManagement.aspx?eventId=" + eventId + "&countyId=" + countyId + "&stateCode=" + stateCode;
				hypEditCounty.Visible = true;
			}

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var countyEmergencyManagementInformation = (from em in dc.EmergencyManagments
														join s in dc.USStates on em.StatesId equals s.StatesId
														where s.Code == stateCode
														&& em.CountyId == new Guid(countyId)
														select em).SingleOrDefault();

			if (countyEmergencyManagementInformation != null)
			{
				litWebsite.Text = countyEmergencyManagementInformation.Website;
				litEOCName.Text = countyEmergencyManagementInformation.EOCName;
				litEMName.Text = countyEmergencyManagementInformation.EmergencyManagerName;
				litPhoneNumber.Text = countyEmergencyManagementInformation.PhoneNumber;

				divInfo.Visible = true;
				divInfoMessage.Visible = false;
			}
			else
			{
				//Redirect to the add screen.
				//
				divInfo.Visible = false;
				divInfoMessage.Visible = true;
				//Response.Redirect("/V1/Administration/EmergencyManagement.aspx?eventId=" + eventId + "&countyId= " + countyId + "&stateCode=" +  stateCode);
			}

			var county = (from c in dc.Counties
						  where c.CountyId == new Guid(countyId)
						  select c).SingleOrDefault();

			string countyOrParish = " County";
			if (stateCode.ToUpper() == "LA")
			{
				countyOrParish = " Parish";
			}

			var state = (from s in dc.USStates
						 where s.Code == stateCode
						 select s).SingleOrDefault();

			string countyName = county.Name + countyOrParish;
			lblStateInformation.Text = state.Name + ", " + countyName + ": Emergency Management Information for " + countyName + ".";
		}
	}
}