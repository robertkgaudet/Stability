<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true"  CodeFile="Settings.aspx.cs" Inherits="V1_NonProfit_Settings" %>

<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="/Homer/vendor/iCheck/icheck.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input[type="checkbox"]').each(function () {
                $(this).addClass("i-checks");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <uc1:TeamHeader runat="server" ID="ucTeamHeader" />
    <div class="panel-heading hbuilt">
        <div class="font-normal">
            <h1 class="m-b-none"><i class="fa fa-cog"></i>Settings</h1>
            <small class="text-muted">Adjust Team Settings.</small>
        </div>
    </div>
    <div class="panel-body">
        <div class="form-group">
            <div id="divUpdateMessage" runat="server" class="alert alert-success text-center" visible="false">
                <i class="fa fa-2x fa-check-circle"></i>
                <hr />
                Update Complete
            </div>
            <div class="col-sm-12 m-t-md">
                <div>
                    <label>
                        <input runat="server" type="checkbox" id="chkEnableTicketing" class="i-checks">
                        Allow the public to request help.</label>
                </div>
            </div>
                    <% if (User.IsInRole("Administrator"))
                        { %>
            <div class="col-sm-12 m-t-md">
                <div>
                    <label>
                        <input runat="server" type="checkbox" id="chkEnableTeamVerification" class="i-checks">
                        Enable Team Member Verification</label>
                </div>
            </div>
               <% } %>
            <div class="col-sm-12 m-t-md">
                <div>
                    <label>
                        <input runat="server" type="checkbox" id="chkHideTeamList" class="i-checks">
                        Hide my team list from my team members.</label>
                </div>
            </div>
            <div class="col-sm-12 m-t-lg">
                <asp:Button ID="btnSubmit" runat="server" Text="Update Settings" OnClick="btnSubmit_Click"
                    CssClass="btn btn-primary" />
                <asp:Button ID="btnPaymentConfig" runat="server" Text="Edit Donation Settings" OnClick="btnPaymentConfig_Click"
                    CssClass="btn btn-primary" />
            </div>

        </div>
    </div>
    <uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>
