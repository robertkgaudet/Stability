<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Skillsets.aspx.cs" Inherits="V1_NonProfit_Skillsets" %>
<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
				<uc1:TeamHeader runat="server" ID="ucTeamHeader" />
					<div class="panel-heading hbuilt">
						<div class="font-normal">
							<h1 class="m-b-none"><i class="fa fa-hand-pointer-o"></i> Skillsets</h1>
							<small class="text-muted">This teams skills.</small>
						</div>
					</div>
					<div class="m-t-md">
						<asp:Literal ID="litSkillsets" runat="server"></asp:Literal>
					</div>
				<uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>