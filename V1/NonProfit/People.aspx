<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="People.aspx.cs" Inherits="V1_NonProfit_People" %>

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

        .m-b-xs {
            margin-bottom: 0;
        }

        .messageButton {
            margin-left: auto;
            padding: 5px 15px;
            font-size: 14px;
            border-radius: 5px;
        }

        .stability-badge {
            margin-right: 4px !important;
        }

        .form-check label {
            margin-left: 3px;
        }

        .justify-content-center {
            display: flex !important;
            justify-content: center !important;
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
        });
        var currentUserId = null;
        function setUserId(button) {
            currentUserId = button.getAttribute('data-userid');
            return false;
        }
        function fetchUserData() {
            if (!currentUserId) {
                alert("User ID not set.");
                return;
            }
            $.ajax({
                type: "GET",
                url: "/V1/Handlers/UpdateMemberInfo.ashx",
                data: { action: "fetch", userId: currentUserId },
                dataType: "json",
                success: function (response) {
                    if (response.success) {
                        // Populate modal fields with fetched data
                        $("[name*='rblManageUserStatus'][value='" + response.vettingStatus + "']").prop("checked", true);
                        $('#<%= txtManageVettingNotes.ClientID %>').val(response.vettingNotes);
                        $('#<%= chkManageStabilityVerified.ClientID %>').prop('checked', response.stabilityVerified);
                        $('#<%= chkManageShowDonateButton.ClientID %>').prop('checked', response.showTeamLogo);
                        $('#<%= chkManageTeamAdministrator.ClientID %>').prop('checked', response.makeTeamAdministrator);
                        console.log("User data fetched successfully:", response);
                    } else {
                        alert("Failed to fetch user data.");
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error fetching user data:", error);
                    alert("An error occurred while fetching user data.");
                }
            });
        }
        function saveChanges() {
            if (!currentUserId) {
                alert("No user selected.");
                return;
            }
            var vettingStatus = $("[name*='rblManageUserStatus']:checked").val();
            var vettingNotes = document.getElementById('<%= txtManageVettingNotes.ClientID %>').value;
            var showTeamLogo = false;
            // Check if the checkbox is visible using the JavaScript style display property
           <% if (chkManageShowDonateButton.Visible)
        { %>
            showTeamLogo = document.getElementById('<%= chkManageShowDonateButton.ClientID %>').checked;
              <% }
        else
        { %>
            showTeamLogo = false;
           <% } %>
            var stabilityVerified = false;
            var makeTeamAdministrator = false;
             <% if (User.IsInRole("Administrator"))
        { %>
            stabilityVerified = document.getElementById('<%= chkManageStabilityVerified.ClientID %>').checked;
            makeTeamAdministrator = document.getElementById('<%= chkManageTeamAdministrator.ClientID %>').checked;
    <% } %>
            updateMemberInfo(currentUserId, vettingStatus, vettingNotes, stabilityVerified, showTeamLogo, makeTeamAdministrator);
        }
        function updateMemberInfo(userId, vettingStatus, vettingNotes, stabilityVerified, showTeamLogo, makeTeamAdministrator) {
            var data = {
                action: "update",
                userId: userId,
                vettingStatus: vettingStatus,
                vettingNotes: vettingNotes,
                stabilityVerified: stabilityVerified,
                showTeamLogo: showTeamLogo,
                makeTeamAdministrator: makeTeamAdministrator
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
                        location.reload();
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
        $(document).ready(function () {
            // Function to handle the "Next" button click
            $(document).on("click", "#nextBtn", function () {
                if (!$('#nextBtn').hasClass('disabled')) {
                    var totalPages = document.getElementById('<%= totalPageValue.ClientID %>').value;
                    var currentPage = Math.max(...$(".pagination .page-link").map(function () {
                        return parseInt($(this).attr("tabindex")) || 0;
                    }).get());

                    // Increment the current page number
                    if (currentPage <= totalPages) {

                        // Update tabindex for each page link
                        $('.page-item a').each(function (index) {
                            if ($(this).attr('tabindex') != "-1" && $(this).attr('tabindex') != "0") {
                                $(this).attr('tabindex', index + currentPage - 2);
                                $(this).text(index + currentPage - 2);
                                $(this).attr("onclick", `triggerSearch(${(index + currentPage - 2)}); return false;`);
                            }
                        });

                        // Enable Previous button if not on the first page
                        if (currentPage > 2) {
                            $('#previousBtn').removeClass('disabled');
                        }
                    }

                    // Disable Next button if on the last page
                    if (currentPage == totalPages) {
                        $('#nextBtn').addClass('disabled');
                    } else {
                        $('#nextBtn').removeClass('disabled');
                    }
                }
            });

            // Function to handle the "Previous" button click
            $(document).on("click", "#previousBtn", function () {
                if (!$('#previousBtn').hasClass('disabled')) {
                    var totalPages = document.getElementById('<%= totalPageValue.ClientID %>').value;
                    var currentPage = Math.max(...$(".pagination .page-link").map(function () {
                        return parseInt($(this).attr("tabindex")) || 0;
                    }).get());
                    // Decrease the current page number
                    var firstPage = 1 + currentPage - 4;
                    if (firstPage >= 1) {


                        // Update tabindex for each page link
                        $('.page-item a').each(function (index) {
                            if ($(this).attr('tabindex') != "-1" && $(this).attr('tabindex') != "0") {
                                $(this).attr('tabindex', index + currentPage - 4);
                                $(this).text(index + currentPage - 4);
                                $(this).attr("onclick", `triggerSearch(${(index + currentPage - 4)}); return false;`);
                            }
                        });

                        // Enable Next button if not on the last page
                        if (firstPage + 2 < totalPages) {
                            $('#nextBtn').removeClass('disabled');
                        }

                        // Disable Previous button if on the first page                    
                    }
                    if (firstPage == 1) {
                        $('#previousBtn').addClass('disabled');
                    } else {
                        $('#previousBtn').removeClass('disabled');
                    }
                }

            });
        });


        function triggerSearch(pn) {
            // Set a value to the hidden field            
            document.getElementById('<%= currentPageValue.ClientID %>').value = pn; // Set custom value here
            // Remove "active" class from all <li> elements
            $(".pagination .page-item").removeClass("active");

            // Find the <a> tag with matching tabindex and add "active" to its parent <li>
            $(".pagination .page-link[tabindex='" + pn + "']").closest(".page-item").addClass("active");
            currentPagination = $('.navClass').html();
            // Trigger the search button click event
            __doPostBack('<%= SearchButton.UniqueID %>', '');
        }
        function searchButton() {
            // Set a value to the hidden field
            document.getElementById('<%= currentPageValue.ClientID %>').value = 1; // Set custom value here    
        }
        function resetSearch() {
            // Set a value to the hidden field            
            document.getElementById('<%= currentPageValue.ClientID %>').value = 1; // Set custom value here                        
            document.getElementById('<%= filter.ClientID %>').value = ''; // Set custom value here                        
            document.getElementById('txtlocation').value = ''; // Set custom value here                        
            document.getElementById('<%= txtIsVetted.ClientID %>').checked = false;// Set custom value here                        
            document.getElementById('<%= txtOptedSMS.ClientID %>').checked = false; // Set custom value here                        
            document.getElementById('<%= txtEmailconnect.ClientID %>').checked = false; // Set custom value here                        
            document.getElementById('<%= txtIsVerified.ClientID %>').checked = false; // Set custom value here

          
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
                    if (i == 3) break;
                    newPageItem += '<li class="page-item' + (i == 0 ? " active" : "") + '" id="page' + (i + 1) + '"><a class="page-link" onclick="triggerSearch(' + (i + 1) + '); return false;" tabindex="' + (i + 1) + '" href="javascript:void(0)">' + (i + 1) + '</a></li>';
                }
                if (newPageItem != '') $('#previousBtn').after(newPageItem);
                if (totalPage <= 3) $('#nextBtn').addClass('disabled');
                else $('#nextBtn').removeClass('disabled');
            }
            else {
                $('.navClass').html(currentPagination);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:TeamHeader runat="server" ID="ucTeamHeader" />
    <div class="panel-heading">
        <asp:HyperLink ID="hypInviteTeamMembers" runat="server" Visible="false" Text="Invite Team Members"
            CssClass="btn btn-sm btn-info"></asp:HyperLink>
        <asp:HyperLink ID="hypPrintableTeamList" runat="server" Visible="false" Target="_blank"
            Text="Printable List" CssClass="btn btn-sm btn-info"></asp:HyperLink>
    </div>
    <asp:ScriptManager runat="server" ID="ScriptManager1" />
    <div class="panel-body" style="margin-bottom: -27px; padding: 0px;">
        <div class="col-lg-12">
            <div class="row">
                <div class="hpanel hblue">
                    <div class="panel-tools">
                        <button class="btn btn-link toggle-search-btn" type="button" data-toggle="collapse"
                            data-target="#searchFilters" aria-expanded="false" aria-controls="searchFilters">
                            <i class="fa fa-chevron-down"></i>
                        </button>
                    </div>
                    <h4 style="margin-left: 18px;">Search</h4>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="divUpdateMessage" runat="server" class="alert alert-warning text-center" style="margin-bottom: 20px;" visible="false">
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

                    <div class="" data-child="hpanel" data-effect="fadeInDown" runat="server" id="hpanelMembers" visible="false">
                        <div class="hpanel" runat="server" id="hpanelJoin" visible="true">
                            <a href="/V1/Profile/EditNonProfits.aspx">Join This Team</a>
                        </div>
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
                                            <b>Location :</b>
                                            <input type="text" class="form-control" id="txtlocation" placeholder="Enter Location">
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
                                        <asp:Button ID="SearchButton" runat="server" CssClass="btn btn-info btn-sm me-2" Text="Search" OnClientClick="searchButton();" OnClick="SearchButton_Click" />
                                        <button type="button" class="btn btn-danger btn-sm" onclick="resetSearch()">Clear</button>                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="updatePeopleList" UpdateMode="Conditional">
    <ContentTemplate>
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
                                         <div class="panel-footer d-flex justify-content-between align-items-center" id="divFooter"
                                runat="server" visible="false">
                                <div class="pull-right">
                                    <asp:Button ID="btnManage" runat="server" Text="Manage" CssClass="btn btn-primary manageButton float-end"
                                        data-toggle="modal" data-target="#manageMemberModal"
                                        data-userid='<%# Eval("UserID") %>'
                                        OnClientClick="setUserId(this); fetchUserData(); return false;"></asp:Button>
                                </div>

                                <div class="text-muted small" style="width: 100%;">
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
                    <button type="button" class="btn btn-primary" id="btnSend" onclick="return sendClick(this);">
                        Send</button>
                </div>
            </div>
        </div>
    </div>
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
                    <div class="form-group">
                        <label for="rblManageUserStatus">Update Member Vetting Status:</label>
                        <asp:RadioButtonList ID="rblManageUserStatus" runat="server" CssClass="form-check">
                            <asp:ListItem Text="Active" Value="Active" CssClass="form-check-input"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="Inactive" CssClass="form-check-input"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="Pending" CssClass="form-check-input"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>

                    <!-- Vetting Notes Textarea -->
                    <div class="form-group">
                        <label for="txtManageVettingNotes">Vetting Notes:</label>
                        <asp:TextBox ID="txtManageVettingNotes" TextMode="MultiLine" runat="server" class="form-control"
                            placeholder="Enter Vetting Notes"></asp:TextBox>
                    </div>
                    <div class="form-group form-check">
                        <asp:CheckBox ID="chkManageShowDonateButton" runat="server" class="form-check-input" Visible="false" />
                        <asp:Label ID="chkManageShowDonatelabel" runat="server" class="form-check-label"  Visible="false" AssociatedControlID="chkManageShowDonateButton">    Enable Team Logo
                        </asp:Label>
                    </div>
                    <% if (User.IsInRole("Administrator"))
                        { %>
                    <div class="form-group form-check">
                        <asp:CheckBox ID="chkManageStabilityVerified" runat="server" class="form-check-input" />
                        <label class="form-check-label" for="<%= chkManageStabilityVerified.ClientID %>">
                            Stability
                            Verified</label>
                    </div>
                    <div class="form-group form-check">
                        <asp:CheckBox ID="chkManageTeamAdministrator" runat="server" class="form-check-input" />
                        <label class="form-check-label" for="<%= chkManageTeamAdministrator.ClientID %>">
                            Make
                            Team Administrator</label>
                    </div>
                    <% } %>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" id="btnManage" onclick="saveChanges();">
                        Save Changes</button>
                </div>
            </div>
        </div>
    </div>
    <uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>
