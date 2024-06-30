<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="NonProfitCampaign.aspx.cs" Inherits="V1_NonProfit_NonProfitCampaign" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>
<%@ Register Src="~/V1/UserControls/TimeBoard.ascx" TagPrefix="uc1" TagName="TimeBoard" %>

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

			$('.editCampaign').click(function () {
				window.location.href = '<%=editCampaignLink%>';
				return false;
			});
		});
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="normalheader">
	<div class="hpanel">
		<div class="panel-body">
			<div class="row">
				<div class="col-lg-8">
					Deployment for <asp:Literal ID="litEventName" runat="server"></asp:Literal>
					<h2 class="font-light m-b-xs"> 
						<asp:Literal id="litCampaignName" runat="server"></asp:Literal>
					</h2>
                    <div>
					    <b>Mission</b>
					    <p>
						    <asp:Literal id="litCampaignMission" runat="server"></asp:Literal>
					    </p>
                        <div class="row">
				            <div class="col-lg-6">
                                <h1 class="m-xs font-extra-bold" style="color:#62CB31;"><%=_totalVolunteerHours %></h1>

                                <h4 class="font-extra-bold no-margins text-success">
                                    Volunteer Hours
                                </h4>
                                <small>Total hours recorded by volunteers for this deployment. When a volunteer works through an approved nonprofit, many of the volunteers hours can be credit 
                                    to the county where the volunteer labor is happening. It's important to select approved nonprofits. You can find many of them right here on Stability.org.
                                </small>
				            </div>
				            <div class="col-lg-6">
                                    <h1 class="m-xs font-extra-bold" style="color:#62CB31;"><%=_totalVolunteerValue %>*</h1>

                                    <h4 class="font-extra-bold no-margins text-success">
                                        Volunteer Offset
                                    </h4>
                                    <small>This is how much can be credited against FEMA for the county where this deployment is taking place. 
                                        It is calculated by multiplying volunteer hours by <span  style="color:#62CB31;"><%=_volunteerHourlyRate%></span> which is the value of volunteer time per hour. This number varies by state.</small>
				            </div>
                        </div>
                    </div>
				</div>
				<div class="col-lg-4">
                    <div>
					    <div class="m-b-sm m-t-xs">
						    <asp:LinkButton id="lbGetHelp" runat="server" CssClass="btn btn-warning btn-dark getHelpButton" Text="Get Help" visible="false"></asp:LinkButton>
						    <asp:LinkButton id="lbDonate" runat="server" CssClass="btn btn-success donateButton" Text="Donate to Deployment" visible="false"></asp:LinkButton>
					        <asp:LinkButton id="lbVolunteer" runat="server" CssClass="btn btn-success volunteerButton" Text="Volunteer for Deployment" visible="false"></asp:LinkButton>
		                    <asp:Button ID="btnActiveVolunteer" runat="server" CssClass="btn btn-light" Visible="false" />
                            <asp:Button Visible="false" CssClass="btn w-xs btn-light" ID="hypCauseIsNotActive" runat="server"></asp:Button>
                            <p>
					            <small>
						            Campaign created by: <asp:HyperLink id="hypOrganizationName" runat="server"></asp:HyperLink> 
						            for <asp:HyperLink id="hypEventName" runat="server"></asp:HyperLink>
					            </small>
                            </p>
					        <div runat="server" visible="false" id="divEditCampaign">
						        <div class="m-r-lg">
							        <asp:Button id="btnEdit" runat="server" CssClass="btn btn-xs btn-warning editCampaign" Text="Edit Deployment" />
						        </div>
					        </div>
                         </div>
                    </div>
                    <div class="hpanel stats">
                        <div class="panel-body h-200 list">
                            <div class="stats-title pull-left">
                                <h4>Nonprofit Volunteer Impact Is <span style="color:#62CB31;"><%=_volunteerHourlyRate%></span>/HR</h4>
                            </div>
                            <div class="stats-icon pull-right">
                                <i class="pe-7s-share fa-4x"></i>
                                <span class="label label-success pull-left" style="color:#ffffff; background-color:#62CB31;">LIVE DATA</span>
                            </div>
                            <div class="m-t-xl">
                                <span class="font-bold no-margins">
                                    Volunteer Activity
                                </span>
                                <br/>
                                <small>
                                    Nonprofit value in time and dollars for this deployment.
                                    <br />Updated: <%=DateTime.Now.ToShortDateString() %> <%=DateTime.Now.ToShortTimeString() %>
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
                                    <small class="stats-label">TODAY</small>
                                </div>
                                <div class="col-xs-2">
                                    <h3 class="no-margins font-extra-bold text-success" style="color:#62CB31;"><%=_todayVolunteerCount%></h3>
                                </div>
                                <div class="col-xs-2">
                                    <h3 class="no-margins font-extra-bold text-success" style="color:#62CB31;"><%=_todayVolunteerHours%></h3>
                                </div>
                                <div class="col-xs-6">
                                    <h3 class="no-margins font-extra-bold text-success" style="color:#62CB31;"><%=_todayVolunteerValue%>*</h3>
                                </div>
                            </div>
                            <div class="row m-t-sm bg-success">
                                <div class="col-xs-2">
                                    <small class="stats-label">TOTAL</small>
                                </div>
                                <div class="col-xs-2">
                                    <h3 class="no-margins font-extra-bold text-success" style="color:#62CB31;"><%=_totalVolunteerCount%></h3>
                                </div>
                                <div class="col-xs-2">
                                    <h3 class="no-margins font-extra-bold text-success" style="color:#62CB31;"><%=_totalVolunteerHours%></h3>
                                </div>
                                <div class="col-xs-6">
                                    <h3 class="no-margins font-extra-bold text-success" style="color:#62CB31;"><%=_totalVolunteerValue%>*</h3>
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer">
                             <small>*This number is calcuted based on volunteer hours. It does not reflect a cash donation.</small>
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
		<div class="col-lg-6">
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
		<div class="col-lg-6">
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
</div>
</asp:Content>

