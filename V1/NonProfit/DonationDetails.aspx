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
    <style>
        .btn-secondary {
            border: none !important;
            outline: none !important;
            box-shadow: none !important;
        }

        .custom-green-button {
            background-color: #398439 !important;
            border: 2px solid #255625;
            color: white !important;
            border: none;
        }
    </style>
</head>
<body>
    <div class="header">
        <asp:Image ImageUrl="https://res.cloudinary.com/resilia/image/upload/v1/NonprofitOrgRoot/1960-c6a36c6a-d152-4120-8041-7d936d0d4df5/logos/t2vtpy7jwzafgjwf3u6f?_a=AJFJtWI0" alt="logo" class="logo" runat="server" />
        <h4 class="title"><%=OrganizationName%><%=CampaignName%></h4>
    </div>
    <div style="display: flex; flex-wrap: wrap;">
        <div class="content left">
            <div style="padding: 40px">
                <%=LiteralDescription%>
            </div>
        </div>
        <div class="content right">
            <form id="donationform" runat="server" novalidate="novalidate">
                <div class="row">
                    <div class="col-12 mb-20">
                        <h2>Donation</h2>
                    </div>
                    <div class="col-12">
                        <span class="legend">Amount</span>
                        <asp:TextBox type="hidden" ID="organizationId" runat="server" value="" />
                        <asp:TextBox type="hidden" ID="publishKey" runat="server" value="" />
                    </div>
                    <div class="col-12">
                        <div class="containerLabel">
                            <% for (int i = 0; i < LiteralAmountList.Count; i++)
                                { %>
                            <% if (i == 0)
                            { %>
                            <label class="item selected" for="amount<%= i + 1 %>">
                                $<%= LiteralAmountList[i] %>
                                <input type="radio" id="amount<%= i + 1 %>" name="Amount" value="<%= LiteralAmountList[i] %>" />
                            </label>
                            <% }
                            else
                            { %>
                            <label class="item" for="amount<%= i + 1 %>">
                                $<%= LiteralAmountList[i] %>
                                <input type="radio" id="amount<%= i + 1 %>" name="Amount" value="<%= LiteralAmountList[i] %>" />
                            </label>
                            <% } %>
                            <% } %>
                            <label class="item" for="amount<%= LiteralAmountList.Count + 1 %>">
                                Other
            <input type="radio" id="amount<%= LiteralAmountList.Count + 1 %>" name="Amount" value="0" />
                            </label>
                        </div>
                    </div>
                    <div class="col-6 donatAmn">
                        <div class="input-group">
                            <span class="legend" for="DonationAmount">Donation Amount</span>   <span style="color: red">*</span>
                            <asp:TextBox type="number" ID="txtDonationAmount" runat="server" class="formInput" name="DonationAmount" required="required" />
                        </div>
                    </div>
                    <div class="col-6 donatAmn">
                    </div>
                    <%--   <div class="col-12"><span class="legend" for="firstname">Frequency</span></div>                    <div class="col-12">                        <div class="containerLabel">                            <label class="item freqLabel selected" for="Frequency1">                                One Time            <asp:RadioButton ID="Frequency1" runat="server" GroupName="Frequency" Checked="true" CssClass="radioBtn" value="1" />                            </label>                            <label class="item freqLabel" for="Frequency2">                                Monthly            <asp:RadioButton ID="Frequency2" runat="server" GroupName="Frequency" CssClass="radioBtn" value="2" />                            </label>                        </div>                    </div>--%>
                    <div class="col-12  ">
                        <asp:TextBox type="checkbox" ID="coverfee" name="CoverFee" runat="server" Style="margin-right: 5px" />
                        <span for="coverfee">Yes, I'd like to cover the $<span id="coverFeeAmount"><%= LiteralAmountList[0]*0.06 %></span> transaction fee</span>
                    </div>
                    <div class="col-12 mb-20">
                        <h2 class="mr-3">Donation Details <a id="btnLogin" runat="server" href="/V1/Login.aspx?returnUrl=<%=HttpUtility.UrlEncode(Request.Url.ToString()) %>">Already Registered? Login</a></h2>
                    </div>
                    <div class="col-6">
                        <div class="input-group">
                            <span class="legend" for="firstname">First Name: <span style="color: red">*</span></span>
                            <asp:TextBox class="form-control formInput" ID="txtfirstname" runat="server" name="FirstName" />
                            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtfirstname"
                                ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="input-group">
                            <span class="legend" for="lastname">Last Name: <span style="color: red">*</span></span>
                            <asp:TextBox class="form-control formInput" ID="txtlastname" runat="server" name="LastName" />
                            <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtlastname"
                                ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="input-group">
                            <span class="legend" for="email">Email: <span style="color: red">*</span></span>
                            <asp:TextBox class="form-control formInput" ID="txtemail" runat="server" name="Email" />
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtemail"
                                ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic" />
                            <asp:RegularExpressionValidator
                                ID="revEmail"
                                runat="server"
                                ControlToValidate="txtemail"
                                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                                ErrorMessage="Please enter a valid email address."
                                ForeColor="Red"
                                Display="Dynamic"
                                CssClass="validation-error" />
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="form-group m-b-lg border-bottom">
                            <div class="input-group m-b-md">
                                <span class="legend" for="dateofbirth">Date Of Birth:</span><span style="color: red"></span>
                                <div class="input-group date">
                                    <asp:TextBox type="text" class="formInput " ID="txtdateOfBirth" runat="server" /><span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="input-group">
                            <span class="legend" for="homeaddress">Address:  <span style="color: red">*</span></span>
                            <asp:TextBox class="formInput" type="text" ID="txthomeaddress" runat="server" name="HomeAddress" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator" runat="server" ControlToValidate="txthomeaddress"
                                ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="input-group">
                            <span class="legend" for="city">City:  <span style="color: red">*</span></span>
                            <asp:TextBox class="formInput" type="text" ID="txtCity" runat="server" name="City" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCity"
                                ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="input-group">
                            <span class="legend" for="ddlState">State: <span style="color: red">*</span></span>
                            <asp:DropDownList ID="ddlState" runat="server" CssClass="formInput" AppendDataBoundItems="true">
                                <asp:ListItem Text=" --Select State-- " Value="" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlState"
                                InitialValue="" ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="input-group">
                            <span class="legend" for="zip">Zip:  <span style="color: red">*</span></span>
                            <asp:TextBox class="formInput" ID="txtZip" runat="server" onkeypress="return isNumberKey(event)" MaxLength="15" name="txtZip" type="text" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtZip"
                                ErrorMessage="This field is required." ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="col-12">
                        <asp:TextBox type="checkbox" ID="notShareName" runat="server" name="NotShareName" Style="margin-right: 5px" />
                        <span for="notShareName">Don't share my name publicly as part of this campaign</span>
                    </div>
                    <div class="col-12">
                        <asp:TextBox type="checkbox" ID="honordonation" runat="server" name="IsHonorDonation" />
                        <span for="honor-donation">This is a donation in someone's honor</span>
                    </div>
                    <div class="col-12 showHonor">
                        <h2>In Honor Of</h2>
                    </div>
                    <div class="col-6 showHonor">
                        <div class="input-group">
                            <span class="legend" for="firstnameHonoree">Honoree First Name:</span><span style="color: red"></span>
                            <asp:TextBox class="formInput" type="text" ID="txtfirstnameHonoree" runat="server" name="HonoreeFirstName" />
                        </div>
                    </div>
                    <div class="col-6 showHonor">
                        <div class="input-group">
                            <span class="legend" for="lastnameHonoree">Honoree Last Name:</span><span style="color: red"></span>
                            <asp:TextBox class="formInput" type="text" ID="txtlastnameHonoree" runat="server" name="HonoreeLastName" />
                        </div>
                    </div>
                    <div class="col-6 showHonor">
                        <div class="input-group">
                            <span class="legend" for="emailHonoree">Honoree Email Address:</span><span style="color: red"></span>
                            <asp:TextBox CssClass="formInput" ID="txtemailHonoree" runat="server" TextMode="Email" />
                        </div>
                    </div>
                    <div class="col-12">
                        <div class="input-group">
                            <span class="legend" for="donationNote">Donation Note:</span>
                            <textarea rows="5" id="txtdonationNote" runat="server" name="DonationNote"></textarea>
                        </div>
                    </div>
                    <asp:ValidationSummary ID="vsSummary" runat="server" ForeColor="Red" ValidationGroup="UserForm" />
                    <div class="col-12" style="display: flex; justify-content: right; gap: 10px;">
                        <asp:Button ID="btnEnterPayment" class="btn btn-success"
                            runat="server" CssClass="btn custom-green-button"
                            Text="Send Donation" OnClientClick="return validateForm();"
                            OnClick="AddTransactionDetails_Click" ValidationGroup="UserForm" />
                        <asp:Button ID="btnPrevious" runat="server" CssClass="btn btn-secondary"
                            Text="Previous" OnClick="btnPrevious_Click" CausesValidation="false" />
                    </div>
                </div>
            </form>
        </div>
    </div>
    <script src="https://js.stripe.com/v3/"></script>
    <script type="text/javascript">        $(document).ready(function () {            $(function () {                $('.input-group.date').datepicker({                    autoclose: true,                });            });            $("#honordonation").on('click', function () {                if ($(this).prop("checked")) {                    $('.showHonor').attr("style", "display:block");                } else {                    $('.showHonor').attr("style", "display:none");                }            });            $('input[type="radio"][name="Amount"]').on('click', function () {                $(".item:not(.freqLabel)").removeClass("selected");                $(this).closest("label").addClass("selected");                var selectedValue = $(this).val();                if (selectedValue == "0") {                    $('#txtDonationAmount').val('').show().focus();                    $('.donatAmn').show();                    updateTransactionFee(0);                } else {                    $('#txtDonationAmount').val(selectedValue).hide();                    $('.donatAmn').hide();                    updateTransactionFee(selectedValue);                }            });            $('#txtDonationAmount').on('input', function () {                let donationAmount = parseFloat($(this).val()) || 0;                updateTransactionFee(donationAmount);            });            function updateTransactionFee(amount) {                let transactionFee = amount > 0 ? `${(amount * 0.06).toFixed(2)}` : "--";                $('#coverFeeAmount').text(transactionFee);            }        });        function isNumberKey(evt) {            var charCode = (evt.which) ? evt.which : evt.keyCode;            if (charCode == 110 || charCode == 190 || charCode == 46)                return true;            if (charCode > 31 && (charCode < 48 || charCode > 57))                return false;            return true;        }        function checkFields() {            var requiredFields = [                '<%= txtfirstname.ClientID %>',                '<%= txtlastname.ClientID %>',                '<%= txtemail.ClientID %>',                '<%= txthomeaddress.ClientID %>',                '<%= txtCity.ClientID %>',                '<%= ddlState.ClientID %>',                '<%= txtZip.ClientID %>'            ];            var isValid = true;            requiredFields.forEach(function (id) {                var element = document.getElementById(id);                if (element) {                    if (element.tagName === "SELECT") {                        if (element.value === "") {                            isValid = false;                        }                    } else if (element.value.trim() === "") {                        isValid = false;                    }                }            });            document.getElementById('<%= btnEnterPayment.ClientID %>').disabled = !isValid;        }        function validateForm() {            var email = document.getElementById('<%= txtemail.ClientID %>').value.trim();            var donationAmount = document.getElementById('<%= txtDonationAmount.ClientID %>').value.trim();            if (email == '') {                $("#rfvEmail").css("display", "block");
                return false;
            } else {                $("#rfvEmail").css("display", "none");
            }            var emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;            if (!emailPattern.test(email)) {                $("#revEmail").css("display", "block");                return false;            } else {                $("#revEmail").css("display", "none");
            }            if (isNaN(donationAmount) || parseFloat(donationAmount) <= 0) {                return false;            }            return true;        }    </script>
</body>
</html>
