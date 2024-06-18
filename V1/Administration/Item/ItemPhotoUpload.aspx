<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="ItemPhotoUpload.aspx.cs" Inherits="V1_Administration_Item_ItemPhotoUpload" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<script src="/V1/Scripts/JCrop/jquery.Jcrop.min.js"></script>
	<link href="/V1/Styles/jquery.Jcrop.min.css" rel="stylesheet" />
	<style>
		#<%=imgItemPhoto.ClientID%>
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
			//$("#<%=itemPhotoUpload.ClientID%>").hide();

            var preview = document.querySelector('#<%=imgItemPhoto.ClientID %>');
            var file = document.querySelector('#<%=itemPhotoUpload.ClientID %>').files[0];
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
			<div class="row">
				<div class="col-sm-12 well">
					<div class="form-horizontal">
						<h1>Upload a Item Photo</h1>
						<div class="col-sm-12">
							<label class="col-md-3 control-label pull-left">Upload an Item Photo*</label>
							<div class="m-t-lg">
								<input ID="itemPhotoUpload" type="file" name="itemPhoto" onchange="previewFile()"  runat="server" />
								<div class="m-t-lg m-b-lg">
									<asp:Image ID="imgItemPhoto" runat="server" ImageUrl="../Images/icons8-customer-64.png" />
								</div>
								<div class="m-b-md">
									<asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-success pull-left" Text="Upload Item Photo" CausesValidation="false" OnClick="btnUpdate_Click" />
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
</asp:Content>