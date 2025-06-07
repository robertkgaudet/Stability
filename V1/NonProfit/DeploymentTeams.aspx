<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/2-Column-Child.master" AutoEventWireup="true" CodeFile="DeploymentTeams.aspx.cs" Inherits="V1_NonProfit_DeploymentTeams" %>

<%@ Register Src="~/V1/UserControls/MemberNavigation.ascx" TagPrefix="uc1" TagName="MemberNavigation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        var currentPagination = '';
        var scroll = false;
        $(document).ready(function () {
            $(document).on("click", "#nextBtn", function () {
                if (!$('#nextBtn').hasClass('disabled')) {
                    var totalPages = document.getElementById('<%= totalPageValue.ClientID %>').value;
                    var currentPage = parseInt($('.pagination .active .page-link').text());

                    var newPage = currentPage + 1;
                    if (newPage <= totalPages) {
                        if (newPage > 3) {
                            $(".page-item").not("#previousBtn, #nextBtn").remove();
                            var newPageItem = '';
                            for (var i = 0; i < Math.min(3, totalPages); i++) {

                                newPageItem += '<li class="page-item' + (i == 1 ? " active" : "") + (i == 2 ? " firstpn" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + newPage - 2) + '); return false;" tabindex="' + (i + newPage - 2) + '" href="javascript:void(0)">' + (i + newPage - 2) + '</a></li>';
                            }
                            if (totalPages > 3 && newPage < Math.max(totalPages - 3, 3)) {
                                newPageItem += '<li class="page-item disabled dotpage"><a class="page-link">...</a></li>';
                            }
                            for (var i = Math.max(totalPages - 3, 3), j = 0; i < totalPages; i++, j++) {
                                newPageItem += '<li class="page-item' + (i == (newPage - 1) ? " " : "") + (j == 0 ? " lastpn" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + 1) + '); return false;" tabindex="' + (i + 1) + '" href="javascript:void(0)">' + (i + 1) + '</a></li>';
                            }
                            if (newPageItem != '') $('#previousBtn').after(newPageItem);
                            if (totalPages <= newPage + 3) {
                                $('#nextBtn').addClass('disabled');
                            } else {
                                $('#nextBtn').removeClass('disabled');
                            }
                        }


                        triggerSearch(newPage);
                        searchButton();
                    }

                }
            });
            updatePagination()
            $(".toggle-search-btn").click(function () {
                var icon = $(this).find("i");
                var searchPanel = $("#searchFilters");
                icon.toggleClass("fa-chevron-down fa-chevron-up");
                searchPanel.collapse("toggle");
            });

        });

        $(document).on("click", "#previousBtn", function () {
            if (!$('#previousBtn').hasClass('disabled')) {

                var totalPages = document.getElementById('<%= totalPageValue.ClientID %>').value;
                var currentPage = parseInt($('.pagination .active .page-link').text());
                var newPage = currentPage - 1;

                if (newPage >= 1) {

                    $(".page-item").not("#previousBtn, #nextBtn").remove();
                    var newPageItem = '';

                    // Displaying the first 3 pages
                    for (var i = 0; i < Math.min(3, totalPages); i++) {
                        newPageItem += '<li class="page-item' + (i == (newPage - 1) ? " active" : "") + (i == 2 ? " firstpn" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + 1) + '); return false;" tabindex="' + (i + 1) + '" href="javascript:void(0)">' + (i + 1) + '</a></li>';
                    }

                    // Show "..." if more than 3 pages
                    if (totalPages > 3) {
                        newPageItem += '<li class="page-item disabled dotpage"><a class="page-link">...</a></li>';
                    }

                    // Displaying the last 3 pages
                    for (var i = Math.max(totalPages - 3, 3), j = 0; i < totalPages; i++, j++) {
                        newPageItem += '<li class="page-item' + (i == (newPage - 1) ? " active" : "") + (j == 0 ? " lastpn" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + 1) + '); return false;" tabindex="' + (i + 1) + '" href="javascript:void(0)">' + (i + 1) + '</a></li>';
                    }

                    // Insert new page items after the previous button
                    if (newPageItem != '') $('#previousBtn').after(newPageItem);

                    // Disable Next button if on last page
                    if (totalPages <= newPage) {
                        $('#nextBtn').addClass('disabled');
                    } else {
                        $('#nextBtn').removeClass('disabled');
                    }

                    // Trigger the search for the previous page
                    triggerSearch(newPage);
                    searchButton();
                }
            }
        });




        function triggerSearch(pn) {
            debugger;
            document.getElementById('<%= currentPageValue.ClientID %>').value = pn;
            $(".pagination .page-item").removeClass("active");
            $(".pagination .page-link[tabindex='" + pn + "']").closest(".page-item").addClass("active");

            var totalPages = parseInt(document.getElementById('<%= totalPageValue.ClientID %>').value);

            if (pn <= 1) {
                $('#previousBtn').addClass('disabled');
            } else {
                $('#previousBtn').removeClass('disabled');
            }

            if (pn + 3 >= totalPages) {
                $('#nextBtn').addClass('disabled');
            } else {
                $('#nextBtn').removeClass('disabled');
            }

            currentPagination = $('.navClass').html();
            __doPostBack('<%= SearchButton.UniqueID %>', '');

        }

        function updatePagination() {
            debugger;
            var totalPage = document.getElementById('<%= totalPageValue.ClientID %>').value;
            var currentPage = document.getElementById('<%= currentPageValue.ClientID %>').value;
            if (currentPage == '1') {
                $(".page-item").not("#previousBtn, #nextBtn").remove();
                var newPageItem = ''
                for (var i = 0; i < parseFloat(totalPage); i++) {
                    if (i < 3) {
                        newPageItem += '<li class="page-item' + (i == 0 ? " active" : "") + (i == 2 ? " firstpn" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + 1) + '); return false;" tabindex="' + (i + 1) + '" href="javascript:void(0)">' + (i + 1) + '</a></li>';
                    }
                    else if (i > 3 && i <= 4) {
                        newPageItem += '<li class="page-item disabled dotpage"><a class="page-link">...</a></li>';
                    }

                }
                for (var i = Math.max(parseFloat(totalPage) - 3, 3), j = 0; i < parseFloat(totalPage); i++, j++) {

                    newPageItem += '<li class="page-item' + (j == 0 ? " lastpn" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + 1) + '); return false;" tabindex="' + (i + 1) + '" href="javascript:void(0)">' + (i + 1) + '</a></li>';
                }
                if (newPageItem != '') $('#previousBtn').after(newPageItem);
                if (totalPage <= currentPage) $('#nextBtn').addClass('disabled');
                else $('#nextBtn').removeClass('disabled');
            }
            else {
                $('.navClass').html(currentPagination);
            }
            if (totalPage <= 1) {
                $('tfoot').hide();
            } else {
                $('tfoot').show();
            }

        }
        function searchButton() {
            ;
            // Set a value to the hidden field
            document.getElementById('<%= currentPageValue.ClientID %>').value = 1; // Set custom value here  
            $('#loader').show();
            $('#loader').removeClass('d-none');
            $('#tblDeploymentTeams').hide();
        }
        $(document).on("click", ".pagination .page-link", function () {
            window.scrollTo({
                top: 300,
                behavior: 'instant'
            });
        });
        function resetSearch() {
            $('#<%= location.ClientID %>').val('');
            $('#<%= team.ClientID %>').val('');
            $('#<%= StartDate.ClientID %>').val('');
            $('#<%= EndDate.ClientID %>').val('');
            $('#<%= position.ClientID %>').val('');
            $('#<%= currentPageValue.ClientID %>').val(1);
            $('#tblDeploymentTeams').hide();
            triggerSearch(1);
        }


    </script>
    <style>
        tfoot {
            justify-content: center;
            display: flex;
            width: 180%;
        }
    </style>
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <asp:UpdatePanel ID="updSearch" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12">
                    <div class="hpanel">

                        <div class="panel-body">
                            <asp:HiddenField ID="currentPageValue" runat="server" />
                            <asp:HiddenField ID="totalPageValue" runat="server" />


                            <div class="row">
                                <div class="hpanel hblue">
                                    <div class="panel-tools">
                                        <button class="btn btn-link toggle-search-btn" id="search" type="button" data-toggle="collapse"
                                            data-target="#searchFilters" aria-expanded="false" aria-controls="searchFilters">
                                            <i class="fa fa-chevron-down"></i>
                                        </button>
                                    </div>

                                    <%--<h4 style="margin-left: 18px;">Search</h4>--%>
                                    <h4>
                                        <asp:Literal ID="litPageName" runat="server"></asp:Literal>
                                    </h4>
                                    <div id="UpdatePanel2">
                                        <div id="divUpdateMessage" runat="server" class="alert alert-warning text-center"
                                            style="margin-bottom: 20px;" visible="false">
                                            <asp:Literal ID="litMessage" runat="server"></asp:Literal>
                                        </div>
                                        <div id="divFilterMessage" runat="server" class="alert alert-info text-center" style="margin-bottom: 20px;"
                                            visible="false">
                                            <asp:Literal ID="litFilterMessage" runat="server"></asp:Literal>
                                        </div>
                                    </div>



                                    <div class="" data-child="hpanel" data-effect="fadeInDown" runat="server" id="hpanelMembers"
                                        visible="false">

                                        <div class="container-search">
                                            <!-- Collapsible Search Filters -->
                                            <div class="collapse col-sm-12" id="searchFilters">
                                                <div class="row">
                                                    <div class="col-md-6 mb-3">
                                                        <div class="form-group fix">
                                                            <b>Search By Location :</b>
                                                            <asp:TextBox ID="location" runat="server" CssClass="form-control" placeholder="Search By Location "></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6 mb-3">
                                                        <div class="form-group fix">
                                                            <b>Team :</b>
                                                            <asp:TextBox ID="team" runat="server" CssClass="form-control" placeholder="Search By Location "></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6 mb-3">
                                                        <div class="form-group fix">
                                                            <b class="text-line">Deployment Start Date :</b>
                                                            <asp:TextBox ID="StartDate" runat="server" CssClass="form-control" placeholder="Start Date "></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 mb-3">
                                                        <div class="form-group fix">
                                                            <b class="text-line">Deployment End Date  :</b>
                                                            <asp:TextBox ID="EndDate" runat="server" CssClass="form-control" placeholder="Start Date "></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-6 mb-3">
                                                        <div class="form-group fix">
                                                            <b class="text-line">Position :</b>
                                                            <asp:TextBox ID="position" runat="server" CssClass="form-control" placeholder="Search By Position "></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="row mt-4" style="margin-right: 6px; margin-bottom: 8px;">
                                                    <div class="col-md-12 text-right ">
                                                        <asp:Button ID="SearchButton" runat="server" CssClass="btn btn-info btn-sm me-2"
                                                            Text="Search" OnClientClick="searchButton();" OnClick="SearchButton_Click" />

                                                        <button type="button" class="btn btn-danger btn-sm" onclick="resetSearch()">Clear</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <%--<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search Deployments" />--%>
                            <p>
                                <span id="rowCountWrapper" class="text-muted"></span>
                            </p>
                          <%--  <div id="loader" class="custom-spinner"></div>--%>
                            <table id="tblDeploymentTeams" class="footable table table-stripped toggle-arrow-tiny" data-page-size="8">
                                <thead>
                                    <tr>
                                        <th data-toggle="true">Deployment</th>
                                        <th>Dates</th>
                                        <th data-hide="phone">Location</th>
                                        <th data-hide="phone">Openings</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptDeploymentTeams" runat="server" OnItemDataBound="rptDeploymentTeams_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:HyperLink ID="hypCampaignName" runat="server"></asp:HyperLink><br />
                                                    <asp:HyperLink ID="hypPositions" Target="_blank" CssClass="btn btn-success m-t-md" runat="server" Text="See Open Positions"></asp:HyperLink>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblEarliestDeploymentDate" runat="server"></asp:Label>
                                                    to
                                            <br />
                                                    <asp:Label ID="lbLatestDeploymentDate" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDeploymentLocation" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPositionCount" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td>
                                            <br />
                                            <%--<asp:Button ID="TriggerSearchButton" runat="server" Text="Trigger Search" OnClientClick="triggerSearch(1); return false;" />--%>
                                            <nav class="navClass" aria-label="Page navigation example">
                                                <ul class="pagination justify-content-center">
                                                    <li class="page-item disabled" id="previousBtn">
                                                        <a class="page-link" href="javascript:void(0)" tabindex="-1">Previous</a>
                                                    </li>

                                                    <li class="page-item" id="nextBtn">
                                                        <a class="page-link" href="javascript:void(0)" tabindex="0">Next</a>
                                                    </li>
                                                </ul>
                                            </nav>

                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                    </div>
                </div>

                <!-- NAVIGATION -->
                <div class="col-sm-4 col-lg-3"></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <uc1:MemberNavigation runat="server" ID="ucMemberNavigation" />
</asp:Content>
