using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class CaseManagement_Stages : System.Web.UI.Page
{
	public string recoveryStageId = ConfigurationManager.AppSettings["recoveryStageId"].ToString();

	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var stages = from s in dc.SliderTicks
					 where s.SliderId == new Guid(recoveryStageId)
					 orderby s.Tick
					 select s;

		rptStages.DataSource = stages;
		rptStages.DataBind();
	}

	protected void rptStages_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litStageLabel = (Literal)e.Item.FindControl("litStageLabel");
			Literal litStageDescription = (Literal)e.Item.FindControl("litStageDescription");
			Literal litRecentPost = (Literal)e.Item.FindControl("litRecentPost");
			Literal litTick = (Literal)e.Item.FindControl("litTick");

			string sliderLabel = (string)DataBinder.Eval(dataItem.DataItem, "Lable");
			string description = (string)DataBinder.Eval(dataItem.DataItem, "Description");
			int tick = (int)DataBinder.Eval(dataItem.DataItem, "Tick");
			Guid sliderTickId = (Guid)DataBinder.Eval(dataItem.DataItem, "SliderTickId");

			litStageDescription.Text = description;
			litStageLabel.Text = sliderLabel;
			litRecentPost.Text = String.Empty;
			litTick.Text = tick.ToString();
		}
	}
}