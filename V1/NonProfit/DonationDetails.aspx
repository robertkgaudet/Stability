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
        <h4 class="title"> <%=OrganizationName%>, <%=CampaignName%></h4>
    </div>
        <div style="display: flex; flex-wrap: wrap;">
            <div class="content left">
                <div style="padding: 40px">
                    <%=LiteralDescription%>
                </div>
            </div>
            <div class="content right">
                <form id="donationform" runat="server" novalidate="novalidate" >
                    <div class="row">
                        <div class="col-12 mb-20">
                            <h2>Donation</h2>
                        </div>
                        <div class="col-12">
                            <span class="legend">Amount</span>
                            <asp:TextBox type="hidden" id="organizationId" runat="server" value="" />
                            <asp:TextBox type="hidden" id="publishKey" runat="server" value=""  />
                        </div>
                        <div class="col-12">

                            <div class="containerLabel">
                                <% for (int i = 0; i < LiteralAmountList.Count(); i++)
                                    { %>
                                <% if (i == 0)
                                    { %>
                                <label class="item selected" for="amount<%= i + 1 %>">
                                    $<%= LiteralAmountList[i] %>
                                    <input type="radio" id="amount<%= i + 1 %>" name="Amount"  value="<%= LiteralAmountList[i] %>
                                                " />
                                </label>
                                <% }
                                    else
                                    { %>
                                <label class="item" for="amount<%= i + 1 %>">
                                    $<%= LiteralAmountList[i] %>
                                    <input type="radio" id="amount<%= i + 1 %>" name="Amount"  value="<%= LiteralAmountList[i] %>
                                                " /></label>
                                <% } %>
                                <% } %>
                                <label class="item" for="amount<%=LiteralAmountList.Count + 1 %>">Other<input type="radio" id="amount<%=LiteralAmountList.Count + 1 %>" name="Amount" value="0" /></label>
                            </div>
                        </div>
                        <div class="col-6 donatAmn">
                            <div class="input-group">
                                <span class="legend" for="DonationAmount">Donation Amount</span><span style="color: red">*</span>
                                <asp:TextBox type="number" id="txtDonationAmount" runat="server" class="formInput" name="DonationAmount"  required="required" />
                            </div>
                        </div>
                        <div class="col-6 donatAmn">
                        </div>
                        <div class="col-12"><span class="legend" for="firstname">Frequency</span></div>
                        <div class="col-12">
                            <div class="containerLabel">
                                <label class="item freqLabel selected" for="Frequency1">
                                    One Time
                                    <asp:TextBox type="radio" id="Frequency1" runat="server" checked="checked" name="Frequency" value="1" />
                                </label>
                                <label class="item freqLabel" for="Frequency2">
                                    Monthly
                                    <asp:TextBox type="radio" id="Frequency2" runat="server" name="Frequency" value="2" />
                                </label>
                            </div>
                        </div>
                        <div class="col-12">
                            <asp:TextBox type="checkbox" id="coverfee" name="CoverFee" runat="server" style="margin-right: 5px" />
                            <span for="coverfee">Yes, I'd like to cover the $<span id="coverFeeAmount"><%= LiteralAmountList[0]*0.06 %></span> transaction fee</span>
                        </div>
                        <div class="col-12 mb-20">
                                <h2 class="mr-3">Donation Details <a id="btnLogin" runat="server" href="/V1/Login.aspx?returnUrl=<%=HttpUtility.UrlEncode(Request.Url.ToString()) %>">Already Registered? Login</a></h2>
                                
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="firstname">First Name:</span><span style="color: red"></span>
                                <asp:TextBox class="form-control formInput " type="text" id="txtfirstname" runat="server" name="FirstName" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="lastname">Last Name:</span><span style="color: red"></span>
                                <asp:TextBox class="formInput" type="text" id="txtlastname" runat="server" name="LastName" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="email">Your Email Address:</span><span style="color: red"></span>
                                <asp:TextBox class="formInput" type="email" id="txtemail" runat="server" name="Email" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="form-group m-b-lg border-bottom">
                                <div class="input-group m-b-md">
                                    <span class="legend" for="dateofbirth">Date Of Birth:</span><span style="color: red"></span>
                                    <div class="input-group date">
                                        <asp:TextBox type="text" class="formInput " id="txtdateOfBirth" runat="server" /><span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="homeaddress">Home Address:</span>
                                <asp:TextBox class="formInput" type="text" id="txthomeaddress" runat="server" name="HomeAddress" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="city">City:</span>
                                <asp:TextBox class="formInput" type="text" id="txtCity" runat="server" name="City" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="zip">State:</span>
                                <asp:TextBox class="formInput" id="txtddlState" runat="server"  maxlength="50" name="txtState" type="text" />
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group">
                                <span class="legend" for="zip">Zip:</span>
                                <asp:TextBox class="formInput" id="txtZip" runat="server" onkeypress="return isNumberKey(event)" maxlength="15" name="txtZip" type="text" />
                            </div>
                        </div>

                        <div class="col-12">
                            <asp:TextBox type="checkbox" id="notShareName" runat="server" name="NotShareName" style="margin-right: 5px" />
                            <span for="notShareName">Don't share my name publicly as part of this campaign</span>
                        </div>
                        <div class="col-12">
                            <asp:TextBox type="checkbox" id="honordonation" runat="server" name="IsHonorDonation" />
                            <span for="honor-donation">This is a donation in someone's honor</span>
                        </div>
                        <div class="col-12 showHonor">
                            <h2>In Honor Of</h2>
                        </div>
                        <div class="col-6 showHonor">
                            <div class="input-group">
                                <span class="legend" for="firstnameHonoree">Honoree First Name:</span><span style="color: red"></span>
                                <asp:TextBox class="formInput" type="text" id="txtfirstnameHonoree" runat="server" name="HonoreeFirstName"  />
                            </div>
                        </div>
                        <div class="col-6 showHonor">
                            <div class="input-group">
                                <span class="legend" for="lastnameHonoree">Honoree Last Name:</span><span style="color: red"></span>
                                <asp:textbox class="formInput" type="text" id="txtlastnameHonoree" runat="server" name="HonoreeLastName"  />
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
                        <div class="col-12" style="display: flex; justify-content: end;">

                             <asp:Button type="submit" class="btn submit-btn" runat="server" Text="Select Payment" OnClick="AddTransactionDetails_Click" />
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

    
</body>
</html>



