<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DeploymentListCard.ascx.cs" Inherits="V1_UserControls_DeploymentListCard" %>

	<style type="text/css">
			.grid-item								{width:250px; margin-top: 20px;}
			.panel-body.deploymentPanel				{background-color:#f8f8f8; color:#808080; height:180px; border-top-right-radius:10px !important; border-top-left-radius:10px !important;}
			.panel-body.deploymentPanel:hover		{cursor:pointer; background-color:#5E2E91; color:white;}
			.panel-footer							{ background-color:#f8f8f8; border-bottom-right-radius:10px; border-bottom-left-radius:10px;}
			.panel-footer:hover						{cursor:pointer; background-color:#5E2E91; color:white;}
	</style>

	<script src="/v1/Scripts/masonry.pkgd.min.js"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.grid').each(function () {
				// Initialize Masonry for each grid individually
				$(this).masonry({
					itemSelector: '.grid-item',
					gutter: 20,
					columnWidth: '.grid-item',
					percentPosition: true
				});
			});
		});
	</script>
	<div class="grid">
		<asp:Literal ID="litDeployments" runat="server"></asp:Literal>
	</div>
	<div class="row">
		<div class="col-lg-8">
            <div class="content">
                <asp:HyperLink ID="hypAddNewCampaign" CssClass="btn btn-info pull-left m-r-lg" runat="server" Text="<i class='fa fa-map'></i> Add A Deployment" Visible="false"></asp:HyperLink>
                <asp:Literal ID="litAddTeamMessage" runat="server"></asp:Literal> 
                <asp:HyperLink ID="hypAddTeam" Visible="false" Text="Create Your Team, Organization or Business" runat="server" NavigateUrl="/V1/Administration/NonProfitNew.aspx"></asp:HyperLink>
            </div>
        </div>
    </div>