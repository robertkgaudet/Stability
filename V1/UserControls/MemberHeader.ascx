<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MemberHeader.ascx.cs" Inherits="V1_UserControls_MemberHeader" %>
<%@ Register Src="~/V1/UserControls/TeamLogo.ascx" TagPrefix="uc1" TagName="TeamLogo" %>
<style>
	.divCover {
		width: 100%;
		background-image: url('<%=_coverImage%>'); /* Path to your image */
		background-size: cover; /* Scale the image to cover the entire div */
		background-position: center; /* Center the image */
		background-repeat: no-repeat; /* Prevent the image from repeating */
		height:200px;
		border-top-left-radius: 10px !important;
		border-top-right-radius: 10px !important;
	}
	.memberImageContainer
	{
        overflow: hidden;		display: flex;
		margin-top: -125px; /* Default margin */		z-index: 1;
		position: relative	}
    @media (max-width: 767px) 
	{
        .memberImageContainer
		{
			justify-content:center;
			margin-top: -145px; /* Default margin */
			width:50%;
			position: relative
        }
    }
	.memberPhoto
	{		width: 150px;
		height: 150px;
		border-radius: 50%;
		object-fit: cover; /* Ensures the image covers the entire area */
		border: 5px solid white; /* Adds the white border */
		position: relative
    }
	.memberDetail
	{		z-index: 0;
		text-align:left;
	}
	.content
	{
		padding:0px 0px 0px 0px !important;
	}
	.member-panel-body {
		border-bottom-left-radius: 10px !important;
		border-bottom-right-radius: 10px !important;
	}
    .camera-icon {
        position: absolute;
        bottom: 10px;
        right: 120px;
        background-color: rgba(0, 0, 0, 0.3); /* Optional background for better visibility */color: white;
        padding: 4px;
        border-radius: 40%;
        cursor: pointer;
        font-size: 15px;
    }

    .camera-icon:hover {
        background-color: rgba(0, 0, 0, 0.8); /* Darker background on hover */
    }
	.fa-pending-color {
    color: gainsboro;
	}
	.fa-approved-color {
    color: #63CB31;
	}
	.fa:hover
	{cursor:pointer;}
	.teamPhoto{
	height:22px;
}
	.profile{
      margin-right: -2px !important;
    margin-top: 5px !important;
}

	.user-name {
        color: darkslategrey !important;
    font-size: 20px !important;
    font-weight: 800 !important;
    margin-right: 6px !important;

}
	.StreamLink {
    text-decoration:none !important;
}
	.team-logo{
     margin-bottom: 4px !important;
	}
	.contact-stat:hover
	{cursor:pointer;}

</style>
<style>
.tooltip-wrapper {
    position: relative;
    display: inline-block;
    text-align: center;
}

.tooltip-trigger {
    cursor: pointer;
    font-size: 18px;
    padding: 4px;
    color: #0078D4;
    font-weight: bold;
    border: none;
    background: none;
}

.tooltip-box {
    display: none;
    position: absolute;
    bottom: 125%; /* place above the icon */
    left: 50%;
    transform: translateX(-50%);
    background-color: #5E2E91;
    color: #fff;
    padding: 10px 12px;
    border-radius: 6px;
    white-space: pre-line;
    width: 250px;
    font-size: 15px;
    z-index: 100;
    box-shadow: 0px 4px 10px rgba(0,0,0,0.3);
	text-align: left;
}

.tooltip-box::after {
    content: "";
    position: absolute;
    top: 100%;
    left: 50%;
    margin-left: -6px;
    border-width: 6px;
    border-style: solid;
    border-color: #333 transparent transparent transparent;
}
</style>
<script>
	$(document).ready(function () {

		$('.faIdBadgeClick').click(function () {
			window.location.href = "/IDCard";
			return false;
		});

		$("body").tooltip({ selector: '[data-toggle=tooltip]' });
	});
