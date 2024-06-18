<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="EditPassword.aspx.cs" Inherits="V1_Profile_EditPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<h2 class="font-light m-b-xs">
								Change Your Password
							</h2>
							<div class="form-group">
								<div class="pull-right">
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-6 container">
					
					<div class="hpanel form-horizontal">
						<div class="panel-heading hbuilt">
							Enter a new Password
						</div>
						<div class="panel-body">
							
							<div runat="server" id="divMessage" visible="false">
								<i class="fa fa-bolt"></i>
								<asp:Literal runat="server" id="lblMessage"></asp:Literal>
							</div>
							
							<asp:ChangePassword PasswordLabelText="Current Password:" ID="changePassword" runat="server"></asp:ChangePassword>

						</div>
						<div class="panel-footer">
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>