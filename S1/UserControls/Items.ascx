<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Items.ascx.cs" Inherits="S1_UserControls_Items" %>
	<link rel="stylesheet" href="/Homer/vendor/OwlCarousel2-2.3.4/dist/assets/owl.carousel.min.css" />
    <link rel="stylesheet" type="text/css" href="/Homer/vendor/OwlCarousel2-2.3.4/dist/assets/owl.theme.default.min.css"/>
	<script src="/V1/Scripts/masonry.pkgd.min.js"></script>
	<script src="/Homer/Vendor/OwlCarousel2-2.3.4/dist/owl.carousel.min.js"></script>

	<script>
		$(document).ready(function () {
			$('.grid').masonry({
				// options
				itemSelector: '.grid-item',
				gutter: 20
            });

            $('.owl-carousel').owlCarousel({
                margin: 10,
                autoplay: true,
                loop: true,
                items: 2,
                autoplayTimeout: 4000,
                autoplayHoverPause: true
            });

        });

        $('.PhotoItem').on('click', function () {
            alert('TEST');
            $('#imagepreview').attr('src', this.attr('src'));        // here asign the image to the modal when the user click the enlarge link
            $('#imagemodal').modal('show');                                     // imagemodal is the id attribute assigned to the bootstrap modal, then i use the show function
        });
	</script>
	<style type="text/css">
        .PhotoItem:hover{
            cursor:pointer;
        }
		.grid-item {width:210px;}
		.tinted img 
        {
	        filter: brightness(50%);
		}
        .text-success
        {
            color:#64cc34;
        }
        h3 {white-space: nowrap;}
	</style>

    <div class="modal fade" id="imagemodal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel">Image preview</h4>
                </div>
                <div class="modal-body">
                    <img src="" id="imagepreview" style="width: 400px; height: 264px;" >
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <div class="owl-carousel owl-theme owl-loaded">
                <asp:Repeater ID="rptPhotos" runat="server" OnItemDataBound="rptPhotos_ItemDataBound">
                    <ItemTemplate>
                        <div style="border:solid 5px white;">
                            <asp:Image runat="server" ID="imgCaroselImage" CssClass="PhotoItem"/>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="owl-dots">
                <div class="owl-dot active"><span></span></div>
                <div class="owl-dot"><span></span></div>
                <div class="owl-dot"><span></span></div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-9">
            <div>
		        <h2 class="font-bold m-xs">
                    <asp:Literal ID="litPageTitle" runat="server"></asp:Literal>
		        </h2>
            </div>
	        <!--BEGIN GRID-->
	        <div class="grid">		        <%=gridItem%>	        </div>
        </div>
        <div class="col-sm-3">
	        <div class="panel-body">
                <asp:HyperLink ID="hypSurvivorProfilePage" runat="server" Text="Survivor Profile"></asp:HyperLink>
                <br />
                <asp:HyperLink ID="hypDisasterPage" runat="server"></asp:HyperLink>
                
	            <div class="hpanel m-t-sm">
		            <div class="panel-body">
                        <P>
                            <asp:Label ID="lblDescription" runat="server"></asp:Label>
                        </P>
                        <asp:Literal id="litName" runat="server"></asp:Literal>
		            </div>
	            </div>
	        </div>
        </div>
    </div>