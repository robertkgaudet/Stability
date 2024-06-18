<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Basic.master" AutoEventWireup="true" CodeFile="PasswordReset.aspx.cs" Inherits="V1_PasswordReset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="middle-box text-center loginscreen animated fadeInDown">
        <div>
            <h1 class="logo-name">
				<img class="img-responsive" src="Images/Logo-Horizontal.png" />
            </h1>
        </div>
		<h2>
			Password Reset
		</h2>
		<p>
			Please enter your username, a new password will be emailed to you.
		</p>
		<div class="m-t h-200" role="form">
			<div class="form-group">
				<asp:PasswordRecovery
						ID="prPasswordRecovery"
						runat="server"
						InstructionTextStyle-CssClass="m-l"
						TextBoxStyle-CssClass="form-control"
						TitleTextStyle-CssClass="font-bold"
						LabelStyle-CssClass="font-bold"
						BackColor="White"
						CssClass="form-control"
						OnSendingMail="PasswordRecovery2_SendingMail"
						SuccessText="A temporary password has been emailed to you.">
					<SubmitButtonStyle CssClass="btn btn-primary block full-width m-b"></SubmitButtonStyle>
				</asp:PasswordRecovery>
			</div>
		</div>
		<p><a href="/V1/Login.aspx"><small>Login</small></a></p>
	</div>
</asp:Content>

