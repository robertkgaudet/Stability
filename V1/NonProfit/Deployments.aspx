<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true"
    CodeFile="Deployments.aspx.cs" Inherits="V1_NonProfit_Deployments" %>

<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ Register Src="~/V1/UserControls/DeploymentListCard.ascx" TagPrefix="uc1" TagName="DeploymentListCard" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:TeamHeader runat="server" ID="ucTeamHeader" />
    <div class="m-t-md">
        <div class="m-b-md">
            <asp:HyperLink ID="hypSearchDeployments" runat="server" NavigateUrl="/V1/Deployments.aspx"
                Text="Search Other Teams Deployments" CssClass="font-normal btn btn-sm btn-info"></asp:HyperLink>
            <asp:HyperLink
                ID="hypAddNewPosition"
                runat="server"
                Text="Add New Position"
                CssClass="font-normal btn btn-sm btn-info"
                visable="false">
            </asp:HyperLink>
            <asp:HyperLink ID="hypCreateCause" runat="server" Visible="false" Text="Create A New Deployment"
                CssClass="font-normal btn btn-sm btn-info pull-right"></asp:HyperLink>
        </div>
        <uc1:DeploymentListCard runat="server" ID="ucDeploymentListCard" />
        <div runat="server" id="divNoCause" visible="false">
            <div runat="server" id="divShowInviteAlert" visible="false" class="alert alert-success text-uppercase">
                <i class="fa fa-envelope"></i>Your invitation to create a deployment has been sent.
            </div>
        </div>
    </div>
    <uc1:TeamFooter runat="server" ID="ucTeamFooter" />

    <script src="/v1/Scripts/masonry.pkgd.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.grid').each(function () {
                // Initialize Masonry for each grid individually
                $(this).masonry({
                    itemSelector: '.grid-item',
                    gutter: 20,
                    columnWidth: '.grid-item',
                    percentPosition: true
                });
            });
        });
    </script>
</asp:Content>
