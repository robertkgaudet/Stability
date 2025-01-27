<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DonationDetails.aspx.cs" Inherits="V1_NonProfit_DonationDetails" %>




<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Donation Page</title>
    <script type="text/javascript" src="../Scripts/JCrop/jquery.min.js"></script>
    <link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
    <link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
    <link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
    <link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
    <script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>
    <script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>
    <script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
    <link href="../Styles/DonationStyles.css" rel="stylesheet" />
</head>
<body>

    <div class="header">
        <asp:Image ImageUrl="https://res.cloudinary.com/resilia/image/upload/v1/NonprofitOrgRoot/1960-c6a36c6a-d152-4120-8041-7d936d0d4df5/logos/t2vtpy7jwzafgjwf3u6f?_a=AJFJtWI0" alt="logo" class="logo" runat="server" />
        <h4 class="title">Ground Force Humanitarian Aid, Formerly Cajun Navy Ground Force</h4>
    </div>


   

        <div style="display: flex; flex-wrap: wrap;">
            <div class="content left">
                <div style="padding: 40px">
                    <%--<asp:Image ID="donateimg" class="leftImg" src="https://res.cloudinary.com/resilia/image/upload/v1/NonprofitOrgRoot/1960-c6a36c6a-d152-4120-8041-7d936d0d4df5/story_images/umseeehmroqxwuphbss1?_a=AJFJtWI0" alt="Relief Image" runat="server" />
                    <h1>Donate to North Carolina Relief: Help Now</h1>
                    <p>Ground Force Humanitarian Aid (formerly Cajun Navy Ground Force) is assisting for the long term in North Carolina after Hurricane Helene devastated so many.</p>
                    <p><strong>We turn every $1 you give into $12.</strong></p>
                    <p>With your support, we've had an incredibly efficient track record of data-driven disaster relief and humanitarian aid, providing critical support to communities in crisis. Your donation today will enable us to expand our relief efforts, reaching more vulnerable citizens and sustaining our presence in the impacted areas for longer periods.</p>
                    <p>Every contribution makes a difference - please donate now to support our life-saving work and help the most vulnerable recover from this devastating storm.</p>--%>
                    <%=LiteralDescription%>
                </div>
            </div>
            <div class="content right">
                <form id="donationform" runat="server">
                    <div class="row">
                        <div class="col-12 mb-20">
                            <h2>Donation</h2>
                        </div>
                        <div class="col-12">
                            <span class="legend">Amount</span>
                            <input type="hidden" id="organizationId" runat="server" value="" />
                            <input type="hidden" id="publishKey" runat="server" value="" />
                        </div>
                        <div class="col-12">

                            <div class="containerLabel">
                                <% for (int i = 0; i < LiteralAmountList.Count(); i++)
                                    { %>
                                <% if (i == 0)
                                    { %>
                                <label class="item selected" for="amount<%= i + 1 %>">
                                    $<%= LiteralAmountList[i] %>
                                    <input type="radio" id="amount<%= i + 1 %>" name="Amount" value="<%= LiteralAmountList[i] %>
                                                " />
                                </label>
                                <% }
                                    else
                                    { %>
                                <label class="item" for="amount<%= i + 1 %>">
                                    $<%= LiteralAmountList[i] %>
                                    <input type="radio" id="amount<%= i + 1 %>" name="Amount" value="<%= LiteralAmountList[i] %>
                                                " /></label>
                                <% } %>
                                <% } %>
                                <%--<label class="item selected" for="amount1">$100<input type="radio" id="amount1" name="Amount" value="100" /></label>
                                <label class="item" for="amount2">$50<input type="radio" id="amount2" name="Amount" value="50" /></label>
                                <label class="item" for="amount3">$25<input type="radio" id="amount3" name="Amount" checked="checked" value="25" /></label>--%>
                                <label class="item" for="amount<%=LiteralAmountList.Count + 1 %>">Other<input type="radio" id="amount<%=LiteralAmountList.Count + 1 %>" name="Amount" value="0" /></label>
                            </div>
                        </div>
                        <div class="col-6 donatAmn">
                            <div class="input-group">
                                <span class="legend" for="DonationAmount">Donation Amount</span><span style="color: red">*</span>
                                <input type="number" id="DonationAmount" class="formInput" name="DonationAmount" value="<%=LiteralAmountList[0] %>" required="required" />
                            </div>
                        </div>
                        <div class="col-6 donatAmn">
                        </div>
                        <div class="col-12"><span class="legend" for="firstname">Frequency</span></div>
                        <div class="col-12">
                            <div class="containerLabel">
                                <label class="item freqLabel selected" for="Frequency1">
                                    One Time
                                    <input type="radio" id="Frequency1" checked="checked" name="Frequency" value="1" />
                                </label>
                                <label class="item freqLabel" for="Frequency2">
                                    Monthly
                                    <input type="radio" id="Frequency2" name="Frequency" value="2" />
                                </label>
                            </div>
                        </div>
                        <div class="col-12">
                            <input type="checkbox" id="coverfee" name="CoverFee" runat="server" style="margin-right: 5px" />
                            <span for="coverfee">Yes, I'd like to cover the $<span id="coverFeeAmount"><%= LiteralAmountList[0]*0.06 %></span> transaction fee</span>
                        </div>
                        <div class="col-12 mb-20">
                                <h2 class="mr-3">Donation Details <a id="btnLogin" runat="server" href="/V1/Login.aspx?returnUrl=<%=HttpUtility.UrlEncode(Request.Url.ToString()) %>">Already Registered? Login</a></h2>
                                
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="firstname">First Name:</span><span style="color: red"></span>
                                <input class="form-control formInput " type="text" id="firstname" runat="server" name="FirstName" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="lastname">Last Name:</span><span style="color: red"></span>
                                <input class="formInput" type="text" id="lastname" runat="server" name="LastName" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="email">Your Email Address:</span><span style="color: red"></span>
                                <input class="formInput" type="email" id="email" runat="server" name="Email" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="form-group m-b-lg border-bottom">
                                <div class="input-group m-b-md">
                                    <span class="legend" for="dateofbirth">Date Of Birth:</span><span style="color: red"></span>
                                    <div class="input-group date">
                                        <input type="text" class="formInput " id="dateOfBirth" runat="server" /><span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="homeaddress">Home Address:</span>
                                <input class="formInput" type="text" id="homeaddress" runat="server" name="HomeAddress" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="city">City:</span>
                                <input class="formInput" type="text" id="txtCity" runat="server" name="City" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="zip">State:</span>
                                <input class="formInput" id="ddlState" runat="server" onkeypress="return isNumberKey(event)" maxlength="5" name="txtState" type="text" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="zip">Zip:</span>
                                <input class="formInput" id="txtZip" runat="server" onkeypress="return isNumberKey(event)" maxlength="5" name="txtZip" type="text" />
                            </div>
                        </div>

                        <div class="col-12">
                            <input type="checkbox" id="notShareName" runat="server" name="NotShareName" style="margin-right: 5px" />
                            <span for="notShareName">Don't share my name publicly as part of this campaign</span>
                        </div>
                        <div class="col-12">
                            <input type="checkbox" id="honordonation" runat="server" name="IsHonorDonation" />
                            <span for="honor-donation">This is a donation in someone's honor</span>
                        </div>
                        <div class="col-12 showHonor">
                            <h2>In Honor Of</h2>
                        </div>
                        <div class="col-6 showHonor">
                            <div class="input-group">
                                <span class="legend" for="firstnameHonoree">Honoree First Name:</span><span style="color: red"></span>
                                <input class="formInput" type="text" id="firstnameHonoree" runat="server" name="HonoreeFirstName" required="required" />
                            </div>
                        </div>
                        <div class="col-6 showHonor">
                            <div class="input-group">
                                <span class="legend" for="lastnameHonoree">Honoree Last Name:</span><span style="color: red"></span>
                                <input class="formInput" type="text" id="lastnameHonoree" runat="server" name="HonoreeLastName" required="required" />
                            </div>
                        </div>
                        <div class="col-6 showHonor">
                            <div class="input-group">
                                <span class="legend" for="emailHonoree">Honoree Email Address:</span><span style="color: red"></span>
                                <input class="formInput" type="email" id="emailHonoree" runat="server" name="HonoreeEmail" required="required" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="donationNote">Donation Note:</span>
                                <textarea rows="3" id="donationNote" runat="server" name="DonationNote"></textarea>
                            </div>
                        </div>
                        <div class="col-12" style="display: flex; justify-content: end;">

                            <button type="submit" class="btn submit-btn">Select Payment</button>
                        </div>

                    </div>
                </form>
            </div>

        </div>
    


    <script src="https://js.stripe.com/v3/"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(function () {
                $('.input-group.date').datepicker({
                    autoclose: true,
                });
            });
            $("#honordonation").on('click', function () {
                if ($(this).prop("checked")) {
                    $('.showHonor').attr("style", "display:block");
                } else {
                    $('.showHonor').attr("style", "display:none");
                }
            });
            $(".form-select").select2();
            $("[name='Frequency']").on('click', function () {
                $(".freqLabel").removeClass("selected");
                $(this).closest("label").addClass("selected");
            });
            $('input[type="radio"][name="Amount"]').on('click', function () {
                $(".item:not(.freqLabel)").removeClass("selected");
                $(this).closest("label").addClass("selected");
                var selectedValue = $(this).val();
                if (selectedValue == "0") {
                    $('#DonationAmount').val('');
                    $('.donatAmn').show();
                    $('#coverFeeAmount').text('--');
                } else {
                    $('#DonationAmount').val(selectedValue);
                    $('.donatAmn').hide();
                    $('#coverFeeAmount').text((parseFloat(selectedValue) * 0.06).toFixed(2));
                }
            });
            $('#DonationAmount').on('keyup', function () {
                if ($('#DonationAmount').val() == '') {
                    $('#coverFeeAmount').text('--');
                } else {
                    $('#coverFeeAmount').text((parseFloat($('#DonationAmount').val()) * 0.06).toFixed(2));
                }
            });
        });

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode == 110 || charCode == 190 || charCode == 46)
                return true;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

    <script>
        $(document).ready(function () {
            $("#donationform").validate({
                rules: {
                    DonationAmount: {
                        required: true,
                        number: true,
                        min: 1, // Minimum donation amount
                    },
                    Frequency: {
                        required: true, // Radio button group
                    },
                    FirstName: {
                        minlength: 2, // Minimum 2 characters
                    },
                    LastName: {
                        minlength: 2, // Minimum 2 characters
                    },
                    Email: {
                        email: true, // Must be a valid email
                    },
                    dateOfBirth: {
                        date: true, // Must be a valid date
                    },
                    HonoreeFirstName: {
                        required: "#honordonation:checked", // Required only if honor donation checkbox is checked
                        minlength: 2,
                    },
                    HonoreeLastName: {
                        required: "#honordonation:checked",
                        minlength: 2,
                    },
                    HonoreeEmail: {
                        required: "#honordonation:checked",
                        email: true,
                    },
                },
                messages: {
                    DonationAmount: {
                        required: "Please enter a donation amount.",
                        number: "Please enter a valid number.",
                        min: "Donation amount must be at least $1.",
                    },
                    Frequency: {
                        required: "Please select a frequency.",
                    },
                    FirstName: {
                        required: "Please enter your first name.",
                        minlength: "Your first name must be at least 2 characters long.",
                    },
                    LastName: {
                        required: "Please enter your last name.",
                        minlength: "Your last name must be at least 2 characters long.",
                    },
                    Email: {
                        required: "Please enter your email address.",
                        email: "Please enter a valid email address.",
                    },
                    dateOfBirth: {
                        required: "Please enter your date of birth.",
                        date: "Please enter a valid date.",
                    },
                    HonoreeFirstName: {
                        required: "Please enter the honoree's first name.",
                        minlength: "Honoree's first name must be at least 2 characters long.",
                    },
                    HonoreeLastName: {
                        required: "Please enter the honoree's last name.",
                        minlength: "Honoree's last name must be at least 2 characters long.",
                    },
                    HonoreeEmail: {
                        required: "Please enter the honoree's email address.",
                        email: "Please enter a valid email address.",
                    },
                },
                submitHandler: function (form) {
                    // Handle form submission with AJAX
                    var publishkey = $('#<%= publishKey.ClientID %>').val();
                    var stripe = Stripe(publishkey);
                    var transactionData = {
                        'OrganizationId': $('#<%= organizationId.ClientID %>').val(),
                        'DonationAmount': parseFloat($('#DonationAmount').val()),
                        'HomeAddress': $('#homeaddress').val(),
                        'FirstName': $('#firstname').val(),
                        'LastName': $('#lastname').val(),
                        'Email': $('#email').val(),
                        'DateOfBirth': $('#dateOfBirth').val(),
                        'City': $('#txtCity').val(),
                        'State': $('#ddlState').val(),
                        'Zip': $('#txtZip').val(),
                        'DonationNote': $('#donationNote').val(),
                        'Frequency': parseInt($("input[name='Frequency']:checked").val()),
                        'IsCoverFee': $('#coverfee').prop("checked"),
                        'NotShareName': $('#notShareName').prop("checked"),
                        'IsHonorDonation': $('#honordonation').prop("checked"),
                    };
                    if (transactionData.IsCoverFee) {
                        transactionData.DonationAmount += parseFloat($('#coverFeeAmount').text());
                    }
                    if (transactionData.IsHonorDonation) {
                        transactionData.HonoreeFirstName = $('#firstnameHonoree').val();
                        transactionData.HonoreeLastName = $('#lastnameHonoree').val();
                        transactionData.HonoreeEmail = $('#emailHonoree').val();
                    }
                    $.ajax({
                        type: "POST",
                        url: "DonationDetails.aspx/AddTransactionDetails",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ transactionData: transactionData }),
                        success: function (response) {
                            return stripe.redirectToCheckout({ sessionId: response.d }); // Display success message
                        },
                        error: function (xhr, status, error) {
                            alert("An error occurred: " + error);
                        }
                    });
                },
                errorElement: "span",
                errorClass: "error-message",
                highlight: function (element) {
                    $(element).closest(".input-group").addClass("has-error");
                },
                unhighlight: function (element) {
                    $(element).closest(".input-group").removeClass("has-error");
                },
            });
        });
    </script>
</body>
</html>



