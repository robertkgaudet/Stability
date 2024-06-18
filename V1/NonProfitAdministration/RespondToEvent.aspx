<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="RespondToEvent.aspx.cs" Inherits="V1_NonProfitAdministration_RespondToEvent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
	<script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>

    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />	
	<script>

        $(document).ready(function () {

            $("#disasterEvent.dropdown-menu li").click(function () {
                $("#btn-dropdown.disasterEvent").html($(this).text());
                var eventId = $(this).attr('id');
                window.location.href = "/V1/NonProfitAdministration/RespondToEvent.aspx?userActionModal=false&eventId=" + eventId + "&organizationId=<%=organizationId%>";
            });

            $('input[type="checkbox"]').each(function () {
                $(this).addClass("i-checks");
			});

			$(function () {
				$('.input-group.date').datepicker({});
			});
        });

        // Function to validate alphanumeric input on keypress
        function validateAlphaNumericInput(event) {
            // Get the input element
            var inputElement = document.getElementById('<%=txtURLFriendlyCampaignName.ClientID%>');

            // Get the current input value
            var inputValue = inputElement.value;

            // Get the key code of the pressed key
            var keyCode = event.keyCode || event.which;

            // Define a regular expression pattern that matches only alphanumeric characters
            var pattern = /^[a-zA-Z0-9]+$/;

            // Check if the pressed key is an alphanumeric character or a control key (e.g., backspace)
            if (keyCode == 8 || pattern.test(String.fromCharCode(keyCode))) {
                // Allow the input
                return true;
            } else {
                // Prevent the input
                event.preventDefault();
                return false;
            }
		}

		function updateCountyId() {
			alert(document.getElementById("<%=ddlCounties.ClientID%>").value);
			var hiddenField = $("#<%=hidCountyId.ClientID%>");
			hiddenField.val(document.getElementById("<%=ddlCounties.ClientID%>").value);
			alert(hiddenField.val());
		}

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode == 110 || charCode == 190 || charCode == 46)
                return true;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function loadCounties()
        {
            var stateId = document.getElementById("<%=ddlState.ClientID%>").value;

            const urlParams = new URLSearchParams(window.location.search);
            const eventId = urlParams.get('eventId');
            if (stateId != "")
            {
                var hiddenField = $("#<%=hidCountyId.ClientID%>");
                x = 0;
                $.ajax({
                    type: "GET",
                    url: "/V1/Handlers/GetCountiesByState.ashx?eventId=" + eventId  + "&stateId=" + stateId,
                    dataType: "json",
                    success: function (data) {
                        // Populate the DropDownList with the values
                        var dropdown = $("#<%=ddlCounties.ClientID%>");
                        dropdown.empty();

                        $.each(data, function (index, item) {

                            if (x == 1) {
                                //Set the hid field to the first item returned.
                                hiddenField.val(item.id);
                            }
                            x = x + 1;

                            // Create an <option> element with both value and text attributes
                            var option = $("<option></option>");
                            option.val(item.id).text(item.text);
                            dropdown.append(option);
                        });

                        $('#<%=ddlCounties.ClientID%>').attr('disabled', false);
                    },
                    error: function () {
                        alert("Failed to retrieve data from the server.");
                    }
                });
            }
        }

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
					<%=txtVolunteerLink.UniqueID%>: {
                    url: true
                    },
					<%=txtHelpLink.UniqueID%>: {
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
					    <%=txtVolunteerHourValue.UniqueID%>: {
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

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
     <div class="text-center m-b-md" id="wizardControl">
        <a class="btn btn-default">Step 1 - Create Team</a>
        <a class="btn btn-default">Step 2 - Invite Members</a>
        <a class="btn btn-purple activeTab">Step 3 - Add Deployment</a>
        <a class="btn btn-default">Step 4 - Launch Website</a>
    </div>
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeIn">
					<div class="hpanel">
						<div class="panel-body">
						<h1>	
							<asp:Literal id="litEventName" runat="server"></asp:Literal>
						</h1>
							<h2 class="font-light m-b-xs">
								Create a Deployment
							</h2>
						</div>
                        
						<div class="form-group col-lg-12" runat="server" visible="false" id="divSelectEvent">
							<label>Choose a Disaster</label>
							<div id="div1" class="dropdown m-b-md" runat="server">
								<button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Choose The Disaster <i class="fa fa-sort-down"></i></button>
								<ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required">
									<%=disasterDropDown%>
								</ul>
							</div>
							<input type="hidden" id="hidEventId" runat="server" />
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown" runat="server" visible="false" id="divCreateCause">
		
							
			<div class="row">
				<div class="col-lg-12 container">
					<div class="alert alert-danger" runat="server" id="divMessage" visible="false">
						<i class="fa fa-bolt"></i><asp:Literal runat="server" id="lblMessage"></asp:Literal>
						<p>
						<asp:HyperLink runat="server" id="hypLinkToCampaign"></asp:HyperLink>
						</p>
					</div>
				</div>
			</div>
			<div class="row" id="divForm" runat="server">
				<div class="col-lg-12 container">
					<div class="hpanel form-horizontal">

						<div class="panel-heading hbuilt">
							Disaster Deployment
						    <div class="pull-right">
							    <small>After creating your deployment, you will be able to upload an associated 1600x600 sized image.</small>
                                <asp:Button id="btnUploadLogo" runat="server" Enabled="false" CssClass="btn btn-primary causeImageUploadButton" Text="Upload 1600 x 600 Deployment Image" />
					        </div>
                            <br /><br />
						</div>
						<div class="panel-body">

							<div class="form-group bg-success">
								<label class="col-sm-2 m-t-xs m-b-xs control-label">* State Is Required</label>
								<div class="col-sm-2 m-t-xs m-b-xs">
									<asp:DropDownList ID="ddlState" runat="server" onchange="loadCounties()" DataTextField="Name" DataValueField="StatesId" CssClass="form-control required" Required=""></asp:DropDownList>
								</div>
							</div>
							<div class="form-group bg-success">
								<label class="col-sm-2 m-t-xs m-b-xs control-label">* County Is Required</label>
								<div class="col-sm-3 m-t-xs m-b-xs">
									<asp:DropDownList ID="ddlCounties" runat="server" onchange="updateCountyId()" Enabled="false" DataTextField="Text" DataValueField="Value" CssClass="form-control required" Required=""></asp:DropDownList>
								    <asp:HiddenField ID="hidCountyId" runat="server" />
                                </div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Choose Begin Date</label>
								<div class="col-sm-2">
									<div class="input-group date">
										<input type="text" class="form-control" id="hidDeploymentBeginDate" runat="server"><span class="input-group-addon"><i class="fa fa-calendar"></i></span>
									</div>
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Choose Target End Date</label>
								<div class="col-sm-2">
									<div class="input-group date">
										<input type="text" class="form-control" id="hidDeploymentEndDate" runat="server"><span class="input-group-addon"><i class="fa fa-calendar"></i></span>
									</div>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Point of Contact First and Last Name</label>
								<div class="col-sm-5"><input type="text" runat="server" required id="txtPOCFullname" class="form-control" placeholder="Point of Contact First and Last Name"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Purpose/Mission</label>
								<div class="col-sm-5">
									<textarea id="txtPurposeMission" runat="server" class="form-control" placeholder="Purpose of this Deployment response."></textarea>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Deployment Name</label>
								<div class="col-sm-5">
									<input type="text" runat="server" id="txtCampaignName" class="form-control" placeholder="Give Your Deployment a Memorable Name">
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">URL Friendly Deployment Name
								<br />
								<small>No spaces or special characters allowed.</small></label>
								<div class="col-sm-5">
									<input type="text" runat="server" onkeypress="return validateAlphaNumericInput(event)" id="txtURLFriendlyCampaignName" class="form-control" placeholder="URLFriendlyCampaignName">
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Team Member Per Hour Value</label>
								<div class="col-sm-2 m-t-sm">
									<input type="text" runat="server" maxlength="5" id="txtVolunteerHourValue" onkeypress="return isNumberKey(event)" class="form-control" placeholder="Enter dollars and cents only. 00.00">
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Regional VOAD Member</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkVoad" class="form-control">
								</div>
							</div>
							
							<div class="form-group">
								<label class="col-sm-2 control-label">Accepts Team Members</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkAcceptsVolunteers" class="form-control">
								</div>
							</div>
							
							<div class="form-group">
								<label class="col-sm-2 control-label">Is Active</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" checked="checked" runat="server" id="chkIsActive" class="form-control">
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Team Member Instructions</label>
								<div class="col-sm-5">
									<textarea id="txtVolunteerInstructions" runat="server" class="form-control" placeholder="Team Member Instructions"></textarea>
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
								<label class="col-sm-2 control-label">Team Member Outside Registration Link (http://)</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtVolunteerLink" class="form-control" placeholder="Volunteer Link"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Help Link (http://)</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtHelpLink" class="form-control" placeholder="Help Link"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Team Website (http://)</label>
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