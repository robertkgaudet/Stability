<%@ control language="C#" autoeventwireup="true" inherits="S1_UserControls_DisasterSurvivorStoriesByDisaster, App_Web_ykqui43n" %>

	<script src="/V1/Scripts/masonry.pkgd.min.js"></script>

	<script>
		$(document).ready(function () {
			$('.grid').masonry({
				// options
				itemSelector: '.grid-item',
				gutter: 10
			});
		});
	</script>
	<style type="text/css">

		.grid-item {width:320px;}
		.tinted img {
				  filter: brightness(50%);
				}

	</style>

	
    <div class="hpanel">
        <div class="panel-body">

            <h2 class="font-light m-b-xs">
               <asp:Literal id="litName" runat="server"></asp:Literal>
            </h2>
            <small>
				<asp:Literal id="litFulleName" runat="server"></asp:Literal>
				<asp:HyperLink ID="hypAddStory" Font-Underline="true" CssClass="btn btn-sm btn-info" runat="server" Text="Add A Story" Visible="false" NavigateUrl="/S1/Profile/NewBlogPost.aspx"></asp:HyperLink>
            </small>
			
        </div>
    </div>
	
	<div class="hpanel">
		<div class="panel-heading">
			<div class="panel-tools pull-left m-r-sm">
				<a class="showhide"><i class="fa fa-chevron-up"></i></a>
			</div>
			Choose a Category
		</div>
		<div class="panel-body">
			<p>
				<asp:Literal ID="litCategories" runat="server"></asp:Literal>
			</p>
		</div>
	</div>

	<!--BEGIN GRID-->
	<div class="grid">		<%=gridItem%>	</div>
	<!--END GRID-->