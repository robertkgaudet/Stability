<%@ Page Title="Donations List" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="DonationsList.aspx.cs" Inherits="Administration_DonationsList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" rel="stylesheet" />
    <script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>
    <link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
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
    <style>
    .search-btn {
    background-color: #007bff !important;
    color: white;
    border: none;
    border-radius: 5px;
    cursor: pointer;
}

    .search-btn:hover {
        background-color: #45a049;
    }

.clear-btn {
    background-color: #6c757d !important;
    color: white;
    border: none;
    border-radius: 5px;
    cursor: pointer;
}

    .clear-btn:hover {
        background-color: #e53935;
    }
    
.btn-pdf {
    background-color: #4CAF50;
    color: white;
    padding: 10px 20px;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    font-size: 16px;
    margin-left: 20px;
}

    .btn-pdf:hover {
        background-color: #45a049;
    }

.btn-csv {
    border:none;
    background-color: #f39c12;
    color: white;
    padding: 10px 20px;
    border-radius: 5px;
    cursor: pointer;
    font-size: 16px;
}

    .btn-csv:hover {
        background-color: #e67e22;
    }


.table td, .table th {
    padding: 15px !important;
}

.pager {
    text-align: right;
    margin-top: 10px;
}

    .pager a, .pager span {
        display: inline-block;
        padding: 4px 8px;
        margin: -13px;
        border: 1px solid #ccc;
        border-radius: 4px;
        text-decoration: none;
        color: #007bff;
        background-color: #f9f9f9;
    }

        .pager a:hover {
            background-color: #e9ecef;
            border-color: #007bff;
        }

    .pager .selected {
        font-weight: bold;
        background-color: #007bff;
        color: white;
        border-color: #007bff;
    }
    .form-control {
    margin-bottom: 20px;
}
</style>
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
