<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true"
    CodeFile="FilteredResults.aspx.cs" Inherits="V1_FilteredResults" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .hpanel .panel-body {
    background: #fff;
    border: 1px solid #eaeaea;
    border-radius: 2px;
    padding: 20px;
    position: relative;
    margin-right: 190px;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-sm-4 col-lg-3">
        <div class="hpanel" runat="server" id="divMemberNavigation" visible="true">
            <div class="panel-body">
                <h5>SEARCH FILTER</h5>
                <ul class="mailbox-list">
                    <li>
                        <asp:HyperLink NavigateUrl="/V1/FilteredResults.aspx" runat="server">
            <i class="fa fa-search"></i> All
                        </asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink NavigateUrl="/V1/FilteredResults.aspx?searchType=Teams" runat="server">
            <i class="fa fa-users"></i> Teams
                        </asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink NavigateUrl="/V1/FilteredResults.aspx?searchType=TeamMembers" runat="server">
            <i class="fa fa-user"></i> People
                        </asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink NavigateUrl="/V1/FilteredResults.aspx?searchType=Deployments" runat="server">
            <i class="fa fa-street-view"></i> Deployments
                        </asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink NavigateUrl="/V1/FilteredResults.aspx?searchType=Skills" runat="server">
            <i class="fa fa-user-md"></i> Skills
                        </asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink NavigateUrl="/V1/FilteredResults.aspx?searchType=Resources" runat="server">
            <i class="fa fa-truck"></i> Resources
                        </asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink NavigateUrl="/V1/FilteredResults.aspx?searchType=Portals" runat="server">
            <i class="fa fa-globe"></i> Portals
                        </asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink NavigateUrl="/V1/FilteredResults.aspx?searchType=VolunteerOpportunities"
                            runat="server">
            <i class="fa fa-life-ring"></i> Volunteer Opportunities
                        </asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink NavigateUrl="/V1/FilteredResults.aspx?searchType=Posts" runat="server">
            <i class="fa fa-file-text"></i> Posts
                        </asp:HyperLink>
                    </li>
                </ul>
            </div>
        </div>

    </div>
    <div class="col-sm-8 col-lg-9">
    </div>
</asp:Content>

