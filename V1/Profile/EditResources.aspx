<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="EditResources.aspx.cs" Inherits="V1_Profile_Resources" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>
    <style>
        .resource-btn {
            margin: 5px;
        }
        .resource-btn.active,
        .resource-btn.active:focus,
        .resource-btn.active:hover {
            background-color: #5E2E91;
            color: #fff;
            border-color: #E9D3FE;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            var selected = $('#<%= hfSelectedResources.ClientID %>').val().toLowerCase().split(',');
            $('.resource-btn').each(function () {
                var resourceId = ($(this).data('resourceid') + '').toLowerCase();
                if (selected.indexOf(resourceId) !== -1) {
                    $(this).addClass('active');
                }
            });

            $('.resource-btn').on('click', function () {
                $(this).toggleClass('active');
                updateSelectedResources();
            });

            function updateSelectedResources() {
                var selected = [];
                $('.resource-btn.active').each(function () {
                    selected.push(($(this).data('resourceid') + '').toLowerCase());
                });
                $('#<%= hfSelectedResources.ClientID %>').val(selected.join(','));
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
                <div class="hpanel">
                    <div class="panel-body">
                        <h2 class="font-light m-b-xs">
                            Choose The Types of Equipment You Can Contribute
                        </h2>
						<b>Let us know what resources or equipment you can share in times of need.</b><br />
						From generators and trucks to extra food or supplies, these items can make a big difference when disaster strikes. Select what you’re able to provide so your community can respond faster and more effectively.
                        <div class="form-group">
                            <div class="pull-right">
                                <asp:LinkButton id="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default" Text="Cancel" />
                                <asp:Button id="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Save Changes" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
        <div class="row">
            <div class="col-lg-12 container">
                <div class="hpanel form-horizontal">
                    <div class="panel-heading hbuilt">
                        Choose The Types of Equipment You Can Contribute
                    </div>
                    <div runat="server" id="divMessage" class="alert alert-success" visible="false">
                        <i class="fa fa-bolt"></i>
                        <asp:Literal runat="server" id="lblMessage"></asp:Literal>
                    </div>
                    <div class="panel-body p-lg">
                        <div class="form-group m-t-lg">
                            <asp:HiddenField ID="hfSelectedResources" runat="server" />
                            <asp:Repeater ID="rptResources" runat="server">
                                <ItemTemplate>
                                    <button type="button"
                                            class="btn btn-outline-primary resource-btn m-1"
                                            data-resourceid='<%# Eval("ResourceId").ToString().ToLowerInvariant() %>'>
                                        <%# Eval("Name") %>
                                    </button>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="panel-footer">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>