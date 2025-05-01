<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TeamLogo.ascx.cs" Inherits="V1_UserControls_TeamLogo" %>
<style>
    .stability-badge {
        width: 12px;
        height: 12px;
        border-radius: 50%;
        vertical-align: middle;
        margin-right: 0px;
    }
    .volunteer-name {
        margin-right: 2px;
    }
    .user-name {
        margin-right: 0px;
        color: #050505;
        font-weight: bold;
        vertical-align: middle;
    }
    .Userlink {
        text-decoration: none !important;
    }
	.link-group {
    display: flex;
    align-items: center; /* vertically centers text and images */
    gap: 2px; /* adjust spacing between links as needed */
}

.link-group a img {
    height: 20px; /* adjust height to match the text line nicely */
    width: auto;
    vertical-align: middle;
}
</style>
<div class="link-group">
<asp:HyperLink ID="hypName" runat="server" CssClass="Userlink" Visible="false">
    <asp:Label ID="lblprofileusername" runat="server" CssClass="user-name" Visible="false"></asp:Label>
</asp:HyperLink>
<asp:HyperLink ID="hypStabilityLogo" runat="server" CssClass="Userlink" Visible="true" Style="display: none;">
    <asp:Image ID="imgStabilityBadge" runat="server" CssClass="stability-badge" Visible="true" Style="display: none;" data-toggle="tooltip" data-placement="top" title="Stability Verified" />
</asp:HyperLink>
<asp:HyperLink ID="hypTeamLogo" runat="server" NavigateUrl="/V1/NonProfit/Default.aspx" CssClass="Userlink" Visible="true" Style="display: none;">
    <asp:Image ID="imgTeamLogo" runat="server" CssClass="stability-badge" Visible="true" Style="display: none;" data-toggle="tooltip" data-placement="top" />
</asp:HyperLink>
</div>