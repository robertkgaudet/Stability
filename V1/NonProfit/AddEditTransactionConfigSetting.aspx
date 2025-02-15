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
            $('#<%=txtCampaignSummary.ClientID%>').summernote({
                toolbar: [
                    ['style', ['bold', 'italic']],
                    ['alignment', ['ul', 'ol', 'paragraph']],
                    ['font', ['strikethrough', 'superscript', 'subscript']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['height', ['height']],
                    ['insert', ['picture', 'link', 'video', 'table', 'hr']],
                    ['view', ['fullscreen', 'codeview', 'help']]
                ],
                height: 100
            });
            $('#<%=txtCampaignMailingAddress.ClientID%>').summernote({
                toolbar: [
                    ['style', ['bold', 'italic', 'underline']],
                    ['alignment', ['ul', 'ol', 'paragraph']],
                    ['font', ['strikethrough', 'superscript', 'subscript']],
                    ['color', ['color']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['height', ['height']],
                    ['insert', ['picture', 'link', 'video', 'table', 'hr']],
                    ['view', ['fullscreen', 'codeview', 'help']]
                ],
                height: 125
            });

            $('#<%=txtCampaignDescription.ClientID%>').summernote({
                toolbar: [
                    ['style', ['bold', 'italic', 'underline']],
                    ['alignment', ['ul', 'ol', 'paragraph']],
                    ['font', ['strikethrough', 'superscript', 'subscript']],
                    ['color', ['color']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['height', ['height']],
                    ['insert', ['picture', 'link', 'video', 'table', 'hr']],
                    ['view', ['fullscreen', 'codeview', 'help']]
                ],
                height: 125
            });
        });
    </script>
    <style>
        .modal-fullscreen {
            width: 80%;
            height: 100vh;
        }
    </style>
    <style>
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel-body">
        <h2 class="font-light m-b-xs">Edit Payment Configurations</h2>
        <!-- Save Button -->
        <div class="col-12" style="display: flex; justify-content: end;">
            <asp:Button type="submit" class="save-btn" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="PaymentConfigration_Click" />
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
                    <asp:Button type="button" class="compaign-btn" runat="server" Text="Add New Campaign" CssClass="btn btn-primary" OnClientClick="showModal('Add New Campaign' ); return false;" />
                </div>
                <asp:GridView ID="gvDonationCampaigns" runat="server" AutoGenerateColumns="False" DataKeyNames="DonationCampaignId" OnRowCommand="gvCampaigns_RowCommand" OnRowDataBound="gvDonationCampaigns_RowDataBound" CssClass="table table-bordered">

                    <Columns>
                        <asp:BoundField DataField="DonationCampaignId" HeaderText="Campaign ID" Visible="false" />
                        <asp:BoundField DataField="OrganizationEventName" HeaderText="Deployment" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" HtmlEncode="False" />
                        <asp:TemplateField HeaderText="Default?">
                            <ItemTemplate><%# Convert.ToBoolean(Eval("IsDefault")) ? "True" : "False" %>   </ItemTemplate>
                        </asp:TemplateField>
                        <asp:ButtonField CommandName="EditRow" Text="Edit" ButtonType="Button" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteRow" CommandArgument='<%# Eval("DonationCampaignId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div id="campaignModal" class="modal fade" tabindex="-1" role="dialog">
                    <div class="modal-dialog modal-lg modal-fullscreen" role="document">
                        <div class="modal-content">
                            <%-- <div class="modal-header">
                                <h5 class="modal-title text-center">Add New Campaign</h5>
                                <button type="button"
                                    class="close btn btn-secondary"
                                    runat="server"
                                    aria-label="Close"
                                    onserverclick="btnClose_Click">
                                    <span aria-hidden="true">X</span>
                                </button>

                            </div>--%>
                            <div class="modal-header" style="display: flex; justify-content: space-between; align-items: center;">
                                <h5 class="modal-title" style="flex-grow: 1; text-align: center;">Add New Campaign</h5>
                                <button type="button" class="close btn btn-secondary" runat="server" aria-label="Close" onserverclick="btnClose_Click">
                                    <span aria-hidden="true">X</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Deployment</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="ddlOrganizationEvent" runat="server" class="form-control">
                                            <asp:ListItem Text="Select Deployment" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Campaign Summary</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtCampaignSummary" runat="server" class="form-control" TextMode="MultiLine" ClientIDMode="Static" Style="height: 150px; font-size: 18px;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Campaign Mailing Address</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtCampaignMailingAddress" runat="server" class="form-control" TextMode="MultiLine" ClientIDMode="Static" Style="height: 150px; font-size: 18px;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Campaign Description</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtCampaignDescription" runat="server" class="form-control" TextMode="MultiLine" ClientIDMode="Static" Style="height: 150px; font-size: 18px;"></asp:TextBox>
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
                                <asp:Button type="submit" class="btn btn-primary" runat="server" Text="Save Campaign" OnClick="SaveCampaign" />
                                <%--        <button type="button" class="btn btn-secondary" data-dismiss="modal">close</button>   --%>
                                <asp:Button
                                    ID="btnClose"
                                    runat="server"
                                    Text="Close"
                                    CssClass="btn btn-secondary"
                                    OnClick="btnClose_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="deleteCampaignModal" class="modal fade" tabindex="-1" role="dialog">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header" style="display: flex; justify-content: space-between; align-items: center;">
                                <h5 class="modal-title" style="flex-grow: 1; text-align: center;">Delete Campaign</h5>
                                <button type="button" class="close btn btn-secondary" data-dismiss="modal">
                                    <span aria-hidden="true">X</span>
                                </button>
                            </div>
                            <%-- <button type="button" class="btn btn-secondary" data-dismiss="modal">X</button>--%>
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
        <asp:HiddenField ID="hdnSelectedCampaignId" Value="" runat="server" />
        <script src="https://code.jquery.com/ui/1.14.0/jquery-ui.js"></script>
        <script>
            function showModal(title) {
                setTimeout(function () {
                    $('.modal-title').text(title);
                    $('#campaignModal').modal('show');
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
