<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="EditLocation.aspx.cs" Inherits="V1_Administration_Resources_EditLocation" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>
	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
	

	<script>
		$(function (){
			$("#form1").validate({
				rules:
				{
					<%=txtLocationName.UniqueID%>: 
					{
						required: true
					},
					<%=txtLocationDescription.UniqueID%>: 
					{
						required: true
					},
					<%=txtDonationURL.UniqueID%>:
					{
						url: true
					},
					<%=txtWebsiteURL.UniqueID%>: 
					{
						url: true
					},
					<%=txtFacebookURL.UniqueID%>: 
					{
						url: true
					},
					<%=txtYouTubeURL.UniqueID%>: 
					{
						url: true
					},
					<%=txtPhoneNumber.UniqueID%>: 
					{
						number: true,
						maxlength: 10
					},
					<%=txtCapacity.UniqueID%>: 
					{
						number: true,
						maxlength: 4
					},
					<%=txtPOCPhoneNumber.UniqueID%>: 
					{
						number: true,
						maxlength: 10
					},
					<%=txtPOCEmailAddress.UniqueID%>: 
					{
						email: true
					},
					<%=txtEmailAddress.UniqueID%>: 
					{
						email: true
					},
					<%=txtDateOpened.UniqueID%>:
					{
						date: true
					},
					<%=txtDateClosed.UniqueID%>:
					{
						date: true
					}
				},
				submitHandler: function (form) {
					form.submit();
				},
				errorPlacement: function (error, element) {
					$(element)
						.closest("form")
						.find("label[for='" + element.attr("id") + "']")
						.append(error);
				},
				errorElement: "span",
			});
		});

		$(document).ready(function () {
			$('#divMessage').hide();
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
								Edit Location Profile
							</h2>
						</div>
					</div>
				</div>
			</div>
		</div>
		
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-12 container">
					<div class="hpanel form-horizontal">
						<div class="panel-heading hbuilt">
							Update This Locations Profile
						</div>
						<div class="panel-body">

							<div id="divMessage" runat="server" class="alert alert-success m-b-lg" visible="false">
								<i class="fa fa-bolt"></i>
								<asp:Label runat="server" id="lblMessage"></asp:Label>
								<br />
								<asp:HyperLink runat="server" id="hypAddNewLocation" text="Add New Location"></asp:HyperLink>
                                |
								<asp:HyperLink runat="server" id="hypViewLocation" text="View This Location"></asp:HyperLink>
							</div>

							<div class="form-group" runat="server" id="divExistingLocationAddress">
								<label class="col-sm-2 control-label">Location Address</label>
								<div class="col-sm-5 control-label"><label class="pull-left font-normal text-info" id="lblLocationAddress" runat="server"></label></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Location Name</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtLocationName" class="form-control" placeholder="Location Name"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Location Description</label>
								<div class="col-sm-5"><textarea type="text" rows="10" required runat="server" id="txtLocationDescription" class="form-control" placeholder="Location Description"/></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Phone Number</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtPhoneNumber" maxlength="10" class="form-control" placeholder="Phone Number"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Email Address</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtEmailAddress" class="form-control" placeholder="Email Address"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Has Generator</label>
								<div class="col-sm-1 m-t-sm">
									<input type="checkbox" runat="server" id="chkHasGenerator" class="form-control">
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Generator Size</label>
								<div class="col-sm-5"><input type="text" runat="server" maxlength="249" id="txtGeneratorSize" class="form-control" placeholder="Generator Description"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Is Visible</label>
								<div class="col-sm-1 m-t-sm">
									<input type="checkbox" runat="server" id="chkActive" class="form-control">
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Place On Map</label>
								<div class="col-sm-1 m-t-sm">
									<input type="checkbox" runat="server" id="chkIsOnMap" class="form-control">
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Medical Help Available</label>
								<div class="col-sm-1 m-t-sm">
									<input type="checkbox" runat="server" id="chkMedicalHelpProvided" class="form-control">
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Allows Pets</label>
								<div class="col-sm-1 m-t-sm">
									<input type="checkbox" runat="server" id="chkAllowsPets" class="form-control">
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Needs Volunteers</label>
								<div class="col-sm-1 m-t-sm">
									<input type="checkbox" runat="server" id="chkNeedsVolunteers" class="form-control">
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Capacity</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtCapacity" maxlength="4" class="form-control" placeholder="Capacity"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Point of Contact Name</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtPointOfContactName" class="form-control" placeholder="POC Name"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Point of Contact Phone Number</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtPOCPhoneNumber" maxlength="10" class="form-control" placeholder="POC Phone Number"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Point of Contact Email Address</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtPOCEmailAddress" class="form-control" placeholder="POC Email Address"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Facebook URL (http://)</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtFacebookURL" class="form-control" placeholder="Facebook URL"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Website URL (http://)</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtWebsiteURL" class="form-control" placeholder="Website URL"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Donation URL (http://)</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtDonationURL" class="form-control" placeholder="Donation URL"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">YouTube URL (http://)</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtYouTubeURL" class="form-control" placeholder="YouTube URL"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Twitter Username</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtTwitterUsername" class="form-control" placeholder="Twitter Username"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Instagram Username</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtInstagramUsername" class="form-control" placeholder="Instagram Username"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Date Opened</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtDateOpened" class="form-control" placeholder="Date Opened"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Date Closed</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtDateClosed" class="form-control" placeholder="Date Closed"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label"></label>
								<div class="col-sm-5">
									<div class="pull-right">
										<asp:LinkButton id="btnCancel" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default" Text="Cancel" />
										<asp:Button id="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text=" Submit " />
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>