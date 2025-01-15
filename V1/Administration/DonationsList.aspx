<%@ Page Title="Donations List" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="DonationsList.aspx.cs" Inherits="Administration_DonationsList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" rel="stylesheet" />
    <script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>
    <link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
    <link href="../../Bigshop/bigshop-2.3/src/css/default/DonationList.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            $('#<%= SearchByDate.ClientID %>').datepicker({
                format: 'yyyy-mm-dd',
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
                <div class="hpanel">
                    <div class="panel-body">
                        <a class="small-header-action"></a>
                        <h2 class="font-light m-b-xs">Donations
                            <asp:Button ID="ExportPdfButton" runat="server" Text="Export to PDF" OnClick="ExportPdfButton_Click" CssClass="btn-pdf pull-right" />
                            <asp:Button ID="ExportCsvButton" runat="server" Text="Export to CSV" OnClick="ExportCsvButton_Click" CssClass="btn-csv pull-right" />
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
                    <div class="panel-body">
                        <p>Search Filter</p>
                        <div class="row">
                            <div class="col-md-2">
                                <asp:TextBox ID="SearchByName" runat="server" CssClass="form-control" placeholder="Search by Name"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="SearchByAddress" runat="server" CssClass="form-control" placeholder="Search by Address"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="SearchByDate" runat="server" CssClass="form-control" placeholder="Search by Date" />
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="SearchByAmount" runat="server" CssClass="form-control" placeholder="Search by Amount" TextMode="Number" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="SearchButton" runat="server" CssClass="form-control search-btn" Text="Search" OnClick="SearchButton_Click" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="ClearButton" runat="server" CssClass="form-control clear-btn" Text="Clear" OnClick="ClearButton_Click" />
                            </div>
                        </div>
                        <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False" CssClass="table table-hover table-bordered"
                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" GridLines="None" AllowPaging="True" PageSize="10"
                            OnPageIndexChanging="GridView_PageIndexChanging">
                            <PagerSettings Mode="Numeric" Position="Bottom" />
                            <PagerStyle CssClass="pager text-left" />
                            <Columns>
                                <asp:BoundField DataField="Name" HeaderText="Donor Name" SortExpression="Name">
                                    <ItemStyle CssClass="donor-name-cell" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address">
                                    <ItemStyle CssClass="address-cell" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CreatedAt" HeaderText="Date of Donation" SortExpression="CreatedAt" DataFormatString="{0:yyyy-MM-dd}">
                                    <ItemStyle CssClass="date-of-donation-cell" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Amount" HeaderText="Amount Donated" SortExpression="Amount" DataFormatString="${0:F2}">
                                    <ItemStyle CssClass="amount-cell" />
                                </asp:BoundField>
                            </Columns>
                            <FooterStyle CssClass="text-center" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
