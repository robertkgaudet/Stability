<%@ Page Title="" Language="C#" MasterPageFile="~/CaseManagement/MasterPages/CaseManagement.master" AutoEventWireup="true" CodeFile="AddLocation.aspx.cs" Inherits="CaseManagement_AddLocation" %>
<%@ MasterType VirtualPath="~/CaseManagement/MasterPages/CaseManagement.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>
	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
	<script>
		var address;
		var city;
		var state;
		var zip;
		var lookupComplete = false;
		$(document).ready(function () {
			$('#divAddressMessage').hide();
			$('#divMessage').hide();
			$('#addressform1').show();
			$("#<%=btnSubmit.ClientID%>").hide();
			$("#<%=btnSubmit.ClientID%>").attr("disabled", true);
			$("#<%=btnCancel.ClientID%>").show();
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

							$('#addressform1').hide();
							$("#divMapMessage").addClass("alert-success");
							$("#divMapMessage").removeClass("alert-danger"); 
							$("#iFontAwesome").removeClass("fa-warning"); 
							$("#iFontAwesome").addClass("fa-map-marker"); 
							var successMessage = " \r Google successfully matched your address and returned the following information.\r \r  (" + data + ")";
							$("#<%=hidAddressData.ClientID%>").val(data);
							$('#<%=lblAddressMessage.ClientID%>').text(successMessage);
							lookupComplete = true;
							$("#<%=btnSubmit.ClientID%>").attr("disabled", false);
							$("#<%=btnSubmit.ClientID%>").show();
							$("#<%=btnCancel.ClientID%>").hide();
						}
						else if (duplicate == 'True' || duplicate == 'true')
						{
							//Address already exists.
							$('#addressform1').hide();
							$("#divMapMessage").removeClass("alert-success");
							$("#divMapMessage").addClass("alert-danger");
							$("#iFontAwesome").addClass("fa-warning");
							$("#iFontAwesome").removeClass("fa-map-marker");
							$("#<%=hidAddressData.ClientID%>").val(data);
							lookupComplete = false;
							var errorMessage = " \r This address already exists (" + address + "). \r \r Press 'Next' to edit in the Stability Location Manager. Web Service Message: " + data;
							$('#<%=lblAddressMessage.ClientID%>').text(errorMessage);
							$("#<%=btnSubmit.ClientID%>").attr("disabled", false);
							$("#<%=btnSubmit.ClientID%>").show();
							$("#<%=btnCancel.ClientID%>").hide();
						}
						else
						{
							$('#addressform1').show();
							//Error getting the information
							$("#divMapMessage").removeClass("alert-success");
							$("#divMapMessage").addClass("alert-danger");
							$("#iFontAwesome").addClass("fa-warning");
							$("#iFontAwesome").removeClass("fa-map-marker"); 
							lookupComplete = false;
							var errorMessage = " Please check your address. Google returned an error matching the address you provided.\r \r  (" + address + ") Web Service Message: " + data;
							$('#<%=lblAddressMessage.ClientID%>').text(errorMessage);
							$("#<%=btnSubmit.ClientID%>").attr("disabled", true);
							$("#<%=btnSubmit.ClientID%>").hide();
							$("#<%=btnCancel.ClientID%>").show();
						}
					}
				},
				error: function (request, status, error)
				{
					$('#addressform1').show();
					$("#divMapMessage").removeClass("alert-success");
					$("#divMapMessage").addClass("alert-danger");
					$("#iFontAwesome").addClass("fa-warning");
					$("#iFontAwesome").removeClass("fa-map-marker"); 
					lookupComplete = false;
					$('#<%=lblAddressMessage.ClientID%>').text(" Error retrieving address information from Google.\r \r  " + request.statusText + ' - ' + error + ' - ' + status);
					$("#<%=btnSubmit.ClientID%>").attr("disabled", true);
					$("#<%=btnSubmit.ClientID%>").hide();
					$("#<%=btnCancel.ClientID%>").show();
					
				}
			});
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="content">
	<div class="row">
		<div class="col-lg-12">
			<div class="hpanel">
				<div class="panel-body">
					<h2 class="font-light m-t-xs">
						Add Home to Client
					</h2>
				</div>
			</div>
		</div>
	</div>

	<div class="row">
		<div class="col-lg-9">
			<div class="hpanel">
				<div class="panel-heading bg-light-soft">
					<span class="text-muted m-l-md">Lookup this address by entering the location address, then press next.</span>
				</div>
				<div class="panel-body form-horizontal">
					<span class="text-muted m-b-lg">Address will be automatically verified by Google Places when it is completely entered.</span>
					<br /><br />
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
					<div id="addressform1" class="form-group addressform1">
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
					</div>
					<div class="form-group">
						<label class="col-sm-2 control-label"></label>
						<div class="col-sm-5">
							<div class="pull-right">
								<asp:LinkButton id="btnCancel" CausesValidation="false" runat="server" CssClass="btn btn-success" Text="Lookup Address" />
								<asp:Button id="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Next" />
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="col-lg-3">
		</div>
	</div>
</div>
</asp:Content>