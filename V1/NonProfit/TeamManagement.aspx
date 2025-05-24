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
            gap: 50px;
            flex-wrap: wrap;
            padding: 6px;
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

        .tool-link {
            display: flex;
            align-items: center;
            gap: 8px;
            font-weight: 500;
            color: #007bff;
            text-decoration: none;
            transition: all 0.2s ease-in-out;
        }

            .tool-link:hover {
                text-decoration: underline;
                color: #0056b3;
            }

        .card-header {
            background-color: #f8f9fa;
            font-size: 1rem;
        }

        .card-body {
            margin: 5px;
            padding: 5px;
        }

        .align-items-start {
            margin-top: 20px;
            font-size: 17px;
            display: flex;
            align-items: center;
            gap: 8px;
            font-weight: 500;
            text-decoration: none;
            transition: all 0.2s ease-in-out;
        }
        	.col-md-4.d-flex.align-items-start.gap-2 {
        margin-top: 10px;
        }
	.col-md-3.d-flex.align-items-start.gap-2 {
    margin-top: 3px;
     }
	.card-header{
		background-color:#fff !important;
		height:25px;
	}
    u{
	text-decoration:none;
}
    </style>
    <uc1:TeamHeader runat="server" ID="ucTeamHeader" />

    <div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
        <div class="container-fluid response-tools-section">
            <!-- Admin Tools -->
            <div class="card mb-4">
                <div class="card-header">
                    <h4><u>Administrative Tools</u></h4>
                </div>
                <div class="card-body row gy-3">
                    <div class="col-md-4 d-flex align-items-start gap-2">
                        <i class="fa fa-users text-success"></i>
                        <asp:HyperLink runat="server" ID="hypInviteTeam" CssClass="text-decoration-none">Invite Team Members</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2">
                        <i class="fa fa-tachometer text-primary"></i>
                        <asp:HyperLink runat="server" ID="hypDonationDashboard">Donations Dashboard</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2">
                        <i class="fa fa-indent"></i>
                        <asp:HyperLink runat="server" ID="hypInvitedMembers">Invited Members</asp:HyperLink>
                    </div>
                </div>
            </div>

            <!-- Image Manager -->
            <div class="card mb-4">
                <div class="card-header">
                    <h4><u>Image Manager</u></h4>
                </div>
                <div class="card-body row gy-3">
                    <div class="col-md-4 d-flex align-items-start gap-2">
                        <i class="fa fa-upload text-primary"></i>
                        <asp:HyperLink runat="server" ID="hypSquareLogoUpload">Upload Team Logo</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2">
                        <i class="fa fa-upload text-primary"></i>
                        <asp:HyperLink runat="server" ID="hypLogoUpload">Upload Website Logo</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2">
                        <i class="fa fa-image text-primary"></i>
                        <asp:HyperLink runat="server" ID="hypCoverImageUpload">Upload Cover Image</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2">
                        <i class="fa fa-camera text-primary"></i>
                        <asp:HyperLink runat="server" ID="hypManagePhotos">Manage Photos</asp:HyperLink>
                    </div>
                </div>
            </div>

            <!-- Messaging -->
            <div class="card mb-4">
                <div class="card-header">
                    <h4><u>Message All Team Members</u></h4>
                </div>
                <div class="card-body row gy-3">
                    <div class="col-md-4 d-flex align-items-start gap-2">
                        <i class="fa fa-envelope"></i>
                        <asp:HyperLink runat="server" ID="hypMail" Visible="true">Email Team Members</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2">
                        <i class="fa fa-file-text text-warning"></i>
                        <asp:HyperLink runat="server" ID="hypSms" Visible="true">Text Team Members</asp:HyperLink>
                    </div>
                </div>
            </div>

            <!-- Others -->
            <div class="card mb-4">
                <div class="card-header">
                    <h4><u>Others</u></h4>
                </div>
                <div class="card-body row gy-3">
                    <div class="col-md-3 d-flex align-items-start gap-2">
                        <i class="fa fa-clipboard text-danger"></i>
                        <asp:HyperLink runat="server" ID="hypTickets">Tickets</asp:HyperLink>
                    </div>
                    <div class="col-md-3 d-flex align-items-start gap-2">
                        <i class="fa fa-file-text text-warning"></i>
                        <asp:HyperLink runat="server" ID="hypReports">Reports</asp:HyperLink>
                    </div>
                    <div class="col-md-3 d-flex align-items-start gap-2">
                        <i class="fa fa-cog"></i>
                        <asp:HyperLink runat="server" ID="hypSettings">Settings</asp:HyperLink>
                    </div>
                    <div class="col-md-3 d-flex align-items-start gap-2">
                        <i class="fa fa-pencil text-warning"></i>
                        <asp:HyperLink runat="server" ID="hypUpdateTeamInfo">Update Team Info</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2 mt-3">
                        <i class="fa fa-ban text-danger"></i>
                        <asp:LinkButton ID="btnDeactivatePage" runat="server" OnClick="btnChangePageStatus_Click">Deactivate This Team</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>

