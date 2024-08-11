<%@ page title="" language="C#" masterpagefile="~/S1/MasterPages/Homer.master" autoeventwireup="true" inherits="S1_Default, App_Web_avkw21jy" %>
<%@ MasterType VirtualPath="~/S1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
    <div class="hpanel">
        <div class="panel-body">

            <h2 class="font-light m-b-xs">
               <asp:Literal id="litName" runat="server"></asp:Literal>
            </h2>
            <small>
				<asp:Literal id="litFulleName" runat="server"></asp:Literal>
				<asp:HyperLink ID="hypAddStory" runat="server" Text="Add A Story" Visible="false" NavigateUrl="/S1/Profile/NewBlogPost.aspx"></asp:HyperLink>
            </small>
			
        </div>
    </div>
	
	<div class="hpanel collapsed">
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
	
	<script type="text/javascript">
		$(function () {			$('.deleteButton').on('click', function (e) {
				e.preventDefault();
				swal({
					title: "Are you sure?",
					text: "Your will not be able to recover this article!",
					type: "warning",
					showCancelButton: true,
					confirmButtonColor: "#DD6B55",
					confirmButtonText: "Yes, delete it!"
				},
					function () {
						location.href = "/S1/Default.aspx?d=t&userId=" + userId + "&articleId=" + story.a.ArticleId;				});
			});
		});
	</script>
	<script src="/Homer/vendor/sweetalert/lib/sweet-alert.min.js"></script>
</asp:Content>