<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="EditSkills.aspx.cs" Inherits="V1_Profile_Skills" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>
	<script type="text/javascript">
	$(document).ready(function(){
		$('input[type="checkbox"]').each(function () {
			$(this).addClass("i-checks");
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
								Choose Your Skills
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
			<div class="row">
				<div class="col-lg-12 container">
					
					<div class="hpanel form-horizontal">
						<div class="panel-heading hbuilt">
							Choose Your Skills
						</div>
						<div runat="server" id="divMessage" class="alert alert-success" visible="false">
							<i class="fa fa-bolt"></i>
							<asp:Literal runat="server" id="lblMessage"></asp:Literal>
						</div>
						<div class="panel-body p-lg">
							
							
							<div class="form-group m-t-lg">
								<asp:CheckBoxList ID="chkBoxListSkills" RepeatColumns="2" runat="server" DataTextField="Name" DataValueField="SkillId" RepeatDirection="Vertical"></asp:CheckBoxList>
							</div>

						</div>
						<div class="panel-footer">
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>