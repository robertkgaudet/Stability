<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="TeamManagement.aspx.cs" Inherits="V1_NonProfit_TeamRoles" %>

<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .response-tools {
/*            margin-bottom: 40px;*/
                padding: 15px;
        }

            .response-tools h2 {
                font-size: 20px;
                font-weight: bold;
                color: #222;
                margin-bottom: 20px;
            }

        .icon-row {
                margin-top: 20px;
               display: flex;
    gap: 70px;
    flex-wrap: wrap;
    padding: 10px;
        }

        .icon-item {
            display: flex;
            align-items: center;
            font-size: 18px;
            color: #2563eb;
        }

            .icon-item i {
                margin-right: 8px;
            }
    </style>
    <uc1:TeamHeader runat="server" ID="ucTeamHeader" />

    <div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
        <div class="response-tools">
            <h4><u>Administrative Tools</u></h4>
            <div class="icon-row">
                <div class="icon-item">
                    <i class="fa fa-users text-success"></i><span>
                        <asp:HyperLink runat="server" ID="hypInviteTeam" CssClass="inviteButton"> Invite Team Members
                        </asp:HyperLink></span>
                </div>
                <div class="icon-item">
                    <i class="fa fa-tachometer text-primary"></i><span>
                        <asp:HyperLink runat="server" ID="hypDonationDashboard" CssClass="donationDashboard">  Donations Dashboard
                        </asp:HyperLink></span>
                </div>
                <div class="icon-item"><i class="fa fa-indent"></i><span>
                    <asp:HyperLink runat="server" ID="hypInvitedMembers"> Invited Members</asp:HyperLink></span></div>

            </div>
        </div>
        <div class="response-tools-section">
            <div class="response-tools">
                <h4><u>Image Manager</u></h4>
                <div class="icon-row">
                     <div class="icon-item">
     <i class="fa fa-upload text-primary"></i><span>
         <asp:HyperLink runat="server" ID="hypSquareLogoUpload" CssClass="squareLogoUploadButton">Upload Team Logo
         </asp:HyperLink></span>
 </div>
                      <div class="icon-item">
      <i class="fa fa-upload text-primary"></i><span>
          <asp:HyperLink runat="server" ID="hypLogoUpload" CssClass="squareLogoUploadButton"> Upload Website Logo
          </asp:HyperLink></span>
  </div>
                    <div class="icon-item">
                        <i class="fa fa-image text-primary"></i><span>
                            <asp:HyperLink runat="server" ID="hypCoverImageUpload" CssClass="coverUploadButton">  Upload Cover Image
                            </asp:HyperLink></span>
                    </div>
                   
                    <div class="icon-item">
                        <i class="fa fa-camera text-primary"></i><span>
                            <asp:HyperLink runat="server" ID="hypManagePhotos" CssClass="managePhotosButton"> Manage Photos
                            </asp:HyperLink></span>
                    </div>
                  

                </div>
            </div>

            <div class="response-tools">
                <h4><u>Message All Team Members</u></h4>

                <div class="icon-row">
                    <div class="icon-item">
                        <i class="fa fa-envelope"></i><span>
                            <asp:HyperLink runat="server" ID="hypMail" Visible="true">Email Team Members </asp:HyperLink>
                        </span>
                    </div>
                    <div class="icon-item"><i class="fa fa-file-text text-warning"></i><span>
                        <asp:HyperLink runat="server" ID="hypSms" Visible="true">Text Team Members</asp:HyperLink></span></div>

                </div>
            </div>

            <div class="response-tools">
                <h4><u>Administrative Tools</u></h4>

                <div class="icon-row">
                    <div class="icon-item">
                        <i class="fa fa-clipboard text-danger"></i><span>
                            <asp:HyperLink runat="server" ID="hypTickets"> Tickets</asp:HyperLink>
                        </span>
                    </div>
                    <div class="icon-item">
                        <i class="fa fa-file-text text-warning"></i><span>
                            <asp:HyperLink runat="server" ID="hypReports"> Reports</asp:HyperLink>
                        </span>
                    </div>
                    <div class="icon-item">
                        <i class="fa fa-cog"></i><span>
                            <asp:HyperLink runat="server" ID="hypSettings"> Settings</asp:HyperLink>
                        </span>
                    </div>
                    <div class="icon-item">
                        <i class="fa fa-pencil text-warning"></i><span>
                            <asp:HyperLink runat="server" ID="hypUpdateTeamInfo" CssClass="editButton"> Update Team Information
                            </asp:HyperLink></span>
                    </div>
                    <div class="icon-item">
                        <i class="fa fa-ban text-danger"></i><span>
                            <asp:LinkButton ID="btnDeactivatePage" runat="server" CssClass="deactivateButton" OnClick="btnChangePageStatus_Click"> De-activate This Team
                            </asp:LinkButton></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>

