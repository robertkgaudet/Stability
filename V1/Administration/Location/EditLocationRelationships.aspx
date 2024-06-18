<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="EditLocationRelationships.aspx.cs" Inherits="V1_Administration_Location_EditLocationRelationships" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>

	<script type="text/javascript">
	$(document).ready(function(){
		$('input[type="checkbox"]').each(function () {
			$(this).addClass("i-checks");
		});

		<%=preselectedLocationStatusJQuery%>

		$("#locationStatusEvent.dropdown-menu li").click(function () {
			$("#btn-dropdown.locationStatus").html($(this).text());
			$("#<%=hidLocationStatusId.ClientID%>").val($(this).attr('id'));
		});


		<%=preselectedLocationTypeJQuery%>
		$("#locationTypeEvent.dropdown-menu li").click(function () {
			$("#btn-dropdown-type.locationType").html($(this).text());
			$("#<%=hidLocationParentTypeId.ClientID%>").val($(this).attr('id'));
		});
	});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<a class="small-header-action">
								<div class="clip-header">
									<i class="fa fa-arrow-up"></i>
								</div>
							</a>
							<h2 class="font-light m-b-xs">
								Edit Location Relationships
							</h2>
							<div class="form-group">
								<div class="pull-right">
									<asp:LinkButton id="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default" Text="Cancel" />
									<asp:Button id="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Save Changes" />
									<br />
								</div>
							</div>
							<asp:HyperLink runat="server" id="hypLocation"></asp:HyperLink>
							
							<div runat="server" id="divMessage" class="alert alert-success" visible="false">
								<i class="fa fa-bolt"></i>
								<asp:Literal runat="server" id="lblMessage"></asp:Literal>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-sm-12 container">
					<div class="row">
						<div class="col-sm-6 container">
							<div class="hpanel">
								<div class="panel-heading hbuilt">
									Location Type
								</div>
								<div class="panel-body">
										Update the Primary Location Type.
									<div id="div2" class="dropdown m-b-md" runat="server">
										<button id="btn-dropdown-type" class="btn btn-outline btn-default locationType dropdown-toggle" type="button" data-toggle="dropdown">Choose The Type <i class="fa fa-sort-down"></i></button>
										<ul id="locationTypeEvent" class="dropdown-menu text-center dropdown-volunteer required">
											<%=locationTypeDropDown%>
										</ul>
									</div>
									<input type="hidden" id="hidLocationParentTypeId" runat="server" />
								</div>
								<div class="panel-footer">
								</div>
							</div>
						</div>
						<div class="col-sm-6 container">
							<div class="hpanel">
								<div class="panel-heading hbuilt">
									Location Status
								</div>
								<div class="panel-body">
										Update the location status.
									<div id="div1" class="dropdown m-b-md" runat="server">
										<button id="btn-dropdown" class="btn btn-outline btn-default locationStatus dropdown-toggle" type="button" data-toggle="dropdown">Choose The Status <i class="fa fa-sort-down"></i></button>
										<ul id="locationStatusEvent" class="dropdown-menu text-center dropdown-volunteer required">
											<%=locationStatusDropDown%>
										</ul>
									</div>
									<input type="hidden" id="hidLocationStatusId" runat="server" />
								</div>
								<div class="panel-footer">
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>

			<div class="row">
				<div class="col-lg-6 container">
					<div class="hpanel">
						<div class="panel-heading hbuilt">
							Location Services Provided
						</div>
						<div class="panel-body">
							<div class="form-group m-t-lg">
								<asp:CheckBoxList ID="chkBoxLocationTypes" runat="server" DataTextField="Name" DataValueField="LocationTypeId" RepeatDirection="Vertical"></asp:CheckBoxList>
							</div>
						</div>
						<div class="panel-footer">
						</div>
					</div>
				</div>

				<div class="col-lg-6 container">
					<div class="hpanel">
						<div class="panel-heading hbuilt">
							Choose Disasters Served
						</div>
						<div class="panel-body">
							<div class="form-group m-t-lg">
								<asp:CheckBoxList ID="chkBoxListDisasters" runat="server" DataTextField="Name" DataValueField="EventId" RepeatDirection="Vertical"></asp:CheckBoxList>
							</div>
						<asp:Button id="Button1" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary pull-left" Text="Save Changes" />
						</div>
						<div class="panel-footer">
						</div>
					</div>
					
				</div>
			</div>
		</div>
</asp:Content>