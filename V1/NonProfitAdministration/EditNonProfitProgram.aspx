<%@ Page Title="Edit or Add Nonprofit Program" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="EditNonProfitProgram.aspx.cs" Inherits="V1_NonProfitAdministration_EditNonProfitProgram" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>
	
	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
	
    <script>

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode == 110 || charCode == 190 || charCode == 46)
                return true;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

		$(function () {

			$("#form1").validate({
				rules: {
					<%=txtProgramName.UniqueID%>: {
					required: true,
					maxlength: 100
					},
					<%=txtProgramDescription.UniqueID%>: {
						required: true
					},
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
		});
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeIn">
					<div class="hpanel">
						<div class="panel-body">
						<h1>	
							<asp:Literal id="litEventName" runat="server"></asp:Literal>
						</h1>
							<h2 class="font-light m-b-xs">
								Add a Non-profit Program
							</h2>
						    <div class="form-group">
							    <div class="pull-right">
								    <asp:LinkButton id="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default" Text="Cancel" />
								    <asp:Button id="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Save Changes" />
							    </div>
						    </div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
		
			<div class="row" id="divForm" runat="server">
				<div class="col-lg-12 container">
					<div class="hpanel form-horizontal">

						<div class="panel-heading hbuilt">
							Non-Profit Program
						</div>
						<div class="panel-body">
							
							<div class="form-group">
								<label class="col-sm-2 control-label"> <i class="fa fa-paper-plane"></i> Share Program With Other Teams</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkSharedProgram" class="form-control">
									<span class="text-muted"> <small>Note, once a program has been shared and deployed it can not be unshared.</small></span>
								</div>
							</div>
						
							<div class="form-group">
								<label class="col-sm-2 control-label">Program Name</label>
								<div class="col-sm-5"><input type="text" runat="server" required id="txtProgramName" class="form-control" placeholder="Program Name"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Program Description</label>
								<div class="col-sm-5">
									<textarea id="txtProgramDescription" runat="server" rows="7" class="form-control" placeholder="Describe this program."></textarea>
								</div>
							</div>
							
							<div class="form-group">
								<label class="col-sm-2 control-label">Remote Work</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkRemoteWork" class="form-control">
								</div>
							</div>
							
							<div class="form-group">
								<label class="col-sm-2 control-label">Requires Deployment</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkRequiresDeployment" class="form-control">
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Requires Specialized Training</label>
								<div class="col-sm-5">
									<input type="checkbox" runat="server" id="chkRequiresTraining" class="form-control">
								</div>
							</div>
						
							<div class="form-group">
								<label class="col-sm-2 control-label">Program Order</label>
								<div class="col-sm-1"><input type="text" runat="server" required maxlength="2" id="txtProgramOrder" onkeypress="return isNumberKey(event)" class="form-control" placeholder="0-20"></div>
							</div>
						</div>
					</div>
				</div>

			</div>
		</div>
</asp:Content>