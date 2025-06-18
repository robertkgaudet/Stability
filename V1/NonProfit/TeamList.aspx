<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/2-Column-Child.master"
    AutoEventWireup="true" CodeFile="TeamList.aspx.cs" Inherits="V1_NonProfit_TeamList" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/2-Column-Child.master" %>
<%@ Register Src="~/V1/UserControls/MemberNavigation.ascx" TagPrefix="uc1" TagName="MemberNavigation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .highlight {
            background-color: yellow;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-xs-12">
            <div class="hpanel">
                <div class="panel-body">
                    <a href="/V1/Administration/TeamName.aspx?userActionModal=false" runat="server"
                        id="hrefCreateTeam" visible="false" class="btn btn-info btn-large pull-right">Create
                        A Team</a>
                    <a href="/V1/Profile/EditSkills.aspx?register=true" runat="server"
                        id="skipstep" visible="false" class="btn btn-info btn-large pull-right" style=" margin-right: 15px;">Skip for now</a>
                    <h4>
                        <asp:Literal ID="litPageName" runat="server"></asp:Literal>
                    </h4>

                    <span id="rowCountWrapper" style="font-size: 14px; font-weight: normal;" class="text-muted">
                        <asp:Literal ID="litCount" runat="server"></asp:Literal>
                    </span>

                    <input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search Teams" />

                    <div class="alert alert-success" id="divJoinTeamMessage" runat="server" visible="false">
                        <h4><i class="fa fa-users"></i>Choose A Team or Create Your Own</h4>
                    </div>

                    <table id="tblTeams" class="footable table toggle-arrow-tiny table-hover table-bordered table-striped"
                        data-page-size="500">
                        <tbody>
                            <asp:Repeater ID="rptTeams" runat="server" OnItemDataBound="rptTeams_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div style="height: 50px;">
                                                <asp:Image ID="imgLogo" CssClass="m-r-md" runat="server" />
                                                <asp:HyperLink ID="hypTeamName" CssClass="hypTeamName" Font-Bold="true" runat="server"></asp:HyperLink>
                                                <asp:Label ID="lblDescription" CssClass="lblDescription" runat="server"></asp:Label>
                                            </div>
                                        </td>
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

        <!-- NAVIGATION -->
        <div class="col-sm-4 col-lg-3"></div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            const urlParams = new URLSearchParams(window.location.search);
            const searchValue = urlParams.get('searchTerm');

            function updateVisibleRowCount() {
                const visibleCount = $("#tblTeams tbody tr:visible").length;
                $("#rowCountWrapper").text(visibleCount + " Teams");
            }

            function removeHighlights($element) {
                $element.each(function () {
                    const originalText = $(this).text();
                    $(this).html(originalText); // reset to plain text
                });
            }

            function highlightElementText($element, keyword) {
                if (!keyword) return;
                const regex = new RegExp("(" + keyword.replace(/[.*+?^${}()|[\]\\]/g, "\\$&") + ")", "gi");

                $element.each(function () {
                    const html = $(this).text().replace(regex, "<span class='highlight'>$1</span>");
                    $(this).html(html);
                });
            }

            function applyFilterAndHighlight(keyword) {
                const lowerKeyword = keyword.toLowerCase();

                $("#tblTeams tbody tr").each(function () {
                    const $row = $(this);
                    const $name = $row.find(".hypTeamName");
                    const $desc = $row.find(".lblDescription");

                    const nameText = $name.text().toLowerCase();
                    const descText = $desc.text().toLowerCase();
                    const combinedText = nameText + " " + descText;

                    // Remove old highlights
                    removeHighlights($name);
                    removeHighlights($desc);

                    if (combinedText.includes(lowerKeyword)) {
                        $row.show();
                        highlightElementText($name, keyword);
                        highlightElementText($desc, keyword);
                    } else {
                        $row.hide();
                    }
                });

                updateVisibleRowCount();
            }

            if (searchValue) {
                $("#filter").val(searchValue);
                applyFilterAndHighlight(searchValue);
            }

            $("#filter").on("keyup", function () {
                const keyword = $(this).val().trim();
                applyFilterAndHighlight(keyword);
            });

            updateVisibleRowCount();
        });
    </script>

    <script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <uc1:MemberNavigation runat="server" ID="ucMemberNavigation" />
</asp:Content>
