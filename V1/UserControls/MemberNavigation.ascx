<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MemberNavigation.ascx.cs" Inherits="V1_UserControls_MemberNavigation" %>

	<div class="hpanel" runat="server" id="divMemberNavigation" visible="false">
		<div class="panel-body">
				<h5>Account</h5>
				<asp:HyperLink ID="hypMyProfile" runat="server" NavigateUrl="/V1/Member/Default.aspx">My Profile</asp:HyperLink><br />
				<h5>Connect</h5>
				<a href="/V1/Member/Connections.aspx?userId=<%=_userId %>">My Connections</a><br />
				<a href="/V1/Member/PeopleSearch.aspx">Find Connections</a><br />
				<a href="/V1/NonProfit/TeamList.aspx">Find Teams</a><br />
				<h5>Group/Team</h5>
				<asp:HyperLink ID="hypMyTeam" runat="server">My Team</asp:HyperLink><br />
				<asp:HyperLink ID="hypPositions" runat="server" NavigateUrl="/V1/Member/Positions.aspx">My Scheduled Positions</asp:HyperLink><br />
				<asp:HyperLink ID="hypDeployments" CssClass="font-weight-bold" runat="server">Find Open Positions</asp:HyperLink><br />
				
		</div>
	</div>
<%--	<div class="hpanel">
		<div class="panel-body member-panel-body">
				Featured Team Here
		</div>
	</div>
	<div class="hpanel">
		<div class="panel-body member-panel-body">
				Featured Portal Here
		</div>
	</div>
	<div class="hpanel">
		<div class="panel-body member-panel-body">
				Top Team Here
		</div>
	</div>
	<div class="hpanel">
		<div class="panel-body member-panel-body">
				Featured Deployment Here
		</div>
	</div>
	<div class="hpanel">
		<div class="panel-body member-panel-body">
				Featured Program Here
		</div>
	</div>--%>