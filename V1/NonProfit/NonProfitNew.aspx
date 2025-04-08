<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="NonProfitNew.aspx.cs" Inherits="V1_Administration_NonProfitNew" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>	
	<script>
		$(function () {

			$("#form1").validate({
				rules: {
					<%=txtParentOrganization.UniqueID%>: {
					required: true
				},
					<%=txtDescription.UniqueID%>: {
						required: true
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
					<%=txtYouTube.UniqueID%>: {
						url: true
					},
					<%=txtPrimaryPhonenumber.UniqueID%>: {
						required: true,
						number: true,
						maxlength: 12
					},
					<%=txtYearFounded.UniqueID%>: {
						required: true,
						number: true,
						maxlength: 4
					},
					<%=txtSecondaryPhoneNumber.UniqueID%>: {
						number: true,
						maxlength: 12
					},
					<%=txtPublicPhoneNumber.UniqueID%>: {
						number: true,
						maxlength: 12
					},
					<%=txtPOCFullname.UniqueID%>: {
						required: true
					},
					<%=txtPOCPhoneNumber.UniqueID%>: {
						required: true,
						number: true,
						maxlength: 12
					},
					<%=txtPOCEmailAddress.UniqueID%>: {
						required: true,
						email: true
					},
					<%=txtPublicEmailAddress.UniqueID%>: {
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
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<h2 class="font-light m-b-xs">
								Create my own Community Disaster Relief Team
							</h2>
							Capable members of the community can and must help each other, especially the vulnerable, after a natural disaster.
							<br />The features on Stability are designed to connect you with caring members of your network.
							<ul>
								<li>
									Find and respond to current requests for help.
								</li>
								<li>
									Invite friends, family, coworkers, church members anyone in your network who can help.
								</li>
								<li>
									Choose your community and list your team deployments within it to track and share your efforts.
								</li>
							</ul>
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
							<h4><asp:Literal ID="litParentOrganization" runat="server"></asp:Literal></h4>
						</div>
						<div class="panel-body">
							
							<div runat="server" id="divMessage" visible="false">
								<i class="fa fa-bolt"></i>
								<asp:Literal runat="server" id="lblMessage"></asp:Literal>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">My Team Name *</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtParentOrganization" class="form-control" placeholder="Organization Name"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">URL Friendly Name* <br /><small class="text-muted">No Spaces or Special Characters</small></label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtURLFriendlyName" class="form-control" placeholder="Enter Your Organization Name with No Spaces"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Description *</label>
								<div class="col-sm-5">
									<textarea id="txtDescription" runat="server" class="form-control" placeholder="Organization Description"></textarea>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Purpose/Mission *</label>
								<div class="col-sm-5">
									<textarea id="txtPurposeMission" runat="server" class="form-control" placeholder="Organization Mission & Purpose"></textarea>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Year Founded *</label>
								<div class="col-sm-3"><input type="text" required runat="server" id="txtYearFounded" class="form-control" placeholder="Year Founded"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">VOAD Member</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkVoad" class="form-control">
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">501c3 Non Profit</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chk501c3Status" class="form-control">
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">EIN</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtEIN" class="form-control" placeholder="IRS EIN"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Address *</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtAddress" class="form-control i-check" placeholder="Address"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">City *</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtCity" class="form-control" placeholder="City"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">State *</label>
								<div class="col-sm-5">
									<asp:DropDownList ID="ddlState" runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control" required=""></asp:DropDownList>
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Zip *</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtZipCode" class="form-control" placeholder="Zip Code"></div>
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
								<label class="col-sm-2 control-label">Primary Phone Number *</label>
								<div class="col-sm-5"><input type="text" maxlength="10" required runat="server" id="txtPrimaryPhonenumber" class="form-control" placeholder="Primary Phone"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Secondary Phone Number</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtSecondaryPhoneNumber" class="form-control" placeholder="Secondary Phone"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Public Phone Number</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtPublicPhoneNumber" class="form-control" placeholder="Public Phone Number"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Public Email Address</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtPublicEmailAddress" class="form-control" placeholder="Public Email Address"></div>
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
							Point of Contact
						</div>
						<div class="panel-body">

							<div class="form-group">
								<label class="col-sm-2 control-label">Full Name *</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtPOCFullname" class="form-control" placeholder="Point of Contact Full Name"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Phone Number *</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtPOCPhoneNumber" class="form-control" placeholder="Point of Contact Phone Number"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Email Address *</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtPOCEmailAddress" class="form-control" placeholder="Point of Contact Email Address"></div>
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
								<label class="col-sm-2 control-label">YouTube Channel URL</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtYouTube" class="form-control" placeholder="YouTube Channel URL"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Instagram Username</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtInstagram" class="form-control" placeholder="Instagram Username"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Twitter Handle</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtTwitter" class="form-control" placeholder="Twitter Handle"></div>
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