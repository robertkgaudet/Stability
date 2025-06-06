<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PositionNavigation1.ascx.cs" Inherits="V1_UserControls_PositionNavigation1" %>

    <div class="container" style="margin-bottom:15px;">
        <h3>Deployment: <asp:Literal id="litCampaignName" runat="server"></asp:Literal></h3>
		Portal: <asp:HyperLink ID="hypEventName" runat="server"></asp:HyperLink><br />
		Team: <asp:HyperLink id="hypOrganization" runat="server"></asp:HyperLink>
    </div>

<div id="divNavigation" runat="server" visible="false">
	<style>
		.activeLink{
			font-weight:bold;
			text-decoration:underline;
		}
		i{
			color:#5E2E91;
		}
		.activeLink:hover{
			color:#FFFDEA;
		}
		.postions-navigation-menu a{
			color:#5E2E91;
			margin-right:18px;
			font-size:15px;
		}
		.postions-navigation-menu a:hover{
			color:#1E90FF;
		}
		.child-master-header
		{
			background-color:#E8D3FE;
			padding:10px;	
			color:white;
		}
		        .row {
            margin-left: -5px; /* Adjust the left margin */
            margin-right: -5px; /* Adjust the right margin */
        }
        .grid-item {
            padding-left: 5px; /* Decrease column padding (gutter) */
            padding-right: 5px; /* Decrease column padding (gutter) */
            padding: 3px;
        }
	</style>
	<div class="child-master-header">
		<div class="postions-navigation-menu">
            <a href="/V1/NonProfitAdministration/PositionsNeeded1.aspx?organizationEventId=<%=_organizationEventId%>" <%=_positionsActive%>>Add New Positions</a>
			<a href="/V1/NonProfitAdministration/Participants.aspx?organizationEventId=<%=_organizationEventId%>" <%=_participantsActive%>>View Participant List</a>
	        <a href="javascript:void(0);" onclick="openInviteModal()">Invite Team Members</a>
            <span class="pull-right"> <i class="fa fa-arrow-circle-o-up"></i> <b><a href="/SignUp/<%=_urlFriendlyName%>"  <%=_viewPositionsActive%>>View Open Positions</a></b></span>
		</div>
	</div>
</div>