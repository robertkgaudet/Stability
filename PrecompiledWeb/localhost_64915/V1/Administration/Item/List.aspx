<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_Administration_Item_List, App_Web_vfs24er2" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
    <script type="text/javascript">

	    $(document).ready(function () {
		    $(function () {
			    $('#itemTable').footable();
		    });

		    $(".btnAddItem").click(function () {
			    location.href = "/V1/Administration/Item/AddItem.aspx";
		    });
	    });

    </script>

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
								Items
								<label Class="btn btn-primary btn-lg pull-right m-r-lg btnAddItem">Add Item</label>
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
							Items
						</div>
						<div class="panel-body">
							<div class="p-l-lg">
								To filter, type a term.
								<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table"/>
							
								<asp:Repeater ID="rpItems" runat="server" OnItemDataBound="rpItemTable_ItemDataBound">
									<HeaderTemplate>
										<table id="itemTable" class="footable table table-bordered table-hover" data-page-size="50" data-filter="#filter">
											<thead>
												<tr>    
													<th data-toggle="true" data-hide="phone,tablet">Edit</th>
													<th data-toggle="true" data-hide="phone,tablet">Delete</th>
													<th data-toggle="true">Name</th>
													<th data-toggle="true">Price</th>
													<th data-toggle="true">SKU</th>
												</tr>
											</thead>
											<tbody>
									</HeaderTemplate>
									<ItemTemplate>
											<tr>
												<td>
													<a href='/V1/Administration/Item/AddItem.aspx?itemId=<%# DataBinder.Eval(Container.DataItem, "itemId") %>' class="EventLink">Edit</a> - 
													<a href='/V1/Administration/Item/ItemPhotoUpload.aspx?itemId=<%# DataBinder.Eval(Container.DataItem, "itemId") %>' class="EventLink">Image</a>
                                                    <asp:Literal ID="litPhoto" runat="server"></asp:Literal>
												</td>
												<td>
                                                    <asp:Literal ID="litDeleted" runat="server"></asp:Literal>
                                                    <asp:HyperLink ID="hypDelete" runat="server"></asp:HyperLink>
												</td>
												<td>
                                                    <asp:HyperLink ID="hypItem" runat="server"></asp:HyperLink>
												</td>
												<td><asp:Literal ID="litPrice" runat="server"></asp:Literal></td>
												<td><asp:Literal ID="litSKU" runat="server"></asp:Literal></td>
											</tr>
									</ItemTemplate>
									<FooterTemplate>
											</tbody>
											<tfoot>
												<tr>
													<td colspan="7">
														<ul class="pagination pull-right"></ul>
													</td>
												</tr>
											</tfoot>
										</table>
									</FooterTemplate>
								</asp:Repeater>
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
			</div>
		</div>
</asp:Content>

