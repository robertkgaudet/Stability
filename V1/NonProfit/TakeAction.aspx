<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="TakeAction.aspx.cs" Inherits="V1_NonProfit_TakeAction" %>
<%@ Register Src="~/V1/UserControls/DeploymentListCard.ascx" TagPrefix="uc1" TagName="DeploymentListCard" %>

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
				window.location.href = "/V1/NonProfit/TakeAction.aspx?organizationId=" + $(this).attr('id');
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
	<div class="content animate-panel">
		<div class="row">
			<div class="form-group col-lg-12">
				<button id="btn-NonProfitDropdown" class="btn btn-outline btn-default nonProfit dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Select Another Team Activity Page <i class="fa fa-sort-down"></i></button>
				<ul id="nonProfit" class="dropdown-menu text-center required">
					<%=nonProfitDropDown%>
				</ul>
			</div>
		</div>
		<p>
		<h2 class="m-xs pull-left">
			Team Activity
		</h2>
		<asp:HyperLink CssClass="font-normal btn btn-danger" ID="lbGetHelp" runat="server" Text="<i class='fa fa-check'></i> Create Help Request"></asp:HyperLink>
		</p>
		<div class="row">
			<div class="col-md-8">
				<table cellpadding="1" cellspacing="1" class="table table-bordered table-striped">
					<tbody>
						<tr>
							<td colspan="2" rowspan="2" class="small" style="background-color:white;">
								<p>
								<asp:HyperLink id="hypOrgName" runat="server" Font-Bold="true"></asp:HyperLink>
								</p>
								<div class="m-r-md m-b-sm" style="background-color:white; display:inline-block; padding:10px; border:solid 2px #ccc;">
									<asp:HyperLink ID="hypLogo" runat="server">
										<asp:Image ID="imgLogo" runat="server" Width="120" />
									</asp:HyperLink>
								</div>
							</td>
							<td colspan="1" class="small">FOUNDED</td>
							<td colspan="1" class="small">LOCATION</td>
							<td colspan="1" class="small">LEAD</td>
						</tr>
						<tr>
							<td colspan="1" class="small"><asp:Label id="lblFounded" runat="server"></asp:Label></td>
							<td colspan="1" class="small"><asp:Label id="lblLocation" runat="server"></asp:Label></td>
							<td colspan="1" class="small"><asp:HyperLink id="hypTeamPointOfContact" runat="server"></asp:HyperLink></td>
						</tr>
						<tr>
							<td class="small">HOURS</td>
							<td class="small">OFFSET</td>
							<td class="small">MEMBERS</td>
							<td class="small">EVENTS</td>
							<td class="small">DEPLOYMENTS</td>
						</tr>
						<tr>
							<td class="small"><asp:Label id="lblHours" runat="server"></asp:Label></td>
							<td class="small"><asp:Label id="lblOffset" runat="server"></asp:Label></td>
							<td class="small"><asp:Label id="lblTeamCount" runat="server"></asp:Label></td>
							<td class="small"><asp:Label id="lblEvents" runat="server"></asp:Label></td>
							<td class="small"><asp:Label id="lblCauseCount" runat="server"></asp:Label></td>
						</tr>
					</tbody>
				</table>
			</div>
			<div class="col-md-4">
				<div class="hpanel" id="divWorkingWebsite" runat="server" visible="false">
					<asp:HyperLink ID="hypWebsite" runat="server" Target="_blank">
                    <div class="text-center website">
                        <i class="pe-7s-global fa-3x"></i>
                        <h3 class="m-xs">Open Team Website</h3>
                        <small>Share your webpage, grow your team.</small>
                    </div>
					</asp:HyperLink>
                </div>
				
				<div class="hpanel" id="divSubscribeToWebsite" runat="server" visible="true">
					<div class="text-center website" data-toggle="modal" data-target="#subscribeModal">
						<i class="pe-7s-global fa-3x"></i>
						<h2 class="m-xs">Open Team Website</h2>
						<small>Share your webpage, grow your team.</small>
					</div>
					<div class="modal fade" id="subscribeModal" tabindex="-1" role="dialog" aria-hidden="true">
						<div class="modal-dialog">
							<div class="modal-content">
								<div class="color-line"></div>
								<div class="modal-header text-center">
									<h4 class="modal-title">Activate Your Stability Website</h4>
									<small class="font-bold">Subscribe now to activate your public Stability website.</small>
								</div>
								<div class="modal-body">
									<p>
										<asp:HyperLink ID="hypDemoWebsite" runat="server" Text="View Your Website - Example Only Opens in New Window" Target="_blank"></asp:HyperLink>
										<br />
										<strong>Enter your payment information here to subscribe and activate your organizations website.</strong>
										With a website, you can share your team information on your social media pages and with members you want to join you.
									</p>
								</div>
								<div class="modal-footer">
									<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
									<button type="button" class="btn btn-primary">Save changes</button>
								</div>
							</div>
						</div>
					</div>
                </div>
			</div>
		</div>
		<div class="row">
			<div class="col-lg-12">
				<div class="hpanel hgreen">
					<div class="panel-heading hbuilt">
						<h3 class="m-l-sm">Active Deployments</h3>
						<small class="font-normal">
							Choose a deployment to volunteer or donate.<br />
							<asp:HyperLink ID="hypCreateCause" runat="server" Visible="false" Text="Create A New Deployment" CssClass="font-normal btn btn-sm btn-info"></asp:HyperLink>
						</small>
					</div>
					<div class="panel-body">
						<uc1:DeploymentListCard runat="server" ID="ucDeploymentListCard" />
						<div runat="server" id="divNoCause" visible="false">
							This team has no active deployments.
							<br />
							Clicking this button will automatically email the team lead and recommend they create a deployment.
							<br />
							<asp:Button ID="btnEmailTeam" runat="server" Text="Contact Team Lead" OnClick="btnEmailTeam_Click" CssClass="btn btn-success"></asp:Button>
							<hr />
						</div>
						<div runat="server" id="divShowInviteAlert" visible="false" class="alert alert-success text-uppercase">
							<i class="fa fa-envelope"></i> Your invitation to create a deployment has been sent
						</div>
					</div>
					<div class="panel-footer">
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown" runat="server" id="hpanelMembers" visible="false">
			<div class="row">
				<div class="col-lg-12 container">
					<div class="hpanel">
						<div  class="hpanel" runat="server" id="hpanelJoin" visible="true">
							<a href="/V1/Profile/EditNonProfits.aspx">Join This Team</a>
						</div>
						<div class="panel-heading hbuilt">
							<asp:HyperLink ID="hypInviteTeamMembers" runat="server" Visible="false" Text="Invite Team Members" CssClass="font-normal btn btn-sm btn-info pull-right"></asp:HyperLink>
							<h3>Team Members</h3>
						</div>
						<div class="panel-body">
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
															<div class="pull-right">
																<asp:Button ID="btnContact" runat="server" Text="Message" Visible="false" CssClass="btn btn-outline btn-default messageButton" data-toggle="modal" data-target="#messageMemberModal"></asp:Button>
															</div>
															<asp:Literal id="litSkills" runat="server"></asp:Literal>
															<h5 class="m-b-xs">
																<asp:HyperLink ID="hypName" runat="server"></asp:HyperLink>
															</h5>
															<p>
																<asp:Literal ID="litMemberInfo" runat="server"></asp:Literal>
																<asp:Literal ID="litDescription" runat="server"></asp:Literal>
															</p>
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
								</div>


							</div>
						</div>
				</div>
			</div>
			<div class="col-md-5">
			</div>
		</div>
	</div>
</asp:Content>