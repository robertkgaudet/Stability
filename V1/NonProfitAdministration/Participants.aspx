<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Child.master" AutoEventWireup="true" CodeFile="Participants.aspx.cs" Inherits="V1_NonProfitAdministration_Participants" %>
<%@ Register Src="~/V1/UserControls/TeamLogo.ascx" TagPrefix="uc1" TagName="TeamLogo" %>
<%@ Register Src="~/V1/UserControls/PositionNavigation.ascx" TagPrefix="uc1" TagName="PostionNavigation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
    <style>
    /* Basic hover effect for user-row */
    .user-row:hover {
        background-color: #f0f8ff; /* Light blue background on hover */
        cursor: pointer; /* Change cursor to pointer */
    }

    /* Optional: Selected row styling */
    .user-row.selected {
        background-color: #E8D3FE; /* Slightly darker blue when selected */
    }

    /* Style all <td> elements in the table */
    #modalTable td {
        padding: 5px;
        border: 1px solid #ddd;
        text-align: left;
        font-size: 14px;
        color: #333;
    }

        /* Style specific columns using nth-child */
        #modalTable td:nth-child(1) {
            font-weight: bold; /* Make the first column bold */
        }

        #modalTable td:nth-child(5), #modalTable td:nth-child(6) {
            text-align: center; /* Center-align the Arrival and Departure Time columns */
            background-color: #f9f9f9; /* Set a light background color for these columns */
        }
    .modal-dialog {
        max-width: 90%;
        max-height: 90%;
    }

    .modal-body {
        overflow-y: auto;
        max-height: calc(100vh - 200px); /* Adjust the height based on your modal header and footer */
    }
    .user-name {
        margin-right: 5px;
        color: #333 !important;
        font-weight: normal !important;
        font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
        font-size: 14px;
    }
    #participantName {
    }
    i{
     color:white !important;
 }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <uc1:PostionNavigation runat="server" ID="ucPostionNavigation" />
    <table id="tblParticipants" class="footable table table-stripped toggle-arrow-tiny" data-page-size="50" data-filter=#filter>
        <thead>
            <tr>
                <th data-toggle="true">Participant Name</th>
                <th data-hide="phone, tablet">Email</th>
                <th>Positions Claimed</th>
                <th data-hide="phone, tablet">Last Active</th>
                <th>Delete</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptParticipants" runat="server" OnItemDataBound="rptParticipants_ItemDataBound">
                <ItemTemplate>
                    <tr class="user-row" data-userid='<%# Eval("UserId") %>' data-organizationeventid='<%# Eval("OrganizationEventId") %>'>
                        <td>
                            <uc1:TeamLogo runat="server" ID="ucTeamLogo" UserId='<%# Eval("UserId") %>' PageName="participants" />
                        </td>
                        <td>
                            <asp:Literal ID="litParticipantEmail" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="litPositionCount" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="litLastActiveDateTime" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete"></asp:Button>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="5">
                    <ul class="pagination pull-right"></ul>
                </td>
            </tr>
        </tfoot>
    </table>

    <div id="myModal" class="modal fade hmodal-info" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="color-line"></div>
                <div class="modal-header">
                    <h5 class="modal-title"><span id="participantName"></span></h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <div class="panel-body">
                        <div class="row" style="padding:0px !important;">
                            <div class="col-xs-12">
                                <a id="linkViewProfile" href="#" target="_blank">View Profile</a><br />
                                Email: <span id="participantEmail"></span><br />
                                Phone: <span id="participantPhone"></span>
                                <br />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-body">
                    <table id="modalTable" class="footable" data-paging="false" data-filtering="true" data-page-size="100">
                        <thead>
                            <tr>
                                <th data-name="PositionName">Position Name</th>
                                <th data-name="DeploymentDate">Deployment Date</th>
                                <th data-name="ArrivalTime">Arrival Time</th>
                                <th data-name="DepartureTime">Leave Time</th>
                                <th data-hide="phone" data-name="IsRemote">Remote</th>
                            </tr>
                        </thead>
                        <tbody>
                            <!-- Data will be populated here by FooTable -->
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="5">
                                    <ul class="pagination pull-right"></ul>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <script>

        $(window).on('load', function () {
            $('.footable').footable();
        });

        $(document).ready(function () {
            // When a row with a person's name is clicked
            $('.user-row').on('click', function () {
                // Remove 'selected' class from all rows
                $('.user-row').removeClass('selected');

                // Add 'selected' class to the clicked row
                $(this).addClass('selected');

                var userId = $(this).data('userid'); // Assuming userId is stored in a data attribute
                var organizationEventId = $(this).data('organizationeventid'); // Assuming organizationEventId is stored in a data attribute

                $.ajax({
                    type: "POST",
                    url: "/V1/NonProfitAdministration/Participants.aspx/GetUserEventPositions", // Replace with the actual path to your WebMethod
                    data: JSON.stringify({ userId: userId, organizationEventId: organizationEventId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var data = response.d;

                        if (data.length > 0) {

                            // Assuming you want to display the Fullname of the first participant
                            var clickablePhoneNumber = formatPhoneNumber(data[0].PhoneNumber);
                            var clickableEmail = formatEmail(data[0].Email);
                            $('#participantName').text(data[0].Fullname);
                            var userId = data[0].UserId;
                            var linkElement = document.getElementById("linkViewProfile");
                            linkElement.href = "/V1/Member/Default.aspx?userId=" + userId;
                            document.getElementById("participantEmail").innerHTML = clickableEmail;
                            document.getElementById("participantPhone").innerHTML = clickablePhoneNumber;
                        } else {
                            $('#participantName').text('No participants found');
                        }

                        var $tbody = $('#modalTable tbody');
                        $tbody.empty(); // Clear existing rows

                        // Manually create rows and append them to the table body
                        $.each(data, function (i, item) {

                            var arrivalTimeFormatted = formatTime(item.ArrivalTime, true);
                            var departureTimeFormatted = formatTime(item.DepartureTime, false);
                            var remote = item.IsRemote ? "Remote" : "Not Remote";

                            var row = '<tr>' +
                                '<td>' + item.PositionName + '</td>' +
                                '<td>' + item.DeploymentDate + '</td>' +
                                '<td>' + arrivalTimeFormatted + '</td>' +
                                '<td>' + departureTimeFormatted + '</td>' +
                                '<td>' + remote + '</td>' +
                                '</tr>';
                            $tbody.append(row);
                        });

                        // After updating the rows, trigger Footable to reinitialize
                        $('#modalTable').trigger('footable_initialized');

                        //alert(data);
                        // Open the modal
                        $('#myModal').modal('show');
                    },
                    error: function (xhr, status, error) {
                        console.error("Error: " + error);
                    }
                });
            });
        });
        function formatPhoneNumber(phoneNumber) {
            // Remove all non-numeric characters from the input
            phoneNumber = phoneNumber.replace(/\D/g, '');

            // Check if the phone number has 10 digits
            if (phoneNumber.length === 10) {
                // Format the phone number as (###) ###-####
                var formattedPhoneNumber = phoneNumber.replace(/(\d{3})(\d{3})(\d{4})/, "($1) $2-$3");

                // Create a clickable link using the tel: protocol
                return `<a href="tel:${phoneNumber}">${formattedPhoneNumber}</a>`;
            } else {
                // Return the original input if it's not a 10-digit number, or you can handle other formats
                return phoneNumber;
            }
        }
        function formatEmail(email) {
            // Simple validation to check if the input looks like an email address
            if (email && email.includes('@')) {
                // Create a clickable link using the mailto: protocol
                return `<a href="mailto:${email}">${email}</a>`;
            } else {
                // Return the original input if it's not a valid email address
                return email;
            }
        }

        function formatTime(timeObject, isArrivalTime) {
            // Check if timeObject is null or undefined
            if (!timeObject || timeObject.Hours === undefined || timeObject.Minutes === undefined) {
                return isArrivalTime ? "Arrive anytime" : ""; // Custom text for empty arrival time or empty string for departure time
            }

            var hours = timeObject.Hours;
            var minutes = timeObject.Minutes;
            var ampm = hours >= 12 ? 'PM' : 'AM';

            // Convert hours to 12-hour format
            hours = hours % 12;
            hours = hours ? hours : 12; // The hour '0' should be '12'

            // Ensure minutes are two digits
            minutes = minutes < 10 ? '0' + minutes : minutes;

            // Format the time string as "H:MM AM/PM"
            return hours + ':' + minutes + ' ' + ampm;
        }
    </script>
    <script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
</asp:Content>
