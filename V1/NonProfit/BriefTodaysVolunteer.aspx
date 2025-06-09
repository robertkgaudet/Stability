<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="BriefTodaysVolunteer.aspx.cs" Inherits="V1_NonProfit_BriefTodaysVolunteer" %>

<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ Register Src="~/V1/UserControls/SpinningLogo.ascx" TagPrefix="uc1" TagName="SpinningLogo" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
	$(function () {
		var today = new Date();

		$('#startDatePicker').datepicker({
			format: 'yyyy-mm-dd',
			autoclose: true,
			todayHighlight: true
		}).on('changeDate', function (e) {
			$('#<%= hfStartDate.ClientID %>').val(e.format(0, 'yyyy-mm-dd'));
		});

		$('#endDatePicker').datepicker({
			format: 'yyyy-mm-dd',
			autoclose: true,
			todayHighlight: true
		}).on('changeDate', function (e) {
			$('#<%= hfEndDate.ClientID %>').val(e.format(0, 'yyyy-mm-dd'));
			__doPostBack('<%= btnLoad.UniqueID %>', '');
        });

        // Pre-load both dates to today
        var todayStr = moment(today).format('YYYY-MM-DD');
        $('#startDatePicker').datepicker('setDate', today);
        $('#endDatePicker').datepicker('setDate', today);
        $('#<%= hfStartDate.ClientID %>').val(todayStr);
        $('#<%= hfEndDate.ClientID %>').val(todayStr);
	});
</script>

	<!-- Bootstrap Datepicker CSS -->
	<link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
	<script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:TeamHeader runat="server" ID="ucTeamHeader" />
    <div style="margin-bottom:20px; padding:15px; border:1px solid #ccc; border-radius:8px; background-color:#f9f9f9;">
        <div style="overflow:hidden;">
            <h3>Rotary District 6840 — Your CLub Members Are Stepping Up</h3>
            <p>
                New volunteers are joining every day. Be part of the movement to strengthen our communities.
                If you're a Rotary member, there's no better time to connect, serve, and lead.
            </p>
        </div>
    </div>
	<div class="row">
		<div class="col-sm-12">

			<h2>New Registrations</h2>

			<div class="form-group">
				<label>Select Date Range:</label>
				<div class="row">
					<div class="col-md-4">
						<label>Start Date:</label>
						<div id="startDatePicker"></div>
						<asp:HiddenField ID="hfStartDate" runat="server" />
					</div>
					<div class="col-md-4">
						<label>End Date:</label>
						<div id="endDatePicker"></div>
						<asp:HiddenField ID="hfEndDate" runat="server" />
					</div>
					<div class="col-sm-12 col-md-4">
						<asp:Image ID="imgRotaryBanner" runat="server" ImageUrl="~/V1/images/photo_2025-06-06_11-43-43.jpg" AlternateText="Join District 6840 Volunteers" Style="margin:15px;" Width="100%" />
					</div>
				</div>
			</div>
		</div>
	</div>
    <asp:Button ID="btnLoad" runat="server" Text="Load New Signups" OnClick="btnLoad_Click" />
    <br /><br />
	<asp:Label ID="lblOrgCount" runat="server" Font-Bold="true" ForeColor="Green" />
	<br /><br />
    <asp:Label ID="lblCount" runat="server" Font-Bold="true" ForeColor="Green" />
    <br /><br />
	<h2>
	<uc1:SpinningLogo ID="SpinningLogo1" runat="server" LogoSizeCssClass="logo-30" /><asp:Literal ID="litNewCount" runat="server"></asp:Literal>
</h2>
	<div class="row">
		<div class="col-sm-12 col-md-6">
			<h3>New Club Members</h3>



			<asp:Repeater ID="rptGroupedByDate" runat="server">
				<ItemTemplate>
					<h4 style="border-bottom:1px solid #ccc; margin-top:20px;">
						<%# ((System.Linq.IGrouping<DateTime, dynamic>)Container.DataItem).Key.ToString("MMMM dd, yyyy") %>
					</h4>

					<asp:Repeater ID="rptUsers" runat="server" DataSource='<%# ((IGrouping<DateTime, dynamic>)Container.DataItem).ToList() %>'>
						<ItemTemplate>
							<div style="padding-left: 20px;">
								<a href='/V1/Member/Default.aspx?userId=<%# Eval("UserId") %>' target="_blank">
									<%# Eval("Firstname") + " " + Eval("Lastname") %>
								</a>
								<b>Rank: <%# Eval("RankPosition") %></b>  – <%# Eval("OrganizationName") %>
							</div>
						</ItemTemplate>
					</asp:Repeater>
				</ItemTemplate>
			</asp:Repeater>







<%--
			<asp:GridView ID="gvNewUsers" runat="server" AutoGenerateColumns="False" GridLines="None" Style="width: 100%; min-width: 300px;">
				<Columns>
					<asp:TemplateField HeaderText="Name">
						<ItemTemplate>
							<a href='/V1/Member/Default.aspx?userId=<%# Eval("UserId") %>' target="_blank">
								<%# Eval("Firstname") + " " + Eval("Lastname") %> 
							</a> <b>Rank: <%# Eval("RankPosition")%></b>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="OrganizationName" HeaderText="Organization" />
				</Columns>
			</asp:GridView>--%>

		</div>
		<div class="col-sm-12 col-md-6">
			<h3>Organizations Represented</h3>
			<asp:GridView ID="gvOrganizations" runat="server" AutoGenerateColumns="False" GridLines="None" CssClass="half-width-grid" Style="width: 100%;">
				<Columns>
					<asp:BoundField DataField="SignupCount" HeaderText="New" />
					<asp:BoundField DataField="TotalMembers" HeaderText="Total" />
					<asp:TemplateField HeaderText="Organization">
						<ItemTemplate>
							<a href='/V1/NonProfit/People.aspx?organizationId=<%# Eval("OrganizationId") %>' target="_blank">
								<%# Eval("OrganizationName") %>
							</a>
						</ItemTemplate>
					</asp:TemplateField>

				</Columns>
			</asp:GridView>

		</div>
	</div>
    <br /><br />
	
    <uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>
