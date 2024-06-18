<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/DisasterRegistry.master" AutoEventWireup="true" CodeFile="Survey-Items.aspx.cs" Inherits="V1_Surveys_Survey_Items" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>

    <script>
		$(function (){
            $("#form1").validate({
                rules:
                {
                    <%=numberRules%>
				},
				submitHandler: function (form) {
					form.submit();
				},
				errorPlacement: function (error, element) {
					$(element)
						.closest("form")
						.find("label[for='" + element.attr("id") + "']")
						.append(error);
				},
				errorElement: "span",
			});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="container">
			<div class="hpanel">
				<div class="panel-body">
					<h2 class="font-light m-b-xs">
						Create a New Stability Disaster Recovery Wishlist
					</h2>
                    <asp:Literal id="litName" runat="server"></asp:Literal>
				</div>
			</div>
            <div class="row">
                <div class="col-sm-12">
					<div class="form-group">
						<label class="col-sm-2 control-label">Survey Title</label>
						<div class="col-sm-8"><input type="text" required runat="server" id="txtSurveyTitle" class="form-control" placeholder="Item Name"></div>
					</div>
				</div>
                <div class="col-sm-12">
					<div class="form-group">
						<label class="col-sm-2 control-label">Description *</label>
						<div class="col-sm-8"><textarea required runat="server" id="txtSruveyDescription" class="form-control" rows="5" placeholder="Item Description"/></div>
				    </div>
                </div>
            </div>
            <asp:Repeater ID="rptOneCountItemTypes" runat="server" OnItemDataBound="rptOneCountItemTypes_ItemDataBound">
                <ItemTemplate>
                    <h4><asp:Literal ID="litItemTypeHeaderSingle" runat="server"></asp:Literal></h4>
                    <asp:CheckBoxList runat="server" ID="cblSubItemType" DataTextField="Name" DataValueField="ItemSubTypeId" RepeatDirection="Vertical"></asp:CheckBoxList>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Repeater ID="rptMultiCountItemTypes" runat="server" OnItemDataBound="rptMultiCountItemTypes_ItemDataBound">
                <ItemTemplate>
                    <h3><asp:Literal ID="litItemTypeHeaderMultiple" runat="server"></asp:Literal></h3>
                    <asp:Panel ID="pnlTextBoxes" runat="server"></asp:Panel>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
        </div>
</asp:Content>