<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TeamLogo.ascx.cs" Inherits="V1_UserControls_TeamLogo" %>
<style>
    .stability-badge {
        width: 16px;
        height: 16px;
        border-radius: 50%;
        vertical-align: middle;
        margin-right: 0px;
    }
    .team-logo {
        width: 16px;
        height: 16px;
        border-radius: 50%;
        vertical-align: middle;
        margin-right: 5px;
    }
    .volunteer-name {
        margin-right: 2px;
    }
    .StreamLink {
        margin-right: 2px;
    }
    .user-name {
        margin-right: 8px;
        color: #050505;
        font-weight: bold;
        vertical-align: middle;
    }
    .Userlink {
        text-decoration: none !important;
    }
</style>
<asp:HyperLink ID="hypName" runat="server" CssClass="Userlink" Visible="false">
    <asp:Label ID="lblprofileusername" runat="server" CssClass="user-name" Visible="false"></asp:Label>
    <asp:Image ID="imgStabilityBadge" runat="server" CssClass="stability-badge profile" Visible="false" data-toggle="tooltip" data-placement="top" title="Stability Verified" />
</asp:HyperLink>
<asp:HyperLink ID="hypTeamLogo" runat="server" NavigateUrl="/V1/NonProfit/Default.aspx" CssClass="team-logo profile" Visible="false">
    <asp:Image ID="imgTeamLogo" runat="server" CssClass="team-logo profile" Visible="false" data-toggle="tooltip" data-placement="top" />
</asp:HyperLink>