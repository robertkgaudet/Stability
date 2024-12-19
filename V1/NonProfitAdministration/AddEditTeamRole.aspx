<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Child.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="AddEditTeamRole.aspx.cs" Inherits="V1_NonProfitAdministration_AddEditTeamRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>
	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
	<link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
	<link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
	<link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
	<link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
    <script>

		$(function () {


			<%=preselectedPositionJQuery%>

			$("#teamSupervisorRole.dropdown-menu li").click(function () {
				$("#btn-dropdown.chooseSupervisor").html($(this).text());
				$("#<%=hidSupervisorPositionId.ClientID%>").val($(this).attr('name'));
			});


			$('#<%=txtPositionSummary.ClientID%>').summernote({
				toolbar: [
					['style', ['bold', 'italic']]
				],

				height: 100
			});

			$('#<%=txtRoleChecklist.ClientID%>').summernote({
				height: 200
			});

			$('#<%=txtTraining.ClientID%>').summernote({

				height: 200
			});

			$('#<%=txtPositionDescription.ClientID%>').summernote({
				toolbar: [
					['style', ['bold', 'italic', 'underline']],
					['alignment', ['ul', 'ol', 'paragraph']]
				],

				height: 125
			});

			$("#form1").validate({
				rules: {
					<%=txtPositionName.UniqueID%>: {
					required: true,
					maxlength: 100
					},
					<%=txtPositionDescription.UniqueID%>: {
						required: true
					},
				},
				submitHandler: function (form) {
					form.submit();
				}
			});
		});
	</script>
	<style>
		.i-checks
		{
			margin-right:10px !important;
		}
	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="hpanel">
			<div class="panel-body">
			<h1>	
				<asp:Literal id="litEventName" runat="server"></asp:Literal>
			</h1>
				<h2 class="font-light m-b-xs">
					Add/Edit Team Member Role
				</h2>
				<div class="form-group">
					<div class="pull-right">
						<asp:LinkButton id="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default" Text="Cancel" />
						<asp:Button id="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Save Changes" />
					</div>
				</div>
			</div>
		</div>
		<div class="hpanel">
			<div class="row" id="divForm" runat="server">
				<div class="col-lg-12 container">
					<div class="hpanel form-horizontal">
						<div class="panel-heading">
							 <h3>Details</h3>
						</div>
						<div class="panel-body">
						
							<div class="form-group">
								<label class="col-sm-2 control-label">Role Name</label>
								<div class="col-sm-10">
									<asp:TextBox runat="server" required id="txtPositionName" MaxLength="100" CssClass="form-control" placeholder="Role Name"></asp:TextBox>
								</div>
							</div>
						
							<div class="form-group">
								<label class="col-sm-2 control-label">Role Summary</label>
								<div class="col-sm-10">
									<span class="text-muted">
										Short two sentence summary of this position.
									</span>
									<asp:TextBox ID="txtPositionSummary" MaxLength="250" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="100" ClientIDMode="Static"></asp:TextBox>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Role Description</label>
								<div class="col-sm-10">
										<span class="text-muted">
											Enter information that helps the volunteer understand this role.
										</span>
										<asp:TextBox ID="txtPositionDescription" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="10" ClientIDMode="Static"></asp:TextBox>
								</div>
							</div>
							

							<div class="form-group"><label class="col-sm-2 control-label">Choose Supervisor Role</label>
								<div class="col-sm-10">
									<div id="div1" class="dropdown m-b-md" runat="server">
										<button id="btn-dropdown" class="btn btn-outline btn-default chooseSupervisor dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Choose Supervisor <i class="fa fa-sort-down"></i></button>
										<ul id="teamSupervisorRole" class="dropdown-menu text-center dropdown-volunteer required">
											<%=supervisorListDropDown%>
										</ul>
									</div>
									<input type="hidden" id="hidSupervisorPositionId" runat="server" />
									<div class="hr-line-dashed color-line"></div>
								</div>
							</div>

							
							<div class="form-group">
								<label class="col-sm-2 control-label">Choose Programs</label>
								<div class="col-sm-10">
									<span class="text-muted">
										Select any programs where this role may be applied.
									</span>
									<asp:CheckBoxList ID="chkBoxListPrograms" runat="server"></asp:CheckBoxList>
								</div>
							</div>


							<div class="form-group">
								<label class="col-sm-2 control-label"> <i class="fa fa-paper-plane"></i> Share Role With Other Teams</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkSharedPosition" class="i-checks">
									<span class="text-muted"> <small>(Note, once a role has been shared it can not be unshared.)</small></span>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Deployment Status</label>
								<div class="col-sm-5 m-t-sm">
									<div class="row">
										<div class="col-sm-6">
											<div class="radio">
												<asp:RadioButton runat="server" GroupName="rbDeploymentStatus" id="rdRemoteOnly" value="1" checked="" Text="Remote Only"></asp:RadioButton>
											</div>
											<div class="radio">
												<asp:RadioButton runat="server" GroupName="rbDeploymentStatus" id="rdDeployedOnly" value="2" Text="Deployed Only"></asp:RadioButton>
											</div>
											<div class="radio">
												<asp:RadioButton runat="server" GroupName="rbDeploymentStatus" id="rdRemoteOrDeployed" value="3" Text="Deployed or Remote"></asp:RadioButton>
											</div>
										</div>
									</div>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Deactivated Role</label>
								<div class="col-sm-5">
									<input type="checkbox" runat="server" id="chkIsDeactivated" class="i-checks">
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="hpanel">
			<div class="row" id="div2" runat="server">
				<div class="col-lg-12 container">
					<div class="hpanel form-horizontal">
						<div class="panel-heading">
							 <h3>Training Information</h2>
						</div>
						<div class="panel-body">

							<div class="form-group">
								<label class="col-sm-2 control-label">Certification Required</label>
								<div class="col-sm-5">
									<input type="checkbox" runat="server" id="chkRequiresCertification" class="i-checks">
										<br />
										Requires certifications to perform this role.
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Specialized Training</label>
								<div class="col-sm-5">
									<span class="text-muted">
										<input type="checkbox" runat="server" id="chkRequiresTraining" class="i-checks">
										<br />
										Requires specialized training beyond what is included on Stability.
									</span>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Role Training</label>
								<div class="col-sm-10">
									<asp:TextBox ID="txtTraining" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="100" ClientIDMode="Static"></asp:TextBox>
									<span class="text-muted">
										Enter training material to helps the team member understand this role.
									</span>
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Role Video YouTube Training URL</label>
								<div class="col-sm-5"><asp:TextBox type="text" runat="server" id="txtYouTubeURL" class="form-control" placeholder="Video Training URL"></asp:TextBox></div>
							</div>
							
							<div class="form-group">
								<label class="col-sm-2 control-label">Role Checklist</label>
								<div class="col-sm-10">
									<span class="text-muted">
										Enter role checklist to be used for coordinating daily work.
									</span>
									<asp:TextBox ID="txtRoleChecklist" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="100" ClientIDMode="Static"></asp:TextBox>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
						

	<script src="https://code.jquery.com/ui/1.14.0/jquery-ui.js"></script>
	<script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
</asp:Content>

