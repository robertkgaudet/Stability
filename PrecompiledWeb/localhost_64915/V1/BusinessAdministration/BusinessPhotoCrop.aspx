<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_BusinessAdministration_BusinessPhotoCrop, App_Web_hdd3xljm" %>

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
			CropImage();
		});

		function CropImage()
		{
			jQuery(function ($) {
				$('#<%=imgBusinessPhoto.ClientID %>').Jcrop({
					boxWidth: 300,
					bgColor: 'black',
					setSelect: [100, 100, 200, 400],
					aspectRatio: 1 / 1,
					bgColor: 'black',
					bgOpacity: .3,
					onSelect: storeCoords
				});
			});
		}

		function storeCoords(c)
		{
			jQuery('#<%=X.ClientID%>').val(c.x);
			jQuery('#<%=Y.ClientID%>').val(c.y);
			jQuery('#<%=W.ClientID%>').val(c.w);
			jQuery('#<%=H.ClientID%>').val(c.h);
		};
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<div class="row">
				<div class="col-sm-12 well">
					<div class="form-horizontal">
						<h1>Crop Your Business Photo</h1>
						<div class="col-sm-12">
							<div class="m-t-lg">
								<div class="m-t-lg m-b-lg">
									<asp:Image ID="imgBusinessPhoto" runat="server" ImageUrl="../Images/icons8-customer-64.png" />
								</div>
							</div>
							<asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-success" Text="Add Business Photo" CausesValidation="false" OnClick="btnUpdate_Click" />
							<asp:HiddenField ID="X" runat="server" />
							<asp:HiddenField ID="Y" runat="server" />
							<asp:HiddenField ID="W" runat="server" />
							<asp:HiddenField ID="H" runat="server" />
						</div>
					</div>
				</div>
			</div>
</asp:Content>