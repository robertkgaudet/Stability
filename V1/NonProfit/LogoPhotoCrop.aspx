<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="LogoPhotoCrop.aspx.cs" Inherits="V1_Logo_LogoPhotoCrop" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<script src="/V1/Scripts/JCrop/jquery.Jcrop.min.js"></script>
	<link href="/V1/Styles/jquery.Jcrop.min.css" rel="stylesheet" />
	<style>
		#<%=imgLogoPhoto.ClientID%>
		{
			max-width:100%;
			max-height:100%;
		}
	</style>

	<script type="text/javascript">
        $(document).ready(function () {
            CropImage();
        });

        function CropImage() {
            jQuery(function ($) {
                $('#<%=imgLogoPhoto.ClientID %>').Jcrop({
                    boxWidth: 500,
                    bgColor: 'black',
					setSelect: [50, 50, 300, 170],
					aspectRatio: 2.5,
                    bgColor: 'black',
                    bgOpacity: .3,
                    onSelect: storeCoords
                });
            });
        }

        function storeCoords(c) {
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
						<h1 runat="server" id="lbltext"></h1>
						<div class="col-sm-12">
							<div class="m-t-lg">
								<div class="m-t-lg m-b-lg">
									<asp:Image ID="imgLogoPhoto" runat="server" ImageUrl="../Images/icons8-customer-64.png" />
								</div>
							</div>
							<asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-success" Text="Add Logo Photo" CausesValidation="false" OnClick="btnUpdate_Click" />
							<asp:HiddenField ID="X" runat="server" />
							<asp:HiddenField ID="Y" runat="server" />
							<asp:HiddenField ID="W" runat="server" />
							<asp:HiddenField ID="H" runat="server" />
						</div>
					</div>
				</div>
			</div>
</asp:Content>