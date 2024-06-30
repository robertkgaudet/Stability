<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EventHeader.ascx.cs" Inherits="V1_UserControls_EventHeader" %>
		<style>
		.rowBody
		{
			display: table;
		}

		.rowBody [class*="col-"]
		{
			float: none;
			display: table-cell;
			vertical-align: top;
		}
		</style>

		<div class="row">
            <div class="col-lg-12">
                <div class="hpanel">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-2">
                                <div class="hpanel">
                                    <div class="panel-body" style="background-color:#C0392B; color:white;">
                                        <dl>
                                            <dt>
                                                <div style="border:solid 3px white; text-align:center; margin-bottom:5px;" class="font-extra-bold font-uppercase">
                                                    <asp:Literal ID="litWeatherType" runat="server"></asp:Literal>
                                                </div>
                                            </dt>
                                            <dt>WHEN: <i class="fa fa-calendar"></i> <asp:Literal ID="litDate" runat="server"></asp:Literal></dt>
                                            <dt> <small><i class="fa fa-clock-o"></i> <asp:Literal ID="litAge" runat="server"></asp:Literal></small></dt>
                                            <dt>STATES:</dt>
                                            <dd>
                                                <asp:Literal ID="litStates" runat="server"></asp:Literal>
                                            </dd>
                                        </dl>
                                    </div>
                                    <div class="panel-footer">
                                        <div class="row">
                                            <div class="col-xs-5 border-right">
                                                <div class="contact-stat"><span>Deployments: </span> <strong><asp:Literal ID="litCauseCount" runat="server"></asp:Literal></strong></div>
                                            </div>
                                            <div class="col-xs-3 border-right">
                                                <div class="contact-stat"><span>Teams: </span> <strong><asp:Literal ID="litTeamCount" runat="server"></asp:Literal></strong></div>
                                            </div>
                                            <div class="col-xs-4">
                                                <div class="contact-stat"><span>Tickets: </span> <strong><asp:Literal ID="litTicketCount" runat="server"></asp:Literal></strong></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-10">
								<div id="hbreadcrumb" class="pull-right">
									<ol class="hbreadcrumb breadcrumb">
										<li>
											<asp:HyperLink ID="hypBreadcrumbTeamName" runat="server"></asp:HyperLink>
										</li>
										<li class="active">
											<span>
												<asp:Literal ID="litBreadcrumbPageName" runat="server"></asp:Literal>
											</span>
										</li>
									</ol>
								</div>
                                <h2>
                                    <asp:Literal ID="litEventName" runat="server"></asp:Literal>
                                </h2>
                                <h4>Community Relief Portal</h4>
                                <p>
					                <asp:Literal ID="litEventDescription" runat="server"></asp:Literal>
                                </p>
								<asp:Literal ID="litStatesCounties" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>