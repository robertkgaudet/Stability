<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="People.aspx.cs" Inherits="V1_NonProfit_People" %>

<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ Register Src="~/V1/UserControls/TeamLogo.ascx" TagPrefix="uc1" TagName="TeamLogo" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
    <script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/css/bootstrap-multiselect.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/js/bootstrap-multiselect.min.js"></script>
    <link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
    <script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>
    <style>
        .website {
            background-color: #5E2E91;
            padding: 17px;
            color: white;
        }

            .website:hover {
                background-color: #902F91;
                color: white;
                cursor: pointer;
            }

        .modal-dialog {
            margin-top: 100px;
        }

        .container-search {
            margin-bottom: 10px;
        }

        .panel-heading h4 {
            margin-bottom: 0 !important;
            margin-top: -4px;
        }

        .container-search {
            margin-top: 5px !important;
        }

        .fix {
            margin-right: 8px;
            margin-left: 8px;
        }
        .input-group {
            display: flex;
            align-items: center;
        }

        .input-group-append {
            margin-left: 10px;
        }
      </style>
    <script>
        // Step 1: Select all the buttons in the table

        var recipientsName;
        var recipientsEmail;
        var message;
        var teamName;
        var divMessageSuccess;
        var divMessageTextBox;
        var divMessageError;
        var divErrorMessage
        var btnSend;
        var messageModelTitle;
        var txtMessage;

        $(document).ready(function () {

            // Initialize Example 1
            $('#tblVolunteers').footable();


            messageModelTitle = document.getElementById('messageModelTitle');
            txtMessage = document.getElementById('<%=txtMessage.ClientID%>');
            divMessageTextBox = document.getElementById('messageTextBox');
            btnSend = document.getElementById('btnSend');
            divMessageError = document.getElementById('divMessageError');
            divErrorMessage = document.getElementById('divErrorMessage');

            divMessageSuccess = document.getElementById('divMessageSuccess');
            divMessageSuccess.style.visibility = 'visible';
            divMessageSuccess.hidden = true;
            divMessageTextBox.hidden = false;
            btnSend.hidden = false;

            divMessageError.style.visibility = 'hidden';
            divErrorMessage.style.visibility = 'hidden';
            divMessageError.hidden = true;
            divErrorMessage.hidden = true;

            $("#nonProfit.dropdown-menu li").click(function () {
                window.location.href = "/V1/NonProfit/Default.aspx?organizationId=" + $(this).attr('id');
            });

            $('#StartDate, #EndDate').datepicker({
                format: 'mm/dd/yyyy',
                autoclose: true,
            });
            $(".toggle-search-btn").click(function () {
                var icon = $(this).find("i");
                var searchPanel = $("#searchFilters");
                icon.toggleClass("fa-chevron-down fa-chevron-up");
                searchPanel.collapse("toggle");
            });

            $('.multiselect').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                maxHeight: 200,
                nonSelectedText: 'Select Options',
                allSelectedText: 'All Selected',
                numberDisplayed: 2
            });
           
            window.onload = function () {
                var selectAllCheckbox = document.getElementById("<%= chkSelectAll.ClientID %>");
    var userCheckboxes = document.querySelectorAll(".select-user");
                var hiddenField = document.getElementById("<%= hdnSelectedUsers.ClientID %>");

                function updateSelectedUsers() {
                    var selectedUserIds = [];
                    for (var i = 0; i < userCheckboxes.length; i++) {
                        if (userCheckboxes[i].checked) {
                            selectedUserIds.push(userCheckboxes[i].getAttribute("data-userid"));
                        }
                    }
                    hiddenField.value = selectedUserIds.join(",");
                }

                // "Select All" Checkbox Click Event (No Change)
                selectAllCheckbox.onclick = function () {
                    for (var i = 0; i < userCheckboxes.length; i++) {
                        userCheckboxes[i].checked = this.checked;
                    }
                    updateSelectedUsers(); // Update hidden field
                };

                // Individual "User" Checkbox Click Event (No Change)
                for (var i = 0; i < userCheckboxes.length; i++) {
                    userCheckboxes[i].onclick = function () {
                        var allChecked = true;
                        for (var j = 0; j < userCheckboxes.length; j++) {
                            if (!userCheckboxes[j].checked) {
                                allChecked = false;
                                break;
                            }
                        }
                        selectAllCheckbox.checked = allChecked;
                        updateSelectedUsers(); // Update hidden field
                    };
                }
            };



        });

        function sendClick(object) {
            message = txtMessage.value;
            teamName = '<%=teamName%>';
            signedInUserFullName = '<%=signedInUserFullName%>';
            organizationId = '<%=organizationId%>';
            sendMessage(signedInUserFullName, organizationId, recipientsName, recipientsEmail, teamName, message);
        }

        function btnClick(object) {
            txtMessage.value = "";
            divMessageSuccess.style.visibility = 'hidden';
            divMessageTextBox.style.visibility = 'visible';
            btnSend.style.visibility = 'visible';
            divMessageSuccess.hidden = true;
            divMessageTextBox.hidden = false;
            btnSend.hidden = false;

            divMessageError.style.visibility = 'hidden';
            divErrorMessage.style.visibility = 'hidden';
            divMessageError.hidden = true;
            divErrorMessage.hidden = true;

            recipientsName = object.getAttribute('data-name');
            recipientsEmail = object.getAttribute('data-email');
            messageModelTitle.innerHTML = "This will send " + recipientsName + " an email from the Stability platform.";
            return false;
        }

        function sendMessage(signedInUserFullName, organizationId, recipientsName, recipientsEmail, teamName, message) {
            $.ajax
                (
                    {
                        type: "POST",
                        url: "/V1/Handlers/MessageMember.ashx",
                        data: JSON.stringify({ senderName: signedInUserFullName, organizationId: organizationId, recipientsName: recipientsName, recipientsEmail: recipientsEmail, teamName: teamName, message: message }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            //alert(divMessageSuccess);
                            //alert(divTextBody);
                            divMessageSuccess.style.visibility = 'visible';
                            divMessageTextBox.style.visibility = 'hidden';
                            btnSend.style.visibility = 'hidden';
                            divMessageSuccess.hidden = false;
                            divMessageTextBox.hidden = true;
                            btnSend.hidden = true;
                        },
                        error: function (xhr, status, error) {
                            alert(xhr.responseText);
                            /*var err = eval("(" + xhr.responseText + ")");*/
                            divMessageError.style.visibility = 'visible';
                            divErrorMessage.style.visibility = 'visible';
                            divMessageError.hidden = false;
                            divErrorMessage.hidden = false;
                            divErrorMessage.innerHTML += xhr.responseText;
                        }
                    }
                );
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:TeamHeader runat="server" ID="ucTeamHeader" />
    <asp:HiddenField ID="hdnSelectedUsers" runat="server" />
    <div id="divEmail" runat="server" class="input-group">
        <asp:TextBox ID="txtemail" runat="server" CssClass="form-control" Placeholder="Enter Email Text"></asp:TextBox>
        <div class="input-group-append">
            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClientClick="updateHiddenField();" OnClick="btnSendEmail_click" />
        </div>
    </div>

    <div id="divSms" runat="server" class="input-group">
        <asp:TextBox ID="txtsms" runat="server" CssClass="form-control" Placeholder="Enter Sms Text"></asp:TextBox>
        <div class="input-group-append">
            <asp:Button ID="btnSms" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnSendSms_click" />
        </div>
    </div>


    <div class="panel-heading">
        <asp:HyperLink ID="hypInviteTeamMembers" runat="server" Visible="false" Text="Invite Team Members" CssClass="btn btn-sm btn-info"></asp:HyperLink>
        <asp:HyperLink ID="hypPrintableTeamList" runat="server" Visible="false" Target="_blank" Text="Printable List" CssClass="btn btn-sm btn-info"></asp:HyperLink>
    </div>
    <div class="panel-body" style="margin-bottom: -27px; padding: 0px;">
        <div class="col-lg-12">
            <div class="row">
                <div class="hpanel hblue">
                    <div class="panel-tools">
                        <button class="btn btn-link toggle-search-btn" type="button" data-toggle="collapse" data-target="#searchFilters" aria-expanded="false" aria-controls="searchFilters">
                            <i class="fa fa-chevron-down"></i>
                        </button>
                    </div>
                    <h4 style="margin-left: 7px;">Search</h4>
                    <div id="divUpdateMessage" runat="server" class="alert alert-warning text-center" style="margin-bottom: 20px;" visible="false">
                        <asp:Literal ID="litMessage" runat="server"></asp:Literal>
                    </div>
                    <div id="divFilterMessage" runat="server" class="alert alert-info text-center" style="margin-bottom: 20px;" visible="false">
                        <asp:Literal ID="litFilterMessage" runat="server"></asp:Literal>
                    </div>
                    <div class="" data-child="hpanel" data-effect="fadeInDown" runat="server" id="hpanelMembers" visible="false">
                        <div class="hpanel" runat="server" id="hpanelJoin" visible="true">
                            <a href="/V1/Profile/EditNonProfits.aspx">Join This Team</a>
                        </div>
                        <div class="container-search">
                            <!-- Collapsible Search Filters -->
                            <div class="collapse" id="searchFilters">
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <div class="form-group fix">
                                            <b>Search By Member  :</b>
                                            <asp:TextBox ID="filter" runat="server" CssClass="form-control" placeholder="Search By Member "></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-6 mb-3">
                                        <div class="form-group fix">
                                            <b>Location :</b>
                                            <input type="text" class="form-control" id="txtlocation" placeholder="Enter Location">
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <div class="form-group fix">
                                            <b class="text-line">Skills :</b>
                                            <asp:ListBox ID="ddlSkills" runat="server" CssClass="form-control multiselect" SelectionMode="Multiple" AppendDataBoundItems="true"></asp:ListBox>
                                        </div>
                                    </div>

                                    <div class="col-md-6 mb-3">
                                        <div class="form-group fix">
                                            <b class="text-line">Resources :</b>
                                            <asp:ListBox ID="ddlResources" runat="server" CssClass="form-control multiselect" SelectionMode="Multiple" AppendDataBoundItems="true"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <div class="form-group fix">
                                            <b class="text-line">Available From :</b>
                                            <asp:TextBox ID="StartDate" runat="server" CssClass="form-control datepicker" placeholder="Start Date" autocomplete="off" ClientIDMode="Static" />
                                        </div>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <div class="form-group fix">
                                            <b class="text-line">Available To :</b>
                                            <asp:TextBox ID="EndDate" runat="server" CssClass="form-control datepicker" placeholder="End Date" autocomplete="off" ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-sm-3 mb-2">
                                                <div class="form-check fix">
                                                    <asp:CheckBox ID="txtIsVetted" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%=txtIsVetted.ClientID%>">Is Vetted</label>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 mb-2">
                                                <div class="form-check">
                                                    <asp:CheckBox ID="txtOptedSMS" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%=txtOptedSMS.ClientID%>">Opted SMS</label>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 mb-2">
                                                <div class="form-check">
                                                    <asp:CheckBox ID="txtEmailconnect" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%=txtEmailconnect.ClientID%>">Email Connected</label>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 mb-2">
                                                <div class="form-check">
                                                    <asp:CheckBox ID="txtIsVerified" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%=txtIsVerified.ClientID%>">Is Verified</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row mt-4" style="margin-right: 6px;">
                                    <div class="col-md-12 text-right ">
                                        <asp:Button ID="SearchButton" runat="server" CssClass="btn btn-info btn-sm me-2" Text="Search" OnClick="SearchButton_Click" />
                                        <asp:Button ID="ClearButton" runat="server" CssClass="btn btn-danger btn-sm" Text="Clear" OnClick="ClearButton_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="select-all" Text="Select All" />
    <table id="tblVolunteers" class="footable" data-page-size="20" data-filter="#filter">
        <tbody>
            <asp:Repeater ID="rptVolunteers" runat="server" OnItemDataBound="rptVolunteers_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td style="background-color: white;">
                            <div class="hpanel">
                                <div class="panel-body">
                                    <!-- Checkbox for each user -->
                                    <input type="checkbox" class="select-user" data-userid='<%# Eval("UserID") %>' />

                                    <h5 class="m-b-xs">
                                        <asp:HyperLink ID="hypName" runat="server" class="volunteer-name"></asp:HyperLink>
                                        <uc1:TeamLogo runat="server" ID="ucUserNameWithBadges" />
                                    </h5>
                                    <p>
                                        <asp:Literal ID="litMemberInfo" runat="server"></asp:Literal>
                                        <asp:Literal ID="litDescription" runat="server"></asp:Literal>
                                    </p>
                                    <div class="pull-right">
                                        <asp:Button ID="btnContact" runat="server" Text="Message" Visible="false" CssClass="btn btn-success messageButton" data-toggle="modal" data-target="#messageMemberModal"></asp:Button>
                                    </div>
                                    <asp:Literal ID="litSkills" runat="server"></asp:Literal>
                                    <asp:Literal ID="litResources" runat="server"></asp:Literal>
                                </div>
                                <div class="panel-footer" id="divFooter" runat="server" visible="false">
                                    <div class="text-muted small">
                                        <asp:Literal ID="litVettingInfo" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
        <tfoot>
            <tr>
                <td>
                    <br />
                    <ul class="pagination pull-right"></ul>
                </td>
            </tr>
        </tfoot>
    </table>
    <div class="modal fade" id="messageMemberModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="color-line"></div>
                <div class="modal-header text-center">
                    <h5 class="modal-title">Send a Message</h5>
                    <div id="divMessageSuccess" class="alert alert-success text-uppercase">
                        <i class="fa fa-envelope"></i>Your message has been sent.
                    </div>
                    <div id="divMessageError" class="alert alert-warning text-uppercase">
                        <i class="fa fa-envelope"></i>
                        <div id="divErrorMessage"></div>
                    </div>
                </div>
                <div class="modal-body" id="messageTextBox">
                    <p>
                        <div id="messageModelTitle"></div>
                        <asp:TextBox TextMode="MultiLine" Width="100%" Rows="5" runat="server" ID="txtMessage"></asp:TextBox>
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" id="btnSend" onclick="return sendClick(this);">Send</button>
                </div>
            </div>
        </div>
    </div>


    <uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>
