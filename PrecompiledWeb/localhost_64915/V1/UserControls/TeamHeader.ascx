<%@ control language="C#" autoeventwireup="true" inherits="V1_UserControls_TeamHeader, App_Web_qaqxyxhv" %>	<script type="text/javascript">

		$(document).ready(function () {
			$("#nonProfit.dropdown-menu li").click(function () {
				window.location.href = "/V1/NonProfit/Default.aspx?organizationId=" + $(this).attr('id');
			});
		});
</script>
<style>
	.divCover {
		width: 100%;
		height: 100vh; /* Full viewport height */
		background-image: url('<%=_coverImage%>'); /* Path to your image */
		background-size: cover; /* Scale the image to cover the entire div */
		background-position: center; /* Center the image */
		background-repeat: no-repeat; /* Prevent the image from repeating */
		height:300px;
		margin-left:-1px;
		margin-top:0px;
		margin-bottom:-100px;
		background-color:#ccc;
	}
	.divTeamStabilityContainer
	{		position: relative;		width: 100%;
        height: auto;
        max-width: 300px; /* Set a max width for the image */
        overflow: hidden;		display: flex;
        justify-content: center; /* Center the first image */	}
	.overlay
	{
		position: absolute;
        bottom: 0;
        left: 0;
        width: 100%;
        height: 30%; /* Cover bottom quarter */
        background: linear-gradient(to top, white, rgba(255, 255, 255, 0) 20%);
     }
	.custom-margin
	{
		margin-top: -100px; /* Default margin */
		width:100%;
    }
    @media (max-width: 767px) 
	{
        .custom-margin
		{
            margin-top: -70px; /* Margin for xs screens */
			width:50%;
        }
    }
	.TeamLogo
	{		max-width: 36%;		position: absolute;
        height: auto;		z-index: 2;		text-align: center;	}
	.StabilityLogo
	{
        display: block;
        width: 100%;
        height: auto;
    }
</style>
	<div id="divCover" class="divCover"></div>
	<div class="normalheader">
        <div class="hpanel">
            <div class="panel-body">
				<div class="divTeamStabilityContainer col-xs-3 col-md-4 custom-margin">
					<asp:Image ID="imgTeamLogo" CssClass="TeamLogo" runat="server" />
					<asp:Image ID="imgPartner1Logo" CssClass="Partner1Logo" runat="server" />
					<asp:Image ID="imgPartner2Logo" CssClass="Partner2Logo" runat="server" />
					<img src="/V1/Images/logo-team-stability.png" class="StabilityLogo" style="margin-bottom:-50px;" />
					<div class="overlay"></div>
				</div>
                <h2 class="font-light m-b-xs">
                    <asp:Literal ID="litTeamName" runat="server"></asp:Literal>
                </h2>
                <small><asp:Literal ID="litTeamDescription" runat="server"></asp:Literal></small>
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
					<button id="btn-NonProfitDropdown" class="btn btn-outline btn-default nonProfit dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">View Another Team <i class="fa fa-sort-down"></i></button>
					<ul id="nonProfit" class="dropdown-menu text-center required">
						<%=_nonProfitDropDown%>
					</ul>
                </div>
            </div>
        </div>
    </div>