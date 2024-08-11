<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_BusinessAdministration_EditBusinessCampaign, App_Web_hdd3xljm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>
	
	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
	
	<script>
		$(function () {

			$("#form1").validate({
				rules: {
					<%=txtPOCFullname.UniqueID%>: {
					required: true,
					maxlength: 100
					},
					<%=txtPurposeMission.UniqueID%>: {
						required: true
					},
					<%=txtWebsite.UniqueID%>: {
						url: true
					},
					<%=txtblogURL.UniqueID%>: {
						url: true
					}, 
					<%=txtDonationLink.UniqueID%>: {
						url: true
					}, 
					<%=txtFacebook.UniqueID%>: {
						url: true
					},
					<%=txtFacebookGroup.UniqueID%>: {
						url: true
					},
					<%=txtPhonenumber.UniqueID%>: {
						number: true,
						maxlength: 12
					},
					<%=txtPOCFullname.UniqueID%>: {
						required: true
					},
					<%=txtURLFriendlyCampaignName.UniqueID%>: {
						required: true,
						maxlength: 250
					},
					<%=txtCampaignName.UniqueID%>: {
						required: true,
						maxlength: 1000
					}, 
					<%=txtEmailAddress.UniqueID%>: {
						email: true
					},
					<%=txtZipCode.UniqueID%>: {
						required: true,
						number: true,
						maxlength: 5
					},
					max: {
						required: true,
						maxlength: 4
					},
					email:
					{
							required: true,
							minlength: 5
					}
				},
				submitHandler: function (form) {
					form.submit();
				}
			});
		});

		$(document).ready(function () {
			$('input[type="checkbox"]').each(function () {
				$(this).addClass("i-checks");
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeIn">
					<div class="hpanel">
						<div class="panel-body">
						<h1>	
							<asp:Literal id="litEventName" runat="server"></asp:Literal>
						</h1>
							<h2 class="font-light m-b-xs">
								Add a Non-profit Disaster Response Campaign
							</h2>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
		
							
			<div class="row">
				<div class="col-lg-12 container">
					<div class="alert alert-info" runat="server" id="divMessage" visible="false">
						<p>
						<i class="fa fa-bolt"></i> <asp:HyperLink runat="server" id="hypLinkToCampaign"></asp:HyperLink>
						</p>
					</div>
				</div>
			</div>
			<div class="row" id="divForm" runat="server">
				<div class="col-lg-12 container">
					<div class="hpanel form-horizontal">

						<div class="panel-heading hbuilt">
							Disaster Campaign Profile
						</div>
						<div class="panel-body">
						
							<div class="form-group">
								<label class="col-sm-2 control-label">Point of Contact First and Last Name</label>
								<div class="col-sm-5"><input type="text" runat="server" required id="txtPOCFullname" class="form-control" placeholder="Point of Contact First and Last Name"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Purpose/Mission</label>
								<div class="col-sm-5">
									<textarea id="txtPurposeMission" runat="server" class="form-control" placeholder="Purpose of this campaign response."></textarea>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Campaign Name</label>
								<div class="col-sm-5">
									<input type="text" runat="server" id="txtCampaignName" class="form-control" placeholder="Give Your Campaign a Memorable Name">
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">URL Friendly Campaign Name
								<br />
								<small>No spaces or special characters allowed.</small></label>
								<div class="col-sm-5">
									<input type="text" runat="server" id="txtURLFriendlyCampaignName" class="form-control" placeholder="URL Friendly Campaign Name">
								</div>
							</div>
							
							<div class="form-group">
								<label class="col-sm-2 control-label">Accept Volunteers</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkAcceptsVolunteers" class="form-control">
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Volunteer Instructions</label>
								<div class="col-sm-5">
									<textarea id="txtVolunteerInstructions" runat="server" class="form-control" placeholder="Volunteer Instructions"></textarea>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Staging Address</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtAddress" class="form-control i-check" placeholder="Address"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Staging City</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtCity" class="form-control" placeholder="City"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Staging State</label>
								<div class="col-sm-5">
									<asp:DropDownList ID="ddlState" runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control" required=""></asp:DropDownList>
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Staging Zip</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtZipCode" maxlength="5" class="form-control" placeholder="Zip Code"></div>
							</div>
									
						</div>
						<div class="panel-footer">
						</div>
					</div>

					<div class="hpanel form-horizontal m-t-lg">
						<div class="panel-heading hbuilt">
							Contact Information
						</div>
						<div class="panel-body">

							<div class="form-group">
								<label class="col-sm-2 control-label">Phone Number</label>
								<div class="col-sm-5"><input type="text" maxlength="10" required runat="server" id="txtPhonenumber" class="form-control" placeholder="Primary Phone"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Zello Channel</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtZelloChannel" class="form-control" placeholder="Zello Channel"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Public Email Address</label>
								<div class="col-sm-5"><input type="text" runat="server" required id="txtEmailAddress" class="form-control" placeholder="Public Email Address"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Donation Link (http://)</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtDonationLink" class="form-control" placeholder="Donation Link"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Website (http://)</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtWebsite" class="form-control" placeholder="Website URL"></div>
							</div>

						</div>

						<div class="panel-footer">
						</div>
					</div>

					<div class="hpanel form-horizontal m-t-lg">
						<div class="panel-heading hbuilt">
							Social Media Links
						</div>
						<div class="panel-body">

							<div class="form-group">
								<label class="col-sm-2 control-label">Blog URL</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtblogURL" class="form-control" placeholder="Blog URL"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Facebook Page URL</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtFacebook" class="form-control" placeholder="Facebook Page URL"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Facebook Group URL</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtFacebookGroup" class="form-control" placeholder="Facebook Page URL"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label"></label>
								<div class="col-sm-5">
									<div class="pull-right">
										<asp:LinkButton id="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default" Text="Cancel" />
										<asp:Button id="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Save Changes" />
									</div>
								</div>
							</div>
						</div>

						<div class="panel-footer">
						</div>
					</div>


				</div>

			</div>
		</div>
</asp:Content>