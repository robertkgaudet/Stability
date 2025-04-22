<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TeamHeader2.ascx.cs" Inherits="V1_UserControls_TeamHeader2" %>
<%@ Register Src="~/V1/UserControls/TeamNavigation.ascx" TagPrefix="uc1" TagName="TeamNavigation" %>

	<%--SECTION: Begin Header Styles--%>
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
        overflow: hidden;
		display: flex;
		margin-top: -125px; /* Default margin */
		z-index: 1;
		position: relative
	}
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
	{
		width: 150px;
		height: 150px;
		border-radius: 50%;
		object-fit: cover; /* Ensures the image covers the entire area */
		border: 2px solid white; /* Adds the white border */
		position: relative;
		background-color:white;
    }
	.memberDetail
	{
		z-index: 0;
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

		.hpanel
		{
			margin-bottom:5px !important;
		}
		.member-panel-body
		{
			border-radius: 10px !important;
			margin-bottom: 0px !important;
		}
		/* Custom gutter class for rows */
		.row.no-gutter {
			margin-left: 0;
			margin-right: 0;
		}

		.row.no-gutter [class*="col-"] {
			padding-left: 1px;  /* Reduced gutter padding */
			padding-right: 1px;
		}

		#desktopNavigation {
			display: block;
		}

		#mobileNavigation {
			display: none;
		}
		/* Media query for mobile devices */
		@media screen and (max-width: 768px) {
			#desktopNavigation {
				display: none;
			}

			#mobileNavigation {
				display: block;
			}
		}
		.stream{
			background-color:#F1F3F6 !important;
		}
</style>
	<%--SECTION: End Header Styles--%>

<script>
	$(document).ready(function () {

		$('.faIdBadgeClick').click(function () {
			window.location.href = "/IDCard";
			return false;
		});

		$("body").tooltip({ selector: '[data-toggle=tooltip]' });
	});
</script>
	<div class="container" style="padding-bottom:100px !important;">
		<div class="row justify-content-center" style="margin-top:40px;">
			<!--MAIN CONTENT CONTAINER-->
			<div class="col-sm-8 col-lg-9">
				<div class="content text-center">
					<div id="divCover" class="divCover"></div>
					<div class="hpanel">
						<div class="panel-body member-panel-body">
							<div class="memberImageContainer col-xs-3 col-md-4">
								<asp:Image runat="server" id="imgTeamLogo" class="memberPhoto" />
								<a href="/V1/Profile/ProfilePhotoUpload.aspx?userId=" class="camera-icon" title="Edit" runat="server" id="linkCamera" visible="false"><i class="fa fa-camera"></i></a>
							</div>
							<div class="row memberDetail">
								<div class="col-xs-12 col-lg-8">
									<div style="margin-top:10px; width:100%;">
										<span style="color:darkslategrey; font-size:20px; font-weight:800; margin-right:10px;">
											<asp:Literal ID="litTitle" runat="server"></asp:Literal>
										</span>
									</div>
									<p style="font-size:16px;">
										<asp:Literal ID="litMemberDescription" runat="server"></asp:Literal>
									</p>
								</div>
								<div class="col-xs-12 col-lg-4 project-info">
									<asp:Literal id="litChapterLabel" Text="Chapter of " runat="server" Visible="false"></asp:Literal>
									<asp:HyperLink ID="hypParentOrganization" runat="server" Visible="false"></asp:HyperLink>
								</div>
							</div>
						</div>
					</div>
				</div>

				<div id="mobileNavigation">
					<uc1:TeamNavigation runat="server" ID="ucTeamNavigation" />
				</div>
				<div class="hpanel">
					<div class="panel-body member-panel-body <%=_streamClass%>">
						<h4>
							<asp:Literal ID="litPageName" runat="server"></asp:Literal>
						</h4>