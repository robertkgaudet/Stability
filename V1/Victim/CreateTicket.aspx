<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Basic.master" AutoEventWireup="true" CodeFile="CreateTicket.aspx.cs" Inherits="V1_Victim_CreateTicket" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


	<script>

<%--		$(function () {
			$("#form1").validate(
				{
					rules:
					{
						<%=txtAmazonWishlist.UniqueID%>:{ url: true }
					},
			});
		});--%>

		function isNumberKey(evt) {
			var charCode = (evt.which) ? evt.which : evt.keyCode;
			if (charCode == 110 || charCode == 190 || charCode == 46)
				return true;

			if (charCode > 31 && (charCode < 48 || charCode > 57))
				return false;

			return true;
		}

		$(document).ready(function () {
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

			$(".btnAddLocation").click(function () {
				location.href = "AddLocation.aspx";
			});
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
	
<script type="text/javascript">


</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="hpanel">
		<div class="panel-body">
			<h2 class="font-light m-b-xs">
				<asp:Literal ID="litEventName" runat="server"></asp:Literal>
			</h2>
			Create a help request and we will attemtp to connect you with a disaster relief team who can help.
		</div>
	</div>
	<div class="hpanel">
        <div class="panel-heading hbuilt">
            <b> A list of states and counties where teams may be deployed.</b>
        </div>
        <div class="alert alert-success">
            <i class="fa fa-bolt"></i> Because teams work on a volunteer basis, we cannot guarantee a team will be availabe to assist.
        </div>
        <div class="panel-body">
			<asp:Literal ID="litStatesCounties" runat="server"></asp:Literal>
        </div>
    </div>
	<div class="hpanel">
		<div class="panel-body">
			<asp:Literal ID="litPastCases" runat="server"></asp:Literal>
		</div>
	</div>
	<div class="hpanel">
		<div class="panel-body">
			
			<div class="form-group"><label class="col-sm-2 control-label">What I Need Help With</label>
				<div class="col-sm-10">
					Choose Help Needed (Choose all that apply)
					<div class="dropdown m-t-sm m-b-md">
						<asp:ListBox ID="ddlBasicNeeds" SelectionMode="Multiple" DataValueField="RebuildStatusId" DataTextField="Status" runat="server" CssClass="js-source-states-1 font-bold"></asp:ListBox>
					</div>
					<div class="hr-line-dashed color-line"></div>
				</div>
			</div>

			<div class="form-group"><label class="col-sm-2 control-label">Vulnerabilities</label>
				<div class="col-sm-10">
					Choose Vulnerabilities (Choose all that apply)
					<div class="dropdown m-t-sm m-b-md">
						<asp:ListBox ID="ddlQualifiers" SelectionMode="Multiple" DataValueField="RebuildStatusId" DataTextField="Status" runat="server" CssClass="js-source-states-1 font-bold"></asp:ListBox>
					</div>
					<div class="hr-line-dashed color-line"></div>
				</div>
			</div>

			<div class="form-group"><label class="col-sm-2 control-label">Choose A Portal</label>
				<div class="col-sm-10">
					<div id="div1" class="dropdown m-b-md" runat="server">
						<button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer font-size-xlarge font-bold" type="button" data-toggle="dropdown">Choose A Community Relief Portal <i class="fa fa-sort-down"></i></button>
						<ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required font-size-xlarge font-bold">
							<%=disasterDropDown%>
						</ul>
						<br />
					Choose a community portal nearby and we will try to connect you to a team who may be able to help you.
					</div>
					<input type="hidden" id="hidEventId" runat="server" />
					<div class="hr-line-dashed color-line"></div>
				</div>
			</div>

			<div class="form-group"><label class="col-sm-2 control-label">Recovery Stage</label>
				<div class="col-sm-10">
					<div id="div3" class="dropdown m-b-md" runat="server">
						<button id="btn-dropdown" class="btn btn-outline btn-default recoveryStage dropdown-toggle dropdown-volunteer font-size-xlarge font-bold" type="button" data-toggle="dropdown">Choose Recovery Stage <i class="fa fa-sort-down"></i></button>
						<ul id="recoveryStage" class="dropdown-menu text-center dropdown-volunteer required">
							<%=recoveryDropDown%>
						</ul>
					</div>
					Tell us about your current living situation.
					<input type="hidden" id="hidRecoveryStage" runat="server" />
					<div class="hr-line-dashed color-line"></div>
				</div>
			</div>

			<div class="form-group"><label class="col-sm-2 control-label">Housing Situation</label>
				<div class="col-sm-10">
					<div id="div2" class="dropdown m-b-md" runat="server">
						<button id="btn-dropdown" class="btn btn-outline btn-default disasterHousing dropdown-toggle dropdown-volunteer font-size-xlarge font-bold" type="button" data-toggle="dropdown">Choose Housing <i class="fa fa-sort-down"></i></button>
						<ul id="disasterHousing" class="dropdown-menu text-center dropdown-volunteer required">
							<%=housingDropDown%>
						</ul>
					</div>
					What best describes current living condition?
					<input type="hidden" id="hidHousingId" runat="server" />
					<div class="hr-line-dashed color-line"></div>
				</div>
			</div>

			<div class="form-group pull-right	">
				<div>
					<asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Create A Help Request" OnClick="btnSubmit_Click" />
				</div>
			</div>
		</div>
	</div>
</asp:Content>
