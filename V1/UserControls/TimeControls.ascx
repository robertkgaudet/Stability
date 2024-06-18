<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimeControls.ascx.cs" Inherits="UserControls_TimeControls" %>
<style>
	.btn-xlarge {
		padding: 10px 28px;
		font-size: 15px;
		line-height: normal;
		margin-top:10px;	
	}

	.timesheet-well{
		background-color:#3498DB;
		max-width:500px;
		color:white;
	}

	.dropdown-volunteer
	{
		font-size: 15px;
		margin-top:10px;
	}
</style>

<script type="text/javascript">
	$(document).ready(function ()
	{
		$("#<%=btnTimeIn.ClientID%>").attr("disabled", "disabled");
		$("#<%=btnTimeIn.ClientID%>").bind('click', function (e) {
			e.preventDefault();
		})

		$(".dropdown-menu li").click(function () {
			$("#btn-dropdown").html($(this).text());
			$("#<%=hidTaskType.ClientID%>").val($(this).attr('id'));
			//alert($("#<%=hidTaskType.ClientID%>").val());
			//$("#<%=btnTimeIn.ClientID%>").show();

			$("#<%=btnTimeIn.ClientID%>").removeAttr('disabled');
			$("#<%=btnTimeIn.ClientID%>").unbind('click')
		});

	});
</script>
<div class="container-fluid">
	<div class="row">
		<div class="col-sm-2"></div>
		<div class="col-xs-12 col-sm-8">
			<div class="text-center">
				<div class="well timesheet-well center-block">
					<h1><asp:Literal ID="litTime" runat="server"></asp:Literal> </h1>
					<small><asp:Literal ID="litTimeMessage" runat="server"></asp:Literal></small>
					<div id="divTaskTypedropdown" class="dropdown" runat="server">
						<input type="hidden" id="hidTaskType" runat="server" />
						<button id="btn-dropdown" class="btn btn-primary dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">SELECT VOLUNTEER TYPE <span class="glyphicon glyphicon-list-alt"></span></button>
						<ul class="dropdown-menu text-center dropdown-volunteer">
							<li id="0CB3112A-D673-4704-9DB2-48410C60576A"><a href="#">Driver</a></li>
							<li id="4A575DBD-A58D-41F3-99A7-B3D83F75FB34"><a href="#">Wellness Checks</a></li>
							<li id="8B258015-811F-4017-BDD9-0EB1B844B2AB"><a href="#">Dispatcher</a></li>
							<li id="f230c0cf-2586-4e24-84d9-68350571761a"><a href="#">Case Work</a></li>
							<li id="8f954a49-24f6-47f4-a266-5666059e9540"><a href="#">Muckout/Cleanup</a></li>
							<li id="baebf9d3-9fd3-46af-8f76-37fd5bcf85fd"><a href="#">Rebuild/Sheetrock/Flooring</a></li>
							<li id="c92cad98-ec29-49b0-94e4-2f6996a70fe8"><a href="#">Surveys</a></li>
						</ul>
					</div>
					<asp:LinkButton id="btnTimeIn" runat="server" name="btnTimeIn" OnClick="btnTimeIn_Click" Text="CLOCK IN" class="btn btn-success btn-block btn-xlarge" />
					<div id="divTimeDescription" runat="server" class="form-group">
							<label class="control-label" for="chkBoxLivingRoom"><h2>Time Description</h2>When done, please enter in detail the work you've been doing!</label>
							<asp:TextBox ID="txtTimeDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control"></asp:TextBox>
					</div>
					<asp:LinkButton id="btnTimeOut" runat="server" name="btnTimeOut" OnClick="btnTimeOut_Click" Text="CLOCK OUT" class="btn btn-success btn-block btn-xlarge" />
					
				</div>
			</div>
		</div>
		<div class="col-sm-2"></div>
	</div>
</div>