<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="People.aspx.cs" Inherits="V1_NonProfit_People" %>
<%@ Register Src="~/V1/UserControls/TeamNavigation.ascx" TagPrefix="uc1" TagName="TeamNavigation" %>
<%@ Register Src="~/V1/UserControls/TeamHeader.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
	<style>
		.website
		{
			background-color:#5E2E91;
			padding:17px;
			color:white;
		}
		.website:hover
		{
			background-color:#902F91;
			color:white;
			cursor:pointer;
		}
		.modal-dialog
		{
			margin-top:100px;
		}
	</style>
	<script>
		// Step 1: Select all the buttons in the table

		var recipientsName;
		var recipientsEmail;
		var message;
		var teamName;
		var divMessageSuccess;
		var divMessageTextBox;
		var divMessageError;
		var divErrorMessage
		var btnSend;
		var messageModelTitle;
		var txtMessage;

		$(document).ready(function () {

			// Initialize Example 1
			$('#tblVolunteers').footable();


			messageModelTitle = document.getElementById('messageModelTitle');
			txtMessage = document.getElementById('<%=txtMessage.ClientID%>');

			divMessageSuccess = document.getElementById('divMessageSuccess');
			divMessageTextBox = document.getElementById('messageTextBox');
			btnSend = document.getElementById('btnSend');
			divMessageError = document.getElementById('divMessageError');
			divErrorMessage = document.getElementById('divErrorMessage');
			divMessageSuccess.style.visibility = 'hidden';
			divMessageSuccess.style.visibility = 'visible';
			divMessageSuccess.style.visibility = 'visible';
			divMessageSuccess.hidden = true;
			divMessageTextBox.hidden = false;
			btnSend.hidden = false;

			divMessageError.style.visibility = 'hidden';
			divErrorMessage.style.visibility = 'hidden';
			divMessageError.hidden = true;
			divErrorMessage.hidden = true;

			$("#nonProfit.dropdown-menu li").click(function () {
				window.location.href = "/V1/NonProfit/Default.aspx?organizationId=" + $(this).attr('id');
			});
		});

		function sendClick(object) {
			message = txtMessage.value;
			teamName = '<%=teamName%>'; 
			signedInUserFullName = '<%=signedInUserFullName%>';
			organizationId = '<%=organizationId%>';
			sendMessage(signedInUserFullName, organizationId, recipientsName, recipientsEmail, teamName, message);
		}

		function btnClick(object) {
			txtMessage.value = "";
			divMessageSuccess.style.visibility = 'hidden';
			divMessageTextBox.style.visibility = 'visible';
			btnSend.style.visibility = 'visible';
			divMessageSuccess.hidden = true;
			divMessageTextBox.hidden = false;
			btnSend.hidden = false;

			divMessageError.style.visibility = 'hidden';
			divErrorMessage.style.visibility = 'hidden';
			divMessageError.hidden = true;
			divErrorMessage.hidden = true;

			recipientsName = object.getAttribute('data-name');
			recipientsEmail = object.getAttribute('data-email');
			messageModelTitle.innerHTML = "This will send " + recipientsName + " an email from the Stability platform.";
			return false;
		}

		function sendMessage(signedInUserFullName, organizationId, recipientsName, recipientsEmail, teamName, message)
		{
			$.ajax
			(
				{
					type: "POST",
					url: "/V1/Handlers/MessageMember.ashx",
					data: JSON.stringify({ senderName: signedInUserFullName, organizationId: organizationId, recipientsName: recipientsName, recipientsEmail: recipientsEmail, teamName: teamName, message: message }),
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: function (response) {
						//alert(divMessageSuccess);
						//alert(divTextBody);
						divMessageSuccess.style.visibility = 'visible';
						divMessageTextBox.style.visibility = 'hidden';
						btnSend.style.visibility = 'hidden';
						divMessageSuccess.hidden = false;
						divMessageTextBox.hidden = true;
						btnSend.hidden = true;
					},
					error: function (xhr, status, error) {
						alert(xhr.responseText);
						/*var err = eval("(" + xhr.responseText + ")");*/
						divMessageError.style.visibility = 'visible';
						divErrorMessage.style.visibility = 'visible';
						divMessageError.hidden = false;
						divErrorMessage.hidden = false;
						divErrorMessage.innerHTML += xhr.responseText;
					}
				}
			);
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

	<uc1:TeamHeader runat="server" ID="ucTeamHeader" />

	<div class="content">
        <div class="row">
            <div class="col-md-3">
				<uc1:TeamNavigation runat="server" ID="ucTeamNavigation" />
            </div>
            <div class="col-md-9">
					<!--PAGE HEADER-->
                <div class="hpanel ">
                    <div class="panel-heading hbuilt">
                        <div class="font-normal">
							<h1 class="m-b-none"><i class="fa fa-vcard"></i> People</h1>
							<small class="text-muted">Team members.</small>
							<div class="pull-right">
								<asp:HyperLink ID="hypInviteTeamMembers" runat="server" Visible="false" Text="Invite Team Members" CssClass="font-normal btn btn-sm btn-info"></asp:HyperLink>
							</div>
                        </div>
                    </div>
					<!--PAGE CONTENT-->
					<div class="panel-body">
						<div id="divUpdateMessage" runat="server" class="alert alert-warning text-center" style="margin-bottom:20px;" visible="false">
							<asp:Literal ID="litMessage" runat="server"></asp:Literal>
						</div>
						<div id="divFilterMessage" runat="server" class="alert alert-info text-center" style="margin-bottom:20px;" visible="false">
							<asp:Literal ID="litFilterMessage" runat="server"></asp:Literal>
						</div>
						<div class="" data-child="hpanel" data-effect="fadeInDown" runat="server" id="hpanelMembers" visible="false">
							<div  class="hpanel" runat="server" id="hpanelJoin" visible="true">
								<a href="/V1/Profile/EditNonProfits.aspx">Join This Team</a>
							</div>
							<b>SEARCH TO FILTER YOUR TEAM</b>
							<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table">
							<table id="tblVolunteers" class="footable" data-page-size="20" data-filter=#filter>
								<tbody>
									<asp:Repeater ID="rptVolunteers" runat="server" OnItemDataBound="rptVolunteers_ItemDataBound">
										<ItemTemplate>
											<tr>
												<td style="background-color:white;">
													<div class="hpanel">
														<div class="panel-body">
															<h5 class="m-b-xs">
																<asp:HyperLink ID="hypName" runat="server"></asp:HyperLink>
															</h5>
															<p>
																<asp:Literal ID="litMemberInfo" runat="server"></asp:Literal>
																<asp:Literal ID="litDescription" runat="server"></asp:Literal>
															</p>
															<div class="pull-right">
																<asp:Button ID="btnContact" runat="server" Text="Message" Visible="false" CssClass="btn btn-success messageButton" data-toggle="modal" data-target="#messageMemberModal"></asp:Button>
															</div>
															<asp:Literal id="litSkills" runat="server"></asp:Literal>
															<asp:Literal id="litResources" runat="server"></asp:Literal>
														</div>
														<div class="panel-footer" id="divFooter" runat="server" visible="false">
															<div class="text-muted small">
																<asp:Literal ID="litVettingInfo" runat="server"></asp:Literal>
															</div>
														</div>
													</div>
												</td>
											</tr>
										</ItemTemplate>
									</asp:Repeater>
								</tbody>
								<tfoot>
									<tr>
										<td>
											<br />
											<ul class="pagination pull-right"></ul>
										</td>
									</tr>
								</tfoot>
							</table>
						</div>
					</div>
				</div>
				<div class="modal fade" id="messageMemberModal" tabindex="-1" role="dialog" aria-hidden="true">
					<div class="modal-dialog">
						<div class="modal-content">
							<div class="color-line"></div>
							<div class="modal-header text-center">
								<h5 class="modal-title">Send a Message</h5>
								<div id="divMessageSuccess" class="alert alert-success text-uppercase">
									<i class="fa fa-envelope"></i> Your message has been sent.
								</div>
								<div id="divMessageError" class="alert alert-warning text-uppercase">
									<i class="fa fa-envelope"></i> 
									<div id="divErrorMessage"></div>
								</div>
							</div>
							<div class="modal-body" id="messageTextBox">
								<p>
									<div id="messageModelTitle"></div>
									<asp:TextBox TextMode="MultiLine" Width="100%" Rows="5" runat="server" ID="txtMessage"></asp:TextBox>
								</p>
							</div>
							<div class="modal-footer">
								<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
								<button type="button" class="btn btn-primary" id="btnSend" onclick="return sendClick(this);">Send</button>
							</div>
						</div>
					</div>
					<!--PAGE FOOTER-->
                </div>
            </div>
        </div>
    </div>
</asp:Content>

