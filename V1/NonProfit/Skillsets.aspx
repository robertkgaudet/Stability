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
							<h1 class="m-b-none"><i class="fa fa-user-md"></i> Skillsets</h1>
							<small>
								This is a dynamic list of the skills and capabilities offered by team members who are ready to step up when their community needs them most — from heavy equipment operators and drone pilots to cooks, counselors, and communicators.
								<br /><br />
								Every skill listed here represents someone who’s already raised their hand to help. Together, these volunteers form the human engine behind Stability’s mission of trust-driven, real-time response.
								<br /><br />
								Do you have a skill your community could count on?<br />
								Sign up to share what you can do — and become part of a trusted network that turns everyday talent into everyday readiness.
								<br />
								[<a href="/V1/Profile/Skillsets.aspx">Join the Network and Add Your Skills →</a>]
							</small>
							<hr />
							You can search this teams skills by clicking on one of the buttons below.
						</div>
					</div>
					<div class="m-t-md">
						<asp:Literal ID="litSkillsets" runat="server"></asp:Literal>
					</div>
				<uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>