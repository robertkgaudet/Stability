<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Stages.aspx.cs" Inherits="V1_Stages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">	
			<div class="col-lg-6">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<a class="small-header-action">
								<div class="clip-header">
									<i class="fa fa-arrow-up"></i>
								</div>
							</a>
							<h2 class="font-light m-b-xs">Flood Recovery Stages</h2>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-md-6">
					<div class="hpanel">
						<div class="v-timeline vertical-container"  data-child="vertical-timeline-block" data-delay="1">
							<asp:Repeater ID="rptStages" runat="server" OnItemDataBound="rptStages_ItemDataBound">
								<ItemTemplate>
									<div class="vertical-timeline-block">
										<div class="vertical-timeline-icon navy-bg h-bg-blue text-white">
											<i class="pe-7s-drop"></i>
										</div>
										<div class="vertical-timeline-content">
											<div class="p-sm">
												<div class="m-n pull-left">
													
												</div>
												<h2>
													<asp:Literal ID="litTick" runat="server"></asp:Literal>:
													<asp:Literal ID="litStageLabel" runat="server"></asp:Literal>
												</h2>
												<p>
													<asp:Literal ID="litStageDescription" runat="server"></asp:Literal>
												</p>
											</div>
											<div class="panel-footer">
												<asp:Literal ID="litRecentPost" runat="server"></asp:Literal>
											</div>
										</div>
									</div>
								</ItemTemplate>
							</asp:Repeater>
						</div>
					</div>
				</div>
				<div class="col-md-6">
				</div>
			</div>
		</div>
</asp:Content>