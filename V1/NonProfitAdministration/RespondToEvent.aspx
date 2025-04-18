<%@ Page Title="" EnableEventValidation="false" Language="C#" ValidateRequest="false" MasterPageFile="~/V1/MasterPages/1-Column-Child.master" AutoEventWireup="true" CodeFile="RespondToEvent.aspx.cs" Inherits="V1_NonProfitAdministration_RespondToEvent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="/Homer/vendor/iCheck/icheck.min.js"></script>
    <script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
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

<%--		function updateCountyId() {
			var hiddenField = $("#<%=hidCountyId.ClientID%>");
			hiddenField.val(document.getElementById("<%=ddlCounties.ClientID%>").value);
		}--%>

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode == 110 || charCode == 190 || charCode == 46)
                return true;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

 <%--       function loadCounties()
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
        }--%>

        $(function () {

            $("#form1").validate({
                rules: {
					<%=txtAddress1.UniqueID%>: {
                required: true
            },
					<%=txtCity1.UniqueID%>: {
                    required: true
                },
					<%=txtZip1.UniqueID%>: {
                    required: true
                },
					<%=txtPOCFullname.UniqueID%>: {
                    required: true,
                    maxlength: 100
                },
					<%=txtPhonenumber.UniqueID%>: {
                    number: true,
                    maxlength: 12
                },
					<%=txtURLFriendlyCampaignName.UniqueID%>: {
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


        function checkUniqueName(name) {
            if (name.trim() === '') {
                // If the textbox is empty, clear the status message
                $('#urlFriendlyNameStatus').text('');
                return;
            }

            $.ajax({
                type: "POST",
                url: "/V1/NonProfitAdministration/RespondToEvent.aspx/IsURLFriendlyNameUnique", // Replace with your ASP.NET page name
                data: JSON.stringify({ urlFriendlyName: name }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        // The name is unique
                        $('#urlFriendlyNameStatus').text('This name is available.').css('color', 'green');
                    } else {
                        // The name is not unique
                        $('#urlFriendlyNameStatus').text('This name is already taken.').css('color', 'red');
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error: " + error);
                }
            });
        }

    </script>
    <style>
    	.container {
    		background-color: white !important;
    	}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="form-group col-lg-12" runat="server" id="divSelectEvent">
                <label>Choose an Event/Disaster</label>
                <div id="div1" class="dropdown m-b-md" runat="server">
                    <button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Choose The Event/Disaster<i class="fa fa-sort-down"></i></button>
                    <ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required">
                        <%=disasterDropDown%>
                    </ul>
                </div>
                <input type="hidden" id="hidEventId" runat="server" />
            </div>
            <h4>
                <asp:Literal ID="litEventName" runat="server"></asp:Literal></h4>
        </div>
    </div>
    <div class="content" runat="server" visible="false" id="divCreateCause">
        <div class="row" id="divForm" runat="server">
            <div class="col-sm-12 container">
                <div class="hpanel form-horizontal">
                    Creating a deployment allows you to add your team, raise money, track your hours, share your impact and more.
						<br />
                    <br />
                    Note, you will need to enter an exact address for your deployment. This can be modified later so if you're not yet sure where you will deploy just enter the nearest reasonable address you can find.
						<div class="panel-heading hbuilt">
                            Create a New Deployment
                        </div>
                    <div class="panel-body">

                        <div class="form-group">
                            <label class="col-sm-3 control-label">Deployment Name *</label>
                            <div class="col-sm-6">
                                <input type="text" runat="server" id="txtCampaignName" class="form-control" placeholder="What do you want to call this deployment?">
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-3 control-label">URL Friendly Deployment Name *</label>
                            <div class="col-sm-6">
                                <input type="text" runat="server" id="txtURLFriendlyCampaignName" class="form-control" placeholder="URL Friendly Name" oninput="checkUniqueName(this.value)">
                                <span id="urlFriendlyNameStatus"></span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-3 control-label">Welcome Message</label>
                            <div class="col-sm-6">
                                <small>You can enter location instructions, reminders, a welcome message or other relevant information.</small>
                                <asp:TextBox ID="txtVolunteerInstructions" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="10" ClientIDMode="Static"></asp:TextBox>

                            </div>
                        </div>








                        <script>
                            var address;
                            var city;
                            var state;
                            var zip;
                            var lookupComplete = false;
                            $(document).ready(function () {
                                $('#divAddressMessage').hide();
                                $('#divMessage').hide();
									<%--$("#<%=btnSubmit.ClientID%>").attr("disabled", true);--%>
                            });

                            function CheckAddressValues(controlName, sender) {

                                switch (controlName) {
                                    case "address":
                                        if (sender.value) {
                                            address = sender.value;
                                        }
                                        break;
                                    case "city":
                                        if (sender.value) {
                                            city = sender.value;
                                        }
                                        break;
                                    case "state":
                                        if (sender.value) {
                                            state = sender.value;
                                        }
                                        break;
                                    case "zip":
                                        if (sender.value) {
                                            zip = sender.value;
                                        }
                                        break;
                                    default:
                                    // code block
                                }

                                if ((address) && (city) && (state) && (zip) && (lookupComplete == false)) {
                                    $('#divAddressMessage').show();
                                    //alert(address + " " + city + ", " + state + " " + zip);
                                    $('#<%=lblMessage.ClientID%>').text(" Retrieving latitude and longitude...");
                                    SetLatitudeLongitude('<%=userId%>', address + " " + city + ", " + state + " " + zip);
                                }
                            }

                            function SetLatitudeLongitude(userId, address) {
                                $.ajax(
                                    {
                                        type: "GET",
                                        url: "/V1/Handlers/InsertDeploymentAddress.ashx?userId=" + userId + "&address=" + address,
                                        contentType: "text/plain; charset=utf-8",
                                        dataType: "html",
                                        success: function (data) {
                                            if (data != "") {
                                                var results = data.split("|");
                                                var isPartialMatch = results[0];
                                                var duplicate = results[12];
                                                if ((isPartialMatch == 'False' || isPartialMatch == 'false') && (duplicate == 'False' || duplicate == 'false')) {
                                                    var latitude = results[1];
                                                    var longitude = results[2];
                                                    var street_number = results[3];
                                                    var street = results[4];
                                                    var city = results[5];
                                                    var state = results[6];
                                                    var country = results[7];
                                                    var postal_code = results[8];
                                                    var county = results[9];
                                                    var googlePlaceId = results[10];
                                                    var formattedAddress = results[11];
                                                    var addressId = results[13];

                                                    $("#divMapMessage").addClass("alert-success");
                                                    $("#divMapMessage").removeClass("alert-danger");
                                                    $("#iFontAwesome").removeClass("fa-warning");
                                                    $("#iFontAwesome").addClass("fa-map-marker");
                                                    var successMessage = " We found your address and returned the following information. (" + data + ")";
                                                    $("#<%=hidAddressData.ClientID%>").val(data);
                                                    $('#<%=lblAddressMessage.ClientID%>').text(successMessage);
                                                        lookupComplete = true;
														<%--$("#<%=btnSubmit.ClientID%>").attr("disabled", false);--%>
                                                    }
                                                    else if (duplicate == 'True' || duplicate == 'true') {
                                                        //Address already exists.
                                                        $("#divMapMessage").removeClass("alert-success");
                                                        $("#divMapMessage").addClass("alert-danger");
                                                        $("#iFontAwesome").addClass("fa-warning");
                                                        $("#iFontAwesome").removeClass("fa-map-marker");
                                                        $("#<%=hidAddressData.ClientID%>").val(data);
                                                        lookupComplete = false;
                                                        var errorMessage = " This address already exists (" + address + "). Press 'Next' to edit in the Stability Location Manager. Web Service Message: " + data;
                                                        $('#<%=lblAddressMessage.ClientID%>').text(errorMessage);
														<%--$("#<%=btnSubmit.ClientID%>").attr("disabled", false);--%>
                                                    }
                                                    else {
                                                        //Error getting the information
                                                        $("#divMapMessage").removeClass("alert-success");
                                                        $("#divMapMessage").addClass("alert-danger");
                                                        $("#iFontAwesome").addClass("fa-warning");
                                                        $("#iFontAwesome").removeClass("fa-map-marker");
                                                        lookupComplete = false;
                                                        var errorMessage = " Please check your address. Google returned an error matching the address you provided. (" + address + ") Web Service Message: " + data;
                                                        $('#<%=lblAddressMessage.ClientID%>').text(errorMessage);
														<%--$("#<%=btnSubmit.ClientID%>").attr("disabled", true);--%>
                                                    }
                                                }
                                            },
                                            error: function (request, status, error) {
                                                $("#divMapMessage").removeClass("alert-success");
                                                $("#divMapMessage").addClass("alert-danger");
                                                $("#iFontAwesome").addClass("fa-warning");
                                                $("#iFontAwesome").removeClass("fa-map-marker");
                                                lookupComplete = false;
                                                $('#<%=lblAddressMessage.ClientID%>').text(" Error retrieving address information from Google. " + request.statusText + ' - ' + error + ' - ' + status);
												<%--$("#<%=btnSubmit.ClientID%>").attr("disabled", true);--%>
                                            }
                                        });
                            }
                        </script>
                        <div class="row">
                            <div class="col-lg-12 container">
                                <div class="hpanel form-horizontal">

                                    <div class="panel-heading hbuilt">
                                        Enter a deployment destination address and it will be verified to be placed on the deployment map.
                                    </div>
                                    <div class="panel-body">

                                        <div id="divMessage" class="alert alert-success m-b-lg">
                                            <i class="fa fa-bolt"></i>
                                            <asp:Label runat="server" ID="lblMessage"></asp:Label>
                                        </div>

                                        <div id="divAddressMessage" class="form-group">
                                            <label class="col-sm-2 control-label">Google Address Details</label>
                                            <div id="divMapMessage" class="alert m-b-lg p-sm col-sm-5">
                                                <i id="iFontAwesome" class="fa"></i>
                                                <asp:Label runat="server" ID="lblAddressMessage"></asp:Label>
                                                <asp:HiddenField ID="hidAddressData" runat="server"></asp:HiddenField>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">Address</label>
                                            <div class="col-sm-5">
                                                <input id="txtAddress1" type="text" onblur="CheckAddressValues('address', this)" required runat="server" class="form-control i-check" placeholder="Address"></div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">City</label>
                                            <div class="col-sm-5">
                                                <input id="txtCity1" type="text" onblur="CheckAddressValues('city', this)" required runat="server" class="form-control" placeholder="City"></div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">State</label>
                                            <div class="col-sm-5">
                                                <asp:DropDownList ID="ddlState1" onblur="CheckAddressValues('state', this)" runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control" required=""></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">Zip</label>
                                            <div class="col-sm-5">
                                                <input id="txtZip1" type="text" onblur="CheckAddressValues('zip', this)" required runat="server" class="form-control" placeholder="Zip Code"></div>
                                        </div>
                                        <%--<div class="form-group">
												<label class="col-sm-2 control-label"></label>
												<div class="col-sm-5">
													<div class="pull-right">
														<asp:LinkButton id="btnCancel1" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default" Text="Cancel" />
														<asp:Button id="Button1" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Next" />
													</div>
												</div>
											</div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>






                        <%--							<div class="form-group">
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
							</div>--%>

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
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Send auto SMS notifications to team members once deployment is finished?</label>
                            <div class="col-sm-5 m-t-sm">
                                <input type="checkbox" runat="server" id="chkNotification" class="form-control">
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
                            <div class="col-sm-5">
                                <input type="text" runat="server" required id="txtPOCFullname" class="form-control" placeholder="Point of Contact First and Last Name"></div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-3 control-label">Phone Number *</label>
                            <div class="col-sm-5">
                                <input type="text" maxlength="10" required runat="server" id="txtPhonenumber" class="form-control" placeholder="Primary Phone"></div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-3 control-label">Email Address *</label>
                            <div class="col-sm-5">
                                <input type="text" runat="server" required id="txtEmailAddress" class="form-control" placeholder="Public Email Address"></div>
                        </div>

                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-2" style="padding: 10px;">
                        <div class="pull-left">
                            <asp:LinkButton ID="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-lg btn-default" Text="Cancel" />
                        </div>
                    </div>
                    <div class="col-sm-8" style=""></div>
                    <div class="col-sm-2" style="padding: 10px; padding-right: 20px;">
                        <div class="pull-right">
                            <asp:Button ID="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-lg btn-primary" Text="Next Step >" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
    <script>
        document.getElementById('<%=txtCampaignName.ClientID%>').addEventListener('input', function () {

            // Get the current value from the input text box
            const inputText = this.value;

            // Filter out non-alphanumeric characters and spaces
            const filteredText = inputText.replace(/[^a-zA-Z0-9]/g, '');

            // Set the filtered text to the output text box
            document.getElementById('<%=txtURLFriendlyCampaignName.ClientID%>').value = filteredText;

            checkUniqueName(filteredText);
        });
    </script>
</asp:Content>
