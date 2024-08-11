<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_NonProfit_AvailableResources, App_Web_z0at4nie" %>
<%@ Register Src="~/V1/UserControls/TeamNavigation.ascx" TagPrefix="uc1" TagName="TeamNavigation" %>
<%@ Register Src="~/V1/UserControls/TeamHeader.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

	<uc1:TeamHeader runat="server" ID="ucTeamHeader" />

	<div class="content">
        <div class="row">
            <div class="col-md-3">
				<uc1:TeamNavigation runat="server" ID="ucTeamNavigation" />
            </div>
            <div class="col-md-9">
					<!--PAGE HEADER-->
                <div class="hpanel ">
                    <div class="panel-heading hbuilt">
                        <div class="font-normal">
							<h1 class="m-b-none"><i class="fa fa-truck"></i> Available Resources</h1>
							<small class="text-muted">This teams available equipment and other resources.</small>
                        </div>
                    </div>
					<!--PAGE CONTENT-->
					<div class="m-t-md">
						<asp:Literal ID="litAvailableResources" runat="server"></asp:Literal>
					</div>
					<!--PAGE FOOTER-->
                </div>
            </div>
        </div>
    </div>
</asp:Content>

