<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="NonProfit.aspx.cs" Inherits="V1_NonProfit" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>
<%@ Register Src="~/V1/UserControls/EventHeader.ascx" TagPrefix="uc1" TagName="EventHeader" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/V1/Scripts/masonry.pkgd.min.js"></script>
	<style>
		.grid-item											{ width:48%; }
		.disasterPanel, .activeDisasterPanel				{height:300px;}
		.disasterPanel:hover								{cursor:pointer; background-color:#FCF8E3; }
		.activeDisasterPanel:hover							{cursor:pointer; background-color:#62CB31; color:white; }
		.alert-success{cursor:pointer; background-color:#62CB31; color:white; }
		.titleHeight{height:60px;}
	</style>

	<script type="text/javascript">
		$(document).ready(function () {

			$('.btnRegisterNonProfit').click(function () {
				window.location.href = '<%=registerNonProfit%>';
				return false;
			})
			
			$('.editCampaign').click(function () {
				window.location.href = '<%=editCampaign%>';
				return false;
			})

			$('.grid').masonry({
				// options
				itemSelector: '.grid-item',
				gutter: 10
			});
		});
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<uc1:EventHeader runat="server" ID="uc1EventHeader" />
		<div class="row m-t-lg" runat="server" id="divAddNonProfit">
			<div class="col-sm-6">
				<div class="hpanel">
					<div class="panel-heading hbuilt ">
						Non-Profit Disaster Campaign
					</div>
					<div class="panel-body">
						<div class="row">
							<div class="col-sm-12">
								<div class="p-l-lg m-b-sm">
									<p>
										<asp:Literal id="litNonProfitName" runat="server"></asp:Literal>
										<asp:Literal id="litRegisterNonProfit" runat="server" visible="true"></asp:Literal>
										<asp:Literal id="litAddNonProfitEvent" runat="server" visible="false"></asp:Literal>
									</p>
									<asp:Button id="btnRegisterNonProfit" runat="server" CssClass="btn btn-primary2 btn-lg margin-top-50 btnRegisterNonProfit btn-block m-b-sm"></asp:Button>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		
		<div class="animate-panel" data-child="hpanel" data-effect="fadeInLeft" runat="server" id="divNonprofitCampaigns">
			<div class="grid">
				<asp:Literal ID="litEvents" runat="server"></asp:Literal>
			</div>
		</div>
</asp:Content>