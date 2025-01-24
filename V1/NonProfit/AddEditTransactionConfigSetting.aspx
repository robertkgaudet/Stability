<%@ Page Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Child.master" AutoEventWireup="true" CodeFile="AddEditTransactionConfigSetting.aspx.cs" Inherits="V1_NonProfit_AddEdit" ValidateRequest="false" %>

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
            $('#<%=txtPaymentSummary.ClientID%>').summernote({
                toolbar: [
                    ['style', ['bold', 'italic']]
                ],
                height: 100
            });
            $('#<%=txtAddress.ClientID%>').summernote({
                toolbar: [
                    ['style', ['bold', 'italic', 'underline']],
                    ['alignment', ['ul', 'ol', 'paragraph']]
                ],
                height: 125
            });

            $('#<%=txtDescription.ClientID%>').summernote({
                toolbar: [
                    ['style', ['bold', 'italic', 'underline']],
                    ['alignment', ['ul', 'ol', 'paragraph']]
                ],
                height: 125
            });
        });
        function initializeSummernote() {
            $('.summernote-textbox').each(function () {
                // Check if Summernote is already initialized
                if (!$(this).next().hasClass('note-editor')) {
                    $(this).summernote({
                        toolbar: [
                            ['style', ['bold', 'italic', 'underline']],
                            ['alignment', ['ul', 'ol', 'paragraph']]
                        ],
                        height: 100
                    });
                }
            });
        }
    </script>
    <style>
        .i-checks {
            margin-right: 10px !important;
        }

        .save-btn {
            background-color: #269abc;
            color: white;
            border: none;
            padding: 2px 12px;
            font-size: 15px;
        }

        .compaign-btn {
            background-color: #269abc;
            color: white;
            margin: 10px;
            border: none;
            padding: 4px 6px;
            font-size: 16px;
        }

        .hidden-text {
            visibility: hidden;
        }

        .fieldset-container {
            margin-bottom: 10px;
            margin-top: 25px;
        }

        .modal-fullscreen {
            width: 50%;
            height: 100vh;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel-body">
        <h2 class="font-light m-b-xs">Edit Payment Configurations</h2>
        <!-- Save Button -->
        <div class="col-12" style="display: flex; justify-content: end;">
            <asp:Button type="submit" class="save-btn" runat="server" Text="Save" OnClick="btnSelectPayment_Click" />
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
                    <label class="col-sm-2 control-label custom-label" for="chkShowDonateButton">Show Donate Button:</label>
                    <div class="col-sm-10">
                        <div class="i-checks">
                            <asp:CheckBox ID="chkShowDonateButton" runat="server" CssClass="custom-checkbox" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label custom-label">PayPal Link:</label>
                    <div class="col-sm-8">
                        <input class="form-control custom-input" type="url" required id="txtPayPal" runat="server" placeholder="Enter PayPal URL" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label custom-label">Venmo Link:</label>
                    <div class="col-sm-8">
                        <input class="form-control custom-input" type="url" required id="txtVenmo" runat="server" placeholder="Enter Venmo URL" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-2 control-label custom-label">Cash Pay Link:</label>
                    <div class="col-sm-8">
                        <input class="form-control custom-input" type="url" required id="txtCashPay" runat="server" placeholder="Enter Cash Pay URL" />
                    </div>
                </div>
                <div class="col-12" style="display: flex; justify-content: end;">
                    <asp:Button type="button" class="compaign-btn" runat="server" Text="Add New Campaign" OnClientClick="showModal('Add New Campaign'); return false;" />
                </div>
                <asp:GridView ID="gvDonationCampaigns" runat="server" AutoGenerateColumns="False" DataKeyNames="DonationCampaignId" OnRowCommand="gvCampaigns_RowCommand"  CssClass="table table-bordered">

                    <Columns>
                         <asp:BoundField DataField="DonationCampaignId" HeaderText="Campaign ID" Visible="False" />
                        <asp:BoundField DataField="OrganizationEventName" HeaderText="Deployment" />                        
                        <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="${0:N2}" HtmlEncode="False" />
                        <asp:TemplateField HeaderText="Default?">
                            <ItemTemplate><%# Convert.ToBoolean(Eval("IsDefault")) ? "True" : "False" %>     </ItemTemplate>
                        </asp:TemplateField>
                 
                          <asp:ButtonField CommandName="EditRow" Text="Edit" ButtonType="Button"  />
                          <asp:ButtonField CommandName="DeleteRow" Text="Delete" ButtonType="Button"  />
                    </Columns>
                </asp:GridView>
                <div id="campaignModal" class="modal fade" tabindex="-1" role="dialog">
                    <div class="modal-dialog modal-lg modal-fullscreen" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title text-center">Add New Campaign</h5>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Deployment</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="ddlOrganizationEvent" runat="server" class="form-control" >
                                            <asp:ListItem Text="Select Deployment" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Payment Summary</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtPaymentSummary" runat="server" class="form-control" TextMode="MultiLine" ClientIDMode="Static" Style="height: 150px; font-size: 18px;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Address</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtAddress" runat="server" class="form-control" TextMode="MultiLine" ClientIDMode="Static" Style="height: 150px; font-size: 18px;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Description</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtDescription" runat="server" class="form-control" TextMode="MultiLine" ClientIDMode="Static" Style="height: 150px; font-size: 18px;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Amount($)</label>
                                    <div class="col-sm-8">
                                        <input class="form-control" type="text" id="textAmount" placeholder="Enter Amount e.g. 10,30,50,100,500" runat="server" style="font-size: 18px;" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Is Default</label>
                                    <div class="col-sm-10">
                                        <div class="checkbox">
                                            <asp:CheckBox ID="chkIsDefault" runat="server" />
                                            <label for="chkIsDefault"></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button type="submit" class="btn btn-primary" runat="server" Text="Save Campaign" OnClick="saveCampaign" />
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="deleteCampaignModal" class="modal fade" tabindex="-1" role="dialog">
             <div class="modal-dialog" role="document">
             <div class="modal-content">
               <div class="modal-header">
                <h5 class="modal-title text-center">Delete Campaign</h5>
                </div>
               <div class="modal-body">
                   Are you sure want to delete this item?
                </div>
                <div class="modal-footer">
                <asp:Button type="submit" class="btn btn-danger" runat="server" Text="Delete" OnClick="DeleteCampaign" />
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
            </div>
        </div>
    </div>
</div>

            </div>
        </div>
        <asp:HiddenField ID="hdnSelectedCampaignId" Value="" runat="server"/>
        <script src="https://code.jquery.com/ui/1.14.0/jquery-ui.js"></script>
        <script>
            function showModal(title) {
                setTimeout(function () {               
                    $('.modal-title').text(title);
                    $('#campaignModal').modal('show') ;
                }, 1000);
                
            }
            function showDeleteModal() {
                setTimeout(function () {
                    $('#deleteCampaignModal').modal('show');
                }, 1000);

            }



        </script>
        <script>
        </script>
        <script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
</asp:Content>
