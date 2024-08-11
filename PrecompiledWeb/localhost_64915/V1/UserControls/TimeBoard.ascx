<%@ control language="C#" autoeventwireup="true" inherits="V1_UserControls_TimeBoard, App_Web_qaqxyxhv" %>

					<asp:Repeater ID="rpNonProfitPeople" runat="server" OnItemDataBound="rpNonProfitPeople_ItemDataBound">
						<ItemTemplate>
								<asp:Literal ID="lblInfo" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:Repeater>