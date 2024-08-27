<%@ Page Title="" EnableEventValidation="false" Language="C#" ValidateRequest="false" MasterPageFile="~/V1/MasterPages/1-Column-Child.master" AutoEventWireup="true" CodeFile="RespondToEvent.aspx.cs" Inherits="V1_NonProfitAdministration_RespondToEvent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
	<script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>

    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
	<link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
	<link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />	
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

			$('#<%=txtVolunteerInstructions.ClientID%>').summernote({
				toolbar: [
					['style', ['bold', 'italic', 'underline']],
					['alignment', ['ul', 'ol', 'paragraph']]
				],

				height: 125
			});
        });

        // Function to validate alphanumeric input on keypress
        function validateAlphaNumericInput(event) {
            // Get the input element
            <%--var inputElement = document.getElementById('<%=txtURLFriendlyCampaignName.ClientID%>');--%>

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
			var hiddenField = $("#<%=hidCountyId.ClientID%>");
			hiddenField.val(document.getElementById("<%=ddlCounties.ClientID%>").value);
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
					<%=txtPhonenumber.UniqueID%>: {
						number: true,
						maxlength: 12
					},
					<%=txtPOCFullname.UniqueID%>: {
						required: true
					},
					<%=txtCampaignName.UniqueID%>: {
						required: true,
						maxlength: 1000
					}, 
					<%=txtEmailAddress.UniqueID%>: {
						email: true
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
	<style>
		.container
		{
			background-color:white !important;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="form-group col-lg-12" runat="server" id="divSelectEvent">
					<label>Choose an Event</label>
					<div id="div1" class="dropdown m-b-md" runat="server">
						<button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Choose The Event<i class="fa fa-sort-down"></i></button>
						<ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required">
							<%=disasterDropDown%>
						</ul>
					</div>
					<input type="hidden" id="hidEventId" runat="server" />
				</div>
				<h4><asp:Literal id="litEventName" runat="server"></asp:Literal></h4>
			</div>
		</div>
		<div class="content" runat="server" visible="false" id="divCreateCause">
			<div class="row" id="divForm" runat="server">
				<div class="col-sm-12 container">
					<div class="hpanel form-horizontal">

						<div class="panel-heading hbuilt">
							Event Details
						</div>
						<div class="panel-body">

							<div class="form-group">
								<label class="col-sm-3 control-label">Event Name *</label>
								<div class="col-sm-6">
									<input type="text" runat="server" id="txtCampaignName" class="form-control" placeholder="What do you want to call this event?">
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-3 control-label">Welcome Message</label>
								<div class="col-sm-6">
									<small>You can enter location instructions, reminders, a welcome message or other relevant information.</small>
									<asp:TextBox ID="txtVolunteerInstructions" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="10" ClientIDMode="Static"></asp:TextBox>
									
								</div>
							</div>
							
							<div class="form-group">
								<label class="col-sm-3 control-label">Address</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtAddress" class="form-control i-check" placeholder="Address"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-3 control-label">City</label>
								<div class="col-sm-3"><input type="text" runat="server" id="txtCity" class="form-control" placeholder="City"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-3 control-label">Zip</label>
								<div class="col-sm-2"><input type="text" runat="server" id="txtZipCode" maxlength="5" class="form-control" placeholder="Zip Code"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-3 m-t-xs m-b-xs control-label">State *</label>
								<div class="col-sm-3 m-t-xs m-b-xs">
									<asp:DropDownList ID="ddlState" runat="server" onchange="loadCounties()" DataTextField="Name" DataValueField="StatesId" CssClass="form-control required" Required=""></asp:DropDownList>
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-3 m-t-xs m-b-xs control-label">County *</label>
								<div class="col-sm-3 m-t-xs m-b-xs">
									<asp:DropDownList ID="ddlCounties" runat="server" onchange="updateCountyId()" Enabled="false" DataTextField="Text" DataValueField="Value" CssClass="form-control required" Required=""></asp:DropDownList>
								    <asp:HiddenField ID="hidCountyId" runat="server" />
                                </div>
							</div>

<%--							<div class="form-group">
								<label class="col-sm-3 control-label">Choose Begin Date</label>
								<div class="col-sm-3">
									<div class="input-group date">
										<input type="text" class="form-control" id="hidDeploymentBeginDate" runat="server"><span class="input-group-addon"><i class="fa fa-calendar"></i></span>
									</div>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-3 control-label">Choose Target End Date</label>
								<div class="col-sm-3">
									<div class="input-group date">
										<input type="text" class="form-control" id="hidDeploymentEndDate" runat="server"><span class="input-group-addon"><i class="fa fa-calendar"></i></span>
									</div>
								</div>
							</div>--%>

<%--							<div class="form-group">
								<label class="col-sm-3 control-label">Event Short Name
								<br />
								<small>No spaces or special characters allowed.</small></label>
								<div class="col-sm-5">
									<input type="text" runat="server" onkeypress="return validateAlphaNumericInput(event)" id="txtURLFriendlyCampaignName" class="form-control" placeholder="URLFriendlyCampaignName">
								</div>
							</div>--%>

<%--							<div class="form-group">
								<label class="col-sm-3 control-label">Team Member Per Hour Value</label>
								<div class="col-sm-3 m-t-sm">
									<input type="text" runat="server" maxlength="5" id="txtVolunteerHourValue" onkeypress="return isNumberKey(event)" class="form-control" placeholder="Enter dollars and cents only. 00.00">
								</div>
							</div>--%>

							<div class="form-group">
								<label class="col-sm-3 control-label">VOAD Participant</label>
								<div class="col-sm-5 m-t-sm">
									<input type="checkbox" runat="server" id="chkVoad" class="form-control">
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>

			<div class="row" id="div2" runat="server">
				<div class="col-xs-12 container">
					<div class="hpanel form-horizontal m-t-lg">
						<div class="panel-heading hbuilt">
							Location and Contact Information
						</div>
						<div class="panel-body">

							<div class="form-group">
								<label class="col-sm-3 control-label">Contact Person *</label>
								<div class="col-sm-5"><input type="text" runat="server" required id="txtPOCFullname" class="form-control" placeholder="Point of Contact First and Last Name"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-3 control-label">Phone Number *</label>
								<div class="col-sm-5"><input type="text" maxlength="10" required runat="server" id="txtPhonenumber" class="form-control" placeholder="Primary Phone"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-3 control-label">Email Address *</label>
								<div class="col-sm-5"><input type="text" runat="server" required id="txtEmailAddress" class="form-control" placeholder="Public Email Address"></div>
							</div>

						</div>
					</div>

					<div class="form-group">
						<div class="col-sm-2" style="padding:10px;">
							<div class="pull-left">
								<asp:LinkButton id="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-lg btn-default" Text="Cancel" />
							</div>
						</div>
						<div class="col-sm-8" style=""></div>
						<div class="col-sm-2" style="padding:10px; padding-right:20px;">
							<div class="pull-right">
								<asp:Button id="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-lg btn-primary" Text="Next Step >" />
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
</asp:Content>