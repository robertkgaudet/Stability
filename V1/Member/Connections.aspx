<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Connections.aspx.cs" Inherits="V1_Member_Connections" %>
<%@ Register Src="~/V1/UserControls/MemberNavigation.ascx" TagPrefix="uc1" TagName="MemberNavigation" %>
<%@ Register Src="~/V1/UserControls/TeamLogo.ascx" TagPrefix="uc1" TagName="TeamLogo" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.hpanel
		{
			margin-bottom:5px !important;
		}
		.member-panel-body
		{
			border-radius: 10px !important;
			margin-bottom: 0px !important;
		}
		/* Custom gutter class for rows */
		.row.no-gutter {
			margin-left: 0;
			margin-right: 0;
		}

		.row.no-gutter [class*="col-"] {
			padding-left: 1px;  /* Reduced gutter padding */
			padding-right: 1px;
		}

        .friendsGrid {
            display: grid;
            grid-template-columns: repeat(2, 1fr); /* Two columns layout */
            gap: 10px;
            width: 100%; /* Ensures the DataList stretches to 100% of the container */
            padding: 0;
            list-style: none;
        }
        
        .friendItem {
            padding: 10px;
            border: 1px solid #ccc;
            text-align: center;
        }

        /* Responsive styling for smaller screens */
        @media screen and (max-width: 768px) {
            .friendsGrid {
                grid-template-columns: 1fr;
            }
        }
		p
		{
			margin:0px !important;
		}
		.link-group {
			    margin-left: 80px;
               margin-top: -40px;
		}
	</style>
	<script>

		function UpdateConnection(button)
		{
			var friendId = button.getAttribute("data-id"); //Friendid
			var action = button.getAttribute("data-action");
			button.disabled = true;

			userId = '<%=userId%>'; //Signed in user

			$.ajax({
				url: '/V1/Member/Connections.aspx/UpdateConnection',
				method: 'POST',
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				data: JSON.stringify({ userId: userId, friendId: friendId, action: action }),
				success: function (response) {

					if (action == "remove")
					{
						$(button).text("Request Removed");
						//$(button).InnerText = "Request Removed";
					}
					else
					{
						$(button).text("Connection Approved").removeClass("btn-primary").addClass("btn-default");
						//$(button).InnerText = "Connection Approved"
					}

				},
				error: function (error) {
					console.error('Error loading events:', error);
				}
			});
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="container" style="padding-bottom:100px !important;">

		<div class="row justify-content-center" style="margin-top:40px;">
			<div class="col-sm-8 col-lg-9">
				<div class="hpanel">
					<div class="panel-body member-panel-body">
						<h4><asp:Literal ID="litPageName" runat="server"></asp:Literal></h4>
						<asp:Literal ID="litConnectionCount" runat="server"></asp:Literal><br />
						<a href="/V1/Member/Connections.aspx?status=Connected&userId=<%=userId%>">Connections</a>
						<%=divider%>
						<asp:HyperLink ID="linkConnectionsReceived" runat="server">Pending</asp:HyperLink>
						<%=divider%>
						<asp:HyperLink ID="linkConnectionsSent" runat="server">Sent</asp:HyperLink>
						<hr />

						<asp:Repeater ID="ConnectionsDataList" runat="server" OnItemDataBound="ConnectionsDataList_ItemDataBound">
							<ItemTemplate>
								<div class="col-xs-12 col-sm-6">
									<div class="hpanel">
										<div class="panel-body member-panel-body">
											<p style="font-size:16px;">
												<img class="img-circle img-small" src="<%=profilePhotoFolder%><%# string.IsNullOrEmpty(Eval("ProfileImage") as string) ? "icons8-customer-64.png" : Eval("ProfileImage")%>" />
													<uc1:TeamLogo runat="server" ID="ucTeamLogo" />
												<div <%=hideFriendControls%> class="block">
													<button data-id="<%#Eval("UserId")%>" data-action="remove" onclick="UpdateConnection(this); return false;" class="btn btn-default pull-right remove-button" <%=hideDeleteButton%>>Remove</button>
													<button data-id="<%#Eval("UserId")%>" data-action="confirm" onclick="UpdateConnection(this); return false;" class="btn btn-primary pull-right confirm-button" <%=hideConfirmButton%>>Confirm</button>
												</div>
											</p>
										</div>
									</div>
								</div>
							</ItemTemplate>
						</asp:Repeater>
					</div>
				</div>
			</div>

			<!-- NAVIGATION -->
			<div class="col-sm-4 col-lg-3">
				<uc1:MemberNavigation runat="server" ID="ucMemberNavigation" />
			</div>
		</div>
	</div>
</asp:Content>