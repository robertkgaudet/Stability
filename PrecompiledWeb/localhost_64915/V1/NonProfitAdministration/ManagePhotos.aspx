<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_NonProfitAdministration_ManagePhotos, App_Web_ib0zyt45" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<script src="/V1/Scripts/JCrop/jquery.Jcrop.min.js"></script>
	<link href="/V1/Styles/jquery.Jcrop.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/Homer/vendor/blueimp-gallery/css/blueimp-gallery.min.css" />
    <script src="/Homer/vendor/blueimp-gallery/js/jquery.blueimp-gallery.min.js"></script>
    <!-- Local style for demo purpose -->
    <style>

        .lightBoxGallery {
            text-align: center;
        }

        .lightBoxGallery a {
            margin: 5px;
            display: inline-block;
        }

    </style>

	<style>
		#<%=imgProfilePhoto.ClientID%>
		{
			max-width:100%;
			max-height:100%;
		}
	</style>

	<script type="text/javascript">

		$(document).ready(function () {

            $("#<%=btnUpdate.ClientID%>").hide();
            $("#divUpload").hide();
            
            $("#causeTypeList.dropdown-menu li").click(function () {
                $("#btn-causeDropdown.dropdown-chooseCause").html("<i class='fa fa-globe'></i> " + causeName);
                $("#<%=hidCauseId.ClientID%>").val($(this).attr('id'));
                $("#divUpload").show();
            });

		});

		function previewFile()
		{
			$("#<%=btnUpdate.ClientID%>").show();
			//$("#<%=profilePhotoUpload.ClientID%>").hide();

            var preview = document.querySelector('#<%=imgProfilePhoto.ClientID %>');
            var file = document.querySelector('#<%=profilePhotoUpload.ClientID %>').files[0];
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
    <div class="content">
		<div class="row">
			<div class="col-sm-12 well">
				<div class="form-horizontal">
					<h5>Choose A Cause to Upload A Photo (.jpg, .png file types only)</h5>
					<div class="col-sm-12">
                        <div class="row" id="chooseCauseRow">
                            <div class="col-lg-12">
                                <div id="divCauseDropdown" class="dropdown m-b-md">
				                    <button id="btn-causeDropdown" class="btn dropdown-toggle dropdown-chooseCause border-danger" style="background-color:forestgreen; color:white;" type="button" data-toggle="dropdown">Choose A Cause <i class="fa fa-globe"></i></button>
				                    <ul id="causeTypeList" class="dropdown-menu border-dark border-danger">
                                        <asp:Literal ID="litCause" runat="server"></asp:Literal>
				                    </ul>
                                    <asp:HiddenField id="hidCauseId" runat="server" />
                                </div>
                            </div>
                        </div>
						<div class="m-t-lg" id="divUpload">
							<input ID="profilePhotoUpload" type="file" name="profilePhoto" onchange="previewFile()"  runat="server" />
							<div class="m-t-lg m-b-lg">
								<asp:Image ID="imgProfilePhoto" runat="server" ImageUrl="../Images/icons8-customer-64.png" />
							</div>
							<div class="m-b-md">
								<asp:LinkButton ID="btnUpdate" runat="server" CssClass="btn btn-success pull-left" Text="Upload Photo" CausesValidation="false" OnClick="btnUpdate_Click" />
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
    </div>
    
    <div class="content">
        <div class="row">
            <div class="col-lg-12">
                <div class="hpanel">
                    <div class="panel-body">
                        <div class="lightBoxGallery">
                            <asp:Literal ID="litPhotoCallery" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="panel-footer">
                        <i class="fa fa-picture-o"> </i> 20 Images
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="content">
	    <div class="row">
		    <div class="col-sm-12 well">
    
                <!-- The Gallery as lightbox dialog, should be a child element of the document body -->
                <div id="blueimp-gallery" class="blueimp-gallery">
                    <div class="slides"></div>
                    <h3 class="title"></h3>
                    <a class="prev">‹</a>
                    <a class="next">›</a>
                    <a class="close">×</a>
                    <a class="play-pause"></a>
                    <ol class="indicator"></ol>
                </div>
		    </div>
	    </div>
    </div>
</asp:Content>