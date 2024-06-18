<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <div>
            <label for="tags">Emails:</label>
            <asp:TextBox ID="txtTags" runat="server" CssClass="select2" ClientIDMode="Static"></asp:TextBox>
        </div>
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />

    <script type="text/javascript">
        $(document).ready(function () {
            $(".select2").select2({
                tags: true,
                tokenSeparators: [',', ' '],
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
                    toastr.error('Invalid email address format.', 'Error');
                    return null;
                }
            }).on('select2:selecting', function (e) {
                var term = e.params.args.data.id;

                // Validate email format
                if (!validateEmail(term)) {
                    // Prevent the tag from being added
                    e.preventDefault();

                    // Show error message using Toastr
                    toastr.error('Invalid email address format.', 'Error');
                }
            });

            // Toastr configuration (optional)
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": true,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
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
    </script>

</asp:Content>

