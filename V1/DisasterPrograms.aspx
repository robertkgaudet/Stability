<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="DisasterPrograms.aspx.cs" Inherits="V1_DisasterPrograms" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.logoDiv:hover
		{
			background-color:#E8D3FE;
			cursor:pointer;
		}
		.logoDiv{
			background-color:white;
			text-align:center;
			padding:10px;
			border:solid 1px #ccc;
		}
		.text-muted
		{color:#F1F3F6;}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
	<div class="content">
        <div class="row">
            <div class="col-md-12">
					<!--PAGE HEADER-->
					<div class="m-t-md">
					<div class="hpanel ">
						<div class="panel-heading hbuilt">
							<div class="font-normal">
								<h1 class="m-b-none">Programs</h1>
								<small>Choose a program that best matches your teams disaster work and deployment. Each is unique programs is created by the community.</small>
								<br />
								<a href="/V1/DisasterPrograms.aspx">All Programs</a>
							</div>
						</div>
					</div>
					<!--PAGE CONTENT-->
					<div class="panel-body">
						<div class="row projects">
							<div id="divPrograms" runat="server" visible="true">
								<asp:Repeater ID="rptPrograms" runat="server" OnItemDataBound="rptPrograms_ItemDataBound">
									<ItemTemplate>
										<div class="col-lg-6">
											<div class="hpanel hbuilt">
												<div class="panel-body">
													<asp:Literal ID="litSharedPrivate" runat="server"></asp:Literal>
													<span class="label label-info pull-right m-r-xs">NEW</span>
													<div class="row">
														<div class="col-sm-8">
															<h4><asp:HyperLink ID="hypProgramName" runat="server"></asp:HyperLink></h4>
															<p><asp:Literal ID="litProgramDescription" runat="server"></asp:Literal></p>
														</div>
														
														<div class="col-sm-4 project-info pull-left">
															<small class="pull-left">Created By</small>
															<div id="logoDiv" class="m-t-md logoDiv">
															
																<asp:ImageButton OnClick="imgLogo_Click" ID="imgLogo" runat="server" Width="100px" />
															</div>
														</div>
													</div>
													<div class="row">
														<div class="col-sm-12 m-b-lg m-t-lg">
															<div class="project-label">Recommended Program Roles</div>
															Select a role below for detailed description and training.
															<br />
															<asp:Repeater ID="dlPositions" runat="server" OnItemDataBound="dlPositions_ItemDataBound">
																<ItemTemplate>
																	<i class="fa fa-user-circle text-muted"></i> <asp:HyperLink ID="hypPosition" Target="_blank" runat="server"></asp:HyperLink>
																</ItemTemplate>
																<SeparatorTemplate><br /></SeparatorTemplate>
																
															</asp:Repeater>
														</div>
													</div>
												<div class="panel-footer">
													<div class="project-action">
														<div class="btn-group">
															<button class="btn btn-xs btn-default"> Create Deployment</button>
															<asp:HyperLink CssClass="btn btn-xs btn-default" id="hypEditPrograms" runat="server" Text="Edit" Visible="false"></asp:HyperLink>
														</div>
													</div>
													<div class="row">
														<div class="col-sm-3">
															<div class="project-label">DEPLOYMENT STATUS</div>
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
												</div>
											</div>
										</div>
									</ItemTemplate>
									<FooterTemplate>
										<asp:Label ID="defaultItem" runat="server" Visible='<%#rptPrograms.Items.Count == 0 %>' Text="No programs found" />
									</FooterTemplate>
								</asp:Repeater>
							</div>
							<asp:HyperLink id="hypEditPrograms" runat="server" Text="Add a Program" Visible="false"></asp:HyperLink>
						</div>
					</div>
					<!--PAGE FOOTER-->
                </div>
            </div>
        </div>
    </div>

</asp:Content>

