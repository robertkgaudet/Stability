<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="DisasterList.aspx.cs" Inherits="V1_DisasterList" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.disaster {
			  padding: 10px 20px;
			  text-align: center; 
			  text-decoration: underline;
			  display: inline-block;
			}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<a class="small-header-action">
								<div class="clip-header">
									<i class="fa fa-arrow-up"></i>
								</div>
							</a>
							<h2 class="font-light m-b-xs">
								Choose a Disaster
							</h2>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-12 container">

					<div class="hpanel">
						<div class="panel-heading hbuilt">
							Choose your disaster
						</div>
						<div class="panel-body">
							<asp:ListView runat="server" ID="lvDisasters">
								<ItemTemplate>
									<p>
									<asp:HyperLink ID="Label1" CssClass="disaster" runat="server" NavigateURL='<%# Eval("URLFriendlyName")%>' Text='<%# Eval("Name")%>'></asp:HyperLink>
									</p>
								</ItemTemplate>
							</asp:ListView>
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>