<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="AvailableResources.aspx.cs" Inherits="V1_NonProfit_AvailableResources" %>
<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
				
				<uc1:TeamHeader runat="server" ID="ucTeamHeader" />
					<div class="panel-heading hbuilt">
						<div class="font-normal">
							<h1 class="m-b-none"><i class="fa fa-truck"></i> Available Equipment</h1>
							<small>
							This is a living inventory of tools, vehicles, trailers, equipment, and supplies that community members are ready to deploy in times of need — from BBQ trailers and dump trucks to medical tents and solar generators.
							<br /><br />
							Each item listed here represents a real asset, volunteered by people who are part of the Stability network and prepared to help during emergencies or community events.
							<br /><br />
							Do you have equipment that could help your community?<br />
							Sign up to add your resources and join a growing network of trusted responders working together to make sure no one is left behind when it matters most.
							<br />
							[<a href="/V1/Profile/EditResources.aspx">Join the Network and Add Your Equipment →</a>]
							</small>
							<hr />
							You can search this teams resources by clicking on one of the buttons below.
						</div>
					</div>
					<asp:Literal ID="litAvailableResources" runat="server"></asp:Literal>
				<uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>