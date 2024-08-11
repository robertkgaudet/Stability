<%@ page title="" language="C#" enableeventvalidation="false" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_NonProfitAdministration_InviteTeam, App_Web_ib0zyt45" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

     <div class="text-center m-b-md" id="wizardControl">
        <a class="btn btn-default">Step 1 - Create Team</a>
        <a class="btn btn-purple activeTab">Step 2 - Invite Members</a>
        <a class="btn btn-default">Step 3 - Add Deployment</a>
        <a class="btn btn-default">Step 3 - Launch Website</a>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <div class="hpanel">
                <div class="panel-heading">
                    <h4 class="m-t-md">Invite team members to join your response team.</h4>
                </div>
                <div class="panel-body">
                    <p>
                        <strong>Type the email address of your team members.</strong>
                        <br />
                        <small><i>If you don't have their emails, you can invite them later from your team page. You can invite up 10 15 team members here. You can keep inviting more members later from your team page. </i></small>
                    </p>
                    <small><abbr title="Press Space or Enter">Press the space bar or enter after each email.</abbr></small>
                    <asp:DropDownList ID="ddlEmailAddresses" runat="server" CssClass="js-source-states-2"></asp:DropDownList>
                    <asp:HiddenField ID="hidEmailAddresses" runat="server" />
                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary m-t-lg" Text="Invite Team Members" OnClick="btnSubmit_Click" />
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".js-source-states-2").select2({
                tags: true,
                tokenSeparators: [',', ' '],
                maximumSelectionLength: 15,
                placeholder: "Enter email addresses here.",
                createTag: function (params) {
                    var term = $.trim(params.term);

                    // Validate email format
                    if (validateEmail(term)) {
                        return {
                            id: term,
                            text: term
                        };
                    }

                    // Show error message using Toastr
                    toastr.error('Ensure email format is correct.', 'Alert');
                    return null;
                }
            }).on('js-source-states-2:selecting', function (e) {
                var term = e.params.args.data.id;

                // Validate email format
                if (!validateEmail(term)) {
                    // Prevent the tag from being added
                    e.preventDefault();

                    // Show error message using Toastr
                    toastr.error('Ensure email format is correct.', 'Alert');
                }
            });

            // Toastr configuration (optional)
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": true,
                "progressBar": true,
                "positionClass": "toast-top-center",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "2000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };
        });

        function validateEmail(email) {
            var re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            return re.test(email);
        }


        $("#<%=ddlEmailAddresses.ClientID%>").on('change', function () {
            $("#<%=hidEmailAddresses.ClientID%>").val($(this).val());
        });
    </script>

</asp:Content>