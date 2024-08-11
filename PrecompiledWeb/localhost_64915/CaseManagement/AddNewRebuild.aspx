<%@ page title="" language="C#" masterpagefile="~/CaseManagement/MasterPages/CaseManagement.master" autoeventwireup="true" inherits="Account_Organization_AddNewRebuild, App_Web_5cuksoem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
	<link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
	<script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>
	<script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>

	<script>

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
			
			<%=preselectedDisasterJQuery%>

			$("#taskType.dropdown-menu li").click(function () {
				$("#btn-dropdown.taskType").html($(this).text());
				$("#<%=hidTaskType.ClientID%>").val($(this).attr('id'));
			});

			$("#disasterEvent.dropdown-menu li").click(function () {
				$("#btn-dropdown.disasterEvent").html($(this).text());
				$("#<%=hidEventId.ClientID%>").val($(this).attr('id'));
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

	<div class="animate-panel" data-child="hpanel" data-effect="fadeInDown">
		<div class="normalheader">
			<div class="hpanel <%=headerColor%>">
				<div class="panel-body">
					<h2 class="font-light m-b-xs">
						<asp:Literal ID="litEventName" runat="server"></asp:Literal>
					</h2>
					<small>Enter details for a new rebuild project.</small>
				</div>
			</div>
		</div>

		<!-- HTML Form (wrapped in a .bootstrap-iso div) -->
		<div class="content">
			<div class="row projects">
				<div class="container-fluid">
					<div class="row">
						<div class="col-md-6 col-sm-6 col-xs-12">
							<div class="hpanel">
								<div class="panel-heading">
									<div class="panel-tools">
										<a class="showhide"><i class="fa fa-chevron-up"></i></a>
									</div>
									Survivor Information
								</div>
								<div class="panel-body">
									<div class="m-b-lg border-bottom">
										Which Disaster Affected You?
										<div id="div1" class="dropdown m-b-md" runat="server">
											<button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Choose The Disaster That Affected You <i class="fa fa-bolt"></i></button>
											<ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer">
												<%=disasterDropDown%>
											</ul>
										</div>
										<input type="hidden" id="hidEventId" runat="server" />
									</div>
									<div class="form-group ">
										<div class="input-group">
											<div class="input-group-addon">
												Firstname*
											</div>
											<input class="form-control" id="txtFirstname" runat="server" name="txtFirstname" type="text" placeholder="Enter Firstname" required/>
										</div>
									</div>
									<div class="form-group ">
										<div class="input-group">
											<div class="input-group-addon">
												Lastname*
											</div>
											<input class="form-control" id="txtLastName" runat="server" name="txtLastName" type="text" placeholder="Enter Lastname" required/>
										</div>
									</div>
									<div class="form-group ">
										<div class="input-group">
											<div class="input-group-addon">
												Age
											</div>
											<input class="form-control" onkeypress="return isNumberKey(event)" id="txtAge" runat="server" name="txtAge" type="text" maxlength="2" placeholder="Enter Age"/>
										</div>
									</div>
									<div class="form-group ">
										<div class="input-group">
											<div class="input-group-addon">
												Phone*
											</div>
											<input class="form-control" id="txtTelephone" runat="server" onkeypress="return isNumberKey(event)" maxlength="10" name="tel" type="text" placeholder="xxxxxxxxxx" required/>
										</div>
									</div>
									<div class="form-group ">
										<div class="input-group">
											<div class="input-group-addon">
												Email*
											</div>
											<input class="form-control" id="txtEmail" runat="server" name="email" type="email" placeholder="Enter email" required/>
										</div>
											Use support@Stability.org if no email.
									</div>
								</div>
							</div>
							<div class="hpanel">
								<div class="panel-heading">
									<div class="panel-tools">
										<a class="showhide"><i class="fa fa-chevron-up"></i></a>
									</div>
									Lost Property Information
								</div>
								<div class="panel-body">
									<div class="form-group ">
										<div class="input-group">
											<div class="input-group-addon">
												Loss Address*
											</div>
											<input class="form-control" id="txtLossAddress" runat="server" name="txtLossAddress" type="text" placeholder="Enter loss address" required/>
										</div>
									</div>
									<div class="form-group ">
										<div class="input-group">
											<div class="input-group-addon">
												City*
											</div>
											<input class="form-control" id="txtCity" runat="server" name="txtCity" type="text" placeholder="Enter loss city" required/>
										</div>
									</div>
									<div class="form-group ">
										<select class="select form-control" id="ddlState" runat="server" name="ddlState">
											<option value="Choose a State">
												Choose a State*
											</option>
											<option value="Louisiana">
												Louisiana
											</option>
											<option value="Texas">
												Texas
											</option>
											<option value="North Carolina">
												North Carolina
											</option>
											<option value="South Carolina">
												South Carolina
											</option>
											<option value="South Carolina">
												Florida
											</option>
											<option value="South Carolina">
												Mississippi
											</option>
											<option value="South Carolina">
												Alabama
											</option>
											<option value="South Carolina">
												Virginia
											</option>
											<option value="South Carolina">
												Georgia
											</option>
										</select>
									</div>
									<div class="form-group ">
										<div class="input-group">
											<div class="input-group-addon">
												Zip*
											</div>
											<input class="form-control" id="txtZip" runat="server" onkeypress="return isNumberKey(event)" maxlength="5" name="txtZip" type="text" placeholder="Enter loss zip" required/>
										</div>
									</div>
									<div class="row">
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
										<div class="col-md-4 border-right">
											<div class="form-group ">
												<div class="input-group">
													<div class="input-group-addon">
														Baths
													</div>
													<input class="form-control" id="txtBathrooms" runat="server" onkeypress="return isNumberKey(event)" maxlength="1" name="txtBathrooms" type="text" placeholder="Enter # bathrooms"/>
												</div>
											</div>
										</div>
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
									</div>
								</div>
							</div>
						</div>
						<div class="col-md-6 col-sm-6 col-xs-12">
							<div class="hpanel">
								<div class="panel-heading">
									<div class="panel-tools">
										<a class="showhide"><i class="fa fa-chevron-up"></i></a>
									</div>
									Work Information
								</div>
								<div class="panel-body">
									<div class="row">
										<div class="col-xs-4">
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
										<div class="col-xs-8">
											<div class="m-b-lg border-bottom">
												Choose qualifiers (All that apply)
												<div class="dropdown m-t-sm m-b-md">
													<asp:ListBox ID="ddlQualifiers" SelectionMode="Multiple" DataValueField="RebuildStatusId" DataTextField="Status" runat="server" CssClass="js-source-states-1"></asp:ListBox>
												</div>
											</div>
										</div>
									</div>
									<div class="form-group ">
										Describe the rebuild work
										<textarea class="form-control" cols="40" id="txtRebuildDescription" runat="server" name="message" rows="10"></textarea>
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
							<div class="hpanel" runat="server" id="divNonProfitForm" visible="false">
								<div class="panel-heading">
									<div class="panel-tools">
										<a class="showhide"><i class="fa fa-chevron-up"></i></a>
									</div>
									Collaboration
								</div>
								<div class="panel-body">
									<div class="row">
										<div class="col-xs-4">
											<div class="m-b-lg border-bottom">
												Rebuild Status
												<asp:RadioButtonList RepeatLayout="Flow" ID="rblIsActive" runat="server"></asp:RadioButtonList>
											</div>
									
											<div class="m-b-lg border-bottom">
												Rebuild Stage
												<asp:RadioButtonList RepeatLayout="Flow" ID="rblRebuildStatus" runat="server"></asp:RadioButtonList>
											</div>
										</div>
										<div class="col-xs-8">
											<div class="m-b-lg border-bottom">
												Choose collaborators
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
		</div>
	</div>
</asp:Content>