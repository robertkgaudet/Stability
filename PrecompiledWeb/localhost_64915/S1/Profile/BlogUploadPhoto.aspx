<%@ page title="" language="C#" masterpagefile="~/S1/MasterPages/Homer.master" autoeventwireup="true" inherits="S1_Profile_BlogUploadPhoto, App_Web_22v3mdrb" %>
<%@ MasterType VirtualPath="~/S1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<script src="/V1/Scripts/JCrop/jquery.Jcrop.min.js"></script>
	<link href="/V1/Styles/jquery.Jcrop.min.css" rel="stylesheet" />
	<style>
		#<%=imgStoryPhoto.ClientID%>
		{
			max-width:100%;
			max-height:100%;
		}
	</style>

	<script type="text/javascript">

		$(document).ready(function () {

			$("#<%=btnUpdate.ClientID%>").hide();

		});

		function previewFile()
		{
			$("#<%=btnUpdate.ClientID%>").show();
			//$("#<%=storyPhotoUpload.ClientID%>").hide();

            var preview = document.querySelector('#<%=imgStoryPhoto.ClientID %>');
            var file = document.querySelector('#<%=storyPhotoUpload.ClientID %>').files[0];
			var reader = new FileReader();

            reader.onloadend = function () {
				preview.src = reader.result;
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "";
			}

		}
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
					Upload a Covery Photo for Your Story
				</h2>				<div><small>(Minimum should be 1200 pixels wide.) A clear positive image with the smiling survivors produces MANY more views.</small></div>
			</div>
		</div>
		<div class="hpanel">
			<div class="panel-body">
				<div>
					Upload a Photo
					<div>
						<input ID="storyPhotoUpload" type="file" name="profilePhoto" onchange="previewFile()"  runat="server" />
						<div class="m-t-lg m-b-lg">
							<asp:Image ID="imgStoryPhoto" runat="server" ImageUrl="../Images/icons8-customer-64.png" />
						</div>
						<div class="m-b-md">
							<asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-success pull-left" Text="Upload Story Photo" CausesValidation="false" OnClick="btnUpdate_Click" />
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>

