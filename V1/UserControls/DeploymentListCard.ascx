<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DeploymentListCard.ascx.cs" Inherits="V1_UserControls_DeploymentListCard" %>


	<script src="/v1/Scripts/masonry.pkgd.min.js"></script>

	<script type="text/javascript">
		$(document).ready(function () {
			$('.grid').masonry({
				// options
				itemSelector: '.grid-item',
				gutter: 10
			});
		});
	</script>
	<style type="text/css">
			.grid-item								{width:320px;}
			.panel-body.deploymentPanel				{background-color:#E8D3FE; color:#5E2E91; height:150px;}
			.panel-body.deploymentPanel:hover		{cursor:pointer; background-color:#5E2E91; color:white;}
	</style>
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