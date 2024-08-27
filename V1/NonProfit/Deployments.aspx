<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Deployments.aspx.cs" Inherits="V1_NonProfit_Deployments" %>
<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ Register Src="~/V1/UserControls/DeploymentListCard.ascx" TagPrefix="uc1" TagName="DeploymentListCard" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
				<uc1:TeamHeader runat="server" ID="ucTeamHeader" />
                    <div class="panel-heading">
						<div id="hbreadcrumb" class="pull-right">
							<ol class="hbreadcrumb breadcrumb">
								<li>
									<asp:HyperLink ID="hypCreateCause" runat="server" Visible="false" Text="Create A New Deployment" CssClass="font-normal btn btn-sm btn-info"></asp:HyperLink>
								</li>
							</ol>
						</div>
					</div>
					<br /><br />
					<div class="m-t-md">
						<uc1:DeploymentListCard runat="server" ID="ucDeploymentListCard" />
						<div runat="server" id="divNoCause" visible="false">
							<div runat="server" id="divShowInviteAlert" visible="false" class="alert alert-success text-uppercase">
								<i class="fa fa-envelope"></i> Your invitation to create a deployment has been sent.
							</div>
						</div>
					</div>
				<uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>