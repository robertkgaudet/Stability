<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Programs.aspx.cs" Inherits="V1_NonProfit_Programs" %>
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
							<h4 class="m-b-none">Programs</h4>
							<small class="text-muted">This team offers the following programs.</small>
                        </div>
                    </div>
					<!--PAGE CONTENT-->
					<div class="panel-body">
						<div id="divPrograms" runat="server" visible="true">
							<asp:Repeater ID="rptPrograms" runat="server" OnItemDataBound="rptPrograms_ItemDataBound">
								<ItemTemplate>
									<div class="m-b-lg">
										<p>
											<h4>
												<asp:Literal ID="litProgramName" runat="server"></asp:Literal>
												<small class="pull-right"><asp:HyperLink id="hypEditPrograms" runat="server" Text="Edit This Program" Visible="false"></asp:HyperLink></small>
											</h4>
											<asp:Literal ID="litProgramDescription" runat="server"></asp:Literal>
										</p>
										<dl class="dl-horizontal">
											<dt class="font-normal">Remote Work:</dt> <dd><asp:Literal ID="litRemoteWork" runat="server"></asp:Literal></dd>
											<dt class="font-normal">Deployment:</dt> <dd><asp:Literal ID="litRequiresDeployment" runat="server"></asp:Literal></dd>
											<dt class="font-normal">Required Training:</dt> <dd><asp:Literal ID="litRequiresTraining" runat="server"></asp:Literal></dd>
										</dl>
									</div>
								</ItemTemplate>
								<FooterTemplate>
									<asp:Label ID="defaultItem" runat="server" Visible='<%#rptPrograms.Items.Count == 0 %>' Text="No programs found" />
								</FooterTemplate>
							</asp:Repeater>
					
							<asp:HyperLink id="hypEditPrograms" runat="server" Text="Add a Program" Visible="false"></asp:HyperLink>
						</div>
					</div>
					<!--PAGE FOOTER-->
                </div>
            </div>
        </div>
    </div>
</asp:Content>

