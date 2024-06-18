<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="NewDisaster.aspx.cs" Inherits="V1_Administration_NewDisaster" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />

    <script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>
	<script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
    <script src="/Homer/vendor/iCheck/icheck.min.js"></script>
    
	<script>

        // Function to validate alphanumeric input on keypress
        function validateAlphaNumericInput(event) {
            // Get the input element
            var inputElement = document.getElementById('<%=txtURLFriendlyName.ClientID%>');

            // Get the current input value
            var inputValue = inputElement.value;

            // Get the key code of the pressed key
            var keyCode = event.keyCode || event.which;

            // Define a regular expression pattern that matches only alphanumeric characters
            var pattern = /^[a-zA-Z0-9]+$/;

            // Check if the pressed key is an alphanumeric character or a control key (e.g., backspace)
            if (keyCode == 8 || pattern.test(String.fromCharCode(keyCode))) {
                // Allow the input
                return true;
            } else {
                // Prevent the input
                event.preventDefault();
                return false;
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode == 110 || charCode == 190 || charCode == 46)
                return true;

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        $(document).ready(function () {

            $("#<%=txtBeginDate.ClientID%>").datepicker();
            $("#<%=txtEndDate.ClientID%>").datepicker();

            $(".states-multiple").select2();
            $('.states-multiple').val([<%=preselectedStates%>]);
            $('.states-multiple').trigger('change'); // Notify any JS components that the value changed

            $("#disasterEventType.dropdown-menu li").click(function () {
                $("#btn-dropdown.disasterEvent").html($(this).text());
                $("#<%=hidEventTypeId.ClientID%>").val($(this).attr('id'));
            });
            <%=preselectedDisasterType%>

            $(".js-source-states-1").select2();
        });

        $(function () {
            $("#form1").validate({
                rules:
                {
			        <%=txtDisasterName.UniqueID%>:
			        {
                        required: true
                    },
			        <%=txtDescription.UniqueID%>:
                    {
                        required: true
                    },
			        <%=txtURLFriendlyName.UniqueID%>:
                    {
                        required: true
                    },
			        <%=txtBeginDate.UniqueID%>:
                    {
                        required: true,
                        date: true
                    },
			        <%=txtEndDate.UniqueID%>:
                    {
                        date: true
                    },
			        <%=txtLatitude.UniqueID%>:
                    {
                        required: true,
                    },
			            <%=txtLongitude.UniqueID%>:
                    {
                        required: true,
                    },
			        <%=txtMapZoomLevel.UniqueID%>: 
                    {
                        required: true,
				        number: true,
				        maxlength: 2
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
							Add New Disaster/Event
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
						Fill in the needed information below.
					</div>
					<div class="panel-body">
                            
						<div class="form-group">
							<label class="col-sm-2 control-label">Disaster/Event Name</label>
							<div class="col-sm-5"><input type="text" runat="server" required id="txtDisasterName" class="form-control" placeholder="Disaster/Event Name"></div>
						</div>
                            
			            <div class="form-group"><label class="col-sm-2 control-label">Disaster Type</label>
				            <div class="col-sm-10">
					            <div id="div1" class="dropdown m-b-md" runat="server">
						            <button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Choose The Disaster/Event Type <i class="fa fa-sort-down"></i></button>
						            <ul id="disasterEventType" class="dropdown-menu text-center dropdown-volunteer required">
							            <%=disasterTypeDropDown%>
						            </ul>
					            </div>
					            <input type="hidden" id="hidEventTypeId" runat="server" />
				            </div>
			            </div>

						<div class="form-group">
							<label class="col-sm-2 control-label">Description</label>
							<div class="col-sm-5"><textarea runat="server" rows="5" required id="txtDescription" class="form-control" placeholder="Description"></textarea></div>
						</div>

						<div class="form-group">
							<label class="col-sm-2 control-label">URL Friendly Name (No Spaces)</label>
							<div class="col-sm-3"><input type="text" onkeypress="return validateAlphaNumericInput(event)" runat="server" required id="txtURLFriendlyName" class="form-control" placeholder="URL Friendly Name (No Spaces)"></div>
						</div>

						<div class="form-group">
							<label class="col-sm-2 control-label">Begin Date</label>
							<div class="col-sm-2"><input runat="server" type="text" required id="txtBeginDate" class="form-control"></div>
						</div>

						<div class="form-group">
							<label class="col-sm-2 control-label">End Date</label>
							<div class="col-sm-2"><input runat="server" id="txtEndDate" type="text" class="form-control"></div>
						</div>

						<div class="form-group">
							<label class="col-sm-2 control-label">Is Currently Active Disaster</label>
							<div class="col-sm-1"><input type="checkbox" runat="server" id="chkActive" class="form-control i-checks"></div>
						</div>

						<div class="form-group">
							<label class="col-sm-2 control-label">Latitude</label>
							<div class="col-sm-2"><input type="text" runat="server" required id="txtLatitude" class="form-control" placeholder="Latitude"></div>
						</div>

						<div class="form-group">
							<label class="col-sm-2 control-label">Longitude</label>
							<div class="col-sm-2"><input type="text" runat="server" required id="txtLongitude" class="form-control" placeholder="Longitude"></div>
						</div>

						<div class="form-group">
							<label class="col-sm-2 control-label">Map Zoom Level</label>
							<div class="col-sm-1"><input type="text" runat="server" maxlength="2" onkeypress="return isNumberKey(event)" required id="txtMapZoomLevel" class="form-control" placeholder="Map Zoom"></div>
						</div>
                        

			            <div class="form-group"><label class="col-sm-2 control-label">States Impacted</label>
				            <div class="col-sm-10">
					            Choose States Impacted (All that apply)
					            <div class="dropdown m-t-sm m-b-md">
						            <asp:ListBox ID="ddlStates" SelectionMode="Multiple" DataValueField="StatesId" DataTextField="Name" runat="server" CssClass="states-multiple"></asp:ListBox>
					            </div>
				            </div>
			            </div>

			            <div class="form-group pull-right	">
				            <div>
					            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Create Disaster/Event" OnClick="btnSubmit_Click" />
				            </div>
			            </div>
					</div>
				</div>
			</div>
		</div>
	</div>

</asp:Content>