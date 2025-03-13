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
        <h5>Settings</h5>
        <asp:HyperLink ID="hypAccountSettings" runat="server"
            NavigateUrl="/V1/Profile/AccountSettings.aspx">
    Account Settings
        </asp:HyperLink><br />

        <asp:HyperLink ID="hypPrivacySettings" runat="server"
            NavigateUrl="/V1/Profile/PrivacySettings.aspx">
    Privacy Settings
        </asp:HyperLink><br />

        <asp:HyperLink ID="hypIDCard" runat="server"
            NavigateUrl="/IDCard">
    ID Card
        </asp:HyperLink><br />

        <asp:HyperLink ID="hypTimeSheet" runat="server"
            NavigateUrl="/V1/Profile/Time.aspx">
    Time Sheet
        </asp:HyperLink><br />

        <asp:HyperLink ID="hypCalendar" runat="server"
            NavigateUrl="/V1/Profile/AvailableDates.aspx">
    <i class="fa fa-calendar"></i> Calendar
        </asp:HyperLink><br />

        <asp:HyperLink ID="HyperLink1" runat="server"
            NavigateUrl="/V1/Profile/EditNonProfitCauses.aspx">
    <i class="fa fa-street-view"></i> Deployments
        </asp:HyperLink><br />

        <asp:HyperLink ID="hypPortals" runat="server"
            NavigateUrl="/V1/Profile/EditDisasters.aspx">
    <i class="fa fa-map-marker"></i> Portals
        </asp:HyperLink><br />

        <asp:HyperLink ID="hypSkills" runat="server"
            NavigateUrl="/V1/Profile/EditSkills.aspx">
    <i class="fa fa-hand-pointer-o"></i> Skills
        </asp:HyperLink><br />

        <asp:HyperLink ID="hypResources" runat="server"
            NavigateUrl="/V1/Profile/EditResources.aspx">
    <i class="fa fa-truck"></i> Resources
        </asp:HyperLink><br />

        <asp:HyperLink ID="hypSignOut" runat="server"
            NavigateUrl="/V1/SignOut.aspx">
       Sign Out
        </asp:HyperLink><br />

    </div>
</div>
