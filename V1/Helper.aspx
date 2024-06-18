<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Helper.aspx.cs" Inherits="V1_Helper" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>
<%@ Register Src="~/V1/UserControls/EventHeader.ascx" TagPrefix="uc1" TagName="EventHeader" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	
	<script type="text/javascript">
		$(document).ready(function () {
			$('.btnVolunteer').click(function () {
				window.location.href = '<%=volunteerLink%>';
				return false;
			});

			$(".btnsendsupplies").click(function () {
				window.location.href = "<%=btnsendsupplies%>"; //"https://crowdrelief-forms.nogginoca.com/donate.html";
			});

			$(".btndonate").click(function () {
				window.location.href = "<%=btndonate%>"; //"http://www.CajunRelief.org/Donate";
			});

			$(".helpASurvivor").click(function () {
				window.location.href = "/S1/Profile/AddNewSurvivor.aspx?eventId=<%=eventId%>";
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<uc1:EventHeader runat="server" ID="uc1EventHeader" />

	<div class="hpanel m-t-lg">
		<div class="panel-heading hbuilt ">
			<h1>Helpers/Volunteers</h1>
			How to Help: Volunteer / Send Supplies / Donate / Connect with the Cajun Navy
		</div>
		<div class="panel-body">
			<div class="row">
				<div class="col-sm-6 col-md-3">
					<button type="button" class="btn btn-primary btn-lg helpASurvivor pull-left m-r-lg m-b-sm btn-block">HELP A SURVIVOR</button>
				</div>
				<div class="col-sm-6 col-md-3">
					<asp:LinkButton ID="btnVolunteer" runat="server" />
				</div>
				<div class="col-sm-6 col-md-3">
					<button type="button" class="btn btn-info btn-lg btnsendsupplies pull-left m-r-lg m-b-sm btn-block">SEND SUPPLIES</button>
				</div>
				<div class="col-sm-6 col-md-3">
					<button type="button" class="btn btn-success btn-lg btndonate pull-left m-r-lg m-b-sm btn-block">DONATE</button>
				</div>
			</div>
			<div class="row">
				<div class="col-sm-12">
					<h4>Connect With Us</h4>
					<dl class="dl-horizontal">
						<dt>Zello Public Channel</dt>
						<dd>
							<asp:HyperLink ID="hypZelloPublicChannel" CssClass="EventLink" runat="server" Target="_blank"></asp:HyperLink>
						</dd>
						<dt>Zello Rescue Channel</dt>
						<dd>
							<asp:HyperLink ID="hypZelloDispatchChannel" CssClass="EventLink" runat="server" Target="_blank"></asp:HyperLink>
						</dd>
						<dt>Zello Supply Channel</dt>
						<dd>
							<asp:HyperLink ID="hypZelloSupplyChannel" CssClass="EventLink" runat="server" Target="_blank"></asp:HyperLink>
						</dd>
						<dt>Glympse Location Tag</dt>
						<dd>	
							<asp:HyperLink ID="hypGlympse" CssClass="EventLink" runat="server" Target="_blank"></asp:HyperLink>
						</dd>
						<dt>Noggin Dispatcher Login</dt>
						<dd>
							<asp:HyperLink ID="hypNogginOCA" CssClass="EventLink" runat="server" Target="_blank"></asp:HyperLink>
						</dd>
					</dl>
				</div>
			</div>
		</div>
		<div class="panel-footer">
			<asp:Literal ID="litVolunteerDescription" runat="server"></asp:Literal>
			<div class="row">
				<div class="col-sm-6">
					<asp:HyperLink CssClass="EventLink" ID="hypMyVolunteerWork" runat="server"></asp:HyperLink>
				</div>
				<div class="col-sm-6">
					<asp:Literal ID="litTotalVolunteerHours" runat="server"></asp:Literal>
				</div>
			</div>
		</div>
	</div>
</asp:Content>