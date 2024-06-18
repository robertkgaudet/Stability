<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="VolunteerList.aspx.cs" Inherits="V1_NonProfitAdministration_VolunteerList" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="normalheader ">
	<div class="hpanel">
		<div class="panel-body">
			<div class="row">
				<div class="col-sm-12">
					<h1 class="font-light m-b-xs">
						<asp:Literal id="litOrganizationName" runat="server"></asp:Literal>
					</h1>
                </div>
			</div>
		</div>
	</div>
</div>

<div class="content">
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-12 container">
					<div class="hpanel">
						<div class="panel-heading">
							Sort, page, search to find volunteers.
						</div>
						<div class="panel-body">
							<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table">
							<table id="tblVolunteers" class="footable table table-stripped toggle-arrow-tiny" data-page-size="50" data-filter=#filter>
								<thead>
									<tr>
										<th data-toggle="true" id="thControl" runat="server" visible="false">DELETE</th>
										<th data-toggle="true">ID</th>
                                        <th data-toggle="true">Time</th>
                                        <th data-toggle="true">Created</th>
                                        <th data-toggle="true">last Login</th>
                                        <th data-toggle="true">Email</th>
									</tr>
								</thead>
								<tbody>
									<asp:Repeater ID="rptVolunteers" runat="server" OnItemDataBound="rptVolunteers_ItemDataBound">
										<ItemTemplate>
											<tr>
												<td id="tdDelete" runat="server" visible="false"><asp:HyperLink ID="hypDelete" Text="Delete" runat="server"></asp:HyperLink></td>
												<td><asp:HyperLink ID="hypID" runat="server"></asp:HyperLink></td>
                                                <td><asp:Literal ID="litActiveStatus" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litCreateDate" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litLastLoginDate" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litEmail" runat="server"></asp:Literal></td>
											</tr>
										</ItemTemplate>
									</asp:Repeater>
								</tbody>
								<tfoot>
									<tr>
										<td colspan="5">
											<ul class="pagination pull-right"></ul>
										</td>
									</tr>
								</tfoot>
							</table>
						</div>
					</div>
				</div>
			</div>
		</div>
</div>
<script>

    $(function () {

        // Initialize Example 1
        $('#tblVolunteers').footable();
    });

</script>
</asp:Content>

