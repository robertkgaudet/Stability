using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_DisasterPrograms : BaseWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var programs = from p in dc.Programs
					   join op in dc.OrganizationPrograms on p.ProgramId equals op.ProgramId
					   join o in dc.Organizations on op.OrganizationId equals o.OrganizationId
					   where p.IsShared == true
					   orderby p.Order
					   select new { op.OrganizationId, p.Name, p.Description, o.Logo, p.IsDeploymentRequired, p.IsRemoteOnly, p.IsTrainingRequired, p.IsShared, p.ProgramId };

		rptPrograms.DataSource = programs;
		rptPrograms.DataBind();

		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		Master.PageTitle = "Deploy Your Team With A Disaster Relief Program From Stability";
		Master.PageDescription = "Deploy your team with a prebuilt disaster relief program in your community.";
		Master.FbDescription = "Deploy your team with a prebuilt disaster relief program in your community.";
		Master.FbImage = causePhotoFolder + "slide11.png";
		Master.FbImageType = "image/png";
		Master.FbSite_name = "Deploy Your Team With A Disaster Relief Program From Stability";
		Master.FbURL = Request.Url.AbsoluteUri;
	}

	protected void rptPrograms_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			string programName = (string)DataBinder.Eval(dataItem.DataItem, "Name");
			string programDescription = (string)DataBinder.Eval(dataItem.DataItem, "Description");
			string logo = (string)DataBinder.Eval(dataItem.DataItem, "Logo");
			bool isDeploymentRequired = (bool)DataBinder.Eval(dataItem.DataItem, "IsDeploymentRequired");
			bool isRemoteOnly = (bool)DataBinder.Eval(dataItem.DataItem, "IsRemoteOnly");
			bool isTrainingRequired = (bool)DataBinder.Eval(dataItem.DataItem, "IsTrainingRequired");
			bool isShared = (bool)DataBinder.Eval(dataItem.DataItem, "IsShared");
			Guid programId = (Guid)DataBinder.Eval(dataItem.DataItem, "ProgramId");
			Guid organizationId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationId"); 

			Literal litProgramName = (Literal)e.Item.FindControl("litProgramName");
			Literal litProgramDescription = (Literal)e.Item.FindControl("litProgramDescription");
			Literal litRemoteWork = (Literal)e.Item.FindControl("litRemoteWork");
			Literal litRequiresDeployment = (Literal)e.Item.FindControl("litRequiresDeployment");
			Literal litRequiresTraining = (Literal)e.Item.FindControl("litRequiresTraining");
			Literal litSharedPrivate = (Literal)e.Item.FindControl("litSharedPrivate");
			ImageButton imgLogo = (ImageButton)e.Item.FindControl("imgLogo");

			string sharedIcon = string.Empty;
			litSharedPrivate.Text = "";// "<span class=\"label label-danger pull-right\">PRIVATE PROGRAM</span>";
			if (isShared)
			{
				sharedIcon = "<i class=\"fa fa-paper-plane\"></i>";
				litSharedPrivate.Text = "<span class=\"label label-success pull-right\">GLOBAL PROGRAM</span>";
			}
			HyperLink hypEditPrograms = (HyperLink)e.Item.FindControl("hypEditPrograms");

			litProgramDescription.Text = programDescription;
			litProgramName.Text =  programName + " " + sharedIcon;
			litRemoteWork.Text = isRemoteOnly ? "Remote work requred." : "No remote work.";
			litRequiresDeployment.Text = isDeploymentRequired ? "Requires deployment." : "Deployment not required.";
			litRequiresTraining.Text = isTrainingRequired ? "Specialized training is required." : "Training not required.";
			imgLogo.ImageUrl = "/Impactoid/Images/Logos/" + logo;
			imgLogo.CommandArgument = organizationId.ToString();

			if (User.IsInRole("Administrator"))
			{
				hypEditPrograms.Visible = true;
				hypEditPrograms.NavigateUrl = "/V1/NonProfitAdministration/EditNonProfitProgram.aspx?organizationId=" + organizationId + "&programId=" + programId;
			}
		}
	}
	protected void imgLogo_Click(object sender, ImageClickEventArgs e)
	{
		ImageButton button = (ImageButton)sender;
		string parameter = button.CommandArgument;

		Response.Redirect("/V1/NonProfit/Default.aspx?organizationId=" + parameter);
	}
}