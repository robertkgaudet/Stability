<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="NonProfitCampaign.aspx.cs" Inherits="V1_NonProfitCampaign" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>
<%@ Register Src="~/V1/UserControls/EventHeader.ascx" TagPrefix="uc1" TagName="EventHeader" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<script type="text/javascript">
		$(document).ready(function () {

			$('.btnRegisterNonProfit').click(function () {
				window.location.href = '<%=registerNonProfit%>';
				return false;
			})
		});
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<uc1:EventHeader runat="server" ID="uc1EventHeader" />
	
				
				<div class="row m-t-lg">
					<div class="col-sm-6">
						<div class="hpanel">
							<div class="panel-heading hbuilt ">
								Non-Profit Interactive Survivor Recovery Portal
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
</asp:Content>