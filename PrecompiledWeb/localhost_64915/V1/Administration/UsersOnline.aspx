<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_Administration_VolunteerIDList, App_Web_ijye2wuz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<h3>User List</h3>

								Number of Users Online: <asp:Label id="UsersOnlineLabel" runat="Server" /><br />

								<asp:Panel id="NavigationPanel" Visible="false" runat="server">
								<table border="0" cellpadding="3" cellspacing="3">
									<tr>
									<td style="width:100">Page <asp:Label id="CurrentPageLabel" runat="server" />
										of <asp:Label id="TotalPagesLabel" runat="server" /></td>
									<td style="width:60"><asp:LinkButton id="PreviousButton" Text="< Prev"
														OnClick="PreviousButton_OnClick" runat="server" /></td>
									<td style="width:60"><asp:LinkButton id="NextButton" Text="Next >"
														OnClick="NextButton_OnClick" runat="server" /></td>
									</tr>
								</table>
								</asp:Panel>

								<asp:DataGrid id="UserGrid" runat="server"
											CellPadding="2" CellSpacing="1"
											Gridlines="Both">
								<HeaderStyle BackColor="darkblue" ForeColor="white" />
								</asp:DataGrid>
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>