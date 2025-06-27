using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_TeamRole : BaseWebForm
{
	public string positionId;
	public string organizationId;
	public int linkDetailCounter = 0;
	public string videoId;
	public string positionName;
	public string programId = string.Empty;
	public string programName = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		organizationId		= Request.QueryString["organizationId"];
		positionId			= Request.QueryString["positionId"];
		programId = Request.QueryString["programId"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		if (!String.IsNullOrEmpty(programId))
		{
			hypProgramRoles.Text = "Back to Program";
			hypProgramRoles.NavigateUrl = "/V1/NonProfit/Program.aspx?programId=" + programId + "&organizationId=" + organizationId;

			Guid parsedProgramId = new Guid(programId); // Convert programId to Guid

			// Join and filter on ProgramId
			var positions = from p in dc.Positions
							join pp in dc.ProgramPositions on p.PositionId equals pp.PositionId
							where pp.ProgramId == parsedProgramId
							select new { p.Name, p.PositionId, pp.ProgramId, p.DeploymentStatus, p.VideoTrainingURL, p.CheckListText };

			dlPositions.DataSource = positions;
			dlPositions.DataBind();
		}
		else
		{
			hypProgramRoles.Text = "Back to Programs";
			hypProgramRoles.NavigateUrl = "/V1/NonProfit/Programs.aspx?organizationId=" + organizationId;
			var positions = from p in dc.Positions
							where p.OrganizationId == new Guid(organizationId)
							&& p.IsDeleted == false
							orderby p.Name
							select new { p.Name, p.PositionId, ProgramId = (Guid?)null, p.DeploymentStatus, p.VideoTrainingURL, p.CheckListText };

			dlPositions.DataSource = positions;
			dlPositions.DataBind();
		}

		if(!String.IsNullOrEmpty(programId))
		{
			programName = (from p in dc.Programs
						   where p.ProgramId == new Guid(programId)
						   select p.Name).SingleOrDefault();
			
			lblProgramName.Text = programName + " Program Roles";
		}

		LoadPositionDetails(positionId);
		LoadProgramsForPosition(positionId);
		LoadEventData(positionId);

		var isOwner = (from o in dc.Organizations
					   where o.OwnerId == userId && o.OrganizationId == new Guid(organizationId)
					   select o).Take(1).SingleOrDefault();

		var isTeamMember = (from uo in dc.UserOrganizations
					   where uo.UserId == userId && uo.OrganizationId == new Guid(organizationId) && (uo.Status== (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
                            select uo).Take(1).SingleOrDefault();

		if (User.IsInRole("Administrator") || isOwner != null || (isTeamMember != null && User.IsInRole("EOC-VettingTeam")))
		{
			divEditAdd.Visible = true;
		}

		var organization = (from o in dc.Organizations
						   where o.OrganizationId == new Guid(organizationId)
						   select o).SingleOrDefault();


		hypLogo.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + organizationId;
		if (!String.IsNullOrEmpty(organization.Logo))
		{
			imgLogo.Visible = true;
			imgLogo.ImageUrl = "/Impactoid/Images/Logos/" + organization.Logo;
			imgLogo.AlternateText = organization.Name + " Logo";
		}
		else
		{
			//Use placeholder image.imgLogo.Visible = true;
			imgLogo.ImageUrl = "/V1/Images/Logo-Placeholder.png";
			imgLogo.AlternateText = organization.Name + " Logo";
		}
		imgLogo.ImageUrl = "/Impactoid/Images/Logos/" + organization.Logo;
	}

	private void LoadEventData(string positionId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		// Query to join tables and fetch the required data
		//var eventData = from uoep in dc.UserOrganizationEventPositions
		//					join oep in dc.OrganizationEventPositions on uoep.OrganizationEventPositionId equals oep.OrganizationEventPositionId
		//					join oe in dc.OrganizationEvents on oep.OrganizationEventId equals oe.OrganizationEventId
		//					join e in dc.Events on oe.EventId equals e.EventId
		//					join profile in dc.Profiles on uoep.UserId equals profile.UserId
		//					where oep.PositionId == new Guid(positionId)
		//					group new { uoep, oep, e, profile } by new { e.Name, oep.DeploymentDate } into groupedData
		//					select new
		//					{
		//						EventName = groupedData.Key.Name,
		//						DeploymentDate = groupedData.Key.DeploymentDate,
		//						Users = groupedData.Select(g => new { g.profile.Firstname, g.profile.Lastname }).ToList()
		//					};

		var eventData = (from uoep in dc.UserOrganizationEventPositions
						 join oep in dc.OrganizationEventPositions on uoep.OrganizationEventPositionId equals oep.OrganizationEventPositionId
						 join oe in dc.OrganizationEvents on oep.OrganizationEventId equals oe.OrganizationEventId
						 join e in dc.Events on oe.EventId equals e.EventId
						 join profile in dc.Profiles on uoep.UserId equals profile.UserId
						 where oep.PositionId == new Guid(positionId)
						 group new { uoep, oep, oe, e, profile } by new { e.Name, oe.OrganizationEventId, oep.DeploymentDate } into groupedData
						 orderby groupedData.Key.DeploymentDate descending
						 select new
						 {
							 EventName = groupedData.Key.Name,
							 OrganizationEventId = groupedData.Key.OrganizationEventId,
							 DeploymentDate = groupedData.Key.DeploymentDate, // Leave as DateTime
							 Users = groupedData.Select(g => new { g.profile.Firstname, g.profile.Lastname, g.profile.UserId }).ToList()
						 }).ToList();

		var formattedEventData = eventData.Select(ed => new
		{
			ed.EventName,
			ed.OrganizationEventId,
			// Format DeploymentDate after data retrieval
			DeploymentDate = ed.DeploymentDate.HasValue ? ed.DeploymentDate.Value.ToString("D") : string.Empty,
			ed.Users
		}).ToList();

		// Bind the result to the Repeater
		rptEvent.DataSource = formattedEventData.ToList();
		rptEvent.DataBind();


		var userCountData = (from uoep in dc.UserOrganizationEventPositions
							 join oep in dc.OrganizationEventPositions on uoep.OrganizationEventPositionId equals oep.OrganizationEventPositionId
							 join oe in dc.OrganizationEvents on oep.OrganizationEventId equals oe.OrganizationEventId
							 join e in dc.Events on oe.EventId equals e.EventId
							 join profile in dc.Profiles on uoep.UserId equals profile.UserId
							 where oep.PositionId == new Guid(positionId) // Filter by the specific PositionId
							 group new { uoep, profile } by new { uoep.UserId, profile.Firstname, profile.Lastname } into groupedData
							 select new
							 {
								 UserId = groupedData.Key.UserId,
								 Firstname = groupedData.Key.Firstname,
								 Lastname = groupedData.Key.Lastname,
								 Count = groupedData.Count() // Count occurrences
							 }).OrderByDescending(x => x.Count).ToList();

		UserCountRepeater.DataSource = userCountData.ToList();
		UserCountRepeater.DataBind();
	}

	private void LoadProgramsForPosition(string positionId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		// Query to fetch programs associated with the specified positionID
		var associatedPrograms = from pp in dc.ProgramPositions
									 join p in dc.Programs on pp.ProgramId equals p.ProgramId
									 where pp.PositionId == new Guid(positionId)
									 select new
									 {
										 ProgramName = p.Name,
										 ProgramDescription = p.Description // Adjust field names as per your schema
									 };

		// Binding the result to the Repeater
		ProgramRepeater.DataSource = associatedPrograms.ToList();
		ProgramRepeater.DataBind();
	}
	protected void LoadPositionDetails(string positionId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var position = (from p in dc.Positions
					   where p.PositionId == new Guid(positionId)
					   select p).SingleOrDefault();

		//Total count of items and total cost.
		positionName = position.Name;
		string description = position.Description;
		int deploymentStatus = position.DeploymentStatus;
		bool requiresTraining = position.RequiresTraining;
		bool requiresCertification = position.RequiresCertification;
		bool sharedPosition = (bool)position.IsShared;
		string trainingText = position.TrainingText;
		string videoTrainingURL = position.VideoTrainingURL;
		string positionSummary = position.PositionSummary;
		string trainingCheclist = position.CheckListText;

		btnEdit.Attributes.Add("onclick", "window.location.href='/V1/NonProfitAdministration/AddEditTeamRole.aspx?positionId=" + positionId + "&organizationId=" + organizationId + "'; return false;");
		btnAddRole.Attributes.Add("onclick", "window.location.href='/V1/NonProfitAdministration/AddEditTeamRole.aspx?organizationId=" + organizationId + "'; return false;");
		
		string requiresTrainingText = requiresTraining ? "<li>Previous Training Is Required for This Role</li>" : "<li>No Previous Training Required</li>";
		string requiresCertificationText = requiresCertification ? "<li>Certification Is Required for This Role</li>" : "<li>No Certification Is Required</li>";
		string sharedText = sharedPosition ? "<li>This Is A Globaly Shared Role</li>" : "<li>This Is An Internal Team Role</li>";

		litRequiresTraininig.Text = requiresTrainingText;
		litRequiresCertification.Text = requiresCertificationText;
		litSharedOrTeam.Text = sharedText;
		litPositionSummary.Text = Server.HtmlDecode(positionSummary);

		litRoleName.Text = !String.IsNullOrEmpty(programName) ? programName + ": " + positionName : positionName;
		litRoleDescription.Text = Server.HtmlDecode(description);
		litTrainingText.Text = Server.HtmlDecode(trainingText);
		litChecklist.Text = Server.HtmlDecode(trainingCheclist);
		litChecklistPrintable.Text = Server.HtmlDecode(trainingCheclist);
		litChecklistPostionName.Text = positionName + " Checklist";
		litChecklistPositionSummary.Text = Server.HtmlDecode(positionSummary);

		if (!String.IsNullOrEmpty(videoTrainingURL))
		{
			divTrainingVideo.Visible = true;
			Uri uri = new Uri(videoTrainingURL);
			string query = uri.Query;
			var queryParameters = HttpUtility.ParseQueryString(query);
			videoId = queryParameters["v"];
		}

		string deploymentStatusName = "<li>Remote Only</li>";
		switch (deploymentStatus)
		{
			case 1:
				deploymentStatusName = "<li>Remote Only</li>";
				break;
			case 2:
				deploymentStatusName = "<li>Deployment Only</li>";
				break;
			case 3:
				deploymentStatusName = "<li>Remote or Deployment</li>";
				break;
			default:
				deploymentStatusName = "<li>Remote Only</li>";
				break;
		}

		litDeploymentStatus.Text = deploymentStatusName;

	}

	protected void dlPositions_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			Literal litDeploymentStatus = (Literal)e.Item.FindControl("litDeploymentStatus");
			Literal litRoleName		= (Literal)e.Item.FindControl("litRoleName");
			HyperLink hypRoles		= (HyperLink)e.Item.FindControl("hypRoles");
			HtmlGenericControl iVideo = (HtmlGenericControl)e.Item.FindControl("iVideo");
			HtmlGenericControl iChecklist = (HtmlGenericControl)e.Item.FindControl("iChecklist"); 


			RepeaterItem dataItem	= (RepeaterItem)e.Item;

			//Total count of items and total cost.
			Guid positionId			= (Guid)DataBinder.Eval(dataItem.DataItem, "PositionId");
			string positionName		= (string)DataBinder.Eval(dataItem.DataItem, "Name");
			int deploymentStatus	= (int)DataBinder.Eval(dataItem.DataItem, "DeploymentStatus");
			string videoTrainingURL = (string)DataBinder.Eval(dataItem.DataItem, "VideoTrainingURL");
			string checkListText = (string)DataBinder.Eval(dataItem.DataItem, "CheckListText");
			var programId = DataBinder.Eval(e.Item.DataItem, "ProgramId");

			hypRoles.NavigateUrl	= "/V1/NonProfit/TeamRole.aspx?ProgramId=" + programId + "&organizationId=" + organizationId + "&positionId=" + positionId;

			litRoleName.Text = positionName;

			string deploymentStatusName = "Remote Only";
			switch (deploymentStatus)
			{
				case 1:
					deploymentStatusName = "Remote Only";
					break;
				case 2:
					deploymentStatusName = "Deployment Only";
					break;
				case 3:
					deploymentStatusName = "Remote or Deployment";
					break;
				default:
					deploymentStatusName = "Remote Only";
					break;
			}

			if(!String.IsNullOrEmpty(videoTrainingURL))
			{
				iVideo.Visible = true;
			}
			if (!String.IsNullOrEmpty(checkListText))
			{
				iChecklist.Visible = true;
			}

			litDeploymentStatus.Text = deploymentStatusName;
		}
	}
}