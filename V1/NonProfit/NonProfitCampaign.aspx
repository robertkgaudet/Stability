<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Child.master" AutoEventWireup="true" CodeFile="NonProfitCampaign.aspx.cs" Inherits="V1_NonProfit_NonProfitCampaign" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/1-Column-Child.master"%>
<%@ Register Src="~/V1/UserControls/TimeBoard.ascx" TagPrefix="uc1" TagName="TimeBoard" %>
<%@ Register Src="~/V1/UserControls/PositionNavigation.ascx" TagPrefix="uc1" TagName="PostionNavigation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script type="text/javascript">

		$(document).ready(function () {

			$('.donateButton').click(function () {
				window.location.href = '<%=donateLink%>';
				return false;
			});

			$('.volunteerButton').click(function () {
				window.location.href = '<%=volunteerLink%>';
				return false;
			});

            $('.getHelpButton').click(function () {
                window.location.href = '<%=getHelpLink%>';
                return false;
            });
			
		});
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<uc1:PostionNavigation runat="server" ID="ucPostionNavigation" />
	<div class="hpanel">
		<div class="panel-body">
			<div class="row">
				<div class="col-lg-8">
                    <div>
					    <b>Mission</b>
					    <p>
						    <asp:Literal id="litCampaignMission" runat="server"></asp:Literal>
					    </p>
						<div class="m-t-lg">
							If you would like to help you can <asp:HyperLink id="lbVolunteer" runat="server" CssClass="volunteerButton" Text="view open volunteer positions" visible="false"></asp:HyperLink>, <a href="/V1/Administration/TeamName.aspx?userActionModal=false">create your own team</a> or <asp:HyperLink id="linkDeploymentMap" runat="server" CssClass="mapButton" Text="view the deployment map"></asp:HyperLink> showing areas that need assistance.
							
							<asp:Button ID="btnActiveVolunteer" runat="server" CssClass="btn btn-light" Visible="false" />
							<asp:LinkButton id="lbGetHelp" runat="server" CssClass="btn btn-warning btn-dark getHelpButton" Text="Get Help" visible="false"></asp:LinkButton>
							<asp:Button Visible="false" CssClass="btn w-xs btn-light" ID="hypCauseIsNotActive" runat="server"></asp:Button>
							<br /><br />
							<div class="col-md-3 m-t-lg">
								<asp:LinkButton id="lbDonate" runat="server" CssClass="btn btn-success btn-block donateButton" Text="Donate Here" visible="false"></asp:LinkButton>
							</div>
							<div class="col-md-3 m-t-lg">
								<asp:Hyperlink id="hypViewCases" runat="server" CssClass="btn btn-info btn-block" Text="View Cases" visible="false"></asp:Hyperlink>
							</div>
							<div class="col-md-3 m-t-lg">
								<asp:Hyperlink id="hypAddCase" runat="server" CssClass="btn btn-info btn-block" Text="Add A Case" visible="false"></asp:Hyperlink>
							</div>
						</div>
                    </div>
				</div>
				<div class="col-lg-4">
                    <div class="hpanel stats">
                        <div class="panel-body h-200 list">
                            <div>
                                <div class="font-bold no-margins">
                                    Deployment Activity
                                </div>
                                <small>
                                    Time/monetary value of this deployment.
                                </small>
                            </div>
                            <div class="row m-t-sm bg-info">
                                <div class="col-xs-2"></div>
                                <div class="col-xs-2"><small class="stats-label">Vols</small></div>
                                <div class="col-xs-2"><small class="stats-label">Hours</small></div>
                                <div class="col-xs-6"><small class="stats-label">Total</small></div>
                            </div>
                            <div class="row m-t-sm bg-success">
                                <div class="col-xs-2">
                                    <small class="stats-label">Today</small>
                                </div>
                                <div class="col-xs-2">
                                    <span class="no-margins"><%=_todayVolunteerCount%></span>
                                </div>
                                <div class="col-xs-2">
                                    <span class="no-margins"><%=_todayVolunteerHours%></span>
                                </div>
                                <div class="col-xs-6">
                                    <span class="no-margins"><%=_todayVolunteerValue%>*</span>
                                </div>
                            </div>
                            <div class="row m-t-sm bg-success">
                                <div class="col-xs-2">
                                    <small class="stats-label">Total</small>
                                </div>
                                <div class="col-xs-2">
                                    <span class="no-margins"><%=_totalVolunteerCount%></span>
                                </div>
                                <div class="col-xs-2">
                                    <span class="no-margins"><%=_totalVolunteerHours%></span>
                                </div>
                                <div class="col-xs-6">
                                    <span class="no-margins"><%=_totalVolunteerValue%>*</span>
                                </div>
                            </div>
                            <div class="stats-icon pull-right m-t-sm">
                                <span class="label label-success pull-left" style="color:#ffffff; background-color:#62CB31;">LIVE DATA</span>
                            </div>
                            <div class="stats-title pull-left">
                                <h4>Volunteer Rate <span style="color:orangered;"><%=_volunteerHourlyRate%>/HR</span></h4>
                            </div>
                        </div>
                        <div class="panel-footer">
                             <small>*This number is calcuted based on volunteer hours. It does not reflect a cash donation. <br />
                                    Latest Update: <%=DateTime.Now.ToShortDateString() %> <%=DateTime.Now.ToShortTimeString() %></small>
                        </div>
                    </div>
				</div>
			</div>
		</div>
        <div class="panel-footer">
            <div class="alert alert-success" runat="server" id="divAlertMessage" visible="false">
                <i class="fa fa-bolt"></i> <asp:Literal id="litAlertMessage" runat="server"></asp:Literal>
            </div>
        </div>
	</div>
