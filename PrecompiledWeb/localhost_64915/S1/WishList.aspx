<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/DisasterRegistry.master" autoeventwireup="true" inherits="S1_WishList, App_Web_avkw21jy" %>

<%@ Register Src="~/S1/UserControls/Items.ascx" TagPrefix="uc1" TagName="Items" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="container">
            

<%--	
            <div class="hpanel">
                <div class="panel-body">

                    <h2 class="font-light m-b-xs">
                       <asp:Literal id="Literal1" runat="server"></asp:Literal>
                    </h2>
                    <small>
				        <asp:Literal id="litFulleName" runat="server"></asp:Literal>
				        <asp:HyperLink ID="hypAddStory" Font-Underline="true" CssClass="btn btn-sm btn-info" runat="server" Text="Add A Story" Visible="false" NavigateUrl="/S1/Profile/NewBlogPost.aspx"></asp:HyperLink>
                    </small>
			
                </div>
            </div>
	
	        <div class="hpanel">
		        <div class="panel-heading">
			        <div class="panel-tools pull-left m-r-sm">
				        <a class="showhide"><i class="fa fa-chevron-up"></i></a>
			        </div>
			        Choose a Category
		        </div>
		        <div class="panel-body">
			        <p>
				        <asp:Literal ID="litCategories" runat="server"></asp:Literal>
			        </p>
		        </div>
	        </div>--%>
            <uc1:Items runat="server" ID="Items" />
		</div>
</asp:Content>

