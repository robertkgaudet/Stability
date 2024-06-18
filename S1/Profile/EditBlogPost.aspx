<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/S1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="EditBlogPost.aspx.cs" Inherits="S1_Profile_EditBlogPost" %>
<%@ MasterType VirtualPath="~/S1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
	<script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
	<script>

		$(function () {

			$('.summernote').summernote({
				height: 300,
				toolbar: [
					
					['style', ['bold', 'italic', 'underline']],
					['alignment', ['ul', 'ol', 'paragraph']],
					['insert', ['link']],
					['misc', ['codeview']],
				]
			});
		});

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="hpanel">
        <div class="panel-body">
            <a class="small-header-action" href="">
                <div class="clip-header">
                    <i class="fa fa-arrow-up"></i>
                </div>
            </a>
            <h2 class="font-light m-b-xs">
                Edit Your Story
            </h2>
            <small>Describe your current situation in a public story.</small>
        </div>
    </div>
	<div>
		<div class="alert alert-success m-b" runat="server" id="divAlertArticleMessage" visible="false">
			Your story has been added.
		</div>
		<div class="form-group ">
			<div class="input-group">
				<div class="input-group-addon">
					Story Title
				</div>
				<input class="form-control" id="txtTitle" runat="server" name="txtTitle" type="text" placeholder="Enter a Story Title" required/>
			</div>
		</div>
		<asp:TextBox TextMode="MultiLine" runat="server" ID="txtArticle" CssClass="summernote" required></asp:TextBox>
	</div>
	<div class="form-group">
		<div class="input-group pull-right">
			<asp:Button ID="btnSubmitArticle" runat="server" Text="Update Article Contents" CssClass="btn btn-primary" OnClick="btnSubmitArticle_Click" />
		</div>
	</div>
</asp:Content>

