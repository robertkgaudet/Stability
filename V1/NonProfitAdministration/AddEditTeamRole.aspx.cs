using System;
using System.Activities;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_AddEditTeamRole : BaseWebForm
{
	public string preselectedPositionJQuery;
	public string supervisorListDropDown;
	public string supervisorPositionId;
	public string positionId;
	public string organizationId;

	protected void Page_Load(object sender, EventArgs e)
	{
		positionId = Request.QueryString["positionId"];
		organizationId = Request.QueryString["organizationId"];
		if (!IsPostBack)
		{
			LoadPrograms();
			if (!String.IsNullOrEmpty(positionId))
			{
				//Load the edit forms
				CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
				var position = (from p in dc.Positions
								where p.PositionId == new Guid(positionId)
								select p).SingleOrDefault();

				txtPositionDescription.Text = Server.HtmlDecode(position.Description);
				txtPositionName.Text = position.Name;
				txtPositionSummary.Text = Server.HtmlDecode(position.PositionSummary);
				txtRoleChecklist.Text = Server.HtmlDecode(position.CheckListText);
				chkSharedPosition.Checked = (bool)position.IsShared;
				chkIsDeactivated.Checked = (bool)position.IsDeleted;

				if ((bool)position.IsShared)
				{
					//Prevents shared positions from being unshared.
					chkSharedPosition.Disabled = true;
				}

				//Training form elements
				chkRequiresCertification.Checked = (bool)position.RequiresCertification;
				chkRequiresTraining.Checked = (bool)position.RequiresTraining;
				txtTraining.Text = Server.HtmlDecode(position.TrainingText);
				txtYouTubeURL.Text = position.VideoTrainingURL;

				switch (position.DeploymentStatus)
				{
					case 1:
						rdRemoteOnly.Checked = true;
						break;
					case 2:
						rdDeployedOnly.Checked = true;
						break;
					case 3:
						rdRemoteOrDeployed.Checked = true;
						break;
					default:
						rdRemoteOrDeployed.Checked = true;
						break;
				}

				if(position.IsRemote != null && (bool)position.IsRemote)
				{
					//override based on this flag
					rdDeployedOnly.Checked = true;
				}
			}
		}

		LoadRolesForSupervisor();
		ApplyIChecksStyle();
	}
	private void ApplyIChecksStyle()
	{
		// Loop through each ListItem in the CheckBoxList
		foreach (ListItem item in chkBoxListPrograms.Items)
		{
			// Generate HTML with the desired styling class
			item.Attributes.CssStyle.Add("class", "i-checks");

			// Alternatively, use this for more direct control over rendering:
			item.Attributes.Add("class", "i-checks");
		}
	}
	private void LoadPrograms()
	{
		// Assuming you have a DataContext named 'MyDataContext'
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		// Fetching programs
		var programs = from p in dc.Programs
					   join op in dc.OrganizationPrograms on p.ProgramId equals op.ProgramId
					   where op.OrganizationId == new Guid(organizationId)
					   orderby p.Order
					   select p;

		// Binding the CheckBoxList with all available programs
		chkBoxListPrograms.DataSource = programs;
		chkBoxListPrograms.DataTextField = "Name";
		chkBoxListPrograms.DataValueField = "ProgramId";
		chkBoxListPrograms.DataBind();

		if (!String.IsNullOrEmpty(positionId))
		{
			// Fetching existing associations between the current position and programs
			var selectedProgramIDs = dc.ProgramPositions
				.Where(pp => pp.PositionId == new Guid(positionId))
				.Select(pp => pp.ProgramId)
				.ToList();

			// Pre-selecting the checkboxes based on existing associations
			foreach (ListItem item in chkBoxListPrograms.Items)
			{
				if (selectedProgramIDs.Contains(Guid.Parse(item.Value)))
				{
					item.Selected = true;
				}
			}
		}
	}

	public void LoadRolesForSupervisor()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var teamPostions = from p in dc.Positions
						orderby p.Name descending
						where p.IsDeleted == false
						orderby p.Name
						select new { p };

		foreach (var teamPostion in teamPostions)
		{
			supervisorListDropDown = supervisorListDropDown + "<li name=\"" + teamPostion.p.PositionId + "\"><a href=\"#\">" + teamPostion.p.Name + "</a></li>" + Environment.NewLine;
		}
		if (!String.IsNullOrEmpty(supervisorPositionId))
		{
			//Get the supervisor role name.
			var positionSupervisor = (from p in dc.Positions
									 where p.PositionId == new Guid(supervisorPositionId)
									 select new {p.Name}).SingleOrDefault();

			preselectedPositionJQuery = "$(\"#btn-dropdown.chooseSupervisor\").html('" + positionSupervisor.Name + "');";
			hidSupervisorPositionId.Value = supervisorPositionId;
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		string positionDescription = Server.HtmlEncode(txtPositionDescription.Text);
		string positionName = txtPositionName.Text;
		bool sharedPosition = chkSharedPosition.Checked;
		bool requiresCertification = chkRequiresCertification.Checked;
		bool requiresTraining = chkRequiresTraining.Checked;
		string trainingText = Server.HtmlEncode(txtTraining.Text);
		string youTubeURL = txtYouTubeURL.Text;
		int deploymentStatus = 1;
		bool isRemote = false;
		bool isDeactivated = chkIsDeactivated.Checked;
		string supervisorPositionId = hidSupervisorPositionId.Value;
		string positionChecklist = Server.HtmlEncode(txtRoleChecklist.Text);
		string positionSummary = Server.HtmlEncode(txtPositionSummary.Text);

		if (rdRemoteOnly.Checked)
		{
			deploymentStatus = 1;
			isRemote = true;
		}
		else if (rdDeployedOnly.Checked)
		{
			deploymentStatus = 2;
		}
		else if (rdRemoteOrDeployed.Checked)
		{
			deploymentStatus = 3;
		}

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();



		//Add or edit.
		if (!String.IsNullOrEmpty(positionId))
		{
			//Update ProgramPositions
			if (!String.IsNullOrEmpty(positionId))
			{
				var existingProgramPositions = dc.ProgramPositions
				.Where(pp => pp.PositionId == new Guid(positionId));

				// Remove existing associations
				dc.ProgramPositions.DeleteAllOnSubmit(existingProgramPositions);
				dc.SubmitChanges();
			}

			//EDIT
			var teamPostions = (from p in dc.Positions
							   orderby p.Name descending
							   where p.PositionId == new Guid(positionId)
							   select p).SingleOrDefault();

			if(teamPostions != null)
			{
				teamPostions.Description			= positionDescription;
				teamPostions.PositionSummary		= positionSummary;
				teamPostions.CheckListText			= positionChecklist;
				teamPostions.Name					= positionName;
				teamPostions.DeploymentStatus		= deploymentStatus;
				teamPostions.IsRemote				= isRemote;
				teamPostions.IsShared				= sharedPosition;
				teamPostions.RequiresTraining		= requiresTraining;
				teamPostions.VideoTrainingURL		= youTubeURL;
				teamPostions.TrainingText			= trainingText;
				teamPostions.RequiresCertification	= requiresCertification;
				teamPostions.IsDeleted				= isDeactivated;
				if (!String.IsNullOrEmpty(supervisorPositionId))
				{
					teamPostions.SupervisorPositionId = new Guid(supervisorPositionId);
				}
				dc.SubmitChanges();
			}
		}
		else
		{
			//ADD
			positionId = Guid.NewGuid().ToString();
			Position position = new Position();
			position.PositionId = new Guid(positionId);
			position.OrganizationId = new Guid(organizationId);
			position.Description = positionDescription;
			position.PositionSummary = positionSummary;
			position.CheckListText = positionChecklist;
			position.Name = positionName;
			position.DeploymentStatus = deploymentStatus;
			position.IsRemote = isRemote;
			position.IsShared = sharedPosition;
			position.RequiresTraining = requiresTraining;
			position.VideoTrainingURL = youTubeURL;
			position.TrainingText = trainingText;
			position.RequiresCertification = requiresCertification;
			position.IsDeleted = isDeactivated;
			position.CreatedOn = DateTime.Now;
			position.CreatedBy = userId;
			if (!String.IsNullOrEmpty(supervisorPositionId))
			{
				position.SupervisorPositionId = new Guid(supervisorPositionId);
			}

			dc.Positions.InsertOnSubmit(position);
			dc.SubmitChanges();
		}

		// Add new ProgramPosition associations based on selected checkboxes
		foreach (ListItem item in chkBoxListPrograms.Items)
		{
			if (item.Selected)
			{
				// Create a new association between position and program
				ProgramPosition newProgramPosition = new ProgramPosition
				{
					ProgramPositionId = Guid.NewGuid(),
					PositionId = new Guid(positionId),
					ProgramId = Guid.Parse(item.Value),
					CreatedBy = userId,
					CreatedOn = DateTime.Now,
					IsActive = true
				};
				dc.ProgramPositions.InsertOnSubmit(newProgramPosition);
			}
		}

		// Save changes to the database
		dc.SubmitChanges();



		Response.Redirect("/V1/NonProfit/TeamRole.aspx?positionId=" + positionId + "&organizationId=" + organizationId);
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/NonProfit/TeamRoles.aspx?positionId=" + positionId + "&organizationId=" + organizationId);
	}
}