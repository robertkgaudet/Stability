<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="AddArticle.aspx.cs" Inherits="V1_Profile_AddArticle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
	<script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
	<script>

		$(function () {

			$('.summernote').summernote({
				toolbar: [
					['headline', ['style']],
					['style', ['bold', 'italic', 'underline']],
					['alignment', ['ul', 'ol', 'paragraph']],
					['insert', ['link']],
				]
			});
		});

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"><div class="hpanel panel-body">
	<div class="panel-heading">
		<h2>Enter Useful Information</h2>
		Short and sweet is best.
	</div>
	<div>
		<div class="alert alert-success m-b" runat="server" id="divAlertArticleMessage" visible="false"><asp:Literal ID="litArticleMessage" runat="server"></asp:Literal></div>
		<div class="form-group m-t-md">
			<div class="input-group">
				<div class="input-group-addon">
					Choose Information Category
				</div>
				<asp:DropDownList CssClass="form-control" ID="ddlInformationCategory" runat="server" DataTextField="category" DataValueField="CategoryId" ></asp:DropDownList>
			</div>
		</div>
		<div class="form-group ">
			<div class="input-group">
				<div class="input-group-addon">
					Link Title
				</div>
				<input class="form-control" id="txtTitle" runat="server" name="txtTitle" type="text" placeholder="Link Title" required/>
			</div>
		</div>
		<asp:TextBox TextMode="MultiLine" Rows="100" runat="server" ID="txtArticle" CssClass="summernote"></asp:TextBox>
	</div>
	<div class="form-group">
		<div class="input-group pull-right">
			<asp:Button ID="btnSubmitArticle" runat="server" Text="Add Information" CssClass="btn btn-primary" OnClick="btnSubmitArticle_Click" />
		</div>
	</div>
</asp:Content>