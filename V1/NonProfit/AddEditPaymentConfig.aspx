<%@ Page Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Child.master" AutoEventWireup="true" CodeFile="AddEditPaymentConfig.aspx.cs" Inherits="V1_NonProfit_AddEditPaymentConfig" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
    <script src="/Homer/vendor/iCheck/icheck.min.js"></script>
    <script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">

    <link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
    <script>
        $(function () {
            // Initialize Summernote for paymentSummary
            $('#<%=txtpaymentSummary.ClientID%>').summernote({
                toolbar: [
                    ['style', ['bold', 'italic']]
                ],
                height: 100
            });

            // Initialize Summernote for txtAddress
            $('#<%=txtAddress.ClientID%>').summernote({
                toolbar: [
                    ['style', ['bold', 'italic', 'underline']],
                    ['alignment', ['ul', 'ol', 'paragraph']]
                ],
                height: 125
            });


        });
    </script>
    <script>
        $("#form1").validate({
            rules: {
					<%=txtPayPal.UniqueID%>: {
            required: true,
            maxlength: 100
        },
            $("#form1").validate({
                rules: {
					<%=txtVenmo.UniqueID%>: {
                required: true,
                maxlength: 100
            },
                $("#form1").validate({
                    rules: {
					<%=txtCashPay.UniqueID%>: {
                    required: true,
                    maxlength: 100
                },
               
                         

         });
    </script>

    <style>
        .i-checks {
            margin-right: 10px !important;
        }

        .custom-textarea {
            width: 100% !important;
            height: 350px !important;
            padding: 12px;
            font-size: 14px;
            background-color: #e0f7fa;
            border: 2px solid #00796b;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

            .custom-textarea:hover {
                background-color: #b2ebf2;
            }

            .custom-textarea:focus {
                border-color: #004d40;
                outline: none;
                box-shadow: 0 0 8px rgba(0, 77, 64, 0.3);
            }

        .blue-btn {
            background-color: blue;
            color: white;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel-body">
        <h2 class="font-light m-b-xs">Edit Payment Configurations</h2>
        <!-- Save Button -->
        <div class="col-12" style="display: flex; justify-content: end;">
            <asp:Button type="submit" class="btn submit-btn" runat="server" Text="Save" OnClick="btnSelectPayment_Click" Style="background-color: blue; color: white;" />
        </div>

        <div class="form-group"></div>
    </div>

    <div class="col-lg-12 container">
        <div class="hpanel form-horizontal">
            <div class="panel-heading">
                <h3>Details</h3>
            </div>
            <div class="panel-body">
                <div class="form-group">
                    <label class="col-sm-2 control-label custom-label">PayPal Link:</label>
                    <div class="col-sm-10">
                        <input class="form-control custom-input" type="url" required id="txtPayPal" runat="server" placeholder="Enter PayPal URL" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-2 control-label custom-label">Venmo Link:</label>
                    <div class="col-sm-10">
                        <input class="form-control custom-input" type="url" required id="txtVenmo" runat="server" placeholder="Enter Venmo URL" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-2 control-label custom-label">Cash Pay Link:</label>
                    <div class="col-sm-10">
                        <input class="form-control custom-input" type="url" required id="txtCashPay" runat="server" placeholder="Enter Cash Pay URL" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-2 control-label">Payment Summary</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtpaymentSummary" runat="server" class="form-control" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-sm-2 control-label">Address</label>
                    <div class="col-sm-10 custom-container">
                        <asp:TextBox ID="txtAddress" runat="server" class="form-control" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label custom-label" for="chkShowDonateButton">Show Donate Button</label>
                    <div class="col-sm-10">
                        <div class="i-checks">
                            <asp:CheckBox ID="chkShowDonateButton" runat="server" Text="" CssClass="custom-checkbox" />
                        </div>
                    </div>
                </div>


            </div>
        </div>

        <!-- Scripts -->
        <script src="https://code.jquery.com/ui/1.14.0/jquery-ui.js"></script>

        <script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
</asp:Content>
