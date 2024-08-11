<%@ page title="" language="C#" validaterequest="false" masterpagefile="~/CaseManagement/MasterPages/CaseManagement.master" autoeventwireup="true" inherits="CaseManagement_EditHome, App_Web_5cuksoem" %>
<%@ MasterType VirtualPath="~/CaseManagement/MasterPages/CaseManagement.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>
	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>


	<link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
	<link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
	<script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>
	<script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>

	<link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
	<script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
	<script>

		$(function () {

			$('.summernote').summernote({
				height: 150,
				toolbar: [

					['style', ['bold', 'italic', 'underline']],
					['alignment', ['ul', 'ol', 'paragraph']],
					['insert', ['link']],
					['misc', ['codeview']],
				]
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
			$(function(){
				$('.input-group.date').datepicker({});
			});

			$("#taskType.dropdown-menu li").click(function () {
				$("#btn-dropdown.taskType").html($(this).text());
				$("#<%=hidTaskType.ClientID%>").val($(this).attr('id'));
			});

			$(".js-source-states-1").select2();
			$(".js-source-states-2").select2();
		});

	</script>
	<style>
		.dropdown-volunteer
		{
			font-size: 15px;
			margin-top:10px;
		}

		#base {
			background: #555555;
			display: inline-block;
			height: 5px;
			margin-left: 5px;
			margin-top: 10px;
			position: relative;
			width: 10px;
		}
		#base:before {
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

		.fa-question-circle:hover
		{
			cursor:pointer;
		}

</style>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="content">
	<div class="row">
		<div class="col-xs-12">
			<div class="hpanel">
				<div class="panel-body">
					<h2 class="font-light m-t-xs">
						Edit Client Home
					</h2>
				</div>
			</div>
		</div>
	</div>
		
	<div class="row">
		<div class="col-xs-12">
			<div class="row">
				<div class="col-xs-4">
					<div class="hpanel hblue">
						<div class="panel-heading hbuilt">
							Client Home Details
						</div>
						<div class="panel-body form-horizontal h-300">

							<div class="form-group" runat="server" id="divExistingLocationAddress">
								<label class="col-sm-4 control-label font-normal">Address</label>
								<div class="col-sm-8 control-label"><label class="pull-left text-left font-bold" id="lblLocationAddress" runat="server"></label></div>
							</div>
							<div class="form-group">
								<label class="col-sm-4 control-label font-normal"></label>
								<div class="col-sm-8 m-t-sm">
									<asp:CheckBox type="checkbox" runat="server" id="chkPrimaryResidence" Text="Primary Residence" />
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-4 control-label font-normal">Own or Rent</label>
								<div class="col-sm-5 m-t-sm">
									<asp:RadioButtonList ID="rblOwnRent" runat="server" DataValueField="RebuildStatusId" DataTextField="Status">
									</asp:RadioButtonList>
								<div class="col-sm-5 m-t-sm">
								</div>
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-4 control-label font-normal">Home Type</label>
								<div class="col-sm-5 m-t-sm">
									<asp:RadioButtonList ID="rblHomeType" runat="server" DataValueField="RebuildStatusId" DataTextField="Status">
									</asp:RadioButtonList>
								</div>
							</div>
							<div class="form-group">
								<label class="col-sm-4 control-label font-normal">Home Properties</label>
								<div class="col-sm-8 m-t-sm">
									<table border="0">
										<tr>
											<td>
												<asp:CheckBox id="cbMultistory" runat="server" Text="Multistory"></asp:CheckBox>
											</td>
										</tr>
										<tr>
											<td>
												<asp:CheckBox id="cbGarage" runat="server" Text="Garage"></asp:CheckBox>
											</td>
										</tr>
										<tr>
											<td>
												<asp:CheckBox id="cbCarport" runat="server" Text="Carport"></asp:CheckBox>
											</td>
										</tr>
										<tr>
											<td>
												<asp:CheckBox id="cbBasement" runat="server" Text="Basement"></asp:CheckBox>
											</td>
										</tr>
										<tr>
											<td>
												<asp:CheckBox id="cbCrawlspace" runat="server" Text="Crawlspace"></asp:CheckBox>
											</td>
										</tr>
									</table>
								</div>
							</div>
							
							<div class="form-group">
								<label class="col-sm-4 control-label font-normal">Map Inclusion</label>
								<div class="col-sm-8 m-t-sm">
									<table>
										<tr>
											<td>
												<asp:CheckBox type="checkbox" runat="server" id="chkIsOnMap" Text="Show On Agency Map" />
											</td>
										</tr>
										<tr>
											<td>
												<asp:CheckBox type="checkbox" runat="server" id="chkIsOnCleanupMap" Text="Show On Cleanup Map" />
											</td>
										</tr>
									</table>
								</div>
							</div>
						</div>
					</div>
				</div>
				<div class="col-xs-8">
					<div class="hpanel hblue" runat="server" id="divNonProfitForm">
						<div class="panel-heading hbuilt">
							Agencies Collaborating on Home Rebuild/Cleanup 
						</div>
						<div class="panel-body h-300">
							<div class="row">
								<div class="col-xs-8">
									<div class="m-b-lg border-bottom">
										Choose Agency Collaborators
										<div class="dropdown m-t-sm m-b-md">
											<asp:ListBox ID="dllSelectCollaborators" SelectionMode="Multiple" DataValueField="OrganizationId" DataTextField="Name" runat="server" CssClass="js-source-states-2"></asp:ListBox>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div class="row">
		<div class="col-md-8 col-sm-8 col-xs-12">
			<div class="hpanel hblue">
				<div class="panel-heading hbuilt">
					Work Information
				</div>
				<div class="panel-body">
					<div class="row">
						<div class="col-md-4 border-right">
							<div class="form-group ">
								<div class="input-group">
									<div class="input-group-addon">
										Sq Ft
									</div>
									<input class="form-control" id="txtSquareFeet" runat="server" onkeypress="return isNumberKey(event)" maxlength="4" name="txtSquareFeet" type="text" placeholder="Enter Square Feet"/>
								</div>
							</div>
						</div>
						<div class="col-md-4 border-right">
							<div class="form-group ">
								<div class="input-group">
									<div class="input-group-addon">
										Beds
									</div>
									<input class="form-control" id="txtBedrooms" runat="server" onkeypress="return isNumberKey(event)" maxlength="1" name="txtBedrooms" type="text" placeholder="Enter # bedrooms"/>
								</div>
							</div>
						</div>
						<div class="col-md-4">
							<div class="form-group ">
								<div class="input-group">
									<div class="input-group-addon">
										Baths
									</div>
									<input class="form-control" id="txtBathrooms" runat="server" onkeypress="return isNumberKey(event)" maxlength="1" name="txtBathrooms" type="text" placeholder="Enter # bathrooms"/>
								</div>
							</div>
						</div>
					</div>
					<div class="row">
						<div class="col-xs-12">
							<div class="m-b-lg border-bottom">
								Rebuild Status
								<asp:RadioButtonList RepeatLayout="Flow" ID="rblIsActive" runat="server"></asp:RadioButtonList>
							</div>
									
							<div class="m-b-lg border-bottom">
								Rebuild Stage
								<asp:RadioButtonList RepeatLayout="Flow" ID="rblRebuildStatus" runat="server"></asp:RadioButtonList>
							</div>


							<div class="m-b-lg border-bottom">
								Estimate Difficulty <i class="fa fa-question-circle" data-toggle="modal" data-target="#myModal6"></i>
								<div id="divTaskTypedropdown" class="dropdown m-b-md" runat="server">
									<input type="hidden" id="hidTaskType" runat="server" />
									<button id="btn-dropdown" class="btn btn-outline btn-default taskType dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Choose Difficulty <div id="base"></div></button>
									<ul id="taskType" class="dropdown-menu text-center dropdown-volunteer">
										<li id=""><a href="#">Unestimated <i class="fa fa-ban"></i></a></li>
										<li id="0"><a href="#">0 Points</a></li>
										<li id="1"><a href="#">1 Point <div id="base"></div></a></li>
										<li id="2"><a href="#">2 Points <div id="base"></div><div id="base"></div></a></li>
										<li id="3"><a href="#">3 Points <div id="base"></div><div id="base"></div><div id="base"></div></a></li>
									</ul>
								</div>
							</div>
							<div class="form-group m-b-lg border-bottom">
								<div class="input-group m-b-md">
									Set targeted work start date
									<div class="input-group date">
										<input type="text" class="form-control" id="hidTargetedStartDate" runat="server"><span class="input-group-addon"><i class="fa fa-calendar"></i></span>
									</div>
								</div>
							</div>
						</div>
					</div>
					<div class="form-group ">
						Describe the Work to be done
						<asp:TextBox TextMode="MultiLine" runat="server" ID="txtRebuildDescription" CssClass="summernote"></asp:TextBox>
					</div>
					<div class="form-group pull-right	">
						<div>
							<asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
						</div>
					</div>
					<div class="modal fade" id="myModal6" tabindex="-1" role="dialog"  aria-hidden="true">
						<div class="modal-dialog modal-lg">
							<div class="modal-content">
								<div class="color-line"></div>
								<div class="modal-header">
									<h4 class="modal-title">Estimate Difficulty</h4>
									<small class="font-bold">Select unestimated or 0-3 points to indicate the difficulty of the work.</small>
								</div>
								<div class="modal-body">
									<p>
										When estimating difficulty consider the amount of rebuild work observed and make an educated guess as to how long it will take to complete the work.
										The points assigned will be used to judge the overall rebuild effort with all active rebuilds.
										<div class="panel-body">
											<dl class="dl-horizontal">
												<dt>Unestimated</dt>
												<dd>No point value assigned.</dd>
												<dt>0 Points</dt>
												<dd>Quick jobs can be completed in less than 1 day with 1 person. (1 Person)</dd>
												<dt>1 Points <div id="base"></div></dt>
												<dd>Easy jobs can be completed in a couple of days with minimal volunteers. (5 Persons)</dd>
												<dt>2 Points <div id="base"></div><div id="base"></div></dt>
												<dd>Moderately difficult jobs that take many weeks and require a team with few professional skillsets. (10 Persons)</dd>
												<dt>3 Points <div id="base"></div><div id="base"></div><div id="base"></div></dt>
												<dd>Extremely difficult jobs that require months of work, planning and require multiple professional skillsets. (20 Persons)</dd>
											</dl>
										</div>
									</p>
								</div>
								<div class="modal-footer">
									<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
</asp:Content>