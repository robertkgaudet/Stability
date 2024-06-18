<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimeBoard.ascx.cs" Inherits="V1_UserControls_TimeBoard" %>

					<asp:Repeater ID="rpNonProfitPeople" runat="server" OnItemDataBound="rpNonProfitPeople_ItemDataBound">
						<ItemTemplate>
								<asp:Literal ID="lblInfo" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:Repeater>