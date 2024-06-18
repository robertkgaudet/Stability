<%@ Page Title="" Language="C#" MasterPageFile="~/CaseManagement/MasterPages/CaseManagement.master" AutoEventWireup="true" CodeFile="ChooseEvent.aspx.cs" Inherits="CaseManagement_ChooseEvent" %>
<%@ MasterType VirtualPath="~/CaseManagement/MasterPages/CaseManagement.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/V1/Scripts/masonry.pkgd.min.js"></script>
	<script>
		$(document).ready(function () {

			$('.grid').masonry({
				// options
				itemSelector: '.grid-item',
				gutter: 10
			});
		});
	</script>
	<style>
		.grid-item					{ width:250px; }
		.disasterPanel, .activeDisasterPanel				{height:100px;}
		.disasterPanel:hover		{cursor:pointer; background-color:#FCF8E3; }
		.activeDisasterPanel:hover		{cursor:pointer; background-color:#62CB31; color:white; }
		.alert-success{cursor:pointer; background-color:#62CB31; color:white; }
		.titleHeight{height:60px;}

	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
	<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
		<div class="hpanel">
			<div class="panel-body">
				<h2 class="font-light m-b-xs">
					<asp:Literal ID="litEventDescription" runat="server"></asp:Literal>
				</h2>
					Weather disasters leaving citizens with homes in need of repair.
					<br />
					Select the disaster the home was affected by.
			</div>
		</div>
	</div>
	<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
		<div class="grid">
			<asp:Literal ID="litEvents" runat="server"></asp:Literal>
		</div>
	</div>

</asp:Content>