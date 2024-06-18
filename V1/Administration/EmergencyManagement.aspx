<%@ Page Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="EmergencyManagement.aspx.cs" Inherits="V1_Administration_EmergencyManagement" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
    <script>

    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode == 110 || charCode == 190 || charCode == 46)
            return true;

        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

        return true;
        };


    $(function () {
        $("#form1").validate({
            rules:
            {
		        <%=txtEOCName.UniqueID%>:
		        {
                    required: true
                },
		        <%=txtEMName.UniqueID%>:
                {
                    required: true
                },
		        <%=txtEMWebsite.UniqueID%>:
                {
                    required: true,
                    URL: true
                },
		        <%=txtPhoneNumber.UniqueID%>: 
                {
                    required: true,
			        number: true,
			        maxlength: 10
		        }
	        },
                submitHandler: function (form) {
                    form.submit();
                },
                errorPlacement: function (error, element) {
                    $(element)
                        .closest("form")
                        .find("label[for='" + element.attr("id") + "']")
                        .append(error);
                },
                errorElement: "span",
            });
        });


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
							Add/Edit Emergency Management Information
						</h2>
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
					    <asp:Label ID="lblStateInstructions" runat="server"></asp:Label>
				    </div>
				    <div class="panel-body">
                        
					    <div class="form-group">
						    <label class="col-sm-2 control-label">Emergency Operations Center Name</label>
						    <div class="col-sm-5"><input type="text" runat="server" required id="txtEOCName" class="form-control" placeholder="Emergency Operations Center Name"></div>
					    </div>
                        
					    <div class="form-group">
						    <label class="col-sm-2 control-label">Emergency Managers Name</label>
						    <div class="col-sm-5"><input type="text" runat="server" required id="txtEMName" class="form-control" placeholder="Emergency Managers First and Last Name"></div>
					    </div>
                        
					    <div class="form-group">
						    <label class="col-sm-2 control-label">Primary Phone Number</label>
						    <div class="col-sm-5"><input type="text" runat="server" maxlength="10" required onkeypress="return isNumberKey(event)" id="txtPhoneNumber" class="form-control" placeholder="Primary Phone Number"></div>
					    </div>
                        
					    <div class="form-group">
						    <label class="col-sm-2 control-label">Emergency Management Website</label>
						    <div class="col-sm-5"><input type="text" runat="server" required id="txtEMWebsite" class="form-control" placeholder="Emergency Management Website (https:// required)"></div>
					    </div>

		                <div class="form-group pull-right	">
			                <div>
				                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Update Emergency Operations Information" OnClick="btnSubmit_Click" />
			                </div>
		                </div>
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</asp:Content>