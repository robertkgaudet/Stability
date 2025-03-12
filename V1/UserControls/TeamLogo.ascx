<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TeamLogo.ascx.cs" Inherits="V1_UserControls_TeamLogo" %>

<style>
 .team-logo, .stability-badge {
    width: 16px; 
    height: 16px;
    border-radius: 50%;
    margin-left: -3px;
    vertical-align: text-top; 
    margin-right: 2px;
    margin-top: -2px; 
}

    .volunteer-name{
	margin-right:2px;
   }
     .StreamLink {
     margin-right:2px;
 }

   
</style>
<script type="text/javascript">
    function redirectToPage() {
        window.location.href = '/V1/NonProfit/Default.aspx';
    }
</script>
<asp:Image ID="imgStabilityBadge" runat="server" CssClass="stability-badge profile " Visible="false"  ToolTip="Stability Verified" />




<asp:Image ID="imgTeamLogo" runat="server" CssClass="team-logo profile" Visible="false"  ToolTip="Verified" onclick="redirectToPage()" />
