<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_Administration_Item_AddItem, App_Web_vfs24er2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
	<link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
	<script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>
	<script>

        $(document).ready(function () {
            $('#<%=divLocationType.ClientID%>').<%=toggleLocationTypeVisibility%>(); 
            $('#<%=divRoles.ClientID%>').<%=togglePersonVisibility%>();        

            $('#<%=chkLocation.ClientID%>').change(function () {
                if (this.checked) {
                    //Show the person selectors
                    $('#<%=divLocationType.ClientID%>').show();
                } else {
                    //Hide the person selectors
                    $('#<%=divLocationType.ClientID%>').hide();
                }
            });

            $('#<%=chkPerson.ClientID%>').change(function () {
                if (this.checked) {
                    //Show the person selectors
                    $('#<%=divRoles.ClientID%>').show();
                } else {
                    //Hide the person selectors
                    $('#<%=divRoles.ClientID%>').hide();
                }
            });


       <%--     $("#recoveryStage.dropdown-menu li").click(function () {
                $("#btn-dropdown.recoveryStage").html($(this).text());
                $("#<%=hidBrandId.ClientID%>").val($(this).attr('id'));
                });

                $(".js-source-states-1").select2();--%>
        });

		$(function (){
            $("#form1").validate({
                rules:
                {
					<%=txtPrice.UniqueID%>: 
                    {
                        required: true,
                        number: true
                    },
					    <%=txtVendorPrice.UniqueID%>:
                    {
                        number: true
                    },
					    <%=txtCost.UniqueID%>:
                    {
                        number: true
                    },
					    <%=txtShippingCharge.UniqueID%>:
                    {
                        number: true
                    }, 
					    <%=txtMarkupPercent.UniqueID%>:
                    {
                        number: true
                    },
					    <%=txtTaxAmount.UniqueID%>:
                    {
                        number: true
                    },
					    <%=txtURLFriendlyItemName.UniqueID%>:
                    {
                        required: true
                    },
					    <%=txtItemName.UniqueID%>:
                    {
                        required: true
                    },
					    <%=txtSku.UniqueID%>:
                    {
                        required: true
                    },
					    <%=txtItemDescription.UniqueID%>:
                    {
                        required: true
                    },
                    <%=txtSourceURL.UniqueID%>:
                    {
                        url: true
                    }
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
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:HiddenField ID="hidBrandId" runat="server" />
    <asp:HiddenField ID="hidEntityTypeId" runat="server" />
    <asp:HiddenField ID="hidItemSubTypeId" runat="server" />
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<a class="small-header-action">
								<div class="clip-header">
									<i class="fa fa-arrow-up"></i>
								</div>
							</a>
							<h2 class="font-light m-b-xs">
								Add New Item
							</h2>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-12 container">
					<div class="hpanel form-horizontal">

						<div class="panel-heading hbuilt">
							Enter the Item Details
						</div>

						<div class="panel-body">
							<div id="divMessage" class="alert alert-success m-b-lg" runat="server" visible="false">
								<i class="fa fa-bolt"></i>
								<asp:Label runat="server" id="lblMessage"></asp:Label>
							</div>

							<div id="div1" runat="server" class="alert alert-success m-b-lg" visible="false">
								<i class="fa fa-bolt"></i>
								<asp:Label runat="server" id="Label1"></asp:Label>
								<br />
								<asp:HyperLink runat="server" id="hypAddNewItem" text="Add New Item"></asp:HyperLink> |
								<asp:HyperLink runat="server" id="hypViewItem" text="View This Item"></asp:HyperLink>
							</div>
                            
                            
                            <div class="form-group" runat="server" visible="false" id="divUploadImage">
								<label class="col-sm-2 control-label">Upload Item Image</label>
                                <div class="col-sm-5">
                                    <asp:HyperLink ID="hypPhotoUpload" runat="server" Text="Upload Item Image"></asp:HyperLink>
								</div>
                            </div>
                            <div class="form-group">
								<label class="col-sm-2 control-label">Entity Type *</label>  (Required. Choose one or both)
								<div class="col-sm-5">
                                    <asp:CheckBox ID="chkPerson" Text="Person" runat="server" /><br />
                                    <asp:CheckBox ID="chkLocation" Text="Location" runat="server" />
								</div>
                            </div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Item Name *</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtItemName" class="form-control" placeholder="Item Name"></div>
							</div>
                            
							<div class="form-group">
								<label class="col-sm-2 control-label">URL Friendly Item Name (No characters or spaces) *</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtURLFriendlyItemName" class="form-control" placeholder="URL Friendly Item Name (No charaters or spaces allowed)"></div>
							</div>

						    <div class="form-group">
							    <label class="col-sm-2 control-label">Description *</label>
							    <div class="col-sm-5"><textarea required runat="server" id="txtItemDescription" class="form-control" placeholder="Item Description"/></div>
						    </div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Price *</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtPrice" class="form-control" placeholder="Price"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Shipping Charge</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtShippingCharge" class="form-control" placeholder="Shipping Charge"></div>
							</div>
                            
							<div class="form-group">
								<label class="col-sm-2 control-label">Source URL</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtSourceURL" class="form-control" placeholder="Source URL"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">SKU</label>
								<div class="col-sm-5"><input type="text" required runat="server" id="txtSku" class="form-control" placeholder="SKU"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Vendor Price</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtVendorPrice" class="form-control" placeholder="Vendor Price"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Cost</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtCost" class="form-control" placeholder="Cost"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Markup Percent</label>(Overrides default markup percent)
								<div class="col-sm-5"><input type="text" runat="server" id="txtMarkupPercent" class="form-control" placeholder="Markup Percent"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Tax Amount</label>
								<div class="col-sm-5"><input type="text" runat="server" id="txtTaxAmount" class="form-control" placeholder="Tax Amount"></div>
							</div>

							<div class="form-group">
								<label class="col-sm-2 control-label">Select Brand</label>
								<div class="col-sm-5">
                                    <asp:DropDownList ID="ddlBrand" runat="server" CssClass="form-control" DataTextField="BrandName" DataValueField="BrandId"></asp:DropDownList>
								</div>
							</div>
                            <div class="form-group" runat="server" id="div3">
								<label class="col-sm-2 control-label">Item Type</label>
								<div class="col-sm-5">
                                    <asp:DropDownList ID="ddlItemType" runat="server" CssClass="form-control" DataTextField="Name" DataValueField="ItemTypeId"></asp:DropDownList>
								</div>
                            </div>
                            <div class="form-group" runat="server" id="div4">
								<label class="col-sm-2 control-label">Item Sub Type</label>
								<div class="col-sm-5">
                                    <asp:DropDownList ID="ddlItemSubType" runat="server" CssClass="form-control" DataTextField="Name" DataValueField="ItemSubTypeId"></asp:DropDownList>
								</div>
                            </div>
                            
                            
                            <div class="form-group" runat="server" id="divRoles">
								<label class="col-sm-2 control-label">Roles</label>
								<div class="col-sm-5">
                                    <asp:CheckBoxList runat="server" ID="cblRolesTypes" DataTextField="RoleTypeName" DataValueField="RoleTypeId"></asp:CheckBoxList>
								</div>
                            </div>
                            
                            <div class="form-group" runat="server" id="divLocationType">
								<label class="col-sm-2 control-label">Location Type</label>
								<div class="col-sm-5">
                                    <asp:CheckBoxList runat="server" RepeatColumns="2" ID="cblLocationTypes" DataTextField="Name" DataValueField="LocationTypeId"></asp:CheckBoxList>
								</div>
                            </div>

                            <div class="form-group" runat="server" id="div2">
								<label class="col-sm-2 control-label">Recovery Stages</label>
								<div class="col-sm-5">
                                    <asp:CheckBoxList runat="server" ID="cblRecoveryStages" DataTextField="Lable" DataValueField="SliderTickId"></asp:CheckBoxList>
								</div>
                            </div>

                            <div class="form-group" runat="server" id="div5">
								<label class="col-sm-2 control-label">Events</label>
								<div class="col-sm-5">
                                    <asp:CheckBoxList runat="server" ID="chkCblEvent" DataTextField="Name" DataValueField="EventId"></asp:CheckBoxList>
								</div>
                            </div>
                            
							<div class="form-group">
								<label class="col-sm-2 control-label"></label>
								<div class="col-sm-5">
									<div class="pull-right">
										<asp:LinkButton id="btnCancel" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default" Text="Cancel" />
										<asp:Button id="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text=" Submit " />
									</div>
								</div>
							</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>