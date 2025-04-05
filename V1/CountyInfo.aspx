<%@ Page Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="CountyInfo.aspx.cs" Inherits="V1_CountyInfo" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
    <link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#locationTable').footable();

        });
    </script>
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
                        <h2 class="font-light m-b-xs">County Emergency Management Information
                        </h2>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
        <div class="row">
            <div class="col-lg-12 container">
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
                    <div class="panel-body" id="divInfo" runat="server" visible="false">
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Emergency Operations Center Name</label>
                            <div class="col-sm-5">
                                <asp:Literal ID="litEOCName" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Emergency Managers Name</label>
                            <div class="col-sm-5">
                                <asp:Literal ID="litEMName" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Primary Phone Number</label>
                            <div class="col-sm-5">
                                <asp:Literal ID="litPhoneNumber" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Emergency Management Website</label>
                            <div class="col-sm-5">
                                <asp:Literal ID="litWebsite" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="hpanel">
                    <div class="panel-heading hbuilt">
                        Locations
                    </div>
                    <div class="panel-body">
                        <div class="p-l-lg">
                            <asp:Repeater ID="rptLocations" runat="server">
                                <HeaderTemplate>
                                    <table id="locationTable" class="footable table table-bordered table-hover" data-page-size="10"
                                        data-filter="#filter">
                                        <thead>
                                            <tr>
                                                <th data-toggle="true">Location Name</th>
                                                <th data-toggle="true">Full Address</th>
                                                <th data-toggle="true">Description</th>
                                                <th data-toggle="true" data-hide="phone,tablet">Point of Contact</th>
                                                <th data-toggle="true" data-hide="phone,tablet">Phone Number</th>
                                                <th data-toggle="true" data-hide="phone,tablet">Email</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>

                                        <td><%# Eval("LocationName") %></td>
                                        <td><%# Eval("FullAddress") %></td>
                                        <td><%# Eval("Description") %></td>
                                        <td><%# Eval("PointOfContact") ?? "Not Provided" %></td>
                                        <td><%# Eval("PhoneNumber") ?? "N/A" %></td>
                                        <td><%# Eval("Email") ?? "N/A" %></td>
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
