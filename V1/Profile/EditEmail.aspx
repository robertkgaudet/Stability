<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="EditEmail.aspx.cs" Inherits="V1_Profile_EditEmail" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<h2 class="font-light m-b-xs">
								Change Your Email
							</h2>
							<div class="form-group">
								<div class="pull-right">
									<asp:LinkButton id="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default" Text="Cancel" />
									<asp:Button id="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Save Changes" />
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
							Enter a new Email
						</div>
						<div class="panel-body">
							
							<div runat="server" id="divMessage" visible="false">
								<i class="fa fa-bolt"></i>
								<asp:Literal runat="server" id="lblMessage"></asp:Literal>
							</div>
							
							<div class="form-group m-t-lg">
								<label class="col-sm-3 control-label">Email</label>
								<div class="col-sm-9"><input runat="server" id="txtEmailAddress" class="form-control" placeholder="Email Address"></div>
							</div>

						</div>
						<div class="panel-footer">
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>