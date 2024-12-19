<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Program.aspx.cs" Inherits="V1_NonProfit_Program" %>
<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			<%--function loadVideoLinks() {
				$.ajax({
					url: '/V1/Handlers/VideoLinkHandler.ashx',
					type: 'GET',
					data: { programId: '<%=programId%>' },
					success: function (data) {
						const container = $('#videoLinksContainer');
						container.empty(); // Clear existing videos
						data.forEach(function (link) {
							const videoId = extractYouTubeVideoId(link.URL); // Extract the video ID
							if (videoId) {
								container.append(
									'<div class="col-xs-12 col-lg-6 m-4 text-center">' +
									'<div class="text-center mt-2">' +
									'<strong>' + link.Title + '</strong>' +
									'</div>' +
									'<div class="ratio ratio-16x9">' +
									'<iframe src="https://www.youtube.com/embed/' + videoId + '" title="' + link.Title + '" allowfullscreen></iframe>' +
									'</div>' +
									'</div>'
								);
							}
						});
					},
					error: function (xhr, status, error) {
						console.error("Failed to load video links:", xhr.responseText, status, error);
						alert("Failed to load video links. Check the console for details.");
					}
				});
			}--%>

			function loadVideoLinks() {
				$.ajax({
					url: '/V1/Handlers/VideoLinkHandler.ashx',
					type: 'GET',
					data: { programId: '<%=programId%>' },
					success: function (data) {
						const container = $('#videoLinksContainer');
						container.empty(); // Clear existing content
						data.forEach(function (link) {
							const videoId = extractYouTubeVideoId(link.URL); // Extract the video ID
							if (videoId) {
								const thumbnailUrl = `https://img.youtube.com/vi/${videoId}/hqdefault.jpg`; // Thumbnail URL
								container.append(
									'<div class="col-sm-4 text-center">' +
									'<div class="video-title mb-2">' +
									'<strong>' + link.Title + '</strong>' +
									'</div>' +
									'<img src="' + thumbnailUrl + '" class="img-responsive video-thumbnail" alt="' + link.Title + '" data-video-id="' + videoId + '" data-title="' + link.Title + '">' +
									'</div>'
								);
							}
						});

						// Attach click event to thumbnails to open modal
						$('.video-thumbnail').click(function () {
							const videoId = $(this).data('video-id');
							const videoTitle = $(this).data('title');
							const iframeSrc = `https://www.youtube.com/embed/${videoId}`;

							$('#videoIframe').attr('src', iframeSrc);
							$('#videoModalLabel').text(videoTitle); // Set the modal title dynamically
							$('#videoModal').modal('show');
						});

						// Clear iframe when modal is closed
						$('#videoModal').on('hidden.bs.modal', function () {
							$('#videoIframe').attr('src', '');
						});
					},
					error: function (xhr, status, error) {
						console.error("Failed to load video links:", xhr.responseText, status, error);
						alert("Failed to load video links. Check the console for details.");
					}
				});
			}


			loadVideoLinks();
		});

		function extractYouTubeVideoId(url) {
			const match = url.match(/(?:https?:\/\/)?(?:www\.)?youtube\.com\/(?:watch\?v=|embed\/|v\/|.+\?v=)([\w-]{11})|youtu\.be\/([\w-]{11})/);
			return match ? (match[1] || match[2]) : null;
		}



	</script>
	<style>
		.video-title {
			font-size: 16px;
			text-align: center;
			margin-bottom: 8px;
			color: #333;
		}

		.video-thumbnail {
			cursor: pointer;
			border-radius: 5px;
			transition: transform 0.3s ease;
		}

		.video-thumbnail:hover {
			transform: scale(1.05);
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
				<uc1:TeamHeader runat="server" ID="ucTeamHeader" />
				<asp:HyperLink ID="hypAllPrograms" runat="server" Text="All Programs"></asp:HyperLink>

	
				<div class="m-t-md">
					<div class="row projects">
						<div id="divPrograms" runat="server" visible="true">
							<div class="col-lg-12">
								<div class="hpanel hbuilt">
									<div class="panel-body">
										<asp:Literal ID="litSharedPrivate" runat="server"></asp:Literal>
										<span class="label label-info pull-right m-r-xs">NEW</span>
										<div class="row">
											<div class="col-sm-8">
												<h4><asp:HyperLink ID="hypProgramName" runat="server"></asp:HyperLink></h4>
												<p><asp:Literal ID="litProgramDescription" runat="server"></asp:Literal></p>
											</div>
														
											<div class="col-sm-4 project-info pull-left">
												<small class="pull-left">Created By</small>
												<div id="logoDiv" class="m-t-md logoDiv">
															
													<asp:ImageButton OnClick="imgLogo_Click" ID="imgLogo" runat="server" Width="100px" />
												</div>
											</div>
										</div>
										<div id="videoLinksContainer" class="row">
											<!-- Videos will be dynamically added here -->
										</div>
										<div class="row">
											<div class="col-sm-12 m-b-lg m-t-lg">
												<div class="project-label">Recommended Program Roles</div>
												Select a role below for detailed description and training.
												<br />
												<asp:Repeater ID="dlPositions" runat="server" OnItemDataBound="dlPositions_ItemDataBound">
													<ItemTemplate>
														<i class="fa fa-user-circle text-muted"></i> <asp:HyperLink ID="hypPosition" Target="_blank" runat="server"></asp:HyperLink>
													</ItemTemplate>
													<SeparatorTemplate><br /></SeparatorTemplate>
																
												</asp:Repeater>
											</div>
										</div>
									<div class="panel-footer">
										<div class="project-action">
											<div class="btn-group">
												<button class="btn btn-xs btn-default"> Create Deployment</button>
												<asp:HyperLink CssClass="btn btn-xs btn-default" id="hypEditPrograms" runat="server" Text="Edit" Visible="false"></asp:HyperLink>
											</div>
										</div>
										<div class="row">
											<div class="col-sm-3">
												<div class="project-label">DEPLOY</div>
												<asp:Literal ID="litRequiresDeployment" runat="server"></asp:Literal>
											</div>
											<div class="col-sm-3">
												<div class="project-label">REMOTE</div>
												<asp:Literal ID="litRemoteWork" runat="server"></asp:Literal>
											</div>
											<div class="col-sm-3">
												<div class="project-label">TRAINING	</div>
												<asp:Literal ID="litRequiresTraining" runat="server"></asp:Literal>
											</div>
											<div class="col-sm-3">
												<div class="project-label">DEPLOYS	</div>
													0
											</div>
										</div>
									</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
                <div class="panel-heading">
					<div id="hbreadcrumb" class="pull-right">
						<ol class="hbreadcrumb breadcrumb">
							<li>
								<asp:HyperLink CssClass="font-normal btn btn-sm btn-info" id="hypAddPrograms" runat="server" Text="Add a Program" Visible="false"></asp:HyperLink>
							</li>
							<li class="active">
								<span>
									<asp:HyperLink ID="hypGlobalPrograms" runat="server" CssClass="font-normal btn btn-sm btn-info" NavigateUrl="~/V1/DisasterPrograms.aspx" Text="Global Programs"></asp:HyperLink>
								</span>
							</li>
						</ol>
					</div>
                </div>
				<uc1:TeamFooter runat="server" ID="ucTeamFooter" />


<!-- Modal Template -->
<div class="modal fade" id="videoModal" tabindex="-1" role="dialog" aria-labelledby="videoModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="videoModalLabel"></h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div style="position: relative; padding-bottom: 56.25%; height: 0; overflow: hidden;">
                    <iframe id="videoIframe" src="" frameborder="0" allowfullscreen
                            style="position: absolute; top: 0; left: 0; width: 100%; height: 100%;">
                    </iframe>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>