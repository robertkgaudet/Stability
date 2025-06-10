<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchFilter.ascx.cs"
    Inherits="V1_UserControls_SearchFilter" %>
<div class="hpanel" runat="server" id="divMemberNavigation" visible="false">
    <div class="panel-body">
        <ul class="mailbox-list">
            <li>
                <asp:HyperLink ID="hypFeeds" runat="server" NavigateUrl="/feed">
                    <i class="fa fa-th-list"></i> Feed
                </asp:HyperLink>
            </li>
        </ul>
        <hr />

        <h5>MY ACCOUNT</h5>
        <ul class="mailbox-list">
            <li>
                <asp:HyperLink ID="hypMyProfile" runat="server"  NavigateUrl="/V1/Member/Default.aspx" >
                    <i class="fa fa-user"></i> Profile
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hypMyTeam" runat="server">
                    <i class="fa fa-users"></i> Team
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hypMyConnections" runat="server" NavigateUrl="/V1/Member/Connections.aspx">
         <i class=" fa fa-share-alt "></i> My Connections
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hypCalendar" runat="server" NavigateUrl="/V1/Profile/AvailableDates.aspx">
                    <i class="fa fa-calendar"></i> Calendar
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hypSkills" runat="server" NavigateUrl="/V1/Profile/EditSkills.aspx">
                <i class="fa fa-hand-pointer-o"></i> Skills
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hypResources" runat="server" NavigateUrl="/V1/Profile/EditResources.aspx">
        <i class="fa fa-truck"></i> Resources
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hypIDCard" runat="server" NavigateUrl="/IDCard">
                    <i class="fa fa-id-card"></i> ID Card
                </asp:HyperLink>
            </li>
        </ul>
        <hr runat="server" id="hr1"></hr>

        <!-- TAKE ACTION Section -->
        <h5>TAKE ACTION</h5>
        <ul class="mailbox-list">
            <li>
                <asp:HyperLink ID="hypDeployments" CssClass="font-weight-bold" runat="server">
                    <i class="fa fa-life-ring"></i> Find Volunteer Opportunities
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hypConnections" CssClass="font-weight-bold" runat="server" NavigateUrl="/V1/Member/PeopleSearch.aspx">
               <i class="fa fa-search"></i> Find Connections
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hypPositions" runat="server" NavigateUrl="/V1/Member/Positions.aspx">
                    <i class="fa fa-calendar"></i> My Volunteer Schedule
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hypDeployment" runat="server" NavigateUrl="/V1/Profile/EditNonProfitCauses.aspx">
                    <i class="fa fa-street-view"></i> Deployments
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hypPortals" runat="server" NavigateUrl="/V1/Profile/EditDisasters.aspx">
                    <i class="fa fa-globe"></i> Portals
                </asp:HyperLink>
            </li>
        </ul>
        <hr runat="server" id="hr2"></hr>
        <h5>SETTINGS</h5>
        <ul class="mailbox-list">
            <li>
                <asp:HyperLink ID="hypTimeSheet" runat="server" NavigateUrl="/V1/Profile/Time.aspx">
                    <i class="fa fa-clock-o"></i> Time Sheet
                </asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="hypSignOut" runat="server" NavigateUrl="/V1/SignOut.aspx">
                    <i class="fa fa-sign-out"></i> Sign Out
                </asp:HyperLink>
            </li>
        </ul>
    </div>
</div>
