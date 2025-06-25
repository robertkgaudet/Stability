<%@ Page Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Child.master"
    AutoEventWireup="true" CodeFile="CreateWaiverStepUp.aspx.cs" Inherits="V1_NonProfit_CreateWaiverStepUp"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />

    <script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
    <script src="/Homer/vendor/iCheck/icheck.min.js"></script>
    <script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
    <style>
        #ContentPlaceHolder1_ContentPlaceHolder1_btnSaveWaiver {
            margin-bottom: 20px;
        }
    </style>
    <script>
        $(function () {
            $('#txtWaiver').summernote({
                toolbar: [
                    ['style', ['bold', 'italic']],
                    ['alignment', ['ul', 'ol', 'paragraph']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['height', ['height']],
                    ['insert', ['picture', 'link', 'table']]
                ],
                height: 400
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-body">
        <div class="col-sm-12">
            <h2 class="font-light m-b-xs">Waiver Setup</h2>
        </div>
        <div class="form-group">
            <div class="col-sm-12">
                <div class="checkbox checkbox-primary">
                    <input type="checkbox" id="chkRequireWaiver" runat="server" class="checkbox-inline" />
                    <label for="chkRequireWaiver">Require waiver for new members</label>
                </div>
            </div>
        </div>


        <div class="form-group mt-3">
            <div class="col-sm-12">
                <label for="txtWaiver" class="control-label"><strong>Waiver Text:</strong></label>
                <asp:TextBox ID="txtWaiver" runat="server"
                    CssClass="form-control"
                    TextMode="MultiLine"
                    ClientIDMode="Static"
                    Style="height: 150px; font-size: 18px;" />
            </div>
        </div>
        <div class="form-group mt-4">
            <div class="col-sm-12">
                <asp:Button ID="btnSaveWaiver" runat="server" Text="Save" CssClass="btn btn-primary"
                    OnClick="btnSaveWaiver_Click" />
            </div>
        </div>
        <div class="col-sm-12">
            <h3 class="font-light m-b-xs" style="margin-bottom: 15px; margin-left: -15px;">Waiver
                Signatures</h3>
        </div>
        <asp:GridView ID="gvSignatures" runat="server" AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:TemplateField HeaderText="Join Date">
                    <ItemTemplate>
                        <%# Eval("JoinDate") == null 
              ? "–" 
              : String.Format("{0:dd MMM yyyy}", Eval("JoinDate")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Signed">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("IsSigned")) ? "Yes" : "No" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date Signed">
                    <ItemTemplate>
                        <%# Eval("SignedDate") == null 
                       ? "–" 
                       : String.Format("{0:dd MMM yyyy}", Eval("SignedDate")) %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
