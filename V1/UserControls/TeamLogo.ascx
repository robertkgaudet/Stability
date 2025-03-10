<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TeamLogo.ascx.cs" Inherits="V1_UserControls_TeamLogo" %>

<style>
    .team-logo, .stability-badge {
         width: 20px;
    height: 20px;
    border-radius: 50%;
    margin-left: -3px;
    vertical-align: middle;
    margin-right:2px;
  
    }
    .volunteer-name{
	margin-right:2px;
   }
     .StreamLink {
     margin-right:2px;
 }

   
</style>
<asp:Image ID="imgStabilityBadge" runat="server" CssClass="stability-badge" Visible="false" />


<asp:Image ID="imgTeamLogo" runat="server" CssClass="team-logo" Visible="false" />
