<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/DisasterRegistry.master" autoeventwireup="true" inherits="S1_Profile_ItemSubTypePersonSurveyPhoto, App_Web_22v3mdrb" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/DisasterRegistry.master" %>

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

            $("#divPhotoInfo").hide();

		});

		function previewFile()
		{
            $("#divPhotoInfo").show();

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
        <div class="container">
			<div class="row">
				<div class="col-sm-12 well">
					<div class="form-horizontal">
						<h1>Upload a Survey Photo</h1>
						<div class="col-sm-12">
							<div class="m-t-lg">
					            <input ID="itemPhotoUpload" type="file" name="itemPhoto" onchange="previewFile()"  runat="server" />
							    <div class="m-t-lg m-b-lg">
							        <asp:Image ID="imgItemPhoto" runat="server" CssClass="img-responsive" />
                                    <br />
                                         
                                    <div class="well m-t-lg" id="divPhotoInfo">
		                                <div class="form-group ">
			                                <div class="input-group">
				                                <div class="input-group-addon">
					                                Image Title
				                                </div>
                                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="250"></asp:TextBox>
			                                </div>
		                                </div>
		                                <div class="form-group ">
			                                <div class="input-group">
				                                <div class="input-group-addon">
					                                Image Description
				                                </div>
                                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="1000" TextMode="MultiLine" Width="400"></asp:TextBox>
			                                </div>
		                                </div>

								        <div class="m-b-md">
									        <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-success" Text="Upload Item Photo" CausesValidation="false" OnClick="btnUpdate_Click" />
								        </div>
								    </div>
                                </div>
                                <br /><br />
                                <asp:Repeater ID="rptPhotos" runat="server" OnItemDataBound="rptPhotos_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="well bg-light">
                                            <div class="row">
                                                <div class="col-sm-2">
                                                    <asp:HyperLink ID="hypDelete" runat="server">Delete</asp:HyperLink>
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:Image runat="server" ID="imgPhoto" CssClass="img-responsive" />
                                                </div>
                                                <div class="col-sm-6">
                                                    <h4><asp:Literal ID="litTitle" runat="server"></asp:Literal></h4>
                                                    <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
							</div>
						</div>
					</div>
				</div>
			</div>
        </div>
</asp:Content>