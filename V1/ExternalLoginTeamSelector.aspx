<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true"
    CodeFile="ExternalLoginTeamSelector.aspx.cs" Inherits="V1_ExternalLoginTeamSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
    <script src="/Homer/vendor/iCheck/icheck.min.js"></script>
    <script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
    <style>
        .pull-right {
    margin-right: 120px;
    margin-top: 6px;
}
    </style>
    <script>
        $(document).ready(function () {
			<%=preselectedNonProfitJQuery%>
            $("#nonProfit.dropdown-menu li").click(function () {
                $("#btn-NonProfitDropdown.nonProfit").html($(this).text());
                $("#<%=hidOrganizationId.ClientID%>").val($(this).attr('id'));
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
                <div class="hpanel">
                    <div class="panel-body">
                        <h2 class="font-light m-b-xs">Select A Team
                        </h2>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
        <div class="row">
            <div class="col-lg-12 container">
                <div class="hpanel form-horizontal">
                    <div class="panel-body">
                        <div class="form-group">
                            <div class="col-sm-5">

                                <!-- Dropdown -->
                                <div class="dropdown m-t-xs">
                                    <button id="btn-NonProfitDropdown" class="btn btn-outline btn-default nonProfit dropdown-toggle dropdown-volunteer"
                                        type="button" data-toggle="dropdown" style="margin-top: 5px;">
                                        Select A Team, Club or Group (Optional) <i class="fa fa-sort-down"></i>
                                    </button>
                                    <ul id="nonProfit" class="dropdown-menu text-center dropdown-volunteer required"
                                        style="max-height: 300px; overflow-y: auto;">
                                        <%=nonProfitDropDown%>
                                    </ul>
                                </div>

                                <!-- Hidden field to store selected organization ID -->
                                <input type="hidden" id="hidOrganizationId" runat="server" />

                            </div>
                        </div>

                    </div>
                    <div class="form-group">
                        <label class="col-sm-2 control-label"></label>
                        <div class="col-sm-5">
                            <div class="pull-right">
                                <asp:LinkButton ID="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel"
                                    CssClass="btn btn-default" Text="Skip for now" />
                                <asp:Button ID="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary"
                                    Text="Submit" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>