</script>
	<div id="divCover" class="divCover"></div>
	<div class="hpanel">
		<div class="panel-body member-panel-body">
			<div class="memberImageContainer col-xs-3 col-md-4">
				<asp:Image runat="server" id="imgMemberProfilePhoto" class="memberPhoto" />
                <asp:HyperLink runat="server" ID="linkCamera"   CssClass="camera-icon"  ToolTip="Edit"   Visible="false">
                <i class="fa fa-camera"></i>
                </asp:HyperLink>
			</div>	
            <div class="pull-right" id="divMemberEditControl" runat="server">
                <div class="btn-group">
					<div class="btn-group" runat="server" id="divUserSettings" visible="false">
						<button data-toggle="dropdown" class="btn btn-xs btn-default dropdown-toggle"><i class="fa fa-gear"></i> Settings <span class="caret"></span></button>
						<ul class="dropdown-menu">
							<li><asp:HyperLink ID="hypMyTeam" Font-Bold="true" runat="server" Text="My Team"></asp:HyperLink></li>
							<li><a href="/IDCard">ID Card</a></li>
							<li><a href="/V1/Profile/Time.aspx">Time Sheet</a></li>
							<li class="divider"></li>
							<li>
								<a href="/V1/Profile/AvailableDates.aspx">
									<i class="fa fa-calendar"></i> Calendar
								</a>
							</li>
							<li>
								<a href="/V1/Profile/EditNonProfitCauses.aspx">
									<i class="fa fa-street-view"></i> Deployments
								</a>
							</li>
							<li>
								<a href="/V1/Profile/EditDisasters.aspx">
									<i class="fa fa-map-marker"></i> Portals
								</a>
							</li>
							<li>
								<a href="/V1/Profile/EditSkills.aspx">
									<i class="fa fa-hand-pointer-o"></i> Skills
								</a>
							</li>
							<li>
								<a href="/V1/Profile/EditResources.aspx">
									<i class="fa fa-truck"></i> Resources
								</a>
							</li>
							<li class="divider"></li>
							<li><a href="/V1/SignOut.aspx">Sign Out</a></li>
						</ul>
					</div>
					<asp:HyperLink CssClass="btn btn-xs btn-default" runat="server" id="hypProfileAdmin" visible="false"><i class="fa fa-lock"></i> Admin Edit</asp:HyperLink>
					<asp:HyperLink CssClass="btn btn-xs btn-default" runat="server" id="hypProfileEdit" NavigateUrl="/V1/Profile/ProfileEdit.aspx" Visible="false"><i class="fa fa-user-circle"></i> Edit Profile</asp:HyperLink>
				</div>
            </div>
			<div class="row memberDetail">
                <div class="col-xs-12 col-md-8">
					<div style="margin-top:10px; width:100%;">
						<div style="color:darkslategrey; font-size:20px; font-weight:600; margin-right:10px;">
							<uc1:TeamLogo  runat="server" ID="ucTeamLogo" />
							<div style="color:#C0C0C0; font-size:15px; position: relative; top:-5x; font-weight:500; ">
								<asp:Literal ID="litTitle" runat="server"></asp:Literal>
								<asp:Literal ID="litLocation" runat="server"></asp:Literal>
							</div>
						</div>
					</div>
					<div style="clear: both;"></div>
					<p style="font-size:16px;">
						<asp:Literal ID="litMemberDescription" runat="server"></asp:Literal>
					</p>
				</div>
                <div class="col-xs-12 col-md-4">
                    <div class="project-value" style="margin-top:20px;">
						<asp:HyperLink ID="hypTeam" Font-Bold="true" runat="server"></asp:HyperLink>
						<p style="top:10px; position:relative;">
							<i class="fa fa-id-badge pe-2x <%=_badgeVettingStatus%> float-right <%=faIdBadgeClick%>" data-toggle="tooltip" data-placement="top" title="Activate Vetted ID Badge"></i>
							<i class="fa fa-shield pe-2x <%=_badgeCertificationStatus%> float-right" data-toggle="tooltip" data-placement="top" title="Earn Training Certifications To Activate"></i>
							<i class="fa fa-certificate pe-2x <%=_badgeDeployedStatus%> float-right" data-toggle="tooltip" data-placement="top" title="Deploy Once To Activate"></i>
							<i class="fa fa-check-circle pe-2x <%=_badgeHoursRecordedStatus%> float-right" data-toggle="tooltip" data-placement="top" title="Record 8 Hours of Volunteer Time To Activate"></i>
							<i class="fa fa-star pe-2x <%=_badgeTOPStatus%> float-right" data-toggle="tooltip" data-placement="top" title="Record 100 Hours of Volunteer Time or 5 Or More Deployments to Activate"></i>
							<i class="fa fa-ban text-danger pe-2x float-right" runat="server" id="idKwest" visible="false" data-toggle="tooltip" data-placement="top" title="Work in Progress"></i>
						</p>
						<asp:Literal ID="litTeamBreak" runat="server"></asp:Literal>
                    </div>
					<asp:LinkButton runat="server" ID="btnFriend" ClientIDMode="Static">Request Connection</asp:LinkButton>
                </div>
			</div>
			<div class="row memberDetail">
                <div class="col-xs-12 project-info">
					<div class="hpanel">
						<div class="panel-heading">
							Your rank reflects your impact and activity across the Stability network.
							<small>
								<div class="tooltip-wrapper" title="<%= Server.HtmlEncode(rankTooltip) %>">
									<span class="tooltip-trigger" onclick="toggleTooltip()"><i class="pe-7s-info"></i></span>
									<div id="customTooltip" class="tooltip-box">
										<%= Server.HtmlEncode(rankTooltip) %>
									</div>
								</div>
							</small>
						</div>
						<div class="panel-body member-panel-body">
							<div class="row">
								<div class="col-xs-3 border-right"><div class="contact-stat custom-tooltip" data-toggle="tooltip" data-placement="top" title="Members ranking on their primary team."><span>PRIMARY TEAM RANK</span> <strong><asp:Literal ID="litTeamRank" runat="server"></asp:Literal></strong></div></div>
								<div class="col-xs-3 border-right"><div class="contact-stat custom-tooltip" data-toggle="tooltip" data-placement="top" title="This is how many positions this user has filled."><span>TOTAL VOLUNTEER POSITIONS</span> <strong><asp:Literal ID="litVolunteerPositions" runat="server"></asp:Literal></strong></div></div>
								<div class="col-xs-3 border-right"><div class="contact-stat custom-tooltip" data-toggle="tooltip" data-placement="top" title="Total times user deployed."><span>DEPLOYMENT PARTICIPATION</span> <strong><asp:Literal ID="litDeploymentCount" runat="server"></asp:Literal></strong></div></div>
								<div class="col-xs-3 border-right"><div class="contact-stat custom-tooltip" data-toggle="tooltip" data-placement="top" title="Total value of hours."><span>VALUE OF HOURS CONTRIBUTED</span> <strong><asp:Literal ID="litTotalValueOfHours" runat="server"></asp:Literal></strong></div></div>
								<div class="col-xs-3 border-right"><div class="contact-stat custom-tooltip" data-toggle="tooltip" data-placement="top" title="Total hours volunteered."><span>TOTAL HOURS</span> <strong><asp:Literal ID="litTotalHours" runat="server"></asp:Literal></strong></div></div>
								<div class="col-xs-3 border-right"><div class="contact-stat custom-tooltip" data-toggle="tooltip" data-placement="top" title="Members total connections."><span>STABILITY NETWORK CONNECTIONS</span> <strong><asp:HyperLink CssClass="nowrap" ID="hypConnections" Font-Bold="true" runat="server"></asp:HyperLink></strong></div></div>
								<div class="col-xs-3 border-right"><div class="contact-stat custom-tooltip" data-toggle="tooltip" data-placement="top" title="Shows impact of skill entries."><span>SKILL SCORE</span> <strong><asp:HyperLink NavigateUrl="/V1/Profile/EditSkills.aspx" ID="litSkillScore" runat="server"></asp:HyperLink></strong> <br /><small> <asp:Literal ID="litSkillCount" runat="server"></asp:Literal> </small> </div></div>
								<div class="col-xs-3 border-right"><div class="contact-stat custom-tooltip" data-toggle="tooltip" data-placement="top" title="Shows impact of equipment entries."><span>EQUIPMENT SCORE</span> <strong><asp:HyperLink NavigateUrl="/V1/Profile/EditResources.aspx" ID="litEquipmentScore" Font-Bold="true" runat="server"></asp:HyperLink></strong><br /><small> <asp:Literal ID="litEquipmentCount" runat="server"></asp:Literal> </small> </div></div>
							</div>
							<div class="row">
								<div class="col-xs-12 text-center nowrap">
									<h4>
										<asp:Literal ID="litUserRank" runat="server"></asp:Literal>
										<br />
										<small>Rank is recalculated every 5 minutes.</small>
									</h4>
								</div>
							</div>
							<div class="row">
								<div class="col-xs-6 text-center font-bold nowrap" style="display: flex; flex-direction: column;">
									<asp:HyperLink ID="hypPreviousRank" runat="server" CssClass="nowrap"></asp:HyperLink>
								</div>
								<div class="col-xs-6 text-center font-bold nowrap" style="display: flex; flex-direction: column;">
									<asp:HyperLink ID="hypNextRank" runat="server" CssClass="nowrap"></asp:HyperLink>
								</div>
							</div>
						</div>
					</div>
                </div>
            </div>
		</div>
	</div>
<script>
	function toggleTooltip() {
		var tip = document.getElementById("customTooltip");
		tip.style.display = (tip.style.display === "block") ? "none" : "block";
	}

	// Optional: click outside to close
	document.addEventListener("click", function (event) {
		var tip = document.getElementById("customTooltip");
		var trigger = document.querySelector(".tooltip-trigger");

		if (!tip.contains(event.target) && !trigger.contains(event.target)) {
			tip.style.display = "none";
		}
	});
</script>