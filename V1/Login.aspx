<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Basic.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="V1_Login" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Basic.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="middle-box text-center loginscreen animated fadeInDown">
        <div>
            <div>
                <h1 class="logo-name">
					<img class="img-responsive" src="/V1/Images/Logo-Horizontal.png" />
                </h1>
				<div class="p-sm text-center">
					Welcome to Stability<br /> Community Disaster Relief Teams
				</div>
            </div>
            <p>Turn your friends and family into an impactful disaster relief team to help your community.
            </p>
            <p>Sign in now to get started.</p>
            <div class="m-t" role="form">
                <div class="form-group">
					<asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" placeholder="Username" required=""></asp:TextBox>
                </div>
                <div class="form-group">
					<asp:TextBox ID="txtPassword" CssClass="form-control" TextMode="Password" runat="server" placeholder="Password" required=""></asp:TextBox>
                </div>
				<asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Sign In" CssClass="btn btn-info block full-width m-b" />

                <a href="/V1/PasswordReset.aspx"><small>Forgot password?</small></a>
                <p class="text-muted text-center"><small>Do not have an account?</small></p>
                <a class="btn btn-sm btn-success btn-block" href="/Register">Create an Account</a>
            </div>
            <p class="m-t">
				<small>We believe every community member has something valuable to contribute after a natural disaster. Stability was created to empower teams to help the most vulnerable affected by disasters to recovery more quickly.</small>
            </p>
        </div>
    </div>
</asp:Content>