<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true"
    EnableEventValidation="false" ValidateRequest="false" CodeFile="People.aspx.cs"
    Inherits="V1_NonProfit_People" %>

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
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
    <script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
    <link href="../../Homer/vendor/sweetalert/lib/sweet-alert.css" rel="stylesheet" />
    <script src="../../Homer/vendor/sweetalert/lib/sweet-alert.min.js"></script>
    <!-- Vendor styles -->
    <link rel="stylesheet" href="vendor/fontawesome/css/font-awesome.css" />
    <link rel="stylesheet" href="vendor/metisMenu/dist/metisMenu.css" />
    <link rel="stylesheet" href="vendor/animate.css/animate.css" />
    <link rel="stylesheet" href="vendor/bootstrap/dist/css/bootstrap.css" />
    <link rel="stylesheet" href="vendor/ladda/dist/ladda-themeless.min.css" />

    <!-- App styles -->
    <link rel="stylesheet" href="fonts/pe-icon-7-stroke/css/pe-icon-7-stroke.css" />
    <link rel="stylesheet" href="fonts/pe-icon-7-stroke/css/helper.css" />
    <link rel="stylesheet" href="styles/style.css">

    <link rel="stylesheet" href="../../Homer/vendor/ladda/dist/ladda-themeless.min.css" />
    <script src="../../Homer/vendor/ladda/dist/spin.min.js"></script>
    <script src="../../Homer/vendor/ladda/dist/ladda.min.js"></script>
    <script src="../../Homer/vendor/ladda/dist/ladda.jquery.min.js"></script>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <style>
		.panel-footer
		{
			background-color:#f9f5ff !important;
		}

        #messageModelTitle {
            width: 94%;
            margin-left: 19px;
        }

        .suggestion-box {
            margin-top: 148px !important;
        }

        .alert {
            padding: 15px;
            margin-bottom: 20px !important;
            border: 1px solid transparent;
            border-radius: 4px;
        }

        #ContentPlaceHolder1_txtMessage {
            width: 94% !important;
            margin-left: 19px;
        }

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

        .m-b-xs {
            margin-bottom: 0;
        }

        .messageButton {
            margin-left: auto;
            padding: 5px 15px;
            font-size: 14px;
            border-radius: 5px;
        }

        .input-group {
            display: flex;
            align-items: center;
            margin-right: -40px
        }

        .input-group-append {
            margin-left: 10px;
        }

        .chkSelectAll {
            margin-top: 9px;
        }

        input#ContentPlaceHolder1_chkSelectAll {
            margin-top: 10px;
            margin-right: 10px;
        }

        .stability-badge {
            margin-right: 4px !important;
        }

        .form-check label {
            margin-left: 8px;
            font-weight: normal !important;
        }

        .margin {
            margin-left: -6px;
            margin-right: 6px;
        }

        .justify-content-center {
            /*  display: flex !important;*/
            justify-content: center !important;
        }

        .b2 {
            margin-bottom: 160px;
            margin-left: 110px;
            width: 123px;
        }

        .form-group {
            margin-bottom: 15px !important;
            margin-left: 28px !important;
            margin: -6px;
        }

        .note-editor.note-frame.panel.panel-default {
            margin-left: 22px;
            width: 655px;
        }

        button.ladda-button.ladda-button-demo.ladda-button-email.btn.btn-primary.b2 {
            margin-bottom: 155px;
        }

        /*  label {
            margin: 16px 0px 10px 10px;
            font-size: 17px;
        }
*/

        .modal-body {
            position: relative;
            padding: 15px;
            margin-left: -14px;
            margin-bottom: -13px;
        }

        #txtEmail {
            margin-right: 0px !important;
        }

        .input-group-append {
            margin-left: -1px; /* Removes unwanted space */
        }



        .custom-spinner {
            width: 40px;
            height: 40px;
            border: 4px solid #ccc;
            border-top: 4px solid #007bff;
            border-radius: 50%;
            animation: spin 1s linear infinite;
            margin: 20px auto;
            margin-top: 106px;
        }

        #ContentPlaceHolder1_txtManageVettingNotes {
            margin-left: -9px;
        }

        .custom-flex-end {
            display: flex;
            justify-content: flex-end;
            padding: 0 15px 15px 0;
        }

        .custom-footer {
            display: flex;
            flex-wrap: nowrap;
            justify-content: center;
            gap: 10px;
        }

        .btn-row {
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 13px;
            margin-top: 13px;
            margin-right: 4px;
        }

            .btn-row .btn {
                min-width: 170px;
                height: 45px;
                display: flex;
                align-items: center;
                justify-content: center;
                font-weight: 500;
                margin-right: 9px;
            }

        .fotter .btn {
            min-width: 167px;
            height: 45px;
        }

        .btn-row.modal-footer.custom-footer.btn-group-responsive {
            margin-right: 0px;
        }

        .modal-footer .btn {
            margin-left: 0 !important;
            margin-bottom: 0 !important;
        }

        .btn-row.modal-footer.custom-footer.btn-group-responsive {
            padding-bottom: 10px;
        }

        .fotter {
            justify-content: right;
            display: flex;
            align-items: center;
            gap: 25px;
        }

        @media screen and (max-width: 600px) {
            .fotter {
                display: flex;
                align-items: center;
                justify-content: center;
                gap: 20px;
                padding: 10px;
                margin-left: 0;
            }
        }
       .badge {
    display: inline-block;
    min-width: 10px;
    padding: 3px 7px;
    font-size: 12px;
    font-weight: bold;
    line-height: 1;
    color: #fff;
    text-align: center;
    white-space: nowrap;
    vertical-align: middle;
     background-color: #337ab7; 
    border-radius: 10px;
    margin-left: auto;
}
    </style>
    <script>
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
            var radiusSlider = document.getElementById("radiusSlider");
            var radiusValue = document.getElementById("radiusValue");
            var hiddenRadius = document.getElementById("hiddenRadius");

            radiusSlider.oninput = function () {
                radiusValue.innerHTML = this.value + " km";
                hiddenRadius.value = this.value;
            }
            ///script for training search
            document.addEventListener("DOMContentLoaded", function () {
                var dropdown = document.getElementById("iygg");
                var hiddenField = document.getElementById("gfgj");

                if (dropdown && hiddenField) {
                    dropdown.addEventListener("change", function () {
                        hiddenField.value = dropdown.value;
                        console.log("Hidden Field Updated:", hiddenField.value);
                    });
                }

                var form = document.querySelector("form");
                if (form) {
                    form.addEventListener("submit", function () {
                        hiddenField.value = dropdown.value;
                    });
                }
            });
            $('#<%=txtEmail.ClientID%>').summernote({
                toolbar: [
                    ['style', ['bold', 'italic']],
                    ['alignment', ['ul', 'ol', 'paragraph']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['height', ['height']],
                    ['insert', ['picture', 'link', 'table']],

                ],
                height: 100
            });

            $(document).ready(function () {
                var radiusSlider = document.getElementById("radiusSlider");
                var radiusValue = document.getElementById("radiusValue");
                var hiddenRadius = document.getElementById("hiddenRadius");
                var hiddenEvent = document.getElementById("hiddenEvent");

                // Update hidden radius value when slider changes
                radiusSlider.oninput = function () {
                    radiusValue.innerHTML = this.value + " km";
                    hiddenRadius.value = this.value; // Ensure hidden field updates
                };

                $("#<%=ddlEvent.ClientID%>").change(function () {
                    if ($(this).val()) {
                        $("#radiusSection").show();
                        hiddenEvent.value = $(this).val();
                    } else {
                        $("#radiusSection").hide();
                        hiddenEvent.value = "";
                    }
                });


                $("#<%=SearchButton.ClientID%>").click(function () {
                    hiddenRadius.value = radiusSlider.value;
                    hiddenEvent.value = $("#<%=ddlEvent.ClientID%>").val();
                    console.log("Final Selected Radius: " + hiddenRadius.value + " km");
                    console.log("Final Selected Event: " + hiddenEvent.value);
                });
            });

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
            // PageRequestManager reference
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_beginRequest(function () {
                document.getElementById("loader").style.display = "block";
                document.getElementById("tblVolunteers").style.display = "none";
            });
            prm.add_endRequest(function () {
                document.getElementById("loader").style.display = "none";
                document.getElementById("tblVolunteers").style.display = "table";

                window.scrollTo({ top: 300, behavior: "instant" });
            });
        });
        $(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
        var currentUserId = null;
        function setUserId(button) {
            ;
            currentUserId = button.getAttribute('data-userid');
        }
        function fetchUserData() {
            ;
            if (!currentUserId) {
                alert("User ID not set.");
                return;
            }
            $('#loader').show();
            $.ajax({
                type: "GET",
                url: "/V1/Handlers/UpdateMemberInfo.ashx",
                data: {
                    action: "fetch", userId: currentUserId, organizationId: '<%= Request.QueryString["organizationId"] %>'
                },
                dataType: "json",
                success: function (response) {
                    if (response.success) {
                        $('#loader').hide();

                        $("[name*='rblManageUserStatus'][value='" + response.vettingStatus + "']").prop("checked", true);
                        $('#<%= txtManageVettingNotes.ClientID %>').val(response.vettingNotes);
                        $('#<%= cbStabilityVerified.ClientID %>').prop('checked', response.stabilityVerified);
						$('#<%= cbTeamVerified.ClientID %>').prop('checked', response.showTeamLogo); /*Should say showTeamVerified*/
                        $('#<%= chkManageTeamAdministrator.ClientID %>').prop('checked', response.makeTeamAdministrator);
                        $('#<%= btnRemoveTeamMember.ClientID %>').show();
                        $('#<%= btnteamOwner.ClientID %>').show();
                        $('#<%= btnRemoveTeamMember.ClientID %>')
                            .removeClass().addClass("btn btn-danger").removeAttr("style").removeAttr("disabled title data-toggle");

                        if (response.isTeamowner === true) {
                            $('#<%= btnRemoveTeamMember.ClientID %>').hide();
                            $('#<%= btnteamOwner.ClientID %>').hide();
                        }
                        if (response.isShow === true) {
                            $('#<%= btnRemoveTeamMember.ClientID %>').hide();
                            $('#<%= btnteamOwner.ClientID %>').hide();
                        }
                        if (response.iteamadmin === true) {
                            $('#<%= btnRemoveTeamMember.ClientID %>').hide();
                            $('#<%= btnteamOwner.ClientID %>').hide();
                        }
                        if (response.isteamadminowner === true) {
                            if (response.isteamadminowner === true) {
                                $('#<%= btnRemoveTeamMember.ClientID %>').hide();
                                $('#<%= btnteamOwner.ClientID %>').hide();
                            }
                        }
                        else {
                            if (response.iteamadminone === true) {
                                $('#<%= btnteamOwner.ClientID %>').hide();
                            }
                        }
                        if (response.isUserhead === true) {
                            $('#<%= btnteamOwner.ClientID %>').hide();
                            $('#<%= btnRemoveTeamMember.ClientID %>').show();
                            $('#<%= btnRemoveTeamMember.ClientID %>')
                                .removeClass()
                                .addClass('btn btn-secondary disabled')
                                .attr({
                                    'data-toggle': 'tooltip',
                                    'title': 'You made a new team owner. You can remove this person later, or they are the current team owner.',
                                    'disabled': true
                                })
                                .css('border', '2px solid black');
                        }
                    <%--    if (response.isShow === true || response.isTeamowner === true) {
                            $('#<%= btnRemoveTeamMember.ClientID %>').hide();
                            $('#<%= btnteamOwner.ClientID %>').hide();
                        }
                        if (response.teamadmin === true)
                        {
                            $('#<%= btnteamOwner.ClientID %>').hide();
                        }
                        if (response.isUserInThatRole === true) {
                            $('#<%= btnteamOwner.ClientID %>').hide();
                            $('#<%= btnRemoveTeamMember.ClientID %>')
                                .removeClass()
                                .addClass('btn btn-secondary disabled')
                                .attr({
                                    'data-toggle': 'tooltip',
                                    'title': 'You made a new team owner. You can remove this person later, or they are the current team owner.',
                                    'disabled': true
                                })
                                .css('border', '2px solid black');  
                        }--%>

                        console.log("User data fetched successfully:", response);
                    } else {
                        $('#loader').hide();
                        alert("Failed to fetch user data.");
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error fetching user data:", error);
                    alert("An error occurred while fetching user data.");
                }
            });
        }
        function btnMakeTeamOwner() {
            event.preventDefault();
            swal({
                title: 'Are you sure?',
                text: "Do you want to make this user the team owner?",
                icon: "success",
                buttons: ["No", "Yes, make owner!"],
            }).then(function (willSet) {
                if (willSet) {
                    var selectedUser = currentUserId;

                    $.ajax({
                        type: "POST",
                        url: "People.aspx/MakeTeamOwner",
                        data: JSON.stringify({ selectedUser: selectedUser }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            swal("Success", response.d, "success");
                            $('#manageMemberModal').modal('hide');
                        },
                        error: function (xhr, status, error) {
                            console.error("Error:", error);
                            swal("Error", "An error occurred while making the user a team owner.", "error");
                        }
                    });
                }
            });
        }
        function btnMakeTeamRemove() {
            event.preventDefault();
            swal({
                title: 'Are you sure?',
                text: "Are you sure you want to remove this team member?",
                icon: "warning",
                buttons: ["No", "Yes, Remove from team!"],
                dangerMode: true
            }).then(function (willSet) {
                if (willSet) {
                    var selectedUser = currentUserId;

                    $.ajax({
                        type: "POST",
                        url: "People.aspx/RemoveTeamMember",
                        data: JSON.stringify({ selectedUser: selectedUser }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            debugger;
                            $("a.Userlink[href*='" + selectedUser + "']").parents('tr').remove();
                            swal("Success", response.d, "success");
                            $('#manageMemberModal').modal('hide');
                        },
                        error: function (xhr, status, error) {
                            console.error("Error:", error);
                            swal("Error", "An error occurred while making the user a team owner.", "error");
                        }
                    });
                }
            });
        }
        function btnManageSaveChanges() {

            if (!currentUserId) {
                alert("No user selected.");
                return;
            }
            var vettingStatus = $("[name*='rblManageUserStatus']:checked").val();
            var vettingNotes = document.getElementById('<%= txtManageVettingNotes.ClientID %>').value;
            var showTeamVerified = false;
            var showTeamVerifiedVisible = document.getElementById('<%= hiddenShowTeamVerified.ClientID %>').value === "1";
            if (showTeamVerifiedVisible) {
                showTeamVerified = document.getElementById('<%= cbTeamVerified.ClientID %>').checked;
            } else {
                showTeamVerified = false;
            }
            var stabilityVerified = false;
            var makeTeamAdministrator = false;
            var isAdmin = document.getElementById('<%= hiddenAdminRole.ClientID %>').value === "1";
            //if (isAdmin) {
            stabilityVerified = document.getElementById('<%= cbStabilityVerified.ClientID %>').checked;
            makeTeamAdministrator = document.getElementById('<%= chkManageTeamAdministrator.ClientID %>').checked;
            //}

            var $input = $("input[data-userid='" + currentUserId + "']");

            // Traverse up to the panel container
            var $panel = $input.closest('.hpanel');

            // Find the image tags inside the panel
            var $stabilityImg = $panel.find("img[id*='imgStabilityBadge']");
            var $stabilityLink = $panel.find("a[id*='hypStabilityLogo']");
            var $teamLogoImg = $panel.find("img[id*='imgTeamLogo']");
            var $teamLogoLink = $panel.find("a[id*='hypTeamLogo']");
            if (showTeamVerified) {
                $teamLogoImg.show();
                $teamLogoLink.show();
            }
            else {
                $teamLogoImg.hide();
                $teamLogoLink.hide();
            }
            if (stabilityVerified) {
                $stabilityImg.show();
                $stabilityLink.show();
            }
            else {
                $stabilityImg.hide();
                $stabilityLink.hide();
            }

            updateMemberInfo(currentUserId, vettingStatus, vettingNotes, stabilityVerified, showTeamVerified, makeTeamAdministrator);
        }
        function updateMemberInfo(userId, vettingStatus, vettingNotes, stabilityVerified, showTeamVerified, makeTeamAdministrator) {
            var data = {
                action: "update",
                userId: userId,
                vettingStatus: vettingStatus,
                vettingNotes: vettingNotes,
                stabilityVerified: stabilityVerified,
				showTeamLogo: showTeamVerified,
                makeTeamAdministrator: makeTeamAdministrator,
                organizationId: '<%= Request.QueryString["organizationId"] %>'

            };
            $.ajax({
                type: "POST",
                url: "/V1/Handlers/UpdateMemberInfo.ashx",
                data: data,
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.Success) {
                        $('#manageMemberModal').modal('hide');
                    } else {
                        alert("Error: " + response.Message);
                    }
                },
                error: function (xhr, status, error) {
                    divMessageError.style.visibility = 'visible';
                    divErrorMessage.style.visibility = 'visible';
                    divMessageError.hidden = false;
                    divErrorMessage.hidden = false;
                    divErrorMessage.innerHTML += xhr.responseText;
                }
            });
        }
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
    <script type="text/javascript">
        var currentPagination = '';
        var scroll = false;
        $(document).ready(function () {
            $(document).on("click", "#nextBtn", function () {
                if (!$('#nextBtn').hasClass('disabled')) {
                    var totalPages = document.getElementById('<%= totalPageValue.ClientID %>').value;
                    var currentPage = parseInt($('.pagination .active .page-link').text());

                    var newPage = currentPage + 1;
                    if (newPage <= totalPages) {
                        if (newPage > 3) {
                            $(".page-item").not("#previousBtn, #nextBtn").remove();
                            var newPageItem = '';
                            for (var i = 0; i < Math.min(3, totalPages); i++) {

                                newPageItem += '<li class="page-item' + (i == 1 ? " active" : "") + (i == 2 ? " firstpn" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + newPage - 2) + '); return false;" tabindex="' + (i + newPage - 2) + '" href="javascript:void(0)">' + (i + newPage - 2) + '</a></li>';
                            }
                            if (totalPages > 3 && newPage < Math.max(totalPages - 3, 3)) {
                                newPageItem += '<li class="page-item disabled dotpage"><a class="page-link">...</a></li>';
                            }
                            for (var i = Math.max(totalPages - 3, 3), j = 0; i < totalPages; i++, j++) {
                                newPageItem += '<li class="page-item' + (i == (newPage - 1) ? " " : "") + (j == 0 ? " lastpn" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + 1) + '); return false;" tabindex="' + (i + 1) + '" href="javascript:void(0)">' + (i + 1) + '</a></li>';
                            }
                            if (newPageItem != '') $('#previousBtn').after(newPageItem);
                            if (totalPages <= newPage + 3) {
                                $('#nextBtn').addClass('disabled');
                            } else {
                                $('#nextBtn').removeClass('disabled');
                            }
                        }
                        triggerSearch(newPage);
                    }

                }
            });
        });

        $(document).on("click", "#previousBtn", function () {
            if (!$('#previousBtn').hasClass('disabled')) {

                var totalPages = document.getElementById('<%= totalPageValue.ClientID %>').value;
                var currentPage = parseInt($('.pagination .active .page-link').text());
                var newPage = currentPage - 1;

                if (newPage >= 1) {

                    $(".page-item").not("#previousBtn, #nextBtn").remove();
                    var newPageItem = '';

                    // Displaying the first 3 pages
                    for (var i = 0; i < Math.min(3, totalPages); i++) {
                        newPageItem += '<li class="page-item' + (i == (newPage - 1) ? " active" : "") + (i == 2 ? " firstpn" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + 1) + '); return false;" tabindex="' + (i + 1) + '" href="javascript:void(0)">' + (i + 1) + '</a></li>';
                    }

                    // Show "..." if more than 3 pages
                    if (totalPages > 3) {
                        newPageItem += '<li class="page-item disabled dotpage"><a class="page-link">...</a></li>';
                    }

                    // Displaying the last 3 pages
                    for (var i = Math.max(totalPages - 3, 3), j = 0; i < totalPages; i++, j++) {
                        newPageItem += '<li class="page-item' + (i == (newPage - 1) ? " active" : "") + (j == 0 ? " lastpn" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + 1) + '); return false;" tabindex="' + (i + 1) + '" href="javascript:void(0)">' + (i + 1) + '</a></li>';
                    }

                    // Insert new page items after the previous button
                    if (newPageItem != '') $('#previousBtn').after(newPageItem);

                    // Disable Next button if on last page
                    if (totalPages <= newPage) {
                        $('#nextBtn').addClass('disabled');
                    } else {
                        $('#nextBtn').removeClass('disabled');
                    }

                    // Trigger the search for the previous page
                    triggerSearch(newPage);
                }
            }
        });




        function triggerSearch(pn) {
            document.getElementById('<%= currentPageValue.ClientID %>').value = pn;
            $(".pagination .page-item").removeClass("active");
            $(".pagination .page-link[tabindex='" + pn + "']").closest(".page-item").addClass("active");

            var totalPages = parseInt(document.getElementById('<%= totalPageValue.ClientID %>').value);

            if (pn <= 1) {
                $('#previousBtn').addClass('disabled');
            } else {
                $('#previousBtn').removeClass('disabled');
            }

            if (pn + 3 >= totalPages) {
                $('#nextBtn').addClass('disabled');
            } else {
                $('#nextBtn').removeClass('disabled');
            }

            currentPagination = $('.navClass').html();

            __doPostBack('<%= SearchButton.UniqueID %>', '');
        }

        function searchButton() {
            ;
            // Set a value to the hidden field
            document.getElementById('<%= currentPageValue.ClientID %>').value = 1; // Set custom value here    
        }
        function resetSearch() {
            // Set a value to the hidden field   
            document.getElementById('<%= currentPageValue.ClientID %>').value = 1; // Set custom value here                        
            document.getElementById('<%= filter.ClientID %>').value = ''; // Set custom value here                        

            document.getElementById('<%= txtIsVetted.ClientID %>').checked = false;// Set custom value here                        
            document.getElementById('<%= txtOptedSMS.ClientID %>').checked = false; // Set custom value here                        
            document.getElementById('<%= txtEmailconnect.ClientID %>').checked = false; // Set custom value here                        
            document.getElementById('<%= txtTeamVerified.ClientID %>').checked = false; // Set custom value here
            document.getElementById('<%= txtStabilityVerified.ClientID %>').checked = false; // Set custom value here
            document.getElementById('<%= txtTeamAdministrator.ClientID %>').checked = false; // Set custom value here
            document.getElementById('<%= ddlEvent.ClientID %>').value = ''; // Set custom value here
            $('#hiddenEvent').val('');
            document.getElementById('<%= ddlTraining.ClientID %>').value = ''; // Set custom value here
            document.getElementById('radiusSection').style.display = 'none';

            let radiusSlider = document.getElementById('radiusSlider');
            let radiusDisplay = document.getElementById('radiusValue');
            let hiddenRadius = document.getElementById('hiddenRadius');

            if (radiusSlider) {
                radiusSlider.value = 100;
                radiusSlider.dispatchEvent(new Event('input'));

                if (radiusDisplay) {
                    radiusDisplay.innerText = '100 km';
                }
                if (hiddenRadius) {
                    hiddenRadius.value = 100;
                }
            }



            $('#StartDate, #EndDate').val('').datepicker('update');

            $('#ContentPlaceHolder1_ddlSkills').multiselect('deselectAll', false);
            $('#ContentPlaceHolder1_ddlSkills').multiselect('refresh');

            $('#ContentPlaceHolder1_ddlResources').multiselect('deselectAll', false);
            $('#ContentPlaceHolder1_ddlResources').multiselect('refresh');

            // Trigger the search button click event
            __doPostBack('<%= SearchButton.UniqueID %>', '');
        }
        function updatePagination() {
            var totalPage = document.getElementById('<%= totalPageValue.ClientID %>').value;
            var currentPage = document.getElementById('<%= currentPageValue.ClientID %>').value;
            if (currentPage == '1') {
                $(".page-item").not("#previousBtn, #nextBtn").remove();
                var newPageItem = ''
                for (var i = 0; i < parseFloat(totalPage); i++) {
                    if (i < 3) {
                        newPageItem += '<li class="page-item' + (i == 0 ? " active" : "") + (i == 2 ? " firstpn" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + 1) + '); return false;" tabindex="' + (i + 1) + '" href="javascript:void(0)">' + (i + 1) + '</a></li>';
                    }
                    else if (i > 3 && i <= 4) {
                        newPageItem += '<li class="page-item disabled dotpage"><a class="page-link">...</a></li>';
                    }

                }
                for (var i = Math.max(parseFloat(totalPage) - 3, 3), j = 0; i < parseFloat(totalPage); i++, j++) {

                    newPageItem += '<li class="page-item' + (j == 0 ? " lastpn" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + 1) + '); return false;" tabindex="' + (i + 1) + '" href="javascript:void(0)">' + (i + 1) + '</a></li>';
                }
                if (newPageItem != '') $('#previousBtn').after(newPageItem);
                if (totalPage <= currentPage) $('#nextBtn').addClass('disabled');
                else $('#nextBtn').removeClass('disabled');
            }
            else {
                $('.navClass').html(currentPagination);
            }
            updateCheckboxSelection();
            checkSkillResourceFilter();
        }
        function updateSelectedUsers() {
            var selectedUserIds = [];
            var userCheckboxes = document.querySelectorAll(".select-user");
            var hiddenField = document.getElementById("<%= hdnSelectedUsers.ClientID %>");

            userCheckboxes.forEach(function (checkbox) {
                if (checkbox.checked) {
                    selectedUserIds.push(checkbox.getAttribute("data-userid"));
                }
            }); n

            hiddenField.value = selectedUserIds.join(",");
        }
        function sendEmail() {
            var selectedUserIds = document.getElementById('<%= hdnSelectedUsers.ClientID %>').value;
            var userMessage = document.getElementById('<%= txtEmail.ClientID %>').value; // here
            var l = $('.ladda-button-email').ladda();
            if (selectedUserIds.trim() === "" || userMessage.trim() === "") {
                swal({
                    title: "Send Email",
                    text: "Please enter a message and select users.",
                    type: "warning"
                });
                return;
            }

            l.ladda('start');
            $.ajax({
                type: "POST",
                url: "People.aspx/SendEmail",
                data: JSON.stringify({ selectedUserIds: selectedUserIds, userMessage: userMessage }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // Clear the textbox
                    l.ladda('stop');
                    document.getElementById('<%= txtEmail.ClientID %>').value = "";
                    document.getElementById('<%= chkSelectAll.ClientID %>').checked = false;
                    // Get all checked checkboxes and uncheck them
                    document.querySelectorAll(".select-user:checked").forEach(cb => cb.checked = false);

                    swal({
                        title: "Send Email",
                        text: response.d,
                        type: "success"
                    });
                    //document.getElementById('<%= txtEmail.ClientID %>').value = ""; // Clear text box
                    $("#txtEmail").val("");
                    $('#<%=txtEmail.ClientID%>').summernote('code', '');
                },
                error: function (xhr, status, error) {
                    console.error(xhr.responseText);
                    l.ladda('stop');
                    swal({
                        title: "Send Email",
                        text: "An error occurred while sending the email. " + xhr.responseText,
                        type: "warning"
                    });
                }
            });
            updateCheckboxSelection();
        }

        function updateSelectedUsers() {
            var selectedUserIds = [];
            var userCheckboxes = document.querySelectorAll(".select-user");
            var hiddenField = document.getElementById("<%= hdnSelectedUsers.ClientID %>");

            userCheckboxes.forEach(function (checkbox) {
                if (checkbox.checked) {
                    selectedUserIds.push(checkbox.getAttribute("data-userid"));
                }
            });

            hiddenField.value = selectedUserIds.join(",");
        }

        function sendSms() {
            var selectedUserIds = document.getElementById('<%= hdnSelectedUsers.ClientID %>').value;
            var userMessage = document.getElementById('<%= txtsms.ClientID %>').value;
            var l = $('.ladda-button-sms').ladda();
            if (selectedUserIds.trim() === "" || userMessage.trim() === "") {
                swal({
                    title: "Send SMS",
                    text: "Please enter a message and select users.",
                    type: "warning"
                });
                return;
            }
            // Start loading
            l.ladda('start');
            $.ajax({
                type: "POST",
                url: "People.aspx/SendSms",
                data: JSON.stringify({ selectedUserIds: selectedUserIds, smsMessage: userMessage }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // Clear the textbox
                    l.ladda('stop');
                    document.getElementById('<%= txtsms.ClientID %>').value = "";
                    document.getElementById('<%= chkSelectAll.ClientID %>').checked = false;
                    // Get all checked checkboxes and uncheck them
                    document.querySelectorAll(".select-user:checked").forEach(cb => cb.checked = false);
                    swal({
                        title: "Send SMS",
                        text: response.d,
                        type: "success"
                    });
                    document.getElementById('<%= txtsms.ClientID %>').value = ""; // Clear text box
                },
                error: function (xhr, status, error) {
                    console.error(xhr.responseText);
                    l.ladda('stop');
                    swal({
                        title: "Send SMS",
                        text: "An error occurred while sending sms. " + xhr.responseText,
                        type: "warning"
                    });
                }
            });
        }
        $(document).on("click", ".pagination .page-link", function () {
            scroll = true;
        });

        function updateCheckboxSelection() {
            ;
            var selectAllCheckbox = document.getElementById("chkSelectAll");
            var userCheckboxes = document.querySelectorAll(".select-user");
            var hiddenField = document.getElementById("<%= hdnSelectedUsers.ClientID %>");

            if (selectAllCheckbox != null) {
                selectAllCheckbox.addEventListener("change", function () {
                    var isChecked = this.checked;
                    userCheckboxes.forEach(function (checkbox) {
                        checkbox.checked = isChecked;
                    });
                    updateSelectedUsers();
                });
            }

            userCheckboxes.forEach(function (checkbox) {
                checkbox.addEventListener("change", function () {
                    var allChecked = Array.from(userCheckboxes).every(cb => cb.checked);
                    selectAllCheckbox.checked = allChecked;
                    updateSelectedUsers();
                });
            });
        }


        $(document).on("click", ".pagination .page-link", function () {
            window.scrollTo({
                top: 300,
                behavior: 'instant'
            });
        });



    </script>
    <script type="text/javascript">
        function onSkillClick(skillId) {
            var listBox = document.getElementById('<%= ddlSkills.ClientID %>');
            var options = listBox && listBox.options;

            if (!options) return;

            for (var i = 0; i < options.length; i++) {
                if (options[i].value === skillId) {
                    options[i].selected = true;
                    var listItem = document.querySelector(`li input[type="checkbox"][value="${skillId}"]`);
                    if (listItem) {
                        listItem.checked = true;
                        var parentLi = listItem.closest('li');
                        if (parentLi) {
                            parentLi.classList.add('active');
                        }
                    }
                    break;
                }
            }
            document.getElementById('<%= currentPageValue.ClientID %>').value = 1;
            __doPostBack('<%= SearchButton.UniqueID %>', '');
        }
        function onResourceClick(resourceId) {
            var listBox = document.getElementById('<%= ddlResources.ClientID %>');
            var options = listBox && listBox.options;
            if (!options) return;
            for (var i = 0; i < options.length; i++) {
                if (options[i].value === resourceId) {
                    options[i].selected = true;
                    break;
                }
            }
            var listItem = document.querySelector(`li input[type="checkbox"][value="${resourceId}"]`);
            if (listItem) {
                listItem.checked = true;
                var parentLi = listItem.closest('li');
                if (parentLi) {
                    parentLi.classList.add('active');
                }
            }
            document.getElementById('<%= currentPageValue.ClientID %>').value = 1;
            __doPostBack('<%= SearchButton.UniqueID %>', '');
        }
        function clearSkillResource() {
            var anySkill = false;
            var anyResource = false;
            var listBoxS = document.getElementById('<%= ddlSkills.ClientID %>');
            var optionsS = listBoxS && listBoxS.options;
            anySkill = Array.from(listBoxS.options).filter(option => option.selected).length > 0;
            if (optionsS && anySkill) {
                for (var i = 0; i < optionsS.length; i++) {
                    optionsS[i].selected = false;
                    var listItem = document.querySelector(`li input[type="checkbox"][value="${optionsS[i].value}"]`);
                    if (listItem) {
                        listItem.checked = false;
                        var parentLi = listItem.closest('li');
                        if (parentLi) {
                            parentLi.classList.remove('active');
                        }
                    }
                }
            }

            var listBoxR = document.getElementById('<%= ddlResources.ClientID %>');
            var optionsR = listBoxR && listBoxR.options;
            anyResource = Array.from(listBoxR.options).filter(option => option.selected).length > 0;
            if (optionsR && anyResource) {
                for (var i = 0; i < optionsR.length; i++) {
                    optionsR[i].selected = false;
                    var listItem = document.querySelector(`li input[type="checkbox"][value="${optionsR[i].value}"]`);
                    if (listItem) {
                        listItem.checked = false;
                        var parentLi = listItem.closest('li');
                        if (parentLi) {
                            parentLi.classList.remove('active');
                        }
                    }

                }

            }
            if (anySkill || anyResource) {
                document.getElementById('<%= currentPageValue.ClientID %>').value = 1;
                __doPostBack('<%= SearchButton.UniqueID %>', '');
            }
        }
        function checkSkillResourceFilter() {
            var anySkill = false;
            var anyResource = false;
            var listBoxS = document.getElementById('<%= ddlSkills.ClientID %>');
            anySkill = Array.from(listBoxS.options).filter(option => option.selected).length > 0;

            var listBoxR = document.getElementById('<%= ddlResources.ClientID %>');
            anyResource = Array.from(listBoxR.options).filter(option => option.selected).length > 0;

            if (anySkill || anyResource) {
                $("#clearSkillResourceId").show();
            } else {
                $("#clearSkillResourceId").hide();
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:TeamHeader runat="server" ID="ucTeamHeader" />
    <asp:HiddenField ID="hdnSelectedUsers" runat="server" />

    <div id="divEmail" runat="server" class="input-group">
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter Email Text"
            ClientIDMode="Static" TextMode="MultiLine" ValidateRequestMode="Disabled" Rows="3"></asp:TextBox>
        <div class="input-group-append">
            <button type="button" class="ladda-button ladda-button-demo ladda-button-email btn btn-primary b2"
                data-style="slide-right" onclick="sendEmail()">
                Send Email</button>
        </div>
    </div>
    <div id="divSms" runat="server" class="input-group">
        <textarea id="txtsms" runat="server" cssclass="form-control" placeholder="Enter SMS Text (Max 450 characters)"
            rows="6" cols="95"></textarea>
        <div class="input-group-append">
            <button type="button" class="ladda-button ladda-button-demo ladda-button-sms btn btn-primary "
                data-style="slide-right" onclick="sendSms()">
                Send SMS</button>
        </div>
    </div>

    <div class="panel-heading">
        <asp:HyperLink ID="hypInviteTeamMembers" runat="server" Visible="false" Text="Invite Team Members" CssClass="btn btn-sm btn-info"></asp:HyperLink>
        <asp:HyperLink ID="hypPrintableTeamList" runat="server" Visible="false" Target="_blank" Text="Printable List" CssClass="btn btn-sm btn-info"></asp:HyperLink>
    </div>

    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div class="panel-body" style="margin-bottom: -27px; padding: 0px;">
        <div class="col-lg-12">
            <div class="row">
                <div class="hpanel hblue">
                    <div class="panel-tools">
                        <button class="btn btn-link toggle-search-btn" id="search" type="button" data-toggle="collapse"
                            data-target="#searchFilters" aria-expanded="false" aria-controls="searchFilters">
                            <i class="fa fa-chevron-down"></i>
                        </button>
                    </div>
                    <h4 style="margin-left: 18px;">Search</h4>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="divTeamListMessage" runat="server" class="alert alert-warning text-center" style="margin-bottom: 20px;" visible="false">
                                <asp:Literal ID="litMessage" runat="server"></asp:Literal>
                            </div>
                            <div id="divFilterMessage" runat="server" class="alert alert-info text-center" style="margin-bottom: 20px;" visible="false">
                                <asp:Literal ID="litFilterMessage" runat="server"></asp:Literal>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="SearchButton" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <div class="" data-child="hpanel" data-effect="fadeInDown" runat="server" id="hpanelSearchMembers"
                        visible="false">
                        <%--<div class="hpanel" runat="server" id="hpanelJoin" visible="true">
                            <a href="/V1/Profile/EditNonProfits.aspx">Join This Team</a>
                        </div>--%>
                        <div class="container-search">
                            <!-- Collapsible Search Filters -->
                            <div class="collapse col-sm-12" id="searchFilters">
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <div class="form-group fix">
                                            <b>Search By Member Name  :</b>
                                            <asp:TextBox ID="filter" runat="server" CssClass="form-control" placeholder="Search By Member Name "></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-6 mb-3">
                                        <div class="form-group fix">
                                            <b>Training :</b>
                                            <asp:DropDownList ID="ddlTraining" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <div class="form-group fix">
                                            <b class="text-line">Skills :</b>
                                            <asp:ListBox ID="ddlSkills" runat="server" CssClass="form-control multiselect" SelectionMode="Multiple"
                                                AppendDataBoundItems="true"></asp:ListBox>
                                        </div>
                                    </div>

                                    <div class="col-md-6 mb-3">
                                        <div class="form-group fix">
                                            <b class="text-line">Resources :</b>
                                            <asp:ListBox ID="ddlResources" runat="server" CssClass="form-control multiselect"
                                                SelectionMode="Multiple" AppendDataBoundItems="true"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <div class="form-group fix">
                                            <b class="text-line">Available From :</b>
                                            <asp:TextBox ID="StartDate" runat="server" CssClass="form-control datepicker" placeholder="Start Date"
                                                autocomplete="off" ClientIDMode="Static" />
                                        </div>
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <div class="form-group fix">
                                            <b class="text-line">Available To :</b>
                                            <asp:TextBox ID="EndDate" runat="server" CssClass="form-control datepicker" placeholder="End Date"
                                                autocomplete="off" ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3" style="margin-left: -13px;">
                                    <div class="form-group margin ">
                                        <b class="text-line">Portal:</b>
                                        <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3" id="radiusSection" style="display: none;">
                                    <div class="form-group fix">
                                        <b>Radius (km):</b>
                                        <input type="range" id="radiusSlider" min="100" max="1000" step="100" value="100"
                                            class="form-control">
                                        <span id="radiusValue">100 km</span>
                                        <input type="hidden" id="hiddenRadius" name="radiusSlider" value="0" />
                                        <input type="hidden" id="hiddenEvent" name="selectedEvent" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12" style="margin-left: 30px;">
                                        <div class="row">
                                            <!-- Column 1: First 3 Checkboxes -->
                                            <div class="col-sm-6">
                                                <div class="form-check mb-2">
                                                    <asp:CheckBox ID="txtIsVetted" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%=txtIsVetted.ClientID%>">Is Vetted</label>
                                                </div>
                                                <div class="form-check mb-2">
                                                    <asp:CheckBox ID="txtTeamVerified" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%=txtTeamVerified.ClientID%>">Team Verified</label>
                                                </div>
                                                <div class="form-check mb-2">
                                                    <asp:CheckBox ID="txtStabilityVerified" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%=txtStabilityVerified.ClientID%>">Stability Verified</label>
                                                </div>
                                                <div class="form-check mb-2">
                                                    <asp:CheckBox ID="txtTeamAdministrator" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%=txtTeamAdministrator.ClientID%>">Team Administrator</label>
                                                </div>
                                            </div>

                                            <!-- Column 2: Last 2 Checkboxes -->
                                            <div class="col-sm-6">
                                                <div class="form-check mb-2">
                                                    <asp:CheckBox ID="txtOptedSMS" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%=txtOptedSMS.ClientID%>">Accepts SMS Messages</label>
                                                </div>
                                                <div class="form-check mb-2">
                                                    <asp:CheckBox ID="txtEmailconnect" runat="server" CssClass="form-check-input" />
                                                    <label class="form-check-label" for="<%=txtEmailconnect.ClientID%>">Accepts Email Messages</label>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div class="row mt-4" style="margin-right: 6px; margin-bottom: 8px;">
                                    <div class="col-md-12 text-right ">
                                        <asp:Button ID="SearchButton" runat="server" CssClass="btn btn-info btn-sm me-2" Text="Search" OnClientClick="searchButton();" OnClick="SearchButton_Click" />
                                        <button type="button" class="btn btn-danger btn-sm" onclick="resetSearch()">Clear</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="row">
                <div class="custom-flex-end">
                    <button type="button" style="display: none" id="clearSkillResourceId" onclick="clearSkillResource()"
                        class="btn btn-sm btn-info">
                        Clear Skills/Resources</button>
                </div>
            </div>
        </div>
    </div>
    <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="select-all" Text="Select All"
        ClientIDMode="Static" />
    <asp:UpdatePanel runat="server" ID="updatePeopleList" UpdateMode="Conditional">
        <ContentTemplate>
            <%--   <div id="loader" style="text-align: center; padding: 20px;">
        <img src="/path-to-your-loader.gif" alt="Loading..." />
    </div>--%>
            <%--  <div  id="loader"  class="spinner-border" role="status">
  <span class="sr-only">Loading...</span>
</div>--%>
            <div id="loader" class="custom-spinner"></div>
            <asp:Panel runat="server" ID="pnlTable">
                <asp:HiddenField ID="currentPageValue" runat="server" />
                <asp:HiddenField ID="totalPageValue" runat="server" />
                <table id="tblVolunteers" class="footable" data-page-size="20" data-filter="#filter">
                    <tbody>
                        <asp:Repeater ID="rptVolunteers" runat="server" OnItemDataBound="rptVolunteers_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td style="background-color: white;">
                                        <div class="hpanel">
                                            <div class="panel-body">
                                                <h5 class="m-b-xs" id="h5Container" runat="server" style="align-items: center; justify-content: normal;">
                                                    <input type="checkbox" runat="server" class="select-user" style="margin-top: -2px;"
                                                        data-userid='<%# Eval("UserID") %>' visible='<%# (Request.QueryString["type"] == "email" || Request.QueryString["type"] == "sms") %>' />

                                                    &nbsp;&nbsp;                              

                                                <uc1:TeamLogo runat="server" ID="ucUserNameWithBadges" />
                                                     <span class="badge ms-2" id="rankBadge" runat="server" visible="false"></span>
                                                </h5>

                                                <p>
                                                    <asp:Literal ID="litMemberInfo" runat="server"></asp:Literal>
                                                    <asp:Literal ID="litDescription" runat="server"></asp:Literal>
                                                </p>
                                                <div class="pull-right">
                                                    <asp:Button ID="btnContact" runat="server" Text="Message" Visible="false" CssClass="btn btn-success messageButton"
                                                        data-toggle="modal" data-target="#messageMemberModal"></asp:Button>
                                                </div>
                                                <asp:Literal ID="litSkills" runat="server"></asp:Literal>
                                                <asp:Literal ID="litResources" runat="server"></asp:Literal>
                                            </div>
                                            <div class="panel-footer d-flex justify-content-between align-items-center" id="divFooter" runat="server" visible="false">
                                                <div class="pull-right">
                                                    <asp:Button ID="btnManage" runat="server" Text="Edit User Verification Status" CssClass="btn btn-primary manageButton float-end"
                                                        data-toggle="modal" data-target="#manageMemberModal"
                                                        data-userid='<%# Eval("UserID") %>'
                                                        OnClientClick="setUserId(this); fetchUserData(); return false;"></asp:Button>
                                                </div>

                                                <div class="text-muted small" style="width: 100%;">
                                                    <asp:Literal ID="litVettingInfo" runat="server"></asp:Literal>
													<i class="fa fa-lock pull-right" style="color:#5E2E91;"> </i><span style="color:#5E2E91;" class="pull-right">Team Administrators Only</span>
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
                                <%--<asp:Button ID="TriggerSearchButton" runat="server" Text="Trigger Search" OnClientClick="triggerSearch(1); return false;" />--%>
                                <nav class="navClass" aria-label="Page navigation example">
                                    <ul class="pagination justify-content-center">
                                        <li class="page-item disabled" id="previousBtn">
                                            <a class="page-link" href="javascript:void(0)" tabindex="-1">Previous</a>
                                        </li>

                                        <li class="page-item" id="nextBtn">
                                            <a class="page-link" href="javascript:void(0)" tabindex="0">Next</a>
                                        </li>
                                    </ul>
                                </nav>

                            </td>
                        </tr>
                    </tfoot>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="SearchButton" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>



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

    <asp:HiddenField ID="hiddenAdminRole" runat="server" />
    <asp:HiddenField ID="hiddenShowTeamVerified" runat="server" />

    <div class="modal fade" id="manageMemberModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="color-line"></div>
                <div class="modal-header text-center">
                    <h5 class="modal-title">Update Member Status</h5>

                </div>
                <!-- Success and Error Messages -->
                <div id="divManageSuccess" class="alert alert-success text-uppercase" style="display: none;">
                    <i class="fa fa-check-circle"></i>Changes saved successfully.
                </div>
                <div id="divManageError" class="alert alert-warning text-uppercase" style="display: none;">
                    <i class="fa fa-exclamation-triangle"></i>
                    <div id="divManageErrorMessage"></div>
                </div>
                <!-- Modal body -->
                <div class="modal-body">
                    <asp:PlaceHolder ID="phAdminControls" runat="server" Visible="false">
                        <div class="form-group form-check">
                            <asp:CheckBox ID="cbTeamVerified" runat="server" class="form-check-input" />
                            <label class="form-check-label" runat="server" id="lblTeamVerified" for="<%= chkTeamVerified.ClientID %>">
                                Team Verified
                            </label>
                        </div>
                        <div id="divShowTeamVerifiedFeatures" runat="server">
                            <div class="form-group form-check">
                                <asp:CheckBox ID="cbStabilityVerified" runat="server" class="form-check-input" />
                                <label class="form-check-label" for="<%= cbStabilityVerified.ClientID %>">
                                    Stability Verified
                                </label>
                            </div>
                            <div class="form-group form-check">
                                <asp:CheckBox ID="chkManageTeamAdministrator" runat="server" class="form-check-input" />
                                <label class="form-check-label" for="<%= chkManageTeamAdministrator.ClientID %>">
                                    Make Team Administrator
                                </label>
                            </div>

                        </div>
                    </asp:PlaceHolder>
                </div>
                <div class="form-group">
                    <label for="rblManageUserStatus">Update Member Vetting Status:</label>
                    <asp:RadioButtonList ID="rblManageUserStatus" runat="server" CssClass="form-check">
                        <asp:ListItem Text="Start Vetting" Value="1" CssClass="form-check-input"></asp:ListItem>
                        <asp:ListItem Text="Passed Vetting" Value="2" CssClass="form-check-input"></asp:ListItem>
                        <asp:ListItem Text="Failed Vetting" Value="3" CssClass="form-check-input"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>

                <!-- Vetting Notes Textarea -->
                <div class="form-group">
                    <label for="txtManageVettingNotes">Vetting Notes:</label>
                    <asp:TextBox ID="txtManageVettingNotes" TextMode="MultiLine" runat="server" class="form-control" placeholder="Enter Vetting Notes"></asp:TextBox>

                </div>
                <div class="modal-footer">
					<div class="row">
						<div class="col-sm-8">
							<button type="button" class="btn btn-success" runat="server" id="btnteamOwner" visible="false" onclick="btnMakeTeamOwner();">
								Make Team Owner
							</button>
							<button type="button" class="btn btn-danger" runat="server" id="btnRemoveTeamMember" visible="false" onclick="btnMakeTeamRemove();">
								Remove Team Member
							</button>
						</div>
						<div class="col-sm-4">
							<button type="button" class="btn btn-primary" id="btnManage" onclick="btnManageSaveChanges();">
								Save Changes
							</button>
							<button type="button" class="btn btn-default" data-dismiss="modal">
								Close
							</button>
						</div>
					</div>
                </div>
            </div>
        </div>
    </div>
    <uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>
