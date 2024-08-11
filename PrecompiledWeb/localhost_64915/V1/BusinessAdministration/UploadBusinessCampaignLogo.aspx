<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_BusinessAdministration_UploadBusinessCampaignLogo, App_Web_hdd3xljm" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<script src="/V1/Scripts/JCrop/jquery.Jcrop.min.js"></script>
	<link href="/V1/Styles/jquery.Jcrop.min.css" rel="stylesheet" />
	<style>
		#<%=imgBusinessPhoto.ClientID%>
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
			//$("#<%=businessPhotoUpload.ClientID%>").hide();

            var preview = document.querySelector('#<%=imgBusinessPhoto.ClientID %>');
            var file = document.querySelector('#<%=businessPhotoUpload.ClientID %>').files[0];
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
						<h1>Upload Your Business Logo</h1>
						<div class="col-sm-12">
							<label class="col-md-3 control-label pull-left">Upload Your Business Logo*</label>
							<div class="m-t-lg">
								<input ID="businessPhotoUpload" type="file" name="logoPhoto" onchange="previewFile()"  runat="server" />
								<div class="m-t-lg m-b-lg">
									<asp:Image ID="imgBusinessPhoto" runat="server" ImageUrl="../Images/icons8-customer-64.png" />
								</div>
								<div class="m-b-md">
									<asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-success pull-left" Text="Upload Business Logo" CausesValidation="false" OnClick="btnUpdate_Click" />
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
</asp:Content>