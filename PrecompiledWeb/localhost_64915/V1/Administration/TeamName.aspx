<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_Administration_TeamName, App_Web_ijye2wuz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
	<script>
		$(function () {

			$("#form1").validate({
				rules: {
				<%=txtParentOrganization.UniqueID%>: {
				required: true
			},
				submitHandler: function (form) {
					form.submit();
				}
			});
		});

		$(document).ready(function () {
			$('input[type="checkbox"]').each(function () {
				$(this).addClass("i-checks");
			});

			$('.btnJoinTeam').click(function () {
				window.location.href = '/V1/Profile/EditNonProfits.aspx?userActionModal=false';
				return false;
			});
		});

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<h2 class="font-light m-b-xs">
								Add Your Team
							</h2>
                            <div>
                                <p>
                                    We invite churches, families, sports groups, nonprofits, businesses, any group with a passion for helping others to form their own Disaster Relief Team.
                                    <br />
                                    Together, we can make a significant impact in our communities when disaster strikes.
									<hr />
									<div class="alert alert-success">
										<i class="fa fa-bolt"></i>
										OPTION: Don't want to create a team? Click Here To Find and Join Your Team
										<asp:LinkButton id="btnJoin" PostBackUrl="/V1/Profile/EditNonProfits.aspx?userActionModal=false" runat="server" CssClass="btnJoinTeam btn btn-primary" text="Join A Team" />
									</div>
								</p>
                            </div>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-12 container">
					<div class="hpanel form-horizontal">

						<div class="panel-heading hbuilt">
							Pick a fitting team name. Have fun or keep it serious, whatever works for you.
						</div>
						<div class="panel-body">
							
							<div runat="server" id="divMessage" visible="false">
								<i class="fa fa-bolt"></i>
								<asp:Literal runat="server" id="lblMessage"></asp:Literal>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Team/Organization Name *</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtParentOrganization" class="form-control" placeholder="Enter A Team/Organization Name. Examples (Smith Family Responders, Texas Task Force)"></div>
							</div>
						</div>
						<div class="form-group">
							<label class="col-sm-2 control-label"></label>
							<div class="col-sm-5">
								<div class="pull-right">
									<asp:LinkButton id="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default" Text="Cancel" />
									<asp:Button id="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Submit" />
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>