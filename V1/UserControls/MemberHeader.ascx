<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MemberHeader.ascx.cs" Inherits="V1_UserControls_MemberHeader" %>

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
</style>
	<div id="divCover" class="divCover"></div>
	<div class="hpanel">
		<div class="panel-body member-panel-body">
			<div class="memberImageContainer col-xs-3 col-md-4">
				<asp:Image runat="server" id="imgMemberProfilePhoto" class="memberPhoto" />
				<a href="/V1/Profile/ProfilePhotoUpload.aspx?userId=<%=_userId%>" class="camera-icon" title="Edit"><i class="fa fa-camera"></i></a>
			</div>	
            <div class="pull-right" id="divMemberEditControl" runat="server">
                <div class="btn-group">
					<div class="btn-group" runat="server" id="divUserSettings" visible="false">
						<button data-toggle="dropdown" class="btn btn-xs btn-default dropdown-toggle"><i class="fa fa-gear"></i> Settings <span class="caret"></span></button>
						<ul class="dropdown-menu">
							<li><a href="/V1/NonProfit/Stream.aspx?organizationId=79305f85-3816-46a8-911f-0d7e3e227c32">My Team</a></li>
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
                    <h4 style="color:darkslategrey;"><asp:Literal ID="litMemberName" runat="server"></asp:Literal></h4>
                    <p style="font-size:16px;">
						<asp:Literal ID="litMemberDescription" runat="server"></asp:Literal>
					</p>
					<div class="m-t-sm">
						<p style="color:#C0C0C0;"><asp:Literal ID="litLocation" runat="server"></asp:Literal></p>
					</div>

                    <div class="row">
                        <div class="col-xs-4">
                            <div class="project-label">TEAM RANK</div>
                            <small>4</small>
                        </div>
                        <div class="col-xs-4">
                            <div class="project-label">DEPLOYED</div>
                            <small>14 Times</small>
                        </div>
                        <div class="col-xs-4">
                            <div class="project-label">NETWORK</div>
                            <small>325 Connections</small>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-4 project-info">
                    <div class="project-value" style="margin-top:20px;">
						<asp:Literal ID="litTitle" runat="server"></asp:Literal>
						<small>VALUE OF HOURS CONTRIBUTED</small>
                        <h2 style="margin-top:0px;"><span class="text-success">$680,000</span></h2>
                            <div class="project-label">HOURS</div>
                            <small>12.5</small>
                    </div>
                </div>
            </div>
		</div>
	</div>