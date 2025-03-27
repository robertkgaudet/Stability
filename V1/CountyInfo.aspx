<%@ Page Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="CountyInfo.aspx.cs" Inherits="V1_CountyInfo" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .location-table {
            margin-top: 20px;
        }
        .location-table th {
            background-color: #f5f5f5;
        }
        .pagination {
            display: inline-block;
            margin-top: 20px;
        }
        .pagination a {
            color: #007bff;
            padding: 8px 16px;
            text-decoration: none;
            border: 1px solid #ddd;
            margin: 0 4px;
        }
        .pagination a:hover {
            background-color: #f0f0f0;
        }
        .pagination .disabled {
            color: #ccc;
            pointer-events: none;
        }
    </style>
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
                            County Emergency Management Information
                        </h2>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
        <div class="row">
            <div class="col-lg-12">
                <div class="hpanel form-horizontal">
                    <div class="panel-heading hbuilt">
                        <div class="pull-right">
                            <asp:HyperLink ID="hypEditCounty" Visible="false" CssClass="btn btn-xs btn-success m-r-sm" runat="server" Text="Edit"></asp:HyperLink>
                        </div> 
                        <asp:Label ID="lblStateInformation" runat="server"></asp:Label>
                        <div id="divInfoMessage" runat="server" visible="true">Detailed county information has not yet been entered.</div>
                    </div>
                    
                    <!-- COUNTY DETAILS PANEL -->
                    <div class="panel-body" id="divInfo" runat="server" visible="false">
                        <!-- EOC Name -->
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Emergency Operations Center Name</label>
                            <div class="col-sm-5">
                                <asp:Literal ID="litEOCName" runat="server"></asp:Literal>
                            </div>
                        </div>
                        
                        <!-- Emergency Manager -->
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Emergency Managers Name</label>
                            <div class="col-sm-5">
                                <asp:Literal ID="litEMName" runat="server"></asp:Literal>
                            </div>
                        </div>
                        
                        <!-- Phone Number -->
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Primary Phone Number</label>
                            <div class="col-sm-5">
                                <asp:Literal ID="litPhoneNumber" runat="server"></asp:Literal>
                            </div>
                        </div>
                        
                        <!-- Website -->
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Emergency Management Website</label>
                            <div class="col-sm-5">
                                <asp:Literal ID="litWebsite" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
<div class="row">
    <div class="col-lg-12">
        <div class="hpanel">
            <div class="panel-heading">
                <h4>Locations in Disaster Zone</h4>
            </div>
            <div class="panel-body">
                <asp:Panel ID="pnlNoLocations" runat="server" Visible="false" CssClass="alert alert-info">
                    No locations found for this county in the current disaster.
                </asp:Panel>
                <div class="table-responsive">
                    <asp:GridView ID="gvLocations" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-striped table-bordered location-table" GridLines="None"
                        AllowSorting="True" EmptyDataText="No locations available."
                        OnSorting="gvLocations_Sorting">
                        <Columns>
                            <asp:BoundField DataField="LocationName" HeaderText="Location Name" SortExpression="LocationName" />
                            <asp:BoundField DataField="FullAddress" HeaderText="Full Address" SortExpression="FullAddress" />
                            <asp:BoundField DataField="GooglePlacesID" HeaderText="Google Places ID" />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:BoundField DataField="Coordinates" HeaderText="Lat/Lon" />
                            <asp:BoundField DataField="PointOfContact" HeaderText="Point of Contact" />
                            <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                        </Columns>
                    </asp:GridView>
                </div>
                <!-- Pagination Controls -->
                <div class="pagination text-center mt-3">
                    <asp:Button ID="btnPrevious" runat="server" Text="Previous" CssClass="btn btn-primary me-2"
                        OnClick="btnPrevious_Click" />
                    <span id="lblCurrentPage" runat="server" class="fw-bold"></span>
                    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn btn-primary ms-2"
                        OnClick="btnNext_Click" />
                </div>
            </div>
        </div>
    </div>
</div>

    </div>
</asp:Content>
