<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="EditNonProfits.aspx.cs" Inherits="V1_Profile_EditNonProfits" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>

	<script type="text/javascript">
	$(document).ready(function(){
		$('input[type="checkbox"]').each(function () {
			$(this).addClass("i-checks");
		});
		
		$('.btnAddNonProfit').click(function () {
		window.location.href = '/V1/Administration/TeamName.aspx?userActionModal=false';
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
							<div class="form-group">
								<div class="pull-right">
									<asp:LinkButton id="btnSubmit" CausesValidation="false" runat="server" CssClass="btn btn-default" Text="Cancel" />
									<asp:Button id="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Save Changes" />
								</div>
							</div>
							<h2 class="font-light m-b-xs">
								Join A Disaster Relief Team
							</h2>
							When your neighbors have lost everything, there is no time to waste. Stability makes it easy to rally your team to instantly help your community after a disaster.
							<hr />
							<div class="alert alert-success">
								<i class="fa fa-bolt"></i>
								OPTION: Did you want to CREATE a team instead of JOIN one? Click Here To Create Your Team 
								<asp:Button id="btnAdd" runat="server" CssClass="btnAddNonProfit btn btn-primary" text="Create A New Team" />
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
							Select The Team To Join
						</div>
						<div runat="server" id="divMessage" class="alert alert-success" visible="false">
							<i class="fa fa-bolt"></i>
							<asp:Literal runat="server" id="lblMessage"></asp:Literal>
						</div>
						<div class="panel-body p-lg">
                            <div class="radio radio-success">
							    <asp:CheckBoxList ID="rblOrganizations" runat="server" DataTextField="Name" DataValueField="OrganizationId"></asp:CheckBoxList>
							 </div>
						</div>
						<div class="panel-footer">
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>