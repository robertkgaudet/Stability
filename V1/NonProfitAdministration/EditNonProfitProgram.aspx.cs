using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_EditNonProfitProgram : BaseOrganizationWebForm
{
	Guid programId = Guid.NewGuid(); //Send on QS for edit
	Guid organizationId = Guid.Empty; //Send on QS everytime
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			if (Request.QueryString["programId"] != null)
			{
				//Already have a record so load the information.
				loadForm();
			}
		}
	}

	protected void loadForm()
	{
		programId = new Guid(Request.QueryString["programId"]);
		//If org and eventid already exist, then don't allow them here.
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var program = (from p in dc.Programs
						where p.ProgramId == programId
						select p).SingleOrDefault();
		
		txtProgramDescription.Value = program.Description;
		txtProgramName.Value = program.Name;
		chkSharedProgram.Checked = program.IsShared != null ? (bool)program.IsShared : false;
		chkRemoteWork.Checked = (bool)program.IsRemoteOnly;
		chkRequiresDeployment.Checked = (bool)program.IsDeploymentRequired;
		chkRequiresTraining.Checked = (bool)program.IsTrainingRequired;
		txtProgramOrder.Value = program.Order.ToString();
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		string programName = txtProgramName.Value;
		string programDescription = txtProgramDescription.Value;
		bool isRemoteWork = chkRemoteWork.Checked;
		bool deploymentRequies = chkRequiresDeployment.Checked;
		bool requiresTraining = chkRequiresTraining.Checked;
		bool isShared = chkSharedProgram.Checked;
		string programOrder = txtProgramOrder.Value;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		if (Request.QueryString["programId"] == null)
		{
			//Insert
			organizationId = new Guid(Request.QueryString["organizationId"]);

			Program program = new Program();
			program.ProgramId = programId;
			program.IsTrainingRequired = chkRequiresTraining.Checked;
			program.IsShared = chkSharedProgram.Checked;
			program.IsDeploymentRequired = chkRequiresDeployment.Checked;
			program.IsRemoteOnly = chkRemoteWork.Checked;
			program.Description = txtProgramDescription.Value;
			program.Name = txtProgramName.Value;
			if(!String.IsNullOrEmpty(txtProgramOrder.Value))
			{ 
				program.Order = Convert.ToInt32(txtProgramOrder.Value);
			}

			dc.Programs.InsertOnSubmit(program);
			dc.SubmitChanges();

			OrganizationProgram organizationProgram = new OrganizationProgram();
			organizationProgram.ProgramId = programId;
			organizationProgram.OrganizationProgramId = Guid.NewGuid();
			organizationProgram.OrganizationId = organizationId;
			dc.OrganizationPrograms.InsertOnSubmit(organizationProgram);
			dc.SubmitChanges();
		}
		else
		{
			//Update
			programId = new Guid(Request.QueryString["programId"]);
			var program = (from p in dc.Programs
									 where p.ProgramId == programId
									 select p).SingleOrDefault();

			program.IsTrainingRequired = chkRequiresTraining.Checked;
			program.IsDeploymentRequired = chkRequiresDeployment.Checked;
			program.IsRemoteOnly = chkRemoteWork.Checked;
			program.Description = txtProgramDescription.Value;
			program.IsShared = chkSharedProgram.Checked;
			program.Name = txtProgramName.Value;
			if (!String.IsNullOrEmpty(txtProgramOrder.Value))
			{
				program.Order = Convert.ToInt32(txtProgramOrder.Value);
			}
			dc.SubmitChanges();
		}

		//Back to non-profit page that shows the programs.
		Response.Redirect("/V1/NonProfit/Programs.aspx?organizationId=" + Request.QueryString["organizationId"]);
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/NonProfit/Programs.aspx?organizationId=" + Request.QueryString["organizationId"]);
	}
}