<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Narrow.master" AutoEventWireup="true" CodeFile="MemberVerification.aspx.cs" Inherits="MemberVerification" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/1-Column-Narrow.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="form-group">
			<div class="panel panel-default">
			  <div class="panel-body">
				<h3 class="text-info">Member Verification</h3>
				<p class="lead">
				  This page confirms that you’re viewing a verified Stability profile.
				</p>

				<p>
				  Whether you're a neighbor, nonprofit, team lead, or organizer, this profile provides an accurate snapshot of the member’s current skills, equipment, availability, and community involvement.
				</p>

				<h4>Why does this matter?</h4>
				<p>
				  In times of need — from natural disasters to daily support efforts — knowing who’s equipped and ready to help builds faster trust and stronger coordination.
				</p>

				<ul class="list-unstyled">
				  <li>✅ The member has opted in and maintains their own information</li>
				  <li>✅ Skills and resources are transparent and up to date</li>
				  <li>✅ Service dates and readiness are clearly shared</li>
				  <li>✅ Stability’s verification standards are applied consistently</li>
				</ul>
			  </div>
			</div>

			<div class="block">
			<asp:Label ID="lblMessage" runat="server" CssClass="text-danger" />
			</div>
			<h4>To Verify This User, Enter Their Volunteer #.</h4>
			<label for="txtProfileId">Enter 'Volunteer #' Found On The ID Card:</label>
			<asp:TextBox ID="txtProfileId" runat="server" CssClass="form-control" Placeholder="Enter Volunteer #" />
		</div>
		<asp:LinkButton ID="btnViewProfile" runat="server" CssClass="btn btn-primary mt-2" OnClick="btnFindProfile_Click">
			<i class='fa fa-user'></i> Verify Stability Member
		</asp:LinkButton>

</asp:Content>

