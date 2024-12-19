<%@ Page Title="Edit or Add Nonprofit Program" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="EditNonProfitProgram.aspx.cs" Inherits="V1_NonProfitAdministration_EditNonProfitProgram" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>
	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>

    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
	<link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
	<link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
	<link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
	
    <script>

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode == 110 || charCode == 190 || charCode == 46)
                return true;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

		$(function () {

			$("#form1").validate({
				rules: {
					<%=txtProgramName.UniqueID%>: {
					required: true,
					maxlength: 100
					},
					<%=txtProgramDescription.UniqueID%>: {
						required: true
					},
				},
				submitHandler: function (form) {
					form.submit();
				}
			});
		});

		$(document).ready(function () {
			$('input[type="checkbox"]').each(function () {
				$(this).addClass("i-checks");
			});

			const programId = '<%=GetProgramId()%>'; // Pass the program ID to the front-end.

			// Load video links
			function loadVideoLinks() {
				$.ajax({
					url: '/V1/Handlers/VideoLinkHandler.ashx',
					type: 'GET',
					data: { programId: programId },
					success: function (data) {
						const links = data;
						const list = $('#videoLinksList');
						list.empty(); // Clear existing links
						links.forEach(function (link) {
							list.append(
								'<li class="list-group-item" data-id="' + link.VideoLinkId + '">' +
								'<a href="' + link.URL + '" target="_blank">' + link.Title + '</a>' +
								'<button class="btn btn-danger btn-sm float-end delete-link m-l-lg" type="button">Delete</button>' +
								'</li>'
							);
						});
					},
					error: function () {
						alert("Failed to load video links.");
					}
				});
			}

			// Add new video link
			$('#addVideoLink').click(function () {
				const url = $('#txtVideoLink').val();
				const title = $('#txtTitle').val();
				if (!url) {
					alert('Please enter a valid URL.');
					return;
				}

				$.ajax({
					url: '/V1/Handlers/VideoLinkHandler.ashx',
					type: 'POST',
					data: { programId: programId, url: url, title: title },
					success: function (response) {
						const link = response; // Parse the response for the new link
						$('#videoLinksList').prepend(
							'<li class="list-group-item" data-id="' + link.VideoLinkId + '">' +
							'<a href="' + link.URL + '" target="_blank">' + link.Title + '</a>' +
							'<button class="btn btn-danger btn-sm float-end delete-link m-l-lg" type="button">Delete</button>' +
							'</li>'
						);
						$('#newVideoTitle').val(''); // Clear the title field
						$('#newVideoLink').val(''); // Clear the URL field
					},
					error: function () {
						alert('Failed to add the video link.');
					}
				});
			});

			// Delete video link
			$(document).on('click', '.delete-link', function (event) {
				//event.preventDefault(); // Prevent default form submission or navigation
				const linkId = $(this).closest('li').data('id');
				console.log("Attempting to delete VideoLinkId:", linkId);
				$.ajax({
					url: '/V1/Handlers/VideoLinkHandler.ashx',
					type: 'POST',
					data: { videoLinkId: linkId, programId: programId, method: 'DELETE' },
					success: function () {
						console.log("Deleted successfully.");
						loadVideoLinks();
					},
					error: function () {
						console.error("Error deleting video link:", xhr.responseText, status, error);
						alert('Failed to delete the video link.');
					}
				});
			});

			// Initial load
			loadVideoLinks();
		});
	</script>
	<style>
		.i-checks
		{
			margin-right:10px !important;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeIn">
					<div class="hpanel">
						<div class="panel-body">
						<h1>	
							<asp:Literal id="litEventName" runat="server"></asp:Literal>
						</h1>
							<h2 class="font-light m-b-xs">
								Add a Non-profit Program
							</h2>
						    <div class="form-group">
							    <div class="pull-right">
								    <asp:LinkButton id="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default" Text="Cancel" />
								    <asp:Button id="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Save Changes" />
							    </div>
						    </div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
		
			<div class="row" id="divForm" runat="server">
				<div class="col-lg-12 container">
					<div class="hpanel form-horizontal">

						<div class="panel-heading hbuilt">
							Non-Profit Program
						</div>
						<div class="panel-body">
								
							<div class="form-group">
								<label class="col-sm-2 control-label"> <i class="fa fa-paper-plane"></i> Share Role With Other Teams</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkSharedPosition" class="i-checks">
									<span class="text-muted"> <small>(Note, once a role has been shared it can not be unshared.)</small></span>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label"> <i class="fa fa-paper-plane"></i> Share Program With Other Teams</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkSharedProgram" class="i-checks">
									<span class="text-muted"> <small>Note, once a program has been shared and deployed it can not be unshared.</small></span>
								</div>
							</div>
						
							<div class="form-group">
								<label class="col-sm-2 control-label">Program Name</label>
								<div class="col-sm-5"><input type="text" runat="server" required id="txtProgramName" class="form-control" placeholder="Program Name"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Program Description</label>
								<div class="col-sm-5">
									<textarea id="txtProgramDescription" runat="server" rows="7" class="form-control" placeholder="Describe this program."></textarea>
								</div>
							</div>

							

							<div class="form-group">
								<label class="col-sm-2 control-label">Program Video Links</label>
								<div class="col-sm-5">

									<div id="videoLinksContainer" class="video-links-container">
										<ul id="videoLinksList" class="list-group">
											<!-- Video links will be dynamically loaded here -->
										</ul>
										<div class="input-group mb-3">
											<input type="text" id="txtTitle" class="form-control" placeholder="Enter Video Title">
											<input type="text" id="txtVideoLink" class="form-control" placeholder="Enter Video URL">
											<button id="addVideoLink" class="btn btn-primary" type="button">Add Video Link</button>
										</div>
									</div>

								</div>
							</div>



							
							<div class="form-group">
								<label class="col-sm-2 control-label">Remote Work</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkRemoteWork" class="i-checks">
								</div>
							</div>
							
							<div class="form-group">
								<label class="col-sm-2 control-label">Requires Deployment</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkRequiresDeployment" class="i-checks">
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Requires Specialized Training</label>
								<div class="col-sm-5">
									<input type="checkbox" runat="server" id="chkRequiresTraining" class="i-checks">
								</div>
							</div>
						
							<div class="form-group">
								<label class="col-sm-2 control-label">Program Order</label>
								<div class="col-sm-1"><input type="text" runat="server" required maxlength="2" id="txtProgramOrder" onkeypress="return isNumberKey(event)" class="form-control" placeholder="0-20"></div>
							</div>
						</div>
					</div>
				</div>

			</div>
		</div>
	<script src="https://code.jquery.com/ui/1.14.0/jquery-ui.js"></script>
	<script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
</asp:Content>