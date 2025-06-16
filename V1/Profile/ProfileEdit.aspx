<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true"
    CodeFile="ProfileEdit.aspx.cs" Inherits="V1_ProfileEdit" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
    <script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
    <script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>
    <script>
        $(document).ready(function () {
            $(function () {

                $("#form").validate({
                    rules: {
                        password: {
                            required: true,
                            minlength: 3
                        },
                        url: {
                            required: true,
                            url: true
                        },
                        number: {
                            required: true,
                            number: true
                        },
                        max: {
                            required: true,
                            maxlength: 4
                        }
                    },
                    submitHandler: function (form) {
                        form.submit();
                    }
                });
            });
    </script>
    <script type="text/javascript">
            var address;
            var city;
            var state;
            var zip;
            var lookupComplete = false;
            var IsDuplicateClear = true;

            function CheckDuplicate(controlName, sender) {
                if (sender.value) {
                    $.ajax({
                        type: "GET",
                        url: "/V1/Handlers/GetDuplicateUserDetails.ashx?control=" + controlName + "&value=" + sender.value,
                        contentType: "text/plain; charset=utf-8",
                        dataType: "html",
                        success: function (data) {
                            if (data == "Yes") {
                                IsDuplicateClear = false;
                                if (controlName == "Email") {
                                    $(".error-message-email").show();
                                    $(".response-message-email").hide();
                                } else {
                                    $(".error-message-username").show();
                                    $(".response-message-username").hide();
                                }
                                $("#<%=btnSubmit.ClientID%>").attr("disabled", true);
                            } else {
                                IsDuplicateClear = true;
                                if (controlName == "Email") {
                                    $(".error-message-email").hide();
                                    $(".response-message-email").show();
                                } else {
                                    $(".error-message-username").hide();
                                    $(".response-message-username").show();
                                }
                                if (lookupComplete) {
                                    $("#<%=btnSubmit.ClientID%>").attr("disabled", false);
                                }
                            }
                        },
                        error: function () {
                            if (controlName == "Email") {
                                $(".error-message-email").text("An error occurred. Please try again.").show();
                            } else {
                                $(".error-message-username").text("An error occurred. Please try again.").show();
                            }
                            $("#<%=btnSubmit.ClientID%>").attr("disabled", true);
                        }
                    });
                } else {
                    $(".error-message-username, .response-message-username, .error-message-email, .response-message-email").hide();
                    $("#<%=btnSubmit.ClientID%>").attr("disabled", true);
                }
            }

            function CheckAddressValues(controlName, sender) {
                var currentAddress = $("#<%=txtAddress.ClientID%>").val();
                var currentCity = $("#<%=txtCity.ClientID%>").val();
                var currentState = $("#<%=ddlState.ClientID%>").val();
                var currentZip = $("#<%=txtZipCode.ClientID%>").val();

                address = currentAddress;
                city = currentCity;
                state = currentState;
                zip = currentZip;

                if (address && city && state && zip && !lookupComplete) {
                    $('#divAddressMessage').show();
                    SetLatitudeLongitude(address + " " + city + ", " + state + " " + zip);
                }
            }


            function SetLatitudeLongitude(address) {
                $.ajax({
                    type: "GET",
                    url: "/V1/Handlers/GetGoogleAddressInfo.ashx?address=" + encodeURIComponent(address),
                    contentType: "text/plain; charset=utf-8",
                    dataType: "html",
                    success: function (data) {
                        if (data != "") {
                            var results = data.split("|");
                            var isPartialMatch = results[0];
                            var duplicate = results[12];

                            if ((isPartialMatch.toLowerCase() === 'false') && (duplicate.toLowerCase() === 'false')) {
                                $("#divMapMessage").addClass("alert-success").removeClass("alert-danger");
                                $("#iFontAwesome").removeClass("fa-warning").addClass("fa-map-marker");
                                var successMessage = "Address Lookup Successful!";
                                $("#<%=hidAddressData.ClientID%>").val(data);
                                $('#<%=lblAddressMessage.ClientID%>').text(successMessage);
                                lookupComplete = true;
                                if (IsDuplicateClear) {
                                    $("#<%=btnSubmit.ClientID%>").attr("disabled", false);
                                }
                            } else if ((isPartialMatch.toLowerCase() === 'false') && (duplicate.toLowerCase() === 'false')) {
                                $("#divMapMessage").removeClass("alert-success").addClass("alert-danger");
                                $("#iFontAwesome").addClass("fa-warning").removeClass("fa-map-marker");
                                $("#<%=hidAddressData.ClientID%>").val(data);
                                lookupComplete = false;
                                var errorMessage = "This address already exists (" + address + "). Press 'Next' to edit in the Stability Location Manager. Web Service Message: " + data;
                                $('#<%=lblAddressMessage.ClientID%>').text(errorMessage);
                                if (IsDuplicateClear) {
                                    $("#<%=btnSubmit.ClientID%>").attr("disabled", false);
                                }
                            } else {
                                $("#divMapMessage").removeClass("alert-success").addClass("alert-danger");
                                $("#iFontAwesome").addClass("fa-warning").removeClass("fa-map-marker");
                                lookupComplete = false;
                                var errorMessage = "Please check your address. Google returned an error matching the address you provided. (" + address + ") Web Service Message: " + data;
                                $('#<%=lblAddressMessage.ClientID%>').text(errorMessage);
                                $("#<%=btnSubmit.ClientID%>").attr("disabled", true);
                            }
                        }
                    },
                    error: function (request, status, error) {
                        $("#divMapMessage").removeClass("alert-success").addClass("alert-danger");
                        $("#iFontAwesome").addClass("fa-warning").removeClass("fa-map-marker");
                        lookupComplete = false;
                        $('#<%=lblAddressMessage.ClientID%>').text("Error retrieving address information from Google. " + request.statusText + ' - ' + error + ' - ' + status);
                $("#<%=btnSubmit.ClientID%>").attr("disabled", true);
                    }
                });
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
                <div class="hpanel">
                    <div class="panel-body">
                        <h2 class="font-light m-b-xs">Edit Your Profile
                        </h2>

                        <div class="hpanel hgreen" id="divEditProfile" runat="server">
                            <div class="panel-heading hbuilt">
                                Update My Personal Information
                            </div>
                            <div class="panel-body">
                                <asp:HyperLink runat="server" ID="HyperLink4" Text="Change Password" NavigateUrl="EditPassword.aspx"></asp:HyperLink>
                                | 
									<asp:HyperLink runat="server" ID="HyperLink3" Text="Change Email" NavigateUrl="EditEmail.aspx"></asp:HyperLink>
                                | 
									<asp:HyperLink runat="server" ID="HyperLink5" Text="Change Username" NavigateUrl="Username.aspx"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
        <div class="row">
            <div class="col-lg-6 container">
                <div class="hpanel form-horizontal">

                    <div class="panel-heading hbuilt">
                        Personal Information
                    </div>
                    <div class="panel-body">

                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                First Name <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                            <div class="col-sm-9">
                                <input type="text" required runat="server" id="txtFirstname" class="form-control"
                                    placeholder="First Name">
                            </div>
                        </div>

                        <!-- Last Name -->
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Last Name <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                            <div class="col-sm-9">
                                <input type="text" required runat="server" id="txtLastname" class="form-control"
                                    placeholder="Last Name">
                            </div>
                        </div>

                        <!-- Address -->
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Address <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtAddress" runat="server" onblur="CheckAddressValues('address', this)"
                                    CssClass="form-control" required="" placeholder="Address"></asp:TextBox>
                            </div>
                        </div>

                        <!-- City -->
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                City <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtCity" runat="server" onblur="CheckAddressValues('city', this)"
                                    CssClass="form-control" required="" placeholder="City"></asp:TextBox>
                            </div>
                        </div>

                        <!-- State -->
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                State <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlState" runat="server" DataTextField="Text" DataValueField="Value"
                                    CssClass="form-control" required="" onchange="CheckAddressValues('state', this);">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <!-- Zip -->
                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Zip <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtZipCode" runat="server" CssClass="form-control" onblur="CheckAddressValues('zip', this)"
                                    required="" placeholder="Zip Code"></asp:TextBox>
                            </div>
                        </div>
                        <div id="divAddressMessage" class="form-group col-md-12" style="display: none;">
                            <div id="divMapMessage" class="alert p-sm d-flex align-items-center" style="border-radius: 6px;">
                                <i id="iFontAwesome" class="fa me-2 fs-5"></i>
                                <asp:Label runat="server" ID="lblAddressMessage" CssClass="mb-0"></asp:Label>
                                <asp:HiddenField ID="hidAddressData" runat="server"></asp:HiddenField>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="hpanel form-horizontal">
                    <div class="panel-heading hbuilt">
                        Contact Information
                    </div>
                    <div class="panel-body">


                        <div runat="server" id="divMessage" visible="false">
                            <i class="fa fa-bolt"></i>
                            <asp:Literal runat="server" ID="lblMessage"></asp:Literal>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Phone Number <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                            <div class="col-sm-9">
                                <input type="text" maxlength="10" required runat="server" id="txtPhonenumber" class="form-control"
                                    placeholder="Phone Number" name="number">
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-3 control-label">
                                Zello Handle <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                            <div class="col-sm-9">
                                <input type="text" runat="server" id="txtZello" class="form-control" placeholder="Zello Handle">
                            </div>
                        </div>



                    </div>
                </div>
                <div class="hpanel form-horizontal">
                    <div class="panel-heading hbuilt">
                        Notification Information
                    </div>
                    <div class="panel-body">


                        <div runat="server" id="div1" visible="false">
                            <i class="fa fa-bolt"></i>
                            <asp:Literal runat="server" ID="Literal1"></asp:Literal>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Notifications</label>
                            <div class="col-sm-9">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" runat="server" id="receiveSMS" class="form-group">
                                        Receive SMS Notifications
                                    </label>
                                </div>
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" runat="server" id="receiveEmail" class="form-group">
                                        Receive Email Notifications
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-lg-6 container">
                <div class="hpanel form-horizontal">
                    <div class="panel-heading hbuilt">
                        Volunteer Information
                    </div>
                    <div class="panel-body">

                        <div class="form-group">
                            <label class="col-sm-3 control-label">Volunteer Title</label>
                            <div class="col-sm-9">
                                <input type="text" runat="server" id="txtTitle" class="form-control" placeholder="Your title.">
                                <span class="help-block m-b-none">Your title can be assigned by your supervisor.</span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-3 control-label">Dates Available</label>
                            <div class="col-sm-9">
                                <input type="text" runat="server" id="txtDatesAvailable" class="form-control" placeholder="Specific dates you would like to volunteer.">
                                <span class="help-block m-b-none">Specific dates you would like to volunteer.</span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-3 control-label">Number of Days Available</label>
                            <div class="col-sm-9">
                                <input type="text" runat="server" id="txtNumberOfDays" class="form-control" placeholder="Enter the number of days you can volunteer.">
                                <span class="help-block m-b-none">Enter the number of days you can volunteer.</span>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-3 control-label">Volunteer Description</label>
                            <div class="col-sm-9">
                                <textarea runat="server" id="txtVolunteerDescription" rows="10" class="form-control"
                                    placeholder="Describe your background, special skills and generally how you think you can help."></textarea>
                                <span class="help-block m-b-none">Describe your background, special skills and generally
                                    how you think you can help.</span>

                            </div>
                        </div>

                    </div>
                </div>

                <div class="form-group">
                    <div class="pull-right">
                        <asp:LinkButton ID="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel"
                            CssClass="btn btn-default" Text="Cancel" />
                        <asp:Button ID="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary"
                            Text="Save Changes" />
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
