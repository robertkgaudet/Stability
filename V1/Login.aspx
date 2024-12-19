<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Narrow.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="V1_Login" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/1-Column-Narrow.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
		<script>

			$(document).ready(function () {
				$(".loginLogo").click(function () {
					document.location.href = "/default.aspx";
				});
				$("body").tooltip({ selector: '[data-toggle=tooltip]' });
			});
		</script>

		<style>
			.loginLogo
			{
				margin:10px;
				width:300px;
			}
			.loginLogo:hover
			{
				cursor: pointer;
			}
		</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
        <div class="row">
            <div class="col-xs-2 col-sm-4 col-lg-4"></div>
            <div class="col-xs-8 col-sm-4 col-lg-4" style="min-width:320px !important;max-width:320px !important;">
				<div class="middle-box text-center loginscreen animated fadeInDown">
					<h1 class="loginLogo">
						<img class="img-responsive" src="/V1/Images/Logo-Horizontal-cs.png" />
					</h1>
					<div class="m-t-sm text-center">
						<h4>
						Disaster-Ready Communities
						</h4>
					</div>
					<p>
						Be the lifeline your community needs.
					</p>
				</div>
				<div class="m-t" role="form">
					<div class="form-group">
						<asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" placeholder="Username" required=""></asp:TextBox>
					</div>
					<div class="form-group">
						<asp:TextBox ID="txtPassword" CssClass="form-control" TextMode="Password" runat="server" placeholder="Password" required=""></asp:TextBox>
					</div>
					<asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Sign In" CssClass="btn btn-success  btn-block m-b" />

					<a href="/V1/PasswordReset.aspx"><small>Forgot password?</small></a>
					<p class="text-muted text-center"><small>Do not have an account?</small></p>
					<a class="btn btn-sm btn-info btn-block" href="/Register">Create an Account</a>
				</div>
				<p class="m-t-lg">
					<small>Stability empowers community groups to become force multipliers after natural disasters coordinating efforts between citizen run organizations and local emergency managers.</small>
				</p>
            </div>
            <div class="col-xs-2 col-sm-4 col-lg-4"></div>
		</div>
</asp:Content>