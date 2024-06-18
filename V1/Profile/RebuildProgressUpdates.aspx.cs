using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Profile_RebuildProgressUpdates : BaseOrganizationWebForm
{
	public string javascriptSliderCode = string.Empty;
	public string rebuildId = string.Empty;
	public string survivorId = string.Empty;
	public string rebuildProgressSliderId = ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
	public string progressEstimateSliderId = ConfigurationManager.AppSettings["progressEstimateSliderId"].ToString();
	public string recoveryStageId = ConfigurationManager.AppSettings["RecoveryStageId"].ToString();

	protected void Page_Load(object sender, EventArgs e)
	{
		string sliderJavascript = string.Empty;
		string sliderHTML = string.Empty;
		rebuildId = Request["rebuildId"];
		survivorId = Request["survivorId"];

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var rebuild = (from r in dc.Rebuilds
					  where r.RebuildId == new Guid(rebuildId)
					  select new { r.RebuildId }).SingleOrDefault();

		hypRebuildProgressUpdates.NavigateUrl = "Rebuild.aspx?rebuildId=" + rebuild.RebuildId;

		javascriptSliderCode = CreateProgressSlider(new Guid(rebuildProgressSliderId), "RebuildProgressSlider", out sliderJavascript, out sliderHTML);
		javascriptSliderCode += CreateProgressSlider(new Guid(progressEstimateSliderId), "EstimatedRebuildProgressSlider", out sliderJavascript, out sliderHTML);
		javascriptSliderCode += CreateProgressSlider(new Guid(recoveryStageId), "RecoveryStage", out sliderJavascript, out sliderHTML);
	}

	protected string CreateProgressSlider(Guid sliderId, string identifer, out string sliderJavascript, out string sliderHTML)
	{
		sliderJavascript = string.Empty;
		sliderHTML = string.Empty;

		string ticks = string.Empty;
		string tickLables = string.Empty;
		int tickCount = 0;
		int tickValue = 0;
		string sliderName = string.Empty;
		string sliderDescription = string.Empty;
		int tickCounter = 1;
		string results = string.Empty;
		string identiferVariable = "var" + identifer;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var sliderTicks = from st in dc.SliderTicks
						  where st.SliderId == sliderId
						  && st.IsActive == true
						  orderby st.Tick
						  select st;

		tickCount = sliderTicks.Count();

		foreach (var sliderTick in sliderTicks)
		{
			ticks += sliderTick.Tick + ",";
			tickLables += "\"" + sliderTick.Lable.Replace(" ", "<br/>") + "\",";
			tickCounter = tickCounter + 1;
		}

		var slider = (from s in dc.Sliders
					  where s.SliderId == sliderId
					  select s).SingleOrDefault();

		//Get the latest slider value.
		var sliderProgress = (from ust in dc.UserSliderTicks
							  join st in dc.SliderTicks on ust.SliderTickId equals st.SliderTickId
							  where ust.SurvivorId == new Guid(survivorId)
							  && ust.RebuildId == new Guid(rebuildId)
							  && ust.SliderId == sliderId
							  && st.IsActive == true
							  orderby ust.CreatedOn descending
							  select new { st.Tick }).Take(1).SingleOrDefault();

		sliderName = slider.Name;
		sliderDescription = slider.Description;

		if (sliderProgress != null)
		{
			tickValue = sliderProgress.Tick;
		}

		//results = "\r\n\r	<script type=\"text/javascript\">";
		//results += "\r\n\r	$(document).ready(function () { ";

		results += " \r\n\r\n		$(\"#" + identifer + "\").slider({ \r\n" +
							"				ticks:[" + ticks.TrimEnd(',') + "], \r\n" +
							"				ticks_labels:[" + tickLables.TrimEnd(',') + "], \r\n" +
							"				min:1, \r\n" +
							"				max:" + tickCount + ", \r\n" +
							"				value:" + tickValue + ", \r\n" +
							"				step:1, \r\n" +
							"				id:'" + sliderId + "' \r\n" +
							"		});";

		results += "\r\n\r\n		$(\"." + identifer + "\").slider().on('slideStop', function(ev) { \r\n" +
							"				" + identiferVariable + " = $('." + identifer + "').data('slider').getValue(); \r\n" +
							"				updateRebuildProgressSlider(" + identiferVariable + ", '" + sliderId.ToString() + "', '" + rebuildId + "', '" + survivorId + "', '" + userId + "'); \r\n" +
							"		}); \r\n\r";

		//results += "	}); \r\n";
		//results += "	</script> \r\n\r";

		//results +=	"\r\n\r	<div class=\"center-block p-l m-md m-b-xl\"> \r\n" +
		//			"			<div class=\"m-b-lg\"> \r\n" +
		//			"				<h1>" + sliderName + "</h1> \r\n" +
		//			"				" + sliderDescription + " \r\n" +
		//			"			</div> \r\n" +
		//			"			<input id=\"" + identifer + "\" type=\"text\" class=\"" + identifer + "\" /> \r\n" +
		//			"	</div> \r\n\r";


		return results;
	}
}