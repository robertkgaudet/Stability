<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/S1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="NewBlogPost.aspx.cs" Inherits="S1_Profile_NewBlogPost" %>
<%@ MasterType VirtualPath="~/S1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
	<script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>

	<link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
	<script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>

	<script>

		$(document).ready(function () {

			<%=preselectedSurvivorJQuery%>
			<%=preselectedHelperJQuery%>
			
			$("#survivorEvent.dropdown-menu li").click(function () {
				$("#btn-dropdown.survivorEvent").html($(this).text());
				$("#<%=hidSurvivorId.ClientID%>").val($(this).attr('id'));
			});

			$("#helperEvent.dropdown-menu li").click(function () {
				$("#btn-dropdown.helperEvent").html($(this).text());
				$("#<%=hidHelperId.ClientID%>").val($(this).attr('id'));
			});

			$(".js-source-states-1").select2();
		});

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

			//$('.summernote').summernote({
			//	callbacks: {
			//		onPaste: function (e) {
			//			var bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData).getData('Text');

			//			e.preventDefault();

			//			// Firefox fix
			//			setTimeout(function () {
			//				document.execCommand('insertText', false, bufferText);
			//			}, 10);
			//		}
			//	}
			//});

			$('.btn-primary').hover(function () {

				//alert("test");

				var contents = $('#summernote').summernote('code');
				var plainText = $("<p>" + contents + "</p>").text();
				
				alert(plainText.text());

				$("#<%=hidArticle.ClientID%>").val(plainText);
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
                Write a New Story
            </h2>
            <small>Describe the survivors current situation in a new public story.</small>
        </div>
    </div>
	<div class="panel-heading">
		<h4>Some Story Suggestions</h4>
		<ul>
			<li>Describe the moment the disaster affected you.</li>
			<li>Were you rescued? Describe who .</li>
			<li>Tell us how this disaster has changed your everyday life?</li>
			<li>Tell us about your current living situation?</li>
			<li>How many immediate family members, pets, friends were affected and how so?</li>
			<li>What is your current employment situation?</li>
			<li>What kinds of personal possesssions were lost in the disaster?</li>
			<li>Describe the household things you need to recover?</li>
			<li>What is your emotional state?</li>
		</ul>
	</div>
	<div>
		<div class="alert alert-success m-b" runat="server" id="divAlertArticleMessage" visible="false">
			Your story has been added.
		</div>
		<div class="form-group">
			<div class="input-group m-b-lg">
				<div class="input-group-addon">
					Who is This Story About
				</div>
				<div id="div1" class="dropdown	" runat="server">
					<button id="btn-dropdown" class="btn btn-outline btn-default survivorEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Select a <%=storySubject%><i class="fa fa-sort-down"></i></button>
					<ul id="survivorEvent" class="dropdown-menu text-center dropdown-volunteer required">
						<%=userDropDown%>
					</ul>
					<input type="hidden" id="hidSurvivorId" runat="server" />
					<input type="hidden" id="hidHelperId" runat="server" />
				</div>
			</div>
			<div class="input-group m-b-lg">
				<div><label> <input type="checkbox" runat="server" id="chkUploadPhoto" class="i-checks"> Upload a Cover Photo (On Next Page) </label></div>				<div><small>(Minimum should be 1200 pixels wide.) A large positive image with smiling survivors produces MANY more views.</small></div>
			</div>
			<div class="input-group m-b-lg">
				<div class="input-group-addon">
					Choose Story Categories
				</div>
				<div class="dropdown">
					<asp:ListBox ID="ddlQualifiers" SelectionMode="Multiple" DataValueField="CategoryId" DataTextField="Category" runat="server" CssClass="js-source-states-1"></asp:ListBox>
				</div>
			</div>
			<div class="input-group m-b-lg">
				<div class="input-group-addon">
					Story Title
				</div>
				<input class="form-control" id="txtTitle" runat="server" name="txtTitle" type="text" placeholder="Enter a Story Title" required/>
			</div>
		</div>
		<asp:TextBox TextMode="MultiLine" runat="server" ID="txtArticle" CssClass="summernote" required></asp:TextBox>
		<asp:HiddenField ID="hidArticle" runat="server" />
	</div>
	<div class="form-group">
		<div class="input-group pull-right">
			<asp:Button ID="btnSubmitArticle" runat="server" Text="Add New Story" CssClass="btn btn-primary" OnClick="btnSubmitArticle_Click" />
		</div>
	</div>
	<div style="height:500px;"></div>
</asp:Content>

