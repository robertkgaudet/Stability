<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PositionNavigation.ascx.cs" Inherits="V1_UserControls_PositionNavigation" %>

	<style>
		.activeLink{
			font-weight:bold;
			text-decoration:underline;
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
	</style>
	<div class="child-master-header">
		<div class="postions-navigation-menu">
            <a href="/V1/NonProfit/NonProfitCampaign.aspx?organizationEventId=<%=_organizationEventId%>" <%=_detailsActive%>>Details</a>
            <a href="PositionsNeeded.aspx?organizationEventId=<%=_organizationEventId%>" <%=_positionsActive%>>Postions</a>
			<a href="Participants.aspx?organizationEventId=<%=_organizationEventId%>" <%=_participantsActive%>>Participants</a>
            <a href="EventMessages.aspx?organizationEventId=<%=_organizationEventId%>" <%=_messagesActive%>>Message</a>
            <a href="EventReports.aspx?organizationEventId=<%=_organizationEventId%>" <%=_reportsActive%>>Reports</a>
            <a href="EventInvitations.aspx?organizationEventId=<%=_organizationEventId%>" <%=_invitesActive%>>Invite</a>
            <a href="EventMoney.aspx?organizationEventId=<%=_organizationEventId%>" <%=_moneyActive%>>Collect Money</a>
            <a href="EventSettings.aspx?organizationEventId=<%=_organizationEventId%>" <%=_settingsActive%>>Settings</a>
		</div>
	</div>