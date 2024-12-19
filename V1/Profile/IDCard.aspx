<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="IDCard.aspx.cs" Inherits="V1_Profile_IDCard" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		#<%=imgProfile.ClientID%>
		{
			max-width:60%;
		}

		.bg-danger
		{
			background-color:#DD1447;
			color:white;
			margin-top:0px;
			padding:10px;
		}
		.bodyBGColor, .content
		{
			background-color:white;
			color:#333333;
		}
		.hpanel
		{
			border:solid 1px #CCCCCC;
		}
		a
		{
			color:#333333;	
		}
		.panel-body-bg {
			background-color:none;
			background-image: url(/V1/Images/300pxlogo-opacity.png);
			background-repeat: no-repeat;
			background-size: cover;
			background-position: 100px 105px;
			height:280px;
			margin:0px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="content animate-panel no-margins" data-child="hpanel" data-effect="fadeInDown" style="width:340px;">
			<div class="hpanel no-margins">
				<div class="text-center bg-danger">
					<br />
					<h4 class="no-margins font-bold"><i class="fa fa-id-card-o"></i> <asp:Literal id="litTitle" runat="server"></asp:Literal> </h4>
				</div>
				<div class="alert alert-danger text-center">
					<h4 class="font-bold no-margins">GROUND FORCE HUMANITARIAN AID</h4>
				</div>
				<div class="panel-body-bg">
					<div class="m-md">
						<asp:Image runat="server" class="img-rounded" id="imgProfile"></asp:Image>
						<p>
							<dl class="no-margins text-left">
								<dt><h2 class="no-margins"><a href="/V1/Profile/Profile.aspx" class="font-bold"><asp:Literal id="lblName" runat="server"></asp:Literal></a></h2></dt>
								<%=locationDD %>
								<%=zelloDD%>
								<asp:Literal id="litNumber" runat="server"></asp:Literal>
								<dd><h5 class="no-margins">Verify: 203-520-4484 </h5></dd>
							</dl>
						</p>
					</div>
				</div>
				<div class="bg-danger hred text-center">
					<h2 class="font-bold"><i class="fa fa-user-circle"></i> VOLUNTEER</h2>
				</div>
			</div>
		</div>
</asp:Content>