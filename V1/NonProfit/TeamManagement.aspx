<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true"
    CodeFile="TeamManagement.aspx.cs" Inherits="V1_NonProfit_TeamRoles" %>

<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <style>
        .card-body {
            padding: 5px;
        }

        .align-items-start {
            margin-top: 10px;
            font-size: 15px;
            display: flex;
            align-items: center;
            gap: 8px;
            font-weight: 500;
            text-decoration: none;
            transition: all 0.2s ease-in-out;
        }



        .card-header {
            height: 25px;
            font-size: 1rem;
        }

        u {
            text-decoration: none;
        }

        .card-header-container {
            margin-top: 40px;
        }
    </style>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <script type="text/javascript">
        function confirmDeactivation() {
            var btn = document.getElementById('<%= btnDeactivatePage.ClientID %>');
            var text = btn.innerText.trim().toLowerCase();
            var isReactivation = text.includes("re-activate");
            var action = isReactivation ? "reactivate" : "deactivate";
            var confirmText = isReactivation ? "Yes, reactivate it!" : "Yes, deactivate it!";
            var messageText = "You are about to " + action + " this team.";
            swal({
                title: "Are you sure?",
                text: messageText,
                icon: "warning",
                buttons: {
                    cancel: "Cancel",
                    confirm: {
                        text: confirmText,
                        value: true,
                        visible: true,
                        className: "",
                        closeModal: true
                    }
                },
            }).then((willChange) => {
                if (willChange) {
                    __doPostBack('<%= btnDeactivatePage.UniqueID %>', '');

                }
            });

            return false;
        }
    </script>

    <uc1:TeamHeader runat="server" ID="ucTeamHeader" />

    <div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
        <div class="container-fluid response-tools-section">
            <!-- Admin Tools -->
            <div class="card-header-container">
                <div class="card-header">
                    <h4><u>Administrative Tools</u></h4>
                </div>
                <div class="card-body row gy-3">
                    <div class="col-md-4 d-flex align-items-start gap-2">
                        <i class="fa fa-users text-success"></i>
                        <asp:HyperLink runat="server" ID="hypNewTeamMember" CssClass="text-decoration-none">New Team Members</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2">
                        <i class="fa fa-user-plus text-success"></i>
                        <asp:HyperLink runat="server" ID="hypInviteTeam" CssClass="text-decoration-none">Invite Team Members</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2">
                        <i class="fa fa-indent"></i>
                        <asp:HyperLink runat="server" ID="hypInvitedMembers">Invited Members</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2" runat="server" visible="false"
                        id="hypDonationDashboards">
                        <i class="fa fa-tachometer text-primary"></i>
                        <asp:HyperLink runat="server" ID="hypDonationDashboard">Donations Dashboard</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2" runat="server" visible="false"
                        id="hiddenRequest">
                        <i class="fa fa-user-plus"></i>
                        <asp:HyperLink runat="server" ID="hypRequest">Pending Requests</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2" runat="server" id="divCreateWaiver">
                        <i class="fa fa-file-text text-warning"></i>
                        <asp:HyperLink runat="server" ID="hypCreateWaiver" Visible="true">Set up a waiver screen</asp:HyperLink>
                    </div>
                </div>
            </div>

            <!-- Image Manager -->
            <div class="card-header-container">
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
            <div class="card-header-container">
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
            <div class="card-header-container">
                <div class="card-header">
                    <h4><u>Others</u></h4>
                </div>
                <div class="card-body row gy-3">
                    <div class="col-md-3 d-flex align-items-start gap-2" runat="server" visible="false"
                        id="hypSettingss">
                        <i class="fa fa-cog"></i>
                        <asp:HyperLink runat="server" ID="hypSettings">Settings</asp:HyperLink>
                    </div>
                    <div class="col-md-3 d-flex align-items-start gap-2">
                        <i class="fa fa-clipboard text-danger"></i>
                        <asp:HyperLink runat="server" ID="hypTickets">Tickets</asp:HyperLink>
                    </div>
                    <div class="col-md-3 d-flex align-items-start gap-2">
                        <i class="fa fa-file-text text-warning"></i>
                        <asp:HyperLink runat="server" ID="hypReports">Reports</asp:HyperLink>
                    </div>
                    <div class="col-md-3 d-flex align-items-start gap-2" runat="server" visible="false"
                        id="hypUpdateTeamInfos">
                        <i class="fa fa-pencil text-warning"></i>
                        <asp:HyperLink runat="server" ID="hypUpdateTeamInfo">Update Team Info</asp:HyperLink>
                    </div>
                    <div class="col-md-4 d-flex align-items-start gap-2 mt-3" runat="server" visible="false"
                        id="btnDeactivatePages">
                        <i class="fa fa-ban text-danger"></i>
                        <asp:LinkButton ID="btnDeactivatePage" runat="server" OnClick="btnChangePageStatus_Click"
                            OnClientClick=" return confirmDeactivation() ;"> 

                             <i class="fa fa-ban text-danger me-1"></i>
    <span id="deactivateText"><%# btnDeactivatePage.Text %></span>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>

