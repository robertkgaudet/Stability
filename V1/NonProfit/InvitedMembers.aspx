<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true"
    EnableEventValidation="false" ValidateRequest="false" CodeFile="InvitedMembers.aspx.cs"
    Inherits="V1_NonProfit_InvitedMembers" %>

<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ Register Src="~/V1/UserControls/TeamLogo.ascx" TagPrefix="uc1" TagName="TeamLogo" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
    <script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/css/bootstrap-multiselect.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/js/bootstrap-multiselect.min.js"></script>
    <link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
    <script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
    <script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
    <link href="../../Homer/vendor/sweetalert/lib/sweet-alert.css" rel="stylesheet" />
    <script src="../../Homer/vendor/sweetalert/lib/sweet-alert.min.js"></script>

    <link rel="stylesheet" href="../../Homer/vendor/ladda/dist/ladda-themeless.min.css" />
    <script src="../../Homer/vendor/ladda/dist/spin.min.js"></script>
    <script src="../../Homer/vendor/ladda/dist/ladda.min.js"></script>
    <script src="../../Homer/vendor/ladda/dist/ladda.jquery.min.js"></script>

    <style>
        table {
            width: 80%;
            border-collapse: collapse;
            margin: 20px auto;
            font-family: Arial, sans-serif;
        }

        th, td {
            border: 1px solid #ccc;
            padding: 12px;
            text-align: center;
        }

        th {
            background-color: #f4f4f4;
        }
    </style>
    <script>
        $(document).ready(function () {
            $('.ResendInvite').on('click', function () {
                var userOrgInviteId = $(this).data('id');
                debugger;
                $.ajax({
                    type: "POST",
                    url: "/V1/NonProfit/InvitedMembers.aspx/ResendInviteById",
                    data: JSON.stringify({ userOrgInviteId: userOrgInviteId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d) {
                            alert('Invite Resend Successfully!');
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error("Error: " + error);
                    }
                });
            });

            $('.CancelInvite').on('click', function () {
                var userOrgInviteId = $(this).data('id');
                $.ajax({
                    type: "POST",
                    url: "/V1/NonProfit/InvitedMembers.aspx/CancelInviteById",
                    data: JSON.stringify({ userOrgInviteId: userOrgInviteId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d) {
                            alert('Invite Cancel Successfully!');
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error("Error: " + error);
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
        <div class="post-container">
            <div class="post-content">
                <div class="col-md-12">
                    <table class="table table-responsive table-bordered">
                        <thead>
                            <tr>
                                <th>Email Address</th>
                                <th>Status</th>
                                <th>Invited Date</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptInvitedMembers" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("EmailAddress") %></td>
                                        <td><%# Eval("Status") %></td>
                                        <td><%# Eval("CreatedOn", "{0:dd-MM-yyyy}") %></td>

                                        <td>
                                            <button class="btn btn-primary ResendInvite" data-id='<%# Eval("UserOrganizationInviteId") %>'>Resend Invite</button>
                                            <button class="btn btn-danger CancelInvite" data-id='<%# Eval("UserOrganizationInviteId") %>'>Cancel Invite</button>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table> 
                </div>
            </div>
        </div>
    </div>
</asp:Content>
