<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Stream.aspx.cs" Inherits="V1_NonProfit_Stream" %>
<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ Register Src="~/V1/UserControls/Stream.ascx" TagPrefix="uc1" TagName="Stream" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
				<uc1:TeamHeader runat="server" ID="ucTeamHeader" />
				<uc1:Stream runat="server" ID="TeamNavigation1" />
				<uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>