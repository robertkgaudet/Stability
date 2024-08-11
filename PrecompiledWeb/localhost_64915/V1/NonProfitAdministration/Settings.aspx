<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_NonProfitAdministration_Settings, App_Web_ib0zyt45" %>
<%@ Register Src="~/V1/UserControls/TeamNavigation.ascx" TagPrefix="uc1" TagName="TeamNavigation" %>
<%@ Register Src="~/V1/UserControls/TeamHeader.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('input[type="checkbox"]').each(function () {
				$(this).addClass("i-checks");
			});
        });
    </script>
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
							<h1 class="m-b-none"><i class="fa fa-cog"></i> Settings</h1>
							<small class="text-muted">Adjust Team Settings.</small>
                        </div>
                    </div>
					<!--PAGE CONTENT-->
					<div class="panel-body">
						<div class="form-group">
							<div id="divUpdateMessage" runat="server" class="alert alert-success text-center" visible="false">
								<i class="fa fa-2x fa-check-circle"></i><hr />Update Complete
							</div>
							<div class="col-sm-12 m-t-md">
								<div><label> <input runat="server" type="checkbox" id="chkEnableTicketing" class="i-checks"> Allow the public to request help.</label></div>
							</div>
							<div class="col-sm-12 m-t-md">
								<div><label> <input runat="server" type="checkbox" id="chkHideTeamList" class="i-checks"> Hide my team list from my team members.</label></div>
							</div>
							<div class="col-sm-12 m-t-lg">
								<asp:Button ID="btnSubmit" runat="server" Text="Update Settings" OnClick="btnSubmit_Click" />
							</div>
						</div>
					</div>
					<!--PAGE FOOTER-->
                </div>
            </div>
        </div>
    </div>
</asp:Content>