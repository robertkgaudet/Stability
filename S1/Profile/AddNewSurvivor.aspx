<%@ Page Title="" Language="C#" MasterPageFile="~/S1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="AddNewSurvivor.aspx.cs" Inherits="S1_Profile_AddNewSurvivor" %>
<%@ MasterType VirtualPath="~/S1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
	<link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
	<script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>
	<script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>		<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
	<script>

		$(function ()
		{
			$("#form1").validate(
			{
				rules:
					{
						<%=txtAmazonWishlist.UniqueID%>:{url: true}
					},
			});
		});



		function isNumberKey(evt) {
			var charCode = (evt.which) ? evt.which : evt.keyCode;
			if (charCode == 110 || charCode == 190 || charCode == 46)
				return true;

			if (charCode > 31 && (charCode < 48 || charCode > 57))
				return false;

			return true;
        }

		$(document).ready(function ()
		{
			<%=preselectedDisasterJQuery%>

			$("#disasterEvent.dropdown-menu li").click(function () {
				$("#btn-dropdown.disasterEvent").html($(this).text());
				$("#<%=hidEventId.ClientID%>").val($(this).attr('id'));
			});

			$("#disasterHousing.dropdown-menu li").click(function () {
				$("#btn-dropdown.disasterHousing").html($(this).text());
				$("#<%=hidHousingId.ClientID%>").val($(this).attr('id'));
			});

			$("#recoveryStage.dropdown-menu li").click(function () {
				$("#btn-dropdown.recoveryStage").html($(this).text());
				$("#<%=hidRecoveryStage.ClientID%>").val($(this).attr('id'));
			});
			
			$(".js-source-states-1").select2();
		});

       
    </script>
	<style>
		

		#disaster {
			background: #555555;
			display: inline-block;
			height: 5px;
			margin-left: 5px;
			margin-top: 10px;
			position: relative;
			width: 10px;
		}
		#disaster:before {
		  border-bottom: 4px solid #555555;
		  border-left: 5px solid transparent;
		  border-right: 5px solid transparent;
		  content: "";
		  height: 0;
		  left: 0;
		  position: absolute;
		  top: -4px;
		  width: 0;
		}


		
		#housing {
			background: #555555;
			display: inline-block;
			height: 5px;
			margin-left: 5px;
			margin-top: 10px;
			position: relative;
			width: 10px;
		}
		#housing:before {
		  border-bottom: 4px solid #555555;
		  border-left: 5px solid transparent;
		  border-right: 5px solid transparent;
		  content: "";
		  height: 0;
		  left: 0;
		  position: absolute;
		  top: -4px;
		  width: 0;
		}

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="hpanel">
		<div class="panel-body">
			<a class="small-header-action" href="">
				<div class="clip-header">
					<i class="fa fa-arrow-up"></i>
				</div>
			</a>
			<h2 class="font-light m-b-xs">
				<asp:Literal ID="litEventName" runat="server"></asp:Literal>
			</h2>
			<small>By adding this person, you will able to write and share stories on their behalf. The user will be able to login using their email address as their username and the password you assign for them below.</small>
		</div>
	</div>
	<div class="hpanel">
		<div class="panel-body">

			<div class="form-group"><label class="col-sm-2 control-label">Choose Disaster</label>
				<div class="col-sm-10">
					Which Disaster Affected This Person?
					<div id="div1" class="dropdown m-b-md" runat="server">
						<button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Choose The Disaster <i class="fa fa-sort-down"></i></button>
						<ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required">
							<%=disasterDropDown%>
						</ul>
					</div>
					<input type="hidden" id="hidEventId" runat="server" />
					<div class="hr-line-dashed color-line"></div>
				</div>
			</div>

			
			<div class="form-group"><label class="col-sm-2 control-label">Stability Recovery Stage</label>
				<div class="col-sm-10">
					Which Stability Recovery Stage is this person in?
					<a href="/V1/Stages.aspx" target="_blank" class="small">Click to View Stage Descriptions</a> (Opens new Window)
					<div id="div3" class="dropdown m-b-md" runat="server">
						<button id="btn-dropdown" class="btn btn-outline btn-default recoveryStage dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Choose Recovery Stage <i class="fa fa-sort-down"></i></button>
						<ul id="recoveryStage" class="dropdown-menu text-center dropdown-volunteer required">
							<%=recoveryDropDown%>
						</ul>
					</div>
					<input type="hidden" id="hidRecoveryStage" runat="server" />
					<div class="hr-line-dashed color-line"></div>
				</div>
			</div>

			<div class="form-group"><label class="col-sm-2 control-label">Housing Situation</label>
				<div class="col-sm-10">
					What best describes this persons living conditions?
					<div id="div2" class="dropdown m-b-md" runat="server">
						<button id="btn-dropdown" class="btn btn-outline btn-default disasterHousing dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Choose Housing <i class="fa fa-sort-down"></i></button>
						<ul id="disasterHousing" class="dropdown-menu text-center dropdown-volunteer required">
							<%=housingDropDown%>
						</ul>
					</div>
					<input type="hidden" id="hidHousingId" runat="server" />
					<div class="hr-line-dashed color-line"></div>
				</div>
			</div>


			<div class="form-group"><label class="col-sm-2 control-label">Qualifiers</label>
				<div class="col-sm-10">
					Choose qualifiers (All that apply)
					<div class="dropdown m-t-sm m-b-md">
						<asp:ListBox ID="ddlQualifiers" SelectionMode="Multiple" DataValueField="RebuildStatusId" DataTextField="Status" runat="server" CssClass="js-source-states-1"></asp:ListBox>
					</div>
					<div class="hr-line-dashed color-line"></div>
				</div>
			</div>

			<div class="form-group" runat="server" id="divPersonalInformation"><label class="col-sm-2 control-label">Personal Information</label>

				<div class="col-sm-10">
					<span class="help-block m-b-none">Enter the survivors personal information.</span>
					<asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
					<div class="input-group m-b"><span class="input-group-addon w-m">Firstname</span> <input type="text" runat="server" id="txtFirstname" placeholder="Firstname" class="form-control" required></div>
					<div class="input-group m-b"><span class="input-group-addon w-m">Lastname</span> <input type="text" runat="server" id="txtLastname" placeholder="Lastname" class="form-control" required></div>
					<div class="input-group m-b"><span class="input-group-addon w-m">Phone Number</span> <input type="text" runat="server" id="txtPhonenumber" placeholder="Phone Number (Enter your own if you don't have the survivors)" class="form-control"  onkeypress="return isNumberKey(event)" maxlength="10" name="tel" required></div>					
					<div class="input-group m-b"><span class="input-group-addon w-m">Email Address</span> <input name="email" type="email" runat="server" id="txtEmailAddress" placeholder="Email Address" class="form-control" required></div>
					<div class="input-group m-b"><span class="input-group-addon w-m">Username</span> <asp:TextBox ID="txtUsername" required runat="server" CssClass="form-control"></asp:TextBox></div>
					<div class="input-group m-b"><span class="input-group-addon w-m">Password</span> <input type="password" runat="server" id="txtPassword" placeholder="Password" class="form-control" required></div>
					<div class="hr-line-dashed color-line"></div>
				</div>
			</div>
			
			<div class="form-group"><label class="col-sm-2 control-label">Amazon Wish List</label>
				<div class="col-sm-10">
					<span class="help-block m-b-none">Past a full link to the persons Amazon wish list.</span>
					<div class="input-group m-b"><span class="input-group-addon w-m">Link to Amazon Wishlist</span> <input type="text" runat="server" id="txtAmazonWishlist" placeholder="http://" class="form-control"></div>
				</div>
			</div>
			<div class="form-group pull-right	">
				<div>
					<asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Create Survivor Account" OnClick="btnSubmit_Click" />
				</div>
			</div>
		</div>
	</div>
</asp:Content>