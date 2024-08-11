<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_Stream, App_Web_mjkl5wor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="Scripts/infinite-scroll.pkgd.min.js"></script>
	<script src="https://cdnjs.cloudflare.com/ajax/libs/jscroll/#.#.#/jquery.jscroll.min.js"></script>
	<script>

		$(document).ready(function () {
			//$('.container').infiniteScroll({
			//  // options
			//  path: '.pagination__next',
			//  append: '.post',
			//  history: false,
			//});
			var pageNumber = 1;

			$(window).scroll(function () {
				var windowHeight	= $(window).scrollTop() + $(window).height();
				var documentHeight = $(document).height();
				if (windowHeight == documentHeight)
				{
					pageNumber = pageNumber + 1;
					updateStreamPost(pageNumber);
				}
			});

			function AppendPostIntoStream(message) {
				$(".container").append($(message).fadeIn(1000));
			}

			function updateStreamPost(pageNumber) {
			//sending commentId will cause a delete.

			$.ajax(
				{
					type: "GET",
					url: "/V1/Handlers/GetStreamPost.ashx?eventId=<%=eventId%>",
					data: "pageNumber=" + pageNumber,
					contentType: "text/plain; charset=utf-8",
					dataType: "html",
					success: function (data)
					{
						if (data != "")
						{
							AppendPostIntoStream(data);
						}
					},
					error: function (request, status, error) {
						request.statusText + ' - ' + error + ' - ' + status;
					}
				});
			}
		});

    </script>
	<style>
		
		.StreamLink
		{
			color:#365899;
		}
		.StreamLink:hover
		{
			color:#365899;
			text-decoration:underline;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-6">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<a class="small-header-action">
								<div class="clip-header">
									<i class="fa fa-arrow-up"></i>
								</div>
							</a>
							<h2 class="font-light m-b-xs">Live Recovery Feed</h2>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-6 container">
					<asp:Repeater ID="rptPosts" runat="server" OnItemDataBound="rptPosts_ItemDataBound">
						<ItemTemplate>
							<div class="hpanel post">
								<div class="panel-body">
									<div class="message">
										<div class="blog-article-box">
										<h1 class="pull-left m-r-lg"><%# DataBinder.Eval(Container.DataItem, "disasterIcon") %></h1>
										<a href="/V1/Profile/Rebuild.aspx?rebuildId=<%# DataBinder.Eval(Container.DataItem, "rebuildId")%>" class="StreamLink"><%# DataBinder.Eval(Container.DataItem, "fullname") %></a> posted an update for
										<a href="/V1/Event.aspx?eventId=<%# DataBinder.Eval(Container.DataItem, "eventId") %>" class="StreamLink"><%# DataBinder.Eval(Container.DataItem, "disasterName") %></a>
										</div>
										<asp:Label ID="lblMessageDate" runat="server" CssClass="message-date"></asp:Label>
										<span class="message-content">
											<%# DataBinder.Eval(Container.DataItem, "Post") %>
										</span>
										<div class="m">
											<div class="row">
												<div class="col-sm-4">
													<h4><asp:Literal ID="litRebuildStage" runat="server"></asp:Literal></h4>
												</div>
												<div class="col-sm-4">
													<asp:Literal ID="litOverallProgress" runat="server"></asp:Literal>
												</div>
												<div class="col-sm-4">
													<asp:Literal ID="litRebuildProgress" runat="server"></asp:Literal>
												</div>
											</div>
										</div>
									</div>

									<div class="panel-footer">
										<asp:Label ID="lblVolunteersNeeded" runat="server"></asp:Label>
										<asp:HyperLink ID="hypVolunteer" CssClass="StreamLink" runat="server" Text="Volunteer"></asp:HyperLink>

										<div runat="server" id="divFooter" visible="false">
											<div class="social-talk">
												<div class="media social-profile clearfix">
													<a class="pull-left"></a>

													<div class="media-body">
														<span class="font-bold">Mark Smith</span>
														<small class="text-muted">14.04.2015</small>
														<div class="social-content">
															Many desktop publishing packages and web page editors.
														</div>
													</div>
												</div>
											</div>
											<div class="social-form">
												<input class="form-control" placeholder="Your comment">
											</div>
										</div>
									</div>
								</div>
							</div>
						</ItemTemplate>
					</asp:Repeater>
				</div>
				
				<div class="col-lg-6">
				</div>
			</div>
		</div>

	<script type="text/javascript">

    </script>

</asp:Content>