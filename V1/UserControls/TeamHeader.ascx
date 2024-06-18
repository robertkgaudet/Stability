<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TeamHeader.ascx.cs" Inherits="V1_UserControls_TeamHeader" %>	<script type="text/javascript">

		$(document).ready(function () {
			$("#nonProfit.dropdown-menu li").click(function () {
				window.location.href = "/V1/NonProfit/Default.aspx?organizationId=" + $(this).attr('id');
			});
		});
</script>
	<div class="normalheader ">
        <div class="hpanel">
            <div class="panel-body">
                <div id="hbreadcrumb" class="pull-right">
					
					<button id="btn-NonProfitDropdown" class="btn btn-outline btn-default nonProfit dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">View Another Team <i class="fa fa-sort-down"></i></button>
					<ul id="nonProfit" class="dropdown-menu text-center required">
						<%=_nonProfitDropDown%>
					</ul>
                    <ol class="hbreadcrumb breadcrumb">
                        <li>
							<asp:HyperLink ID="hypBreadcrumbTeamName" runat="server"></asp:HyperLink>
                        </li>
                        <li class="active">
                            <span>
								<asp:Literal ID="litBreadcrumbPageName" runat="server"></asp:Literal>
                            </span>
                        </li>
                    </ol>
                </div>
				<div id="logoDiv" class="pull-left m-r-md" style="background-color:white; padding:10px; border:solid 1px #ccc;">
					<asp:Image ID="imgLogo" runat="server" Width="100px" />
				</div>
                <h2 class="font-light m-b-xs">
                    <asp:Literal ID="litTeamName" runat="server"></asp:Literal>
                </h2>
                <small><asp:Literal ID="litTeamDescription" runat="server"></asp:Literal></small>
            </div>
        </div>
    </div>