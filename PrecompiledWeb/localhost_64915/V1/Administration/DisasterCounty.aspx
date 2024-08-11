<%@ page language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_Administration_DisasterCounty, App_Web_ijye2wuz" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />

	<script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
    <script src="/Homer/vendor/iCheck/icheck.min.js"></script>
    
	<script type="text/javascript">
        $(document).ready(function () {
            changeClassLabels();
        });
        
        function changeClassLabels() {
            var checkBoxList = document.getElementById('<%= cblCounties.ClientID %>');
            var checkboxes = checkBoxList.getElementsByTagName('input');

            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type === 'checkbox') {
                    checkboxes[i].classList.add('i-checks'); // Add your new class name here
                    checkboxes[i].classList.add('m-r-5');
                }
            }
        }


    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="row">
		<div class="col-lg-12">
			<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
				<div class="hpanel">
					<div class="panel-body">
						<a class="small-header-action">
							<div class="clip-header">
								<i class="fa fa-arrow-up"></i>
							</div>
						</a>
						<h2 class="font-light m-b-xs">
							Select the counties impacted by <%=disasterName%>.
						</h2>
						<div runat="server" id="divMessage" class="alert alert-success" visible="false">
							<i class="fa fa-bolt"></i>
							<asp:Literal runat="server" id="lblMessage"></asp:Literal>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
		<div class="row">
			<div class="col-lg-12">
				<div class="hpanel form-horizontal">
					<div class="panel-heading hbuilt">
						Select each of the impacted state county combinations that have been activated.
					</div>
					<div class="panel-body">
                        
						<div class="form-group">
							<div class="col-sm-12">
                                <asp:CheckBoxList id="cblCounties" runat="server" DataTextField="displayName" DataValueField="CountyId"></asp:CheckBoxList>
							</div>
						</div>
			            <div class="form-group pull-right	">
				            <div>
					            <%--<asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Create Disaster/Event" OnClick="btnSubmit_Click" />--%>
				            </div>
			            </div>
						<div class="panel-body">
						<asp:Button id="btnUpdate" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary pull-left" Text="Save Changes" />
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>

</asp:Content>