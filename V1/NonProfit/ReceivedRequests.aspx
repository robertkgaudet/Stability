<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/V1/MasterPages/Homer.master"
    CodeFile="ReceivedRequests.aspx.cs" Inherits="V1_NonProfit_ReceivedRequests" %>

<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script>
        function confirmJoinTeam(senderId, organizationId) {
            debugger
            event.preventDefault();
            swal({
                title: "Are you sure you want this user to join the team?",
                icon: "success",
                buttons: ["No", "Yes, Join!"],
            }).then(function (confirmed) {
                if (confirmed) {
                    $.ajax({
                        type: "POST",
                        url: "ReceivedRequests.aspx/JoinTeam",
                        data: JSON.stringify({ senderId: senderId, organizationId: organizationId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            swal("Success", response.d, "success");
                            location.reload();
                        },
                        error: function (xhr, status, error) {
                            console.error("Error:", error);
                            swal("Error", "An error occurred while processing the request.", "error");
                        }
                    });
                }
            });
        }
        function confirmReject(senderId, organizationId) {
            event.preventDefault();
            swal({
                title: "Are you sure you want to reject this request?",
                icon: "warning",
                buttons: ["No", "Yes, Reject!"],
                dangerMode: true
              }).then(function (confirmed) {
                if (confirmed) {
                    $.ajax({
                        type: "POST",
                        url: "ReceivedRequests.aspx/RejectRequest",
                        data: JSON.stringify({ senderId: senderId, organizationId: organizationId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            swal("Rejected", response.d, "success");
                            location.reload();
                        },
                        error: function () {
                            swal("Error", "Something went wrong while rejecting.", "error");
                        }
                    });
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:TeamHeader runat="server" ID="ucTeamHeader" />
    <table class="table table-bordered">
        <thead>
            <tr>
                <th>User Details</th>
                <th style="width: 200px; text-align: center;">Actions</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptRequests" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("Message") %></td>
                        <td style="text-align: center;">
                            <button type="button" class="btn btn-success"
                                onclick="confirmJoinTeam('<%# Eval("SenderId") %>', '<%= organizationId %>')">
                                Accept
                            </button>
                            <button type="button" class="btn btn-danger"
                                onclick="confirmReject('<%# Eval("SenderId") %>', '<%= organizationId %>')">
                                Reject
                            </button>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>

