<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/2-Column-Child.master" AutoEventWireup="true" CodeFile="Positions.aspx.cs" Inherits="V1_Positions" %>
<%@ Register Src="~/V1/UserControls/PositionNavigation.ascx" TagPrefix="uc1" TagName="PostionNavigation" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/2-Column-Child.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
	 <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<style>
		.copy-link {
            color: blue;
            cursor: pointer;
            text-decoration: underline;
        }
		a
		{
			color:black;
		}
		a:hover
		{
			color:darkgreen;
		}
		
        .circle {
            display: inline-block;
            padding: 0px;
			background-color:#9B59B6;
			color: white;
            border-radius: 50%; /* Creates the circular shape */
            text-align: center; /* Horizontally center the text */
            font-size: 12px; /* Font size of the number */	
            width: 30px; /* Circle width */
            height: 30px; /* Circle height */
            line-height: 30px; /* Vertically center the text */
			margin-right:10px;
			cursor:pointer;
        }
		.circle:hover{cursor:pointer;}
		.ui-accordion .ui-accordion-content 
		{
            padding: 10px;
			background-color:red !important;
        }
			@media (min-width: 1200px) {
    .col-lg-9 {
        width: 100%;
    }
}
.map
{
	height:600px;
}
#divMap
{
	height:600px;
}
.panel-body.member-panel-body {
    display: none;
}
i{
	color:#fff !important
}
	</style>
	<script>

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


			messageModelTitle = document.getElementById('messageModelTitle');
			txtMessage = document.getElementById('<%=txtMessage.ClientID%>');
			divMessageTextBox = document.getElementById('messageTextBox');
			btnSend = document.getElementById('btnSend');
			divMessageError = document.getElementById('divMessageError');
			divErrorMessage = document.getElementById('divErrorMessage');

			divMessageSuccess = document.getElementById('divMessageSuccess');
			divMessageSuccess.style.visibility = 'visible';
			divMessageSuccess.hidden = true;
			divMessageTextBox.hidden = false;
			btnSend.hidden = false;

			divMessageError.style.visibility = 'hidden';
			divErrorMessage.style.visibility = 'hidden';
			divMessageError.hidden = true;
			divErrorMessage.hidden = true;

			// Attach click event to all buttons with class 'insert-btn'
			$('.insert-btn').on('click', function () {
				// Get the organizationEventPositionId from the button's data-value attribute
				var organizationEventPositionId = $(this).data('value');

				// Call the AJAX function
				insertPositionRecord(organizationEventPositionId);
				$(this).prop('disabled', true);
				$(this).removeClass('btn-success');
				$(this).addClass('btn-default');
				$(this).prop('value', 'Spot Claimed');
				return false;
			});

			$('.delete-btn').on('click', function (e) {
				e.preventDefault(); // Prevent the default action

				var $thisButton = $(this); // Store the button reference
				var organizationEventPositionId = $thisButton.data('value');

				// SweetAlert 1.x confirmation
				swal({
					title: 'Are you sure?',
					text: 'Do you really want to delete this spot? All claimed spots will also be removed.',
					type: 'warning',
					showCancelButton: true,
					confirmButtonColor: '#d33',
					cancelButtonColor: '#3085d6',
					confirmButtonText: 'Yes, delete it!',
					cancelButtonText: 'No, cancel',
					closeOnConfirm: false, // Keep dialog open until confirmation action completes
					closeOnCancel: true
				}, function (isConfirm) {
					if (isConfirm) {
						// Call the AJAX function if confirmed
						deletePositionRecord(organizationEventPositionId);

						// Disable the button and update its appearance
						$thisButton.prop('disabled', true);
						$thisButton.removeClass('btn-danger');
						$thisButton.addClass('btn-default');
						var $prevButton = $thisButton.prev('.insert-btn');
						$prevButton.prop('disabled', true);
						$prevButton.addClass('btn-default');
						$thisButton.prop('value', 'Spot Deleted');

						// Show success message
						swal('Deleted!', 'The spot has been deleted.', 'success');
					} else {
						// Optional: Show cancellation message
						swal('Cancelled', 'The spot was not deleted.', 'error');
					}
				});
			});

			//// Attach click event to all buttons with class 'insert-btn'
			//$('.delete-btn').on('click', function () {
			//	// Get the organizationEventPositionId from the button's data-value attribute
			//	var organizationEventPositionId		= $(this).data('value');

			//	// Call the AJAX function
			//	deletePositionRecord(organizationEventPositionId);
			//	$(this).prop('disabled', true);
			//	$(this).removeClass('btn-danger');
			//	$(this).addClass('btn-default');
			//	$(this).prop('value', 'Spot Deleted');
			//	return false;
			//});
		});

		function copyToClipboard(link) {
			// Create a temporary input to hold the link
			var tempInput = document.createElement("input");
			tempInput.value = link;
			document.body.appendChild(tempInput);
			tempInput.select();
			document.execCommand("copy");
			document.body.removeChild(tempInput);

			// Optional: Show a message or alert
			alert("Link copied to clipboard!");
		}

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
			divMessageSuccess.hidden = true;

			divMessageTextBox.style.visibility = 'visible';

			btnSend.style.visibility = 'visible';
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

		function sendMessage(signedInUserFullName, organizationId, recipientsName, recipientsEmail, teamName, message) {
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

		function deletePositionRecord(organizationEventPositionId) {
			$.ajax({
				type: "POST",
				url: "/V1/Positions.aspx/DeleteUserOrganizationEventPosition",
				data: JSON.stringify({ organizationEventPositionId: organizationEventPositionId }),
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (response) {
					//alert(response.d); // Displays the response from the WebMethod
					toastr.success(response.d);
				},
				error: function (xhr, status, error) {
					console.error("Error: " + error);
				}
			});
		}

		function insertPositionRecord(organizationEventPositionId) {
			$.ajax({
				type: "POST",
				url: "/V1/Positions.aspx/InsertUserOrganizationEventPosition",
				data: JSON.stringify({ organizationEventPositionId: organizationEventPositionId, userId: '<%=_userId%>' }),
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (response) {
					//alert(response.d); // Displays the response from the WebMethod
					toastr.success(response.d);
				},
				error: function (xhr, status, error) {
					console.error("Error: " + error);
				}
			});
		}

		toastr.options = {
			"debug": false,
			"newestOnTop": false,
			"positionClass": "toast-top-center",
			"closeButton": true,
			"toastClass": "animated fadeInDown",
		};
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<uc1:PostionNavigation runat="server" ID="ucPostionNavigation" />
	<asp:Literal ID="litVolunteerInstructions" runat="server"></asp:Literal>
	<%--<div class="border-right border-left" style="height:300px;" runat="server" id="divMap">
		<iframe width="100%" height="100%" frameborder="0" style="border:0" src="https://www.google.com/maps/embed/v1/place?q=place_id:<%=googlePlacesId%>&key=<%=mapApiKey%>" allowfullscreen="true"></iframe>
	</div>--%>
	<%--<div class="input-group-btn pull-right">
        <button class="btn btn-default dropdown-toggle pull-right" data-toggle="dropdown" type="button">My Positions <span class="caret"></span></button>
        <ul class="dropdown-menu pull-right">
            <li><a href="/V1/MyPositions.aspx?type=all">All Deployments</a></li>
            <li class="divider"></li>
			<li><span style="margin-left:20px; font-weight:bold;">Upcoming</span></li>
            <li><a href="/V1/MyPositions.aspx?type=upcoming" class="font-weight-bold">SAFE Camp</a></li>
        </ul>
    </div>--%>
	<h3><literal id="litPageTitle" runat="server" class="font-normal"></literal></h3>
	Open a date and select Claim Spot to fill the position.
	<div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
	<asp:Repeater ID="rptPositionDate" runat="server" OnItemDataBound="rptPositionDate_ItemDataBound">
		<ItemTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="divHeading" runat="server">
                        <h4 class="panel-title">
							<asp:Literal ID="litDate" runat="server"></asp:Literal>
                        </h4>
                    </div>
                    <div runat="server" id="divCollapse" class="panel-collapse collapse" role="tabpanel">
                        <div class="panel-body">
							<table id="tblPositions" class="footable table table-stripped toggle-arrow-tiny" data-page-size="50" data-filter=#filter>
								<thead>
									<tr>
										<th data-toggle="true">Position</th>
										<th data-hide="phone">Participants</th>
										<th></th>
									</tr>
								</thead>
								<tbody>
									<asp:Repeater ID="rptPositions" runat="server" OnItemDataBound="rptPositions_ItemDataBound">
										<ItemTemplate>
											<tr>
												<td style="line-height:1.5em;">
													<asp:Label CssClass="text-info font-bold" ID="lblPosition" runat="server"></asp:Label>
													<br />
													<asp:Label CssClass="text-info" ID="lblTime" runat="server"></asp:Label>
													<div class="text-muted small M-0">
														<asp:Literal ID="litRemainingPositions" runat="server"></asp:Literal>
													</div>
													<asp:HyperLink ID="hypGetTrained" Font-Underline="true" Target="_blank" runat="server" Text="Get Trained"></asp:HyperLink>
													<i class="fa fa-book"></i>
												</td>
												<td>
													<asp:Literal ID="litParticipants" runat="server"></asp:Literal>
												</td>
												<td>
													<asp:Button ID="btnSignUp" runat="server"></asp:Button>
													<asp:Button ID="btnDeleteSpot" runat="server" Visible="false"></asp:Button>
													<asp:HyperLink ID="hypSignIn" runat="server" Visible="false"></asp:HyperLink>
												</td>
											</tr>
										</ItemTemplate>
									</asp:Repeater>
								</tbody>
								<tfoot>
									<tr>
										<td colspan="5">
											<ul class="pagination pull-right"></ul>
										</td>
									</tr>
								</tfoot>
							</table>
						</div>
					</div>
				</div>
		</ItemTemplate>
	</asp:Repeater>
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
	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
	<script src="/Homer/vendor/sweetalert/lib/sweet-alert.min.js"></script>
<script>


	$(window).on('load', function () {

		$('.footable').footable();
	});

</script>
</asp:Content>

<%--<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
	<div class="text-center" style="margin-bottom:10px;">
		<asp:HyperLink ID="hypBack" runat="server" text="Back to Deployment"></asp:HyperLink>
	</div>
	<br />
	<div id="divAddHome" class="alert alert-success text-center" style="margin-bottom:10px;">
		<asp:HyperLink ID="litLink" runat="server"></asp:HyperLink>
	</div>
</asp:Content>--%>