using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class V1_Profile_ChooseEvent : BaseOrganizationWebForm
{
	public string rebuildProgressSliderId = ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
	protected void Page_Load(object sender, EventArgs e)
	{

		if (User.IsInRole("Administrator"))
		{
			hypNewDisaster.Visible = true;
		}

		///Get a list of the disasters and display them.

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var pastDisasters = from d in dc.Events
							where d.IsDisaster == true
							orderby d.BeginDate descending
							select d;

		dlDisasterPortal.DataSource = pastDisasters;
		dlDisasterPortal.DataBind();

		litEventDescription.Text = "Select a Community Based Disaster Portal";
		string action = Request.QueryString["action"];
		if(!string.IsNullOrEmpty(action))
		{
			if(action == "volunteer")
			{
				litEventDescription.Text = "Select a Community Based Disaster Portal";
			}
			else if(action =="rebuild")	
			{
				litEventDescription.Text = "Select a Community Based Disaster Portal";
			}
		}
	}

	protected void dlDisasterPortal_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			HyperLink hypName = (HyperLink)e.Item.FindControl("hypName");
			HyperLink hypEdit = (HyperLink)e.Item.FindControl("hypEdit");
			HyperLink hypMap = (HyperLink)e.Item.FindControl("hypMap"); 
			Literal litStatus = (Literal)e.Item.FindControl("litStatus");
			Literal litEventDate = (Literal)e.Item.FindControl("litEventDate");
			Literal litStates = (Literal)e.Item.FindControl("litStates");
			Literal litSimulation = (Literal)e.Item.FindControl("litSimulation"); 
			HtmlGenericControl iSimulation = (HtmlGenericControl)e.Item.FindControl("iSimulation");

			//Total count of items and total cost.
			Guid eventId = (Guid)DataBinder.Eval(dataItem.DataItem, "EventId");
			DateTime? beginDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "BeginDate");
			string disasterColor = (string)DataBinder.Eval(dataItem.DataItem, "Color");
			bool isActive = (bool)DataBinder.Eval(dataItem.DataItem, "IsActive");
			bool isSimulation = DataBinder.Eval(dataItem.DataItem, "IsSimulation") == null ? false : (bool)DataBinder.Eval(dataItem.DataItem, "IsSimulation");
			string name = (string)DataBinder.Eval(dataItem.DataItem, "Name");
			string URLFriendlyName = (string)DataBinder.Eval(dataItem.DataItem, "URLFriendlyName");

			string rebuildTickLabel = string.Empty;
			string rebuildProgress = GetPercent(true, eventId, Guid.Empty, new Guid(rebuildProgressSliderId), out rebuildTickLabel).ToString() + "%";

			string headerColor = string.Empty;
			if (disasterColor != null)
			{
				headerColor = "h" + CrowdRelief.Tools.GetColor(disasterColor);
			}

			if (isActive)
			{
				rebuildProgress = "<h6 style=\"margin:0px;\"><i class=\"fa fa-bolt\"></i> ACTIVE</h6>" + Environment.NewLine;
			}
			else
			{
				rebuildProgress = "<h6 style=\"margin:0px;\"><i class=\"fa fa-tachometer\"></i> RECOVERY " + rebuildProgress + "</h6>";
			}

			if(isSimulation)
			{
				iSimulation.Visible = true;
				litSimulation.Text = "Simulation Only";
			}

			hypMap.Text = "Maps";
			hypMap.NavigateUrl = "/Maps/" + URLFriendlyName;
			if(User.IsInRole("Administrator"))
			{ 
			hypEdit.Text = "Edit";
			hypEdit.NavigateUrl = "/V1/Administration/NewDisaster.aspx?eventId=" + eventId;
			}
			hypName.Text = name;
			hypName.NavigateUrl = "/Disaster/" + URLFriendlyName;

			litStatus.Text = rebuildProgress;
			litEventDate.Text = String.Format("{0:Y}", beginDate);
			litStates.Text = CrowdRelief.Tools.GetImpactedStateCountyString(eventId);
		}
	}

	//protected string CreateDisasterPanels(IOrderedQueryable<Event> disasters)
	//{
	//	string disasterPanel = string.Empty;

	//	foreach(Event disaster in disasters)
	//	{	
	//		//string icon = string.Empty;
	//		string headerColor = string.Empty;
	//		string eventDate = String.Format("{0:Y}", disaster.BeginDate);
	//		string color = disaster.Color;
			
	//		string rebuildTickLabel = string.Empty;
	//		//string volunteersNeeded = CalculateVolunteersNeeded(Guid.Empty, 0, false, disaster.EventId);
	//		string rebuildProgress = GetPercent(true, disaster.EventId, Guid.Empty, new Guid(rebuildProgressSliderId), out rebuildTickLabel).ToString() + "%";

	//		//if(disaster.Icon != null)
	//		//{
	//		//	icon = disaster.Icon.Replace("COLOR", "btn-" + color + " btn-outline");
	//		//}

	//		if(disaster.Color != null)
	//		{
	//			headerColor = "h" + CrowdRelief.Tools.GetColor(disaster.Color);
	//		}

	//		string activeCSS = string.Empty;
	//		string activeHeader = string.Empty;
			
	//		string tickLabel = string.Empty;
	//		string overallPercent = string.Empty;

	//		if(disaster.IsActive)
	//		{
	//			activeCSS = "activeDisasterPanel";
				
	//			activeHeader =	 "<div class=\"alert alert-danger p-xs\">" + Environment.NewLine +
	//								"<h6 style=\"margin:0px;\"><i class=\"fa fa-bolt\"></i> ACTIVE</h6>" + Environment.NewLine +
	//							"</div>" + Environment.NewLine;
	//			headerColor = "hblue";
	//		}
	//		else
	//		{
	//			activeCSS = "disasterPanel";
	//			//string overallProgressSliderId = ConfigurationManager.AppSettings["overallProgressSliderId"].ToString();
	//			//overallPercent = GetPercent(true, disaster.EventId, Guid.Empty, new Guid(overallProgressSliderId), out tickLabel).ToString();
	//			activeHeader = "<div class=\"alert alert-warning p-xs\">" + Environment.NewLine +
	//								"<h6 style=\"margin:0px;\"><i class=\"fa fa-tachometer\"></i> RECOVERY " + rebuildProgress + "</h6>" + Environment.NewLine +
	//							"</div>" + Environment.NewLine;
	//			headerColor = "horange";
	//		}

	//		//CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
	//		//int homeCount = (from c in dc.Rebuilds
	//		//				where c.EventId == disaster.EventId
	//		//				select c).Count();

	//		//disasterPanel += "<div class=\"grid-item\" onclick=\"window.location.href='/V1/Event.aspx?eventId=" + disaster.EventId + "';\">" +  Environment.NewLine + Environment.NewLine +
	//		color = string.Empty;
	//		disasterPanel += "<div class=\"grid-item\" onclick=\"window.location.href='/Disaster/" + disaster.URLFriendlyName + "';\">" +  Environment.NewLine + Environment.NewLine +
	//							//"<div class=\"hpanel " + headerColor + "\">" + Environment.NewLine +
	//							"<div class=\"hpanel " + headerColor + "\">" + Environment.NewLine +
	//								"<div class=\"panel-body " + activeCSS + "\">" + Environment.NewLine +
	//									"<div class=\"text-center\">" + Environment.NewLine +
	//										"<p class=\"m-b-xs text-" + color + "\"><small>" + eventDate + "</small></p>" + Environment.NewLine +
	//										"<div><h4 class=\"font-bold\">" + disaster.Name + "</h4></div>" + Environment.NewLine +
	//									//"<div class=\"m\"><h4>" + Environment.NewLine +
	//									//	icon + Environment.NewLine +
	//									//"</h4></div>" + Environment.NewLine +
	//									//"<br><div class=\"badge m-t-md badge-info\"><h4>" + homeCount + " Homes</h4></div>" + Environment.NewLine +
	//									//"<br><div class=\"badge m-t-md badge-info\">" + volunteersNeeded + " Volunteers Needed</div>" + Environment.NewLine + 
	//									//"<p class=\"small p-sm\">" + Environment.NewLine +
	//									//	disaster.Description + Environment.NewLine +
	//									//"</p>" + Environment.NewLine +
	//									"</div>" + Environment.NewLine +
	//								"</div>" + activeHeader + Environment.NewLine +
	//							"</div>" + Environment.NewLine +
	//						"</div>" + Environment.NewLine + Environment.NewLine;
	//		}

	//	return disasterPanel;
	//}
}