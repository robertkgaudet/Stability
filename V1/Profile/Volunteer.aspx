<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Volunteer.aspx.cs" Inherits="V1_Profile_Volunteer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function ()
		{
			$("span.checkBoxFix").each(function ()
			{
				$(this).find("input:checkbox").addClass("checkbox-primary");
			});
		});

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

		<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="hpanel h<%=headerColor%>">
				<a class="small-header-action">
					<div class="clip-header">
						<i class="fa fa-arrow-up"></i>
					</div>
				</a>
				<div class="panel-body">
					<h2 class="m-b-xs">
						<%=icon%> <asp:Literal ID="litEventName" runat="server"></asp:Literal>
					</h2>
					<asp:Literal ID="litEventDescription" runat="server"></asp:Literal>
					<p class="font-bold text-info">
						<asp:Literal ID="litDate" runat="server"></asp:Literal>
					</p>
				</div>
			</div>
		</div>

		<div class="content">
			<div class="row projects">
				<div class="container-fluid">
					<div class="row">
						<div class="col-md-6 col-sm-6 col-xs-12">
							<div class="hpanel h<%=headerColor%>">
								<div class="panel-heading hbuilt">
									<div class="panel-tools">
										<a class="showhide"><i class="fa fa-chevron-up"></i></a>
										<a class="closebox"><i class="fa fa-times"></i></a>
									</div>
									<h4>Volunteer Registration</h4>
								</div>
								<div class="panel-body">
									<div id="divFormFields" runat="server" class="divRegisterForm" style="width:600px; padding-bottom:100px;">
										<h4>STATEMENT OF PRACTICE</h4>
										Serving on an assignment means your agreement to the following. 
										<ul class="m-t">
											<li>The commitment of a volunteer is to serve and not to be served.</li>
											<li>I am willing to set aside personal preferences, habits and schedule in the interest of others to fulfill the obligation to which I am assigned.</li>
											<li>I understand there are variations in practice and understanding of volunteerism in some areas of culture. I will abide by the standards of the project to which assigned in all areas including dress, entertainment, activities, etc. This includes a willing agreement to abstain from the use of alcohol, drugs, and tobacco and being sensitive to cultural and regional expectations and standards.</li>
											<li>I will seek to provide excellence in attitude and to present a professional outlook at all times.</li>
										</ul>
										<br /><br />
										<div class="hpanel">
											<div class="panel-heading">
												<h4>Choose A Disaster To Volunteer</h4>
													Leave unchecked if none apply at this time.
											</div>
											<div class="panel-body">
												<asp:CheckBoxList ID="chkBoxListDisasters" runat="server" DataTextField="Name" DataValueField="EventId" RepeatDirection="Vertical"></asp:CheckBoxList>
											</div>
										</div>
										<div class="hpanel">
											<div class="panel-heading">
												<h4>Choose Any Organizations You Belong To</h4>
													Leave unchecked if none apply at this time.
											</div>
											<div class="panel-body">
												<asp:CheckBoxList ID="chkBoxOrganizations" runat="server" DataTextField="Name" DataValueField="OrganizationId" RepeatDirection="Vertical"></asp:CheckBoxList>
											</div>
										</div>
										<div class="hpanel">
											<div class="panel-heading">
												Relevant skills
											</div>
											<div class="panel-body">
												<asp:CheckBoxList ID="chkBoxListSkills" RepeatColumns="2" runat="server" DataTextField="Name" DataValueField="SkillId" RepeatDirection="Vertical"></asp:CheckBoxList>
											</div>
										</div>
										





										<div class="hpanel">
											<div class="panel-body">
												<div class="m-t-md">
													<h2>Volunteer Information</h2>
													Please describe in detail your skills and how you think you can help. (max 1000 characters)
													<asp:RequiredFieldValidator id="RequiredFieldValidator3" Display="Dynamic" runat="server" ControlToValidate="txtDescription" ErrorMessage="Your skills are a required field." Font-Size="X-Small" ForeColor="Red"></asp:RequiredFieldValidator>
													<asp:TextBox CssClass="form-control" ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
												</div>
												
												<div class="m-t-md">
													<div class="form-group ">
														<div class="input-group">
															<div class="input-group-addon">
																Dates Available
															</div>
															<asp:TextBox ID="txtDatesAvailable" runat="server" MaxLength="500" CssClass="form-control" required></asp:TextBox>
														</div>
													</div>
												</div>
												
												<div class="m-t-md">
													<div class="form-group ">
														<div class="input-group">
															<div class="input-group-addon">
																Number of Days Available
															</div>
															<asp:TextBox ID="txtDaysAvailable" runat="server" Width="300" MaxLength="4" CssClass="form-control" required></asp:TextBox>
															<br />
															
														</div>
													</div>
												</div>

												<div class="m-t-md m-b-lg bg-light">
													<h3>Previous Volunteer Experience.</h3> Please include names of other organizations you've volunteered with, places and dates.
													<asp:TextBox ID="txtPreviousExperience" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
												</div>
											
												<div class="form-group m-b">
													<div class="input-group">
														<asp:Button ID="btnSubmit" runat="server" Text="Submit Volunteer Information" OnClick="btnSubmit_Click" CssClass="btn btn-success btn-lg" />
													</div>
												</div>
											</div>
										</div>
									</div>
									<div id="divResults" runat="server" class="divRegisterForm" visible="false" style="max-width:600px; margin-bottom:30px;">
										<h1>Your Volunteer ID Number - <asp:Literal ID="lblNumber" runat="server"></asp:Literal></h1>
										<asp:Label ID="lblResults" runat="server"></asp:Label>
										<br />
									</div>
								</div>
								<div class="panel-footer">
									Stability Community Aid Platform
								</div>
							</div>
						</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>