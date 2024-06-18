<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="CorporateBrands.aspx.cs" Inherits="Administration_CorporateBrands" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
								Corporate Partners
							</h2>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-6 container">

					<div class="hpanel hbgblue">
						<div class="panel-heading hbuilt">
							Rebuild Your Home
						</div>
						<div class="panel-body">
							<button class="btn btn-primary btn-lg pull-left m-r-lg">Rebuild A Home</button>
							<div class="p-l-lg">
								<b>Begin the recovery process.</b>
								<p>
									Add a home profile to track and share your families home and personal recovery.
								</p>
							</div>
						</div>
									
						<div class="panel-footer">
							<div class="row">
								<div class="col-sm-6">
									<asp:HyperLink CssClass="EventLink" ID="hypRebuildingHomesCount" runat="server"></asp:HyperLink>
								</div>
								<div class="col-sm-6">
									<asp:Literal ID="litFollowingHomesCount" runat="server"></asp:Literal>
								</div>
							</div>
						</div>
					</div>

				</div>
				<div class="col-lg-6 container">

					<div class="hpanel hbgblue">
						<div class="panel-heading hbuilt">
							Rebuild Your Home
						</div>
						<div class="panel-body">
							<button class="btn btn-primary btn-lg pull-left m-r-lg">Rebuild A Home</button>
							<div class="p-l-lg">
								<b>Begin the recovery process.</b>
								<p>
									Add a home profile to track and share your families home and personal recovery.
								</p>
							</div>
						</div>
									
						<div class="panel-footer">
							<div class="row">
								<div class="col-sm-6">
									<asp:HyperLink CssClass="EventLink" ID="HyperLink1" runat="server"></asp:HyperLink>
								</div>
								<div class="col-sm-6">
									<asp:Literal ID="Literal1" runat="server"></asp:Literal>
								</div>
							</div>
						</div>
					</div>

				</div>
			</div>
		</div>
</asp:Content>