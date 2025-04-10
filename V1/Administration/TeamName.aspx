<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="TeamName.aspx.cs" Inherits="V1_Administration_TeamName" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
	<script>
		$(function () {

			$("#form1").validate({
				rules: {
					<%=txtOrganization.UniqueID%>: {
					required: true
					},
				},
				submitHandler: function (form) {
					form.submit();
				}
			});
		});

		$(document).ready(function () {

			$('.btnJoinTeam').click(function () {
				window.location.href = '/V1/Profile/EditNonProfits.aspx?userActionModal=false';
				return false;
			});
			
			<%=preselectedNonProfitJQuery%>

            $("#nonProfit.dropdown-menu li").click(function () {
                $("#btn-NonProfitDropdown.nonProfit").html($(this).text());
				$("#<%=hidParentOrganizationId.ClientID%>").val($(this).attr('id'));
			});
		});

		function checkUniqueName(name) 
		{
			if (name.trim() === '') 
			{
				// If the textbox is empty, clear the status message
				$('#friendlyNameStatus').text('');
				return;
			}

			$.ajax({
				type: "POST",
				url: "/V1/Administration/TeamName.aspx/IsFriendlyNameUnique", // Replace with your ASP.NET page name
				data: JSON.stringify({ urlFriendlyName: name }),
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (response) {
					if (response.d) {
						// The name is unique
						$('#friendlyNameStatus').html('This name is available.').css('color', 'green');
					} else {
						// The name is not unique
						$('#friendlyNameStatus').html('This name is already taken.').css('color', 'red');
					}
				},
				error: function (xhr, status, error) {
					console.error("Error: " + error);
				}
			});
		}
	</script>
	<style>
	#friendlyNameStatus
	{
		margin-top:-200px;
	  padding:0px !important ;
	}

	#nonProfit {
		left: 0 !important;
		right: auto !important;
		max-height: 350px;
		overflow-y: auto;
		overflow-x: hidden;
		text-align: left; /* optional, helps if your content is centered */
	}	
	.btn-NonProfitDropdown {
    position: relative;
}
	</style>
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
							
                            <div class="form-group" runat="server" id="divChooseNonprofit">
								<label class="col-sm-2 control-label">Select A Parent Organization (Optional)</label>
                                <small>Use if you are adding a chapter, division or child of another team.</small>
                                <div id="div2" class="dropdown m-b-md" runat="server">
                                    <button id="btn-NonProfitDropdown" class="btn btn-outline btn-default nonProfit dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Select A Team (Optional) <i class="fa fa-sort-down"></i></button>
                                    <ul id="nonProfit" class="dropdown-menu text-center dropdown-volunteer">
                                        <%=nonProfitDropDown%>
                                    </ul>
                                </div>
                                <input type="hidden" id="hidParentOrganizationId" runat="server" />
                            </div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Team/Organization Name *</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtOrganization" class="form-control" placeholder="Team/Organization Name. Examples (Smith Family Responders, Texas Task Force)"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">URL Friendly Name (NO Spaces or Special Characters) *</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtURLFriendlyName" oninput="checkUniqueName(this.value)" class="form-control" placeholder="Enter A Name with NO Spaces. Examples (SmithFamilyResponders, TexasTaskForce)"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2"></label>
								<div class="col-sm-5"> <span id="friendlyNameStatus"></span> </div>
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
		<script>
			document.getElementById('<%=txtOrganization.ClientID%>').addEventListener('input', function () {

				// Get the current value from the input text box
				const inputText = this.value;

				// Filter out non-alphanumeric characters and spaces
				const filteredText = inputText.replace(/[^a-zA-Z0-9]/g, '');

				// Set the filtered text to the output text box
				document.getElementById('<%=txtURLFriendlyName.ClientID%>').value = filteredText;

				checkUniqueName(filteredText);
			});
		</script>
</asp:Content>