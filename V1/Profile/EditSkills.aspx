<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="EditSkills.aspx.cs" Inherits="V1_Profile_Skills" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>    <style>
        .skill-btn {
            margin: 5px;
        }
        .skill-btn.active,
        .skill-btn.active:focus,
        .skill-btn.active:hover {
            background-color: #5E2E91;
            color: #fff;
            border-color: #E9D3FE;
        }
    </style>
    <script type="text/javascript">
		$(document).ready(function () {
			var selected = $('#<%= hfSelectedSkills.ClientID %>').val().toLowerCase().split(',');
            $('.skill-btn').each(function () {
                var skillId = ($(this).data('skillid') + '').toLowerCase();
                if (selected.indexOf(skillId) !== -1) {
                    $(this).addClass('active');
                }
            });

            $('.skill-btn').on('click', function () {
                $(this).toggleClass('active');
                updateSelectedSkills();
            });

            function updateSelectedSkills() {
                var selected = [];
                $('.skill-btn.active').each(function () {
                    selected.push(($(this).data('skillid') + '').toLowerCase());
                });
                $('#<%= hfSelectedSkills.ClientID %>').val(selected.join(','));
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
								Choose Your Skills
							</h2>
							<b>Tell us how you can help when your community needs it most.</b>
							<br />
							Choose from a list of practical skills—like cooking, medical support, logistics, or tech—so we can match your strengths with real needs during a disaster. Whether you're a trained professional or a willing neighbor, every skill matters.
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
							Choose Your Skills
						</div>
						<div runat="server" id="divMessage" class="alert alert-success" visible="false">
							<i class="fa fa-bolt"></i>
							<asp:Literal runat="server" id="lblMessage"></asp:Literal>
						</div>
						<div class="panel-body p-lg">
							
							<div class="form-group m-t-lg">
								<asp:HiddenField ID="hfSelectedSkills" runat="server" />
								<asp:Repeater ID="rptSkills" runat="server">
									<ItemTemplate>
										<button type="button"
												class="btn btn-outline-primary skill-btn m-1"
												data-skillid='<%# Eval("SkillId").ToString().ToLowerInvariant() %>'>
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