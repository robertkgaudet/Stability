<%@ Page Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true"
    CodeFile="CountyInfo.aspx.cs" Inherits="V1_CountyInfo" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        /* Responsive table styling */
        .responsive-table {
            width: 100%;
            margin-top: 20px;
            border-collapse: collapse;
        }   
        .responsive-table th {
            background-color: #f5f5f5;
            position: sticky;
            top: 0;
        }   
        /* Hide less important columns on mobile */
        .mobile-hide {
            display: table-cell; /* Show by default */
        }     
        /* Form group responsive adjustments */
        .form-group-responsive {
            margin-bottom: 15px;
        }       
        /* Media queries for mobile */
        @media (max-width: 768px) {
            /* Hide non-essential columns on mobile */
            .mobile-hide {
                display: none;
            }           
            /* Adjust form layout for mobile */
            .form-group-responsive .control-label {
                float: none;
                width: 100%;
                text-align: left;
                margin-bottom: 5px;
            }
            
            /* Make table scroll horizontally */
            .table-responsive-container {
                overflow-x: auto;
                -webkit-overflow-scrolling: touch;
            }
            
            /* Adjust panel padding */
            .panel-body {
                padding: 10px;
            }
        }
        
        /* Pagination styling */
        .pagination-container {
            display: flex;
            justify-content: center;
            margin-top: 20px;
        }
        
        .pagination-btn {
            margin: 0 5px;
            padding: 8px 16px;
            border: 1px solid #ddd;
            background-color: #fff;
            color: #007bff;
        }
        
        .pagination-btn:hover {
            background-color: #f0f0f0;
        }
        
        .current-page {
            display: inline-block;
            padding: 8px 16px;
            font-weight: bold;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                        <h2 class="font-light m-b-xs">County Emergency Management Information</h2>
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
                            <asp:HyperLink ID="hypEditCounty" Visible="false" CssClass="btn btn-xs btn-success m-r-sm"
                                runat="server" Text="Edit"></asp:HyperLink>
                        </div>
                        <asp:Label ID="lblStateInformation" runat="server"></asp:Label>
                        <div id="divInfoMessage" runat="server" visible="true">Detailed county information has
                            not yet been entered.</div>
                    </div>

                    <!-- COUNTY DETAILS PANEL - Made responsive -->
                    <div class="panel-body" id="divInfo" runat="server" visible="false">
                        <!-- EOC Name -->
                        <div class="form-group form-group-responsive">
                            <label class="col-sm-2 control-label">Emergency Operations Center Name</label>
                            <div class="col-sm-5">
                                <asp:Literal ID="litEOCName" runat="server"></asp:Literal>
                            </div>
                        </div>

                        <!-- Emergency Manager -->
                        <div class="form-group form-group-responsive">
                            <label class="col-sm-2 control-label">Emergency Managers Name</label>
                            <div class="col-sm-5">
                                <asp:Literal ID="litEMName" runat="server"></asp:Literal>
                            </div>
                        </div>

                        <!-- Phone Number -->
                        <div class="form-group form-group-responsive">
                            <label class="col-sm-2 control-label">Primary Phone Number</label>
                            <div class="col-sm-5">
                                <asp:Literal ID="litPhoneNumber" runat="server"></asp:Literal>
                            </div>
                        </div>

                        <!-- Website -->
                        <div class="form-group form-group-responsive">
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
                        
                        <!-- Wrapped table in responsive container -->
                        <div class="table-responsive-container">
                            <asp:GridView ID="gvLocations" runat="server" AutoGenerateColumns="false"
                                CssClass="table table-striped table-bordered responsive-table"
                                GridLines="None"
                                AllowSorting="True" EmptyDataText="No locations available."
                                OnSorting="gvLocations_Sorting">
                                <Columns>
                                    <asp:BoundField DataField="LocationName" HeaderText="Location Name" 
                                        ItemStyle-Width="25%" />
                                    <asp:BoundField DataField="FullAddress" HeaderText="Full Address" 
                                        ItemStyle-Width="35%" />
                                    <asp:BoundField DataField="GooglePlacesID" HeaderText="Google Places ID" 
                                        ItemStyle-Width="20%" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" 
                                        ItemStyle-CssClass="mobile-hide" />
                                    <asp:BoundField DataField="Coordinates" HeaderText="Lat/Lon" 
                                        ItemStyle-CssClass="mobile-hide" />
                                    <asp:BoundField DataField="PointOfContact" HeaderText="Point of Contact" 
                                        ItemStyle-CssClass="mobile-hide" />
                                    <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" 
                                        ItemStyle-CssClass="mobile-hide" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" 
                                        ItemStyle-CssClass="mobile-hide" />
                                </Columns>
                            </asp:GridView>
                        </div> 
                        <div class="pagination-container">
                            <asp:Button ID="btnPrevious" runat="server" Text="Previous" CssClass="pagination-btn"
                                OnClick="btnPrevious_Click" />
                            <span id="lblCurrentPage" runat="server" class="current-page"></span>
                            <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="pagination-btn" OnClick="btnNext_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>