<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Programs.aspx.cs" Inherits="V1_NonProfit_Programs" %>
<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
				<uc1:TeamHeader runat="server" ID="ucTeamHeader" />

	
                    <div class="panel-heading">
						<div id="hbreadcrumb" class="pull-right">
							<ol class="hbreadcrumb breadcrumb">
								<li>
									<asp:HyperLink CssClass="font-normal btn btn-sm btn-info" id="hypEditPrograms" runat="server" Text="Add a Program" Visible="false"></asp:HyperLink>
								</li>
								<li class="active">
									<span>
										<asp:HyperLink ID="hypGlobalPrograms" runat="server" CssClass="font-normal btn btn-sm btn-info" NavigateUrl="~/V1/DisasterPrograms.aspx" Text="Global Programs"></asp:HyperLink>
									</span>
								</li>
							</ol>
						</div>
                    </div>
					<div class="m-t-md">
						<div class="row projects">
							<div id="divPrograms" runat="server" visible="true">
								<asp:Repeater ID="rptPrograms" runat="server" OnItemDataBound="rptPrograms_ItemDataBound">
									<ItemTemplate>
										<div class="col-xs-12">
											<div class="hpanel hbuilt hbgblue">
												<div class="panel-body">
													<asp:Literal ID="litSharedPrivate" runat="server"></asp:Literal>
													<div class="row">
														<div class="col-sm-8">
															<h4><asp:Literal ID="litProgramName" runat="server"></asp:Literal></h4>
															<p><asp:Literal ID="litProgramDescription" runat="server"></asp:Literal></p>
														</div>
														<div class="col-sm-4 project-info">
															<div class="project-action m-t-md">
																<div class="btn-group">
																	<button class="btn btn-xs btn-default"> Create Deployment</button>
																	<asp:HyperLink CssClass="btn btn-xs btn-default" id="hypEditPrograms" runat="server" Text="Edit" Visible="false"></asp:HyperLink>
																</div>
															</div>
															
															<div id="logoDiv" class="m-t-md" style="background-color:white; text-align:center; padding:10px; border:solid 1px #ccc;">
																<asp:Image ID="imgLogo" runat="server" Width="100px" />
															</div>
														</div>
													</div>
													<div class="row">
														<div class="col-sm-3">
															<div class="project-label">DEPLOY</div>
															<asp:Literal ID="litRequiresDeployment" runat="server"></asp:Literal>
														</div>
														<div class="col-sm-3">
															<div class="project-label">REMOTE</div>
															<asp:Literal ID="litRemoteWork" runat="server"></asp:Literal>
														</div>
														<div class="col-sm-3">
															<div class="project-label">TRAINING	</div>
															<asp:Literal ID="litRequiresTraining" runat="server"></asp:Literal>
														</div>
														<div class="col-sm-3">
															<div class="project-label">DEPLOYS	</div>
																0
														</div>
													</div>
												</div>
												<div class="panel-footer">
													Create a Deployment
												</div>
											</div>
										</div>
									</ItemTemplate>
									<FooterTemplate>
										<asp:Label ID="defaultItem" runat="server" Visible='<%#rptPrograms.Items.Count == 0 %>' Text="No programs found" />
									</FooterTemplate>
								</asp:Repeater>
							</div>
						</div>
					</div>
				<uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>