<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/DisasterRegistry.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="V1_DisasterRegistry_Default" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/DisasterRegistry.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">

	    $(document).ready(function () {
	    });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="content-boxed">
        <div class="row">
            <div class="col-sm-8">
                <div class="hpanel">
                    <div class="panel-body">
                        <div class="media social-profile clearfix">
                            <a class="pull-left">
                                <asp:Image runat="server" id="imgItemPhoto" width="200"></asp:Image>
                            </a>
                            <div class="media-body">
                                <h1 class="m-t-none">
                                    <asp:Literal ID="litItemName" runat="server"></asp:Literal>
                                </h1>
                                <small>
                                    <asp:Literal ID="litDescription" runat="server"></asp:Literal>
                                </small>
                                <div class="small m-t-xs">
                                    <strong>Brand:</strong>
                                    <asp:Literal ID="litBrand" runat="server"></asp:Literal>
                                    <br />
                                    Delivery by Walmart
                                </div>
                            </div>
                        </div>
                        <div class="pull-right" id="divAdminlinks" visible="false" runat="server">
                            <asp:HyperLink id="hypEditItem" runat="server" Text="Edit Item"></asp:HyperLink> |
                            <asp:HyperLink id="hypAllItems" runat="server" NavigateURL="/V1/Administration/Item/List.aspx" Text="View All Items"></asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="hpanel stats">
                    <div class="panel-body">
                        <p>
                            <strong>The Soft Bath Towel in Slate Blue by Snowe</strong>
                        </p>
                        <div class="row">
                            <div class="col-xs-6">
                                <small class="stat-label">Survivors</small>
                                <h4>120</h4>
                            </div>
                            <div class="col-xs-6">
                                <small class="stat-label">Purchased</small>
                                <h4>4,309</h4>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6">
                                <small class="stat-label">Shelters</small>
                                <h4>622</h4>
                            </div>
                            <div class="col-xs-6" >
                                <small class="stat-label">Items Needed</small>
                                <h4>140</h4>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="hpanel">
                    <div class="panel-body">
                        <div class="m-b-lg">
                        <p>
                            <button class="btn btn-success btn-md pull-right" style="width:250px;">Create a Disaster Registry</button>
                            <h3>Buy for a Survivor</h3>
                        </p>
                        </div>
                        <div class="table-responsive">
                            <table class="table table-responsive">
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:Image runat="server" id="Image1" CssClass="img-responsive" ImageUrl="http://localhost:50902/V1/Images/HouseholdItems/21ee2df6-931e-4816-844b-a78c167b17c4_resized.jpg"></asp:Image>
                                        </td>
                                        <td>
                                            <h5>
                                                <a href="#">
                                               Set of Towels for Anna Johnson, Hurricane Dorian Survivor
                                                </a>
                                            </h5>
                                            <h5 class="m-xs text-default">
                                                <asp:Literal ID="litPrice" runat="server"></asp:Literal>
                                            </h5>
                                            <p>
                                                Anna and her family sustained a total home loss during Hurricane Dorian. Residents of Wilmington North Carolina, their home flooded with 7 feet of water as the storm passed over the banks near their home. 
                                                <br />
                                                <a href="#">
                                                    Find other items Anna needs (12)
                                                </a>
                                            </p>
                                        </td>
                                        <td>
                                            <button class="btn add_to_cart_button btn-md" style="width:250px;">Add to Cart</button>
                                            <small>
                                                Vetted
                                            </small>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Image runat="server" id="Image2" CssClass="img-responsive" ImageUrl="http://localhost:50902/V1/Images/HouseholdItems/21ee2df6-931e-4816-844b-a78c167b17c4_resized.jpg"></asp:Image>
                                        </td>
                                        <td>
                                            <h5><a href="#">
                                                Natalie Boudreaux - Tropical Storm Imelda
                                            </a></h5>
                                            <p>
                                                Anna and her family sustained a total home loss during Hurricane Dorian. Residents of Wilmington North Carolina, their home flooded with 7 feet of water as the storm passed over the banks near their home. 
                                                <br />
                                                <a href="#">
                                                    Find other items Natalie needs (32)
                                                </a>
                                            </p>
                                        </td>
                                        <td>
                                            <button class="btn add_to_cart_button btn-md wi" style="width:250px;">Add to Cart</button>
                                            <small>
                                                Vetted
                                            </small>
                                        </td>
                                    </tr>   
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

