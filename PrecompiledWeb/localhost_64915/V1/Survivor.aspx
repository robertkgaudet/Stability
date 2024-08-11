<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_Survivor, App_Web_mjkl5wor" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>
<%@ Register Src="~/V1/UserControls/EventHeader.ascx" TagPrefix="uc1" TagName="EventHeader" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">



	
	<script type="text/javascript">
		$(document).ready(function () {

			$('.btnAddHome').click(function () {
				window.location.href = '/V1/Profile/AddNewRebuild.aspx?eventId=<%=eventId%>';
				return false;
			})
			
			$(".btnrescue").click(function () {
				location.href = "<%=btnrescue%>"; //"https://crowdrelief-forms.nogginoca.com/rescue.html";
			});

			$(".btnrequestsupplies").click(function () {
				location.href = "<%=btnrequestsupplies%>"; //"https://crowdrelief-forms.nogginoca.com/supplies.html";
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<uc1:EventHeader runat="server" ID="uc1EventHeader" />
	
	<div class="row m-t-lg">
		<div class="col-sm-12">
			<div class="hpanel">
				<div class="panel-heading hbuilt">
					<h1>Survivors</h1>
					Get Help / Rebuild A Home / Find Supplies
				</div>
				<div class="panel-body">
					<div class="row">
						<div class="col-sm-4">
							<div class="p-l-lg m-b-sm">
								<button type="button" class="btn btn-danger btn-lg margin-top-50 btnrescue btn-block m-b-sm">Request Assistance</button>
								<b>Citizen Assistance Dispatch</b>
								<p>
									For emergencies, please first call 911. Connect with us for non-emergency citizen assistance here. 
									Keep in mind, we are a volunteer rescue organization and will do our best to help you.
								</p>
							</div>
						</div>
						<div class="col-sm-4">
							<button class="btn btn-primary btn-lg btnAddHome pull-left m-r-lg m-b-sm btn-block">Rebuild A Home</button>
							<div class="p-l-lg">
								<b>Begin The Recovery Process</b>
								<p>
									Add a home profile to track and share your families home and personal recovery.
								</p>
							</div>
						</div>
						<div class="col-sm-4">
							<button type="button" class="btn btn-warning btn-lg btnrequestsupplies pull-left m-r-lg m-b-sm btn-block">Request Supplies</button>
							<div class="p-l-lg">
								<b>Get Things You Need</b>
								<p>
									Send our team requests for supplies after the disaster and we will help you locate the things you need.
								</p>
							</div>
						</div>
					</div>
				</div>
				<div class="panel-footer">
					<div class="row">
						<div class="col-sm-12">
							<asp:HyperLink CssClass="EventLink" ID="hypRebuildingHomesCount" runat="server"></asp:HyperLink>
						</div>
					</div>
					<div class="row">
						<div class="col-sm-12">
							<asp:Literal ID="litFollowingHomesCount" runat="server"></asp:Literal>
						</div>
					</div>
				</div>
			</div>

		</div>
	</div>
</asp:Content>