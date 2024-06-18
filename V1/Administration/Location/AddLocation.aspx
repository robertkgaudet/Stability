<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="AddLocation.aspx.cs" Inherits="V1_Administration_Resources_AddLocation" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
	<script>
		var address;
		var city;
		var state;
		var zip;
		var lookupComplete = false;
		$(document).ready(function () {
			$('#divAddressMessage').hide();
			$('#divMessage').hide();
			$("#<%=btnSubmit.ClientID%>").attr("disabled", true);
		});

		function CheckAddressValues(controlName, sender)
		{

			switch (controlName)
			{
				case "address":
					if (sender.value)
					{
						address = sender.value;
					}
					break;
				case "city":
					if (sender.value) {
						city = sender.value;
					}
					break;
				case "state":
					if (sender.value) {
						state = sender.value;
					}
					break;
				case "zip":
					if (sender.value) {
						zip = sender.value;
					}
					break;
				default:
				// code block
			}

			if ((address) && (city) && (state) && (zip) && (lookupComplete == false))
			{
				$('#divAddressMessage').show();
				//alert(address + " " + city + ", " + state + " " + zip);
				$('#<%=lblMessage.ClientID%>').text(" Retrieving latitude and longitude...");
				SetLatitudeLongitude('<%=_userId%>', address + " " + city + ", " + state + " " + zip);
			}
		}

		function SetLatitudeLongitude(userId, address)
		{
			$.ajax(
			{
				type: "GET",
				url: "/V1/Handlers/GetLatitudeLongitude.ashx?userId=" + userId + "&address=" + address,
				contentType: "text/plain; charset=utf-8",
				dataType: "html",
				success: function (data)
				{
					if (data != "")
					{
						var results = data.split("|");
						var isPartialMatch = results[0];
						var duplicate = results[12];
						if ((isPartialMatch == 'False' || isPartialMatch == 'false') && (duplicate == 'False' || duplicate == 'false'))
						{
							var latitude  = results[1];
							var longitude = results[2];
							var street_number = results[3];
							var street = results[4];
							var city = results[5];
							var state = results[6];
							var country = results[7];
							var postal_code = results[8];
							var county = results[9];
							var googlePlaceId = results[10];
							var formattedAddress = results[11];
							var addressId = results[13];

							$("#divMapMessage").addClass("alert-success");
							$("#divMapMessage").removeClass("alert-danger"); 
							$("#iFontAwesome").removeClass("fa-warning"); 
							$("#iFontAwesome").addClass("fa-map-marker"); 
							var successMessage = " Google successfully matched your address and returned the following information. (" + data + ")";
							$("#<%=hidAddressData.ClientID%>").val(data);
							$('#<%=lblAddressMessage.ClientID%>').text(successMessage);
							lookupComplete = true;
							$("#<%=btnSubmit.ClientID%>").attr("disabled", false);
						}
						else if (duplicate == 'True' || duplicate == 'true')
						{
							//Address already exists.
							$("#divMapMessage").removeClass("alert-success");
							$("#divMapMessage").addClass("alert-danger");
							$("#iFontAwesome").addClass("fa-warning");
							$("#iFontAwesome").removeClass("fa-map-marker");
							$("#<%=hidAddressData.ClientID%>").val(data);
							lookupComplete = false;
							var errorMessage = " This address already exists (" + address + "). Press 'Next' to edit in the Stability Location Manager. Web Service Message: " + data;
							$('#<%=lblAddressMessage.ClientID%>').text(errorMessage);
							$("#<%=btnSubmit.ClientID%>").attr("disabled", false);
						}
						else
						{
							//Error getting the information
							$("#divMapMessage").removeClass("alert-success");
							$("#divMapMessage").addClass("alert-danger");
							$("#iFontAwesome").addClass("fa-warning");
							$("#iFontAwesome").removeClass("fa-map-marker"); 
							lookupComplete = false;
							var errorMessage = " Please check your address. Google returned an error matching the address you provided. (" + address + ") Web Service Message: " + data;
							$('#<%=lblAddressMessage.ClientID%>').text(errorMessage);
							$("#<%=btnSubmit.ClientID%>").attr("disabled", true);
						}
					}
				},
				error: function (request, status, error)
				{
					$("#divMapMessage").removeClass("alert-success");
					$("#divMapMessage").addClass("alert-danger");
					$("#iFontAwesome").addClass("fa-warning");
					$("#iFontAwesome").removeClass("fa-map-marker"); 
					lookupComplete = false;
					$('#<%=lblAddressMessage.ClientID%>').text(" Error retrieving address information from Google. " + request.statusText + ' - ' + error + ' - ' + status);
					$("#<%=btnSubmit.ClientID%>").attr("disabled", true);
				}
			});
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
								Add New Location
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

						<div class="panel-heading hbuilt">
							Lookup this address by entering the location address, then press next.
						</div>
						<div class="panel-body">
							
							<div id="divMessage" class="alert alert-success m-b-lg">
								<i class="fa fa-bolt"></i>
								<asp:Label runat="server" id="lblMessage"></asp:Label>
							</div>
							
							<div id="divAddressMessage" class="form-group">
								<label class="col-sm-2 control-label">Google Address Details</label>
								<div id="divMapMessage" class="alert m-b-lg p-sm col-sm-5">
									<i id="iFontAwesome" class="fa"></i>
									<asp:Label runat="server" id="lblAddressMessage"></asp:Label>
									<asp:HiddenField id="hidAddressData" runat="server"></asp:HiddenField>
								</div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Address</label>
								<div class="col-sm-5"><input type="text" onblur="CheckAddressValues('address', this)" required runat="server" id="txtAddress" class="form-control i-check" placeholder="Address"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">City</label>
								<div class="col-sm-5"><input type="text" onblur="CheckAddressValues('city', this)" required runat="server" id="txtCity" class="form-control" placeholder="City"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">State</label>
								<div class="col-sm-5">
									<asp:DropDownList ID="ddlState" onblur="CheckAddressValues('state', this)" runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control" required=""></asp:DropDownList>
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label">Zip</label>
								<div class="col-sm-5"><input type="text" onblur="CheckAddressValues('zip', this)" required runat="server" id="txtZipCode" class="form-control" placeholder="Zip Code"></div>
							</div>
							<div class="form-group">
								<label class="col-sm-2 control-label"></label>
								<div class="col-sm-5">
									<div class="pull-right">
										<asp:LinkButton id="btnCancel" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default" Text="Cancel" />
										<asp:Button id="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Next" />
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>