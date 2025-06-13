<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/2-Column-Child.master" AutoEventWireup="true" CodeFile="Deployments.aspx.cs" Inherits="V1_Deployments" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/2-Column-Child.master" %>
<%@ Register Src="~/V1/UserControls/MemberNavigation.ascx" TagPrefix="uc1" TagName="MemberNavigation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .highlight {
            background-color: yellow;
            font-weight: bold;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-xs-12">
            <div class="hpanel">
                <div class="panel-body">
                    <asp:HyperLink Text="Create A Disaster Deployment" runat="server" Visible="false" ID="hypAddDeployment" CssClass="btn btn-info btn-large pull-right"></asp:HyperLink>
                    <h4><asp:Literal ID="litPageName" runat="server"></asp:Literal></h4>
                    Find an active disaster deployment to join or <a href="/V1/Administration/TeamName.aspx?userActionModal=false">create a team</a> to start your own deployment.
                    <asp:HyperLink ID="hypMapView" runat="server" CssClass="pull-right" Text="Map View <i class='fa fa-map m-t-xs'></i>" NavigateUrl="/MapZone"></asp:HyperLink>

                    <input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search Deployments" />

                    <p>
                        <span id="rowCountWrapper" class="text-muted">
                            <asp:Literal ID="litCount" runat="server"></asp:Literal>
                        </span>
                    </p>

                    <div class="alert alert-success" id="divJoinDeploymentMessage" runat="server" visible="false">
                        <h4><i class="fa fa-users"></i> Choose A Deployment or Create One</h4>
                    </div>

                    <table id="tblDeployments" class="footable table toggle-arrow-tiny table-hover table-bordered table-striped" data-page-size="500">
                        <tbody>
                            <asp:Repeater ID="rptDeployments" runat="server" OnItemDataBound="rptDeployments_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div style="height:50px;">
                                                <div class="row">
                                                    <div class="col-xs-12 col-lg-6">
                                                        <asp:Image ID="imgLogo" CssClass="m-r-md" runat="server" />
                                                        <asp:HyperLink ID="hypDeploymentName" runat="server" Font-Bold="true" CssClass="hypDeploymentName"></asp:HyperLink><br />
                                                        <asp:Label ID="lblDescription" runat="server" CssClass="lblDescription"></asp:Label>
                                                    </div>
                                                    <div class="col-xs-12 col-lg-6">
                                                        <div class="pull-left text-muted">
                                                            <small>
                                                                <asp:HyperLink ID="hypEventName" runat="server"></asp:HyperLink><br />
                                                                <asp:Literal ID="litOrganizationName" runat="server"></asp:Literal><br />
                                                                <asp:Literal ID="litDates" runat="server"></asp:Literal>
                                                            </small>
                                                        </div>
                                                    </div>
                                                </div>
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
        <div class="col-sm-4 col-lg-3"></div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            const urlParams = new URLSearchParams(window.location.search);
            const searchValue = urlParams.get('searchTerm');

            function updateVisibleRowCount() {
                const visibleCount = $("#tblDeployments tbody tr:visible").length;
                $("#rowCountWrapper").text(visibleCount + " Deployments");
            }

            function removeHighlights($element) {
                $element.each(function () {
                    const originalText = $(this).text();
                    $(this).html(originalText);
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

                $("#tblDeployments tbody tr").each(function () {
                    const $row = $(this);
                    const $name = $row.find(".hypDeploymentName");
                    const $desc = $row.find(".lblDescription");

                    const nameText = $name.text().toLowerCase();
                    const descText = $desc.text().toLowerCase();
                    const combinedText = nameText + " " + descText;

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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <uc1:MemberNavigation runat="server" ID="ucMemberNavigation" />
</asp:Content>
