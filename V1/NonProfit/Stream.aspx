<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Stream.aspx.cs" Inherits="V1_NonProfit_Stream" %>
<%@ Register Src="~/V1/UserControls/TeamNavigation.ascx" TagPrefix="uc1" TagName="TeamNavigation" %>
<%@ Register Src="~/V1/UserControls/TeamHeader.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/Stream.ascx" TagPrefix="uc1" TagName="Stream" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

	<uc1:TeamHeader runat="server" ID="ucTeamHeader" />

	<div class="content">
        <div class="row">
            <div class="col-md-2"></div>
            <div class="col-md-3">
				<uc1:TeamNavigation runat="server" ID="ucTeamNavigation" />
            </div>
            <div class="col-md-5">
					<!--PAGE HEADER-->
                <div class="hpanel ">
					<!--PAGE CONTENT-->
					<div class="m-t-md">
						<uc1:Stream runat="server" ID="TeamNavigation1" />
					</div>
					<!--PAGE FOOTER-->
                </div>
            </div>
            <div class="col-md-2"></div>
        </div>
    </div>
</asp:Content>

