<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MemberHeader.ascx.cs" Inherits="V1_UserControls_MemberHeader" %>

<%@ Register Src="~/V1/UserControls/TeamLogo.ascx" TagPrefix="uc1" TagName="LoadNameWithBadges" %>

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
        margin-right: 0px !important;
    margin-top: 5px !important;
}

	.additional-name {
        color: darkslategrey !important;
    font-size: 20px !important;
    font-weight: 800 !important;
    margin-right: 6px !important;

}
	.StreamLink {
    text-decoration:none !important;
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
				<a href="/V1/Profile/ProfilePhotoUpload.aspx?userId=<%=_userId%>"
					class="camera-icon" title="Edit" runat="server" id="linkCamera" visible="false">
					<i class="fa fa-camera"></i></a>
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
                <div class="col-xs-12 col-lg-8">
					<div style="margin-top:10px; width:100%;">
					<span style="color:darkslategrey; font-size:20px; font-weight:800; margin-right:10px;">
				<%--	<asp:Literal ID="litMemberName" runat="server"></asp:Literal>--%>
							
                  <uc1:LoadNameWithBadges  runat="server" ID="ucTeamLogo" />

					</span>

				
						
				<i class="fa fa-id-badge pe-2x <%=_badgeVettingStatus%> float-right <%=faIdBadgeClick%>" data-toggle="tooltip" data-placement="top" title="Activate Vetted ID Badge"></i>
					<i class="fa fa-shield pe-2x <%=_badgeCertificationStatus%> float-right" data-toggle="tooltip" data-placement="top" title="Earn Training Certifications To Activate"></i>
					<i class="fa fa-certificate pe-2x <%=_badgeDeployedStatus%> float-right" data-toggle="tooltip" data-placement="top" title="Deploy Once To Activate"></i>
					<i class="fa fa-check-circle pe-2x <%=_badgeHoursRecordedStatus%> float-right" data-toggle="tooltip" data-placement="top" title="Record 8 Hours of Volunteer Time To Activate"></i>
					<i class="fa fa-star pe-2x <%=_badgeTOPStatus%> float-right" data-toggle="tooltip" data-placement="top" title="Record 100 Hours of Volunteer Time or 5 Or More Deployments to Activate"></i>
					<i class="fa fa-ban text-danger pe-2x float-right" runat="server" id="idKwest" visible="false" data-toggle="tooltip" data-placement="top" title="Work in Progress"></i>
					</div>
					<div style="clear: both;"></div>
					<asp:Literal ID="litTitle" runat="server"></asp:Literal>
					<p style="font-size:16px;">
						<asp:Literal ID="litMemberDescription" runat="server"></asp:Literal>
					</p>
					<div class="m-t-sm">
						<p style="color:#C0C0C0;"><asp:Literal ID="litLocation" runat="server"></asp:Literal></p>
					</div>

                    <div class="row">
                        <div class="col-xs-4">
                            <div class="project-label nowrap">Team Rank</div>
                            <small>4</small>
                        </div>
                        <div class="col-xs-4">
                            <div class="project-label nowrap">Deployed</div>
                            <small><asp:Literal ID="litDeploymentCount" runat="server"></asp:Literal> Times</small>
                        </div>
                        <div class="col-xs-4">
                            <div class="project-label nowrap">Network</div>
                            <small>
								<asp:HyperLink CssClass="nowrap" ID="hypConnections" Font-Bold="true" runat="server" Text="325 Connections"></asp:HyperLink>
                            </small>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-lg-4 project-info">
                    <div class="project-value" style="margin-top:20px;">
						<asp:HyperLink ID="hypTeam" Font-Bold="true" runat="server"></asp:HyperLink>
						<asp:Literal ID="litTeamBreak" runat="server"></asp:Literal>
						<small>VALUE OF HOURS CONTRIBUTED</small>
                        <h2 style="margin-top:0px;"><span class="text-success">$0.00</span></h2>
                    </div>
					<asp:LinkButton runat="server" ID="btnFriend" ClientIDMode="Static">Request Connection</asp:LinkButton>
                </div>
            </div>
		</div>
	</div>