</div>

<div class="content">
	<div class="row">
		<div class="col-xs-12">
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					Social Media Links
				</div>
				<div class="panel-body">
					<dl class="dl-horizontal">
						<dt runat="server" id="dtWebsite" visible="false">
							Website
						</dt>
						<dd runat="server" id="ddWebsite" visible="false" class="m-b-sm">
							<asp:HyperLink id="hypWebsite" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							Facebook Page
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypFacebookPage" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							Facebook Group
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypFacebookGroup" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							Blog
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypBlog" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							YouTube Channel
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypYouTube" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							Twitter
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypTwitter" runat="server" target="new"></asp:HyperLink>
						</dd>
					</dl>
				</div>
                <div class="panel-footer"></div>
			</div>
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					This Campaign Created By:
				</div>
				<div class="panel-body">
                    <div class="row">
	                    <div class="col-sm-6">
					        <dl class="dl-vertical">
						        <dt>
							        Point of Contact
						        </dt>
						        <dd class="m-b-sm">
							        <asp:Label id="lblPointOfContactPerson" runat="server"></asp:Label>
						        </dd>
						
						        <dt id="dtZelloChannel" runat="server" visible="false">
							        Zello Channel
						        </dt>
						        <dd id="ddZelloChannel" runat="server" class="m-b-sm" visible="false">
							        <asp:Label id="lblZelloChannel" runat="server"></asp:Label>
						        </dd>

						        <dt id="dtPhone" runat="server">
							        Phone Number
						        </dt>
						        <dd id="ddPhone" runat="server" class="m-b-sm">
							        <asp:HyperLink id="hypPointOfContactPhone" runat="server"></asp:HyperLink>
						        </dd>
						
						        <dt id="dtEmail" runat="server">
							        Email
						        </dt>
						        <dd id="ddEmail" runat="server" class="m-b-sm">
							        <asp:HyperLink id="hypPointOfContactEmail" runat="server"></asp:HyperLink>
						        </dd>

						        <dt id="dtPrimaryPhone" runat="server">
							        Phone
						        </dt>
						        <dd id="ddPrimaryPhone" runat="server" class="m-b-sm">
							        <asp:HyperLink id="hypPrimaryPhone" runat="server" target="new"></asp:HyperLink>
						        </dd>

					        </dl>
                        </div>
	                    <div class="col-sm-6">
					        <dl class="dl-verticle">
						        <dd>
							        <asp:Label id="lblParentOrgName" runat="server"></asp:Label>
						        </dd>
						        <dd id="ddParentAddress" runat="server" class="m-b-sm">
							        <asp:HyperLink id="hypParentAddress" runat="server" target="new"></asp:HyperLink>
						        </dd>
						        <dd id="ddParentPhone" runat="server" visible="false">
							        <asp:HyperLink id="hypParentPhone" runat="server" target="new"></asp:HyperLink>
						        </dd>
						        <dd id="ddParentEmail" runat="server" visible="false">
							        <asp:HyperLink id="hypParentEmail" runat="server" target="new"></asp:HyperLink>
						        </dd>
						        <dd id="ddParentWebsite" runat="server" visible="false">
							        <asp:HyperLink id="hypParentWebsite" runat="server" target="new"></asp:HyperLink>
						        </dd>	
					        </dl>
					        <dl class="dl-horizontal">
						        <dt>
							        VOAD Member
						        </dt>
						        <dd>
							        <asp:Label id="lblVoadMember" runat="server"></asp:Label>
						        </dd>
						        <dt>
							        501c3
						        </dt>
						        <dd class="m-b-sm">
							        <asp:Label id="lbl501c3" runat="server"></asp:Label>
						        </dd>
					        </dl>
                        </div>
                     </div>
				</div>
                <div class="panel-footer"></div>
			</div>
		</div>
		<div class="col-xs-12 l-lg-6">
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					Volunteers for this Cause (Active within the past 30 days.)
				</div>
				<div class="panel-body">
					<asp:Repeater ID="rpNonProfitPeople" runat="server" OnItemDataBound="rpNonProfitPeople_ItemDataBound">
						<HeaderTemplate>
						</HeaderTemplate>
						<ItemTemplate>
						<asp:Literal id="lblInfo" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:Repeater>
				</div>
                <div class="panel-footer"></div>
			</div>
		</div>
	</div>
</asp:Content>

