<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TeamNavigation.ascx.cs" Inherits="V1_UserControls_TeamNavigation" %>

                <div class="hpanel">
                    <div class="panel-body">
						<asp:HyperLink runat="server" ID="hypGetHelp" CssClass="btn btn-danger btn-block"><i class='fa fa-check'></i> Get Help</asp:HyperLink>
                        <ul class="mailbox-list m-t-lg">
                            <li <%=_teamPageActive%>>
                                <asp:HyperLink runat="server" ID="hypTeamName"><i class="fa fa-th-large"></i> <asp:Literal ID="litTeamName" runat="server"></asp:Literal></asp:HyperLink>
                            </li>
                            <li <%=_activityPageActive%>>
                                <asp:HyperLink runat="server" ID="hypActivity"><i class="fa fa-rocket"></i> Activity Dashboard</asp:HyperLink>
                            </li>
                            <li <%=_deploymentPageActive%>>
                                <asp:HyperLink runat="server" ID="hypDeployments"><i class="fa fa-street-view"></i> Deployments</asp:HyperLink>
                            </li>
                            <li <%=_programsPageActive%>>
                                <asp:HyperLink runat="server" ID="hypPrograms"><i class="fa fa-plane"></i> Programs</asp:HyperLink>
                            </li>
                            <li <%=_peoplePageActive%>>
                                <asp:HyperLink runat="server" ID="hypPeople"><i class="fa fa-vcard"></i> People</asp:HyperLink>
                            </li>
                            <li <%=_websitePageActive%>>
                                <asp:HyperLink runat="server" ID="hypWebsite" Target="_blank"><i class="fa fa-globe text-website"></i> Website</asp:HyperLink>
                            </li>
                        </ul>
                        <hr runat="server" id="hrAdmin" visible="false"></hr>
                        <ul class="mailbox-list" runat="server" id="ulAdmin" visible="false">
                            <li <%=_ticketPageActive%>>
                                <asp:HyperLink runat="server" ID="hypTickets"><i class="fa fa-clipboard text-danger"></i> Tickets</asp:HyperLink>
                            </li>
                            <li <%=_reportPageActive%>>
                                <asp:HyperLink runat="server" ID="hypReports"><i class="fa fa-file-text text-warning"></i> Reports</asp:HyperLink>
                            </li>
                            <li <%=_settingsPageActive%>>
                                <asp:HyperLink runat="server" ID="hypSettings"><i class="fa fa-cog"></i> Settings</asp:HyperLink>
                            </li>
                        </ul>
                        <hr>
                        <ul class="mailbox-list">
                            <li <%=_supportPageActive%>>
                                <asp:HyperLink runat="server" ID="hypSupport"><i class="fa fa-info-circle"></i> Support</asp:HyperLink>
                            </li>
                        </ul>
                    </div>
                </div>