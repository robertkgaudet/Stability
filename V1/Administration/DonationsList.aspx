<%@ Page Title="Donations List" Language="C#" EnableViewState="true" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="DonationsList.aspx.cs" Inherits="Administration_DonationsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" rel="stylesheet" />
    <script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>
    <link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
    

    <script>
        $(document).ready(function () {
            $('#<%= StartDate.ClientID %>').datepicker({
                format: 'yyyy-mm-dd',
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
            $('#<%= EndDate.ClientID %>').datepicker({
                format: 'yyyy-mm-dd',
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
        });

        function __doPostBack(eventTarget, eventArgument) {
            var theForm = document.getElementById('form1');
            if (!theForm) {
                theForm = document.form1;
            }

            if (theForm.onsubmit && theForm.onsubmit() === false) {
                return false;
            }

            theForm.__EVENTTARGET.value = eventTarget;
            theForm.__EVENTARGUMENT.value = eventArgument;
            theForm.submit();
        }
        document.addEventListener('DOMContentLoaded', function () {
            var sortExpression = '<%= ViewState["SortExpression"] %>';
          var sortDirection = '<%= ViewState["SortDirection"] %>';

          function updateArrow(columnId, isAscending) {
              var arrowElement = document.getElementById(columnId);
              if (isAscending) {
                  arrowElement.innerHTML = '▼'; 
              } else {
                  arrowElement.innerHTML = '▲'; 
              }
          }
          if (sortExpression === 'Name') {
              updateArrow('arrowName', sortDirection === 'ASC');
          } else if (sortExpression === 'Address') {
              updateArrow('arrowAddress', sortDirection === 'ASC');
          } else if (sortExpression === 'Date') {
              updateArrow('arrowDate', sortDirection === 'ASC');
          } else if (sortExpression === 'Amount') {
              updateArrow('arrowAmount', sortDirection === 'ASC');
          }
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
            border: none;
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
            cursor: pointer;
        }

        .pager-right {
            text-align: right;
            margin-top: 10px;
            padding-left: 10px;
        }

            .pager-right .selected {
                font-weight: bold;
                background-color: #007bff;
                color: white;
                border-color: #007bff;
            }

        .page-info {
            font-weight: bold;
            margin: 20px;
        }


        .form-control {
            margin-bottom: 20px;
        }

        .sort-arrow {
            display: inline-block;
            margin-left: 5px;
            font-size: 12px;
            cursor: pointer;
        }

        .sort-arrow-desc {
            transform: rotate(180deg);
        }

        .page-button {
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 1px;
            padding: 3px 11px;
            cursor: pointer;
        }

            .page-button:hover {
                background-color: #0056b3;
            }

            .page-button:disabled {
                background-color: #ccc;
                color: #999;
                cursor: not-allowed;
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
                                <asp:TextBox ID="SearchByName" runat="server" CssClass="form-control" placeholder="Search by Name" autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="SearchByAddress" runat="server" CssClass="form-control" placeholder="Search by Address" autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="StartDate" runat="server" CssClass="form-control" placeholder="Start Date" autocomplete="off" />
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="EndDate" runat="server" CssClass="form-control" placeholder="End Date" autocomplete="off" />
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="SearchByAmount" runat="server" CssClass="form-control" placeholder="Search by Amount" TextMode="Number" autocomplete="off" />
                            </div>

                            <div class="col-md-1">
                                <asp:Button ID="SearchButton" runat="server" CssClass="form-control search-btn" Text="Search" OnClick="SearchButton_Click" />
                            </div>
                            <div class="col-md-1">
                                <asp:Button ID="ClearButton" runat="server" CssClass="form-control clear-btn" Text="Clear" OnClick="ClearButton_Click" />
                            </div>
                        </div>

                        <asp:Repeater ID="RepeaterDonations" runat="server">
                            <HeaderTemplate>
                                <table class="table table-hover table-bordered">
                                    <thead>
                                        <tr>
                                            <th onclick="__doPostBack('Sort', 'Name')">Donor Name
   
                                                <span class="sort-arrow" id="arrowName">&#9660;</span>
                                               
                                            </th>
                                            <th onclick="__doPostBack('Sort', 'Address')">Address
           
                                                <span id="arrowAddress" class="sort-arrow">&#9660;</span>
                                            </th>
                                            <th onclick="__doPostBack('Sort', 'Date')">Date of Donation
           
                                                <span id="arrowDate" class="sort-arrow">&#9660;</span>
                                            </th>
                                            <th onclick="__doPostBack('Sort', 'Amount')">Amount Donated
           
                                                <span id="arrowAmount" class="sort-arrow">&#9660;</span>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("Name") %></td>
                                    <td><%# Eval("Address") %></td>
                                    <td><%# Eval("CreatedAt", "{0:yyyy-MM-dd}") %></td>
                                    <td><%# Eval("Amount", "${0:F2}") %></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>

                        <div class="pager-right">
                            <asp:Label ID="lblCurrentPage" runat="server" CssClass="page-info" />
                            <asp:Button ID="btnPrevious" runat="server" CssClass="page-button" Text="Previous" OnClick="btnPrevious_Click" Visible="false" />
                            <asp:Literal ID="lblPageNumbers" runat="server" />
                            <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="page-button" OnClick="btnNext_Click" Visible="false" />
                        </div>
                        <input type="hidden" name="__EVENTTARGET" id="__EVENTTARGET" value="" />
                        <input type="hidden" name="__EVENTARGUMENT" id="__EVENTARGUMENT" value="" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
