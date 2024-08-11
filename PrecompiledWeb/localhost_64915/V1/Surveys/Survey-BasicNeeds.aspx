<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/DisasterRegistry.master" autoeventwireup="true" inherits="S1_Profile_Survey_BasicNeeds, App_Web_2mg1w42w" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="normalheader ">
    <div class="hpanel">
        <div class="panel-body">
            <a class="small-header-action" href="">
                <div class="clip-header">
                    <i class="fa fa-arrow-up"></i>
                </div>
            </a>
            <h2 class="font-light m-b-xs">
                Stability Wish List
            </h2>
            <small>What things do you need help buying?</small>
        </div>
    </div>
</div>
	
				<div class="container help-1" id="divFormFields" runat="server">
					<div class="row">
						<div class="col-sm-2"></div>
						<div class="col-xs-12 col-sm-8">
							<div class="form-horizontal">
							<fieldset>

							<!-- Form Name -->
							<legend><h1>Request Help</h1></legend>
							<small>
								<b>Stability Basic Needs Survey</b> captures only the basic household needs of a person in distress.
								<br /><br />
								We do not offer rebuild assistance. If you need immediate help of any kind, such as help rebuilding your home or information about federal, state or local resources please work with your case manager to address those needs.
								<br /><br />
								By posting this information you agree to allow Stability to share personal and non-personal information with citizens and other organizations who are willing to help you meet these needs.
								If you are posting on behalf of another party, you must obtain permission from that person before sharing their needs here.
								<br /><br />
							</small>
								<br /><br />

							<!-- Text input-->
							<div class="form-group">
								<div class="col-md-12 center-block bg-4">
									<label class="bg-4"><h1>INFORMATION ABOUT PERSON NEEDING HELP</h1></label>
								</div>
							</div>

							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtFirstname">Submitting Organization</label>
							  <div class="col-md-4">
								  <asp:Label ID="lblOrganization" runat="server"></asp:Label><asp:HiddenField ID="hidOrganization" runat="server" />
								  <asp:DropDownList ID="ddlOrganization" runat="server" DataValueField="OrganizationId" DataTextField="Name" class="form-control input-md" required=""></asp:DropDownList>
								  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlOrganization" ForeColor="Red" ErrorMessage="Please choose!" InitialValue="-- Select --"></asp:RequiredFieldValidator>
							  </div>
							</div>

							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtFirstname">First Name*</label>  
							  <div class="col-md-4">
								  <asp:TextBox ID="txtFirstname" name="txtFirstname" runat="server" placeholder="" class="form-control input-md" required=""></asp:TextBox>
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtLastname">Last Name*</label>  
							  <div class="col-md-4">
								  <asp:TextBox ID="txtLastname" name="txtLastname" runat="server" placeholder="" class="form-control input-md" required=""></asp:TextBox>
	
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtFemaNumber">FEMA #</label>
							  <div class="col-md-4">
								  <asp:TextBox ID="txtFemaNumber" name="txtFemaNumber" runat="server" placeholder="Enter your FEMA # if you have one." class="form-control input-md"></asp:TextBox>
	
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtPhoneNumber">Phone Number*</label>  
							  <div class="col-md-4">
							  <asp:TextBox id="txtPhoneNumber" name="txtPhoneNumber" runat="server" type="text" placeholder="" class="form-control input-md" required=""></asp:TextBox>
	
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtEmail">Email Address*</label>  
							  <div class="col-md-4">
							  <asp:TextBox id="txtEmail" name="txtEmail" runat="server" type="text" placeholder="" class="form-control input-md" required=""></asp:TextBox>
	
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtLossAddress">Home/Loss Address*</label>  
							  <div class="col-md-4">
							  <asp:TextBox id="txtLossAddress" name="txtLossAddress" runat="server" type="text" placeholder="Address where the loss occurred." class="form-control input-md" required=""></asp:TextBox>
	
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtLossCity">Home/Loss City*</label>  
							  <div class="col-md-4">
							  <asp:TextBox id="txtLossCity" name="txtLossCity" runat="server" type="text" placeholder="City where the loss occurred." class="form-control input-md" required=""></asp:TextBox>
	
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtLossCountyParish">Home/Loss Parish/County*</label>  
							  <div class="col-md-4">
							  <asp:TextBox id="txtLossCountyParish" name="txtLossCountyParish" runat="server" type="text" placeholder="Parish where the loss occurred." class="form-control input-md" required=""></asp:TextBox>
	
							  </div>
							</div>

							<!-- Select Basic -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="ddlLossState">Home/Loss State*</label>
							  <div class="col-md-4">
								  <asp:DropDownList id="ddlLossState" runat="server" name="ddlLossState" CssClass="form-control">
									<asp:ListItem value="AL">Alabama</asp:ListItem>
									<asp:ListItem value="AK">Alaska</asp:ListItem>
									<asp:ListItem value="AZ">Arizona</asp:ListItem>
									<asp:ListItem value="AR">Arkansas</asp:ListItem>
									<asp:ListItem value="CA">California</asp:ListItem>
									<asp:ListItem value="CO">Colorado</asp:ListItem>
									<asp:ListItem value="CT">Connecticut</asp:ListItem>
									<asp:ListItem value="DE">Delaware</asp:ListItem>
									<asp:ListItem value="DC">District Of Columbia</asp:ListItem>
									<asp:ListItem value="FL">Florida</asp:ListItem>
									<asp:ListItem value="GA">Georgia</asp:ListItem>
									<asp:ListItem value="HI">Hawaii</asp:ListItem>
									<asp:ListItem value="ID">Idaho</asp:ListItem>
									<asp:ListItem value="IL">Illinois</asp:ListItem>
									<asp:ListItem value="IN">Indiana</asp:ListItem>
									<asp:ListItem value="IA">Iowa</asp:ListItem>
									<asp:ListItem value="KS">Kansas</asp:ListItem>
									<asp:ListItem value="KY">Kentucky</asp:ListItem>
									<asp:ListItem value="LA" Selected="True">Louisiana</asp:ListItem>
									<asp:ListItem value="ME">Maine</asp:ListItem>
									<asp:ListItem value="MD">Maryland</asp:ListItem>
									<asp:ListItem value="MA">Massachusetts</asp:ListItem>
									<asp:ListItem value="MI">Michigan</asp:ListItem>
									<asp:ListItem value="MN">Minnesota</asp:ListItem>
									<asp:ListItem value="MS">Mississippi</asp:ListItem>
									<asp:ListItem value="MO">Missouri</asp:ListItem>
									<asp:ListItem value="MT">Montana</asp:ListItem>
									<asp:ListItem value="NE">Nebraska</asp:ListItem>
									<asp:ListItem value="NV">Nevada</asp:ListItem>
									<asp:ListItem value="NH">New Hampshire</asp:ListItem>
									<asp:ListItem value="NJ">New Jersey</asp:ListItem>
									<asp:ListItem value="NM">New Mexico</asp:ListItem>
									<asp:ListItem value="NY">New York</asp:ListItem>
									<asp:ListItem value="NC">North Carolina</asp:ListItem>
									<asp:ListItem value="ND">North Dakota</asp:ListItem>
									<asp:ListItem value="OH">Ohio</asp:ListItem>
									<asp:ListItem value="OK">Oklahoma</asp:ListItem>
									<asp:ListItem value="OR">Oregon</asp:ListItem>
									<asp:ListItem value="PA">Pennsylvania</asp:ListItem>
									<asp:ListItem value="RI">Rhode Island</asp:ListItem>
									<asp:ListItem value="SC">South Carolina</asp:ListItem>
									<asp:ListItem value="SD">South Dakota</asp:ListItem>
									<asp:ListItem value="TN">Tennessee</asp:ListItem>
									<asp:ListItem value="TX">Texas</asp:ListItem>
									<asp:ListItem value="UT">Utah</asp:ListItem>
									<asp:ListItem value="VT">Vermont</asp:ListItem>
									<asp:ListItem value="VA">Virginia</asp:ListItem>
									<asp:ListItem value="WA">Washington</asp:ListItem>
									<asp:ListItem value="WV">West Virginia</asp:ListItem>
									<asp:ListItem value="WI">Wisconsin</asp:ListItem>
									<asp:ListItem value="WY">Wyoming</asp:ListItem>
								</asp:DropDownList>
							  </div>
							</div>
								

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtZipCode">Zip Code*</label>  
							  <div class="col-md-4">
							  <asp:TextBox id="txtZipCode" name="txtZipCode" MaxLength="5" runat="server" type="text" class="form-control input-md" required=""></asp:TextBox>
	
							  </div>
							</div>

							<div class="form-group">
								<div class="col-md-12 center-block bg-4">
									<label class="bg-4"><h1>LOSS INFORMATION</h1></label>
								</div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtPeopleInHome">Number of people living in home during disaster?*</label>  
							  <div class="col-md-2">
							  <asp:TextBox id="txtPeopleInHome" name="txtPeopleInHome" runat="server" type="text" placeholder="" maxlength="2" class="form-control input-md" required="" data-bind="value:replyNumber"></asp:TextBox>
								
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtBedrooms">Number of Bedrooms</label>  
							  <div class="col-md-1">
							  <asp:TextBox id="txtBedrooms" name="txtBedrooms" runat="server" type="text" placeholder="" maxlength="1" class="form-control input-md" data-bind="value:replyNumber"></asp:TextBox>
	
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtBathrooms">Number of Bathrooms</label>  
							  <div class="col-md-1">
							  <asp:TextBox id="txtBathrooms" name="txtBathrooms" runat="server" type="text" placeholder="" maxlength="1" class="form-control input-md" data-bind="value:replyNumber"></asp:TextBox>
	
							  </div>
							</div>

							<!-- Multiple Radios (inline) -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="radioRebuildRepair">Is your home habitable?</label>
							  <div class="col-md-4"> 
								  <asp:RadioButtonList CssClass="radio-inline" ID="rblRebuildRepair" runat="server" RepeatDirection="Vertical" RepeatLayout="Table">
									  <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
									  <asp:ListItem Text="No" Value="No"></asp:ListItem>
								  </asp:RadioButtonList>
							  </div>
							</div>

							<!-- Multiple Radios (inline) -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="radioHousingNeed">Immediate housing needed?</label>
							  <div class="col-md-4"> 
								  <asp:RadioButtonList CssClass="radio-inline" ID="rblHousingNeed" runat="server" RepeatDirection="Vertical" RepeatLayout="Table">
									  <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
									  <asp:ListItem Text="No" Value="No"></asp:ListItem>
								  </asp:RadioButtonList>
							  </div>
							</div>

							<!-- Multiple Radios (inline) -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="radioSupportSystem">Is any support already being provided?</label>
							  <div class="col-md-4"> 
								  <asp:RadioButtonList CssClass="radio-inline" ID="rblSupportSystem" runat="server" RepeatDirection="Vertical" RepeatLayout="Table">
									  <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
									  <asp:ListItem Text="No" Value="No"></asp:ListItem>
								  </asp:RadioButtonList>
							  </div>
							</div>
								
							<div class="form-group">
								<div class="col-md-12 center-block bg-4">
									<label class="bg-4"><h1>BASIC NEEDS</h1></label>
								</div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtSheetRock">Panels of Sheetrock Needed</label>  
							  <div class="col-md-2">
							  <asp:TextBox id="txtSheetRock" name="txtSheetRock" guid="e9e2af70-87d6-4f63-a277-2a4660494e32" runat="server" type="text" placeholder="" maxlength="3" class="form-control input-md" data-bind="value:replyNumber"></asp:TextBox>
							  <span class="help-block">Enter number of panels of sheetrock needed.</span>  
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtInsulation">Rolls of Insulation Needed</label>  
							  <div class="col-md-2">
							  <asp:TextBox id="txtInsulation" name="txtInsulation" guid="6a9f913c-0e8a-4134-a977-8af1c0419f2f" runat="server" type="text" placeholder="" maxlength="3" class="form-control input-md" data-bind="value:replyNumber"></asp:TextBox>
							  <span class="help-block">Enter number of rolls of insulation needed.</span>  
							  </div>
							</div>

							<!-- Multiple Radios (inline) -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="radioClothingNeed">Clothing Needed</label>
							  <div class="col-md-4">
								  <asp:CheckBox CssClass="radio-inline" guid="3c79f7dc-9113-4fa9-83ed-53d829a24332" ID="cbClothing" runat="server" />
							  </div>
							</div>
								
							<!-- Multiple Radios (inline) -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="radioLinen">Is linen needed?</label>
							  <div class="col-md-4"> 
								  <asp:CheckBoxList CssClass="radio-inline" ID="cblLinen" runat="server" RepeatLayout="Table" RepeatColumns="1" Font-Bold="false">
									  <asp:ListItem Text="Sheets & Pillow Cases" Value="50ce64ab-ce4f-426c-a094-eff308174d7e"></asp:ListItem>
									  <asp:ListItem Text="Towels & Washclothes" Value="2b01802d-eb4c-472c-a240-9bb653d29e21"></asp:ListItem>
									  <asp:ListItem Text="Dish Towels" Value="d0efd83c-5233-4d55-a541-c2e54c88d10c"></asp:ListItem>
								  </asp:CheckBoxList>
							  </div>
							</div>
								
							<!-- Multiple Radios (inline) -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="radioWindowTreatments">Are window treatments needed?</label>
							  <div class="col-md-4"> 
								  <asp:CheckBoxList CssClass="radio-inline" ID="cblWindowTreatments" runat="server" RepeatLayout="Table" RepeatColumns="1" Font-Bold="false">
									  <asp:ListItem Text="Blinds" Value="400b0539-582d-44dd-98e4-efa9ab71dd28"></asp:ListItem>
									  <asp:ListItem Text="Curtains" Value="85819339-80f6-4130-8af1-7d4e3d10151c"></asp:ListItem>
									  <asp:ListItem Text="Shades" Value="9792511b-b1c4-4906-ad34-f188f167921f"></asp:ListItem>
								  </asp:CheckBoxList>
							  </div>
							</div>

							<div class="form-group">
							  <label class="col-md-5 control-label" for="txtSingleBeds">BEDROOM FURNITURE NEEDED</label> 
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtSingleBeds">Single</label>  
							  <div class="col-md-1">
							  <asp:TextBox id="txtSingleBeds" name="txtSingleBeds" guid="8c6ad21e-3e75-4acb-9bfb-5c8156aa0f1d" runat="server" type="text" placeholder="" maxlength="1" class="form-control input-md" data-bind="value:replyNumber"></asp:TextBox>
	
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtDoubleBeds">Double</label>  
							  <div class="col-md-1">
							  <asp:TextBox id="txtDoubleBeds" name="txtDoubleBeds" guid="863c0f26-eba2-4113-8c1a-19fca3a60501" runat="server" type="text" placeholder="" maxlength="1" class="form-control input-md" data-bind="value:replyNumber"></asp:TextBox>
	
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtQueenBeds">Queen</label>  
							  <div class="col-md-1">
							  <asp:TextBox id="txtQueenBeds" name="txtQueenBeds" guid="98a515f2-8d08-456e-889e-d7c143973599" runat="server" type="text" placeholder="" maxlength="1" class="form-control input-md" data-bind="value:replyNumber"></asp:TextBox>
	
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtDressers">Clothes Dressers</label>  
							  <div class="col-md-1">
							  <asp:TextBox id="txtDressers" name="txtDressers" guid="6c0dc9b8-f674-46c8-910c-af05bc995e02" runat="server" type="text" placeholder="" maxlength="1" class="form-control input-md" data-bind="value:replyNumber"></asp:TextBox>
	
							  </div>
							</div>

							<!-- Text input-->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="txtBedFrames">Bed Frames</label>  
							  <div class="col-md-1">
							  <asp:TextBox id="txtBedFrames" name="txtBedFrames" guid="c3c54616-169a-4564-a6a5-53cf20f350a3" runat="server" type="text" placeholder="" maxlength="1" class="form-control input-md" data-bind="value:replyNumber"></asp:TextBox>
	
							  </div>
							</div>
								
								

							<!-- Multiple Checkboxes -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="chkBoxLivingRoom">Living/Dining Room Furnishings</label>
							  <div class="col-md-4">

								  
								  <asp:CheckBoxList CssClass="radio-inline" ID="cblFurniture" runat="server" RepeatLayout="Table" RepeatColumns="1" Font-Bold="false">
									  <asp:ListItem Text="Table and Chairs" Value="47d9b5ef-da2f-4770-85fd-c6304d9bc14f"></asp:ListItem>
									  <asp:ListItem Text="Couch" Value="e1c0a636-8f54-47c8-9804-7b549d049cb5"></asp:ListItem>
									  <asp:ListItem Text="Love Seat" Value="5a5b4407-4769-4b3d-ba6e-f1a8d8ac5683"></asp:ListItem>
									  <asp:ListItem Text="Chair" Value="0948621c-ac6d-4ee2-8d62-dae3a1355651"></asp:ListItem>
									  <asp:ListItem Text="TV" Value="b92e59b2-fbdc-486f-96d5-3f0a4e5b56be"></asp:ListItem>
									  <asp:ListItem Text="TV Stand" Value="759d4cfb-dbc3-4724-969f-9ab3cb5f6c63"></asp:ListItem>
									  <asp:ListItem Text="Coffee Table" Value="de47ad3c-520f-4b5b-bc7b-acbccd220012"></asp:ListItem>
									  <asp:ListItem Text="End Tables" Value="7334230e-8a69-442b-a7b3-43782109f57b"></asp:ListItem>
									  <asp:ListItem Text="Lamps" Value="0d02e4a4-20c7-415b-8466-fa3e10a0a825"></asp:ListItem>
									  <asp:ListItem Text="Rugs" Value="0cdd6401-f760-4a81-a707-f0c70a8b9dad"></asp:ListItem>
								  </asp:CheckBoxList>

							  
							  </div>
							</div>
								
							<div class="form-group">
							  <label class="col-md-5 control-label" for="txtSingleBeds">HOUSEHOLD ITEMS NEEDED</label> 
							</div>

							<!-- Multiple Radios (inline) -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="radioGroupWaterHeater">Water Heater</label>
							  <div class="col-md-4"> 
									<asp:RadioButtonList CssClass="radio-inline" ID="rblGroupWaterHeater" runat="server" RepeatDirection="Vertical" RepeatLayout="Table">
										<asp:ListItem Text="Gas" Value="dab4c613-3b76-4b5f-b12c-299d37e5c96d"></asp:ListItem>
										<asp:ListItem Text="Electric" Value="33ed86f4-26bb-4334-a44b-797543b2aaf7"></asp:ListItem>
										<asp:ListItem Text="Don't Need" Value="17a48277-a38d-4350-9dd9-081a8687a974"></asp:ListItem>
									</asp:RadioButtonList>
							  </div>
							</div>

							<!-- Multiple Radios (inline) -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="radioGroup">Dryer</label>
							  <div class="col-md-4"> 
									<asp:RadioButtonList CssClass="radio-inline" ID="rblDryer" runat="server" RepeatDirection="Vertical" RepeatLayout="Table">
										<asp:ListItem Text="Gas" Value="db8acf4f-0cca-40b7-9628-788df2d189fb"></asp:ListItem>
										<asp:ListItem Text="Electric" Value="6ab4e735-add2-4f3b-b06b-eb89ae17abe4"></asp:ListItem>
										<asp:ListItem Text="Don't Need" Value="17a48277-a38d-4350-9dd9-081a8687a974"></asp:ListItem>
									</asp:RadioButtonList>
							  </div>
							</div>

							<!-- Multiple Radios (inline) -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="radioGroupStove">Stove Top Oven</label>
							  <div class="col-md-4"> 
									<asp:RadioButtonList CssClass="radio-inline" ID="rblStoveTop" runat="server" RepeatDirection="Vertical" RepeatLayout="Table">
										<asp:ListItem Text="Gas" Value="553cf64d-8ef6-4017-a4d8-b598f1437715"></asp:ListItem>
										<asp:ListItem Text="Electric" Value="1db435e6-8439-4943-9686-f7af248096be"></asp:ListItem>
										<asp:ListItem Text="Don't Need" Value="17a48277-a38d-4350-9dd9-081a8687a974"></asp:ListItem>
									</asp:RadioButtonList>
							  </div>
							</div>

							<!-- Multiple Checkboxes -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="chkBoxAppliances">Appliances Needed</label>
							  <div class="col-md-4">

								  <asp:CheckBoxList CssClass="radio-inline" ID="cblAppliances" runat="server" RepeatLayout="Table" RepeatColumns="1" Font-Bold="false">
									  <asp:ListItem Text="Washing Machine" Value="9c0febfe-cdfa-492d-ad54-84c8455a58e8"></asp:ListItem>
									  <asp:ListItem Text="Refrigerator" Value="d23bfd73-956c-4865-b1f5-0ca8e85fd8d5"></asp:ListItem>
									  <asp:ListItem Text="Dish Washer" Value="5db11416-b659-4b52-afff-d9d0bd2734ef"></asp:ListItem>
									  <asp:ListItem Text="Coffee Pot" Value="c635a79f-58b0-4f55-8b74-4ec80bff3b5b"></asp:ListItem>
									  <asp:ListItem Text="Toaster" Value="60117723-1382-45c3-ad17-12b8d385bec7"></asp:ListItem>
									  <asp:ListItem Text="Microwave Oven" Value="df938585-ee73-4bbc-88ea-aad94aeb37c2"></asp:ListItem>
								  </asp:CheckBoxList>

							  </div>
							</div>

								

							<!-- Multiple Checkboxes -->
							<div class="form-group">
							  <label class="col-md-4 control-label" for="chkBoxUtensils">Pots, Pans, Dishes, Utensils, Silverware</label>
							  <div class="col-md-4">

								  
								  <asp:CheckBoxList CssClass="radio-inline" ID="cblUtensils" runat="server" RepeatLayout="Table" RepeatColumns="1" Font-Bold="false">
									  <asp:ListItem Text="Pots and Pans" Value="e12030e4-fc30-4fbd-acdd-64befd770688"></asp:ListItem>
									  <asp:ListItem Text="Plates and Bowls" Value="2209f603-6545-42fe-a4d3-ee260d419da4"></asp:ListItem>
									  <asp:ListItem Text="Silverware" Value="f21be773-a379-44e0-984b-89412933fdbf"></asp:ListItem>
									  <asp:ListItem Text="Cooking Utensils" Value="aba0b607-7ce9-40d7-8750-c86de7b56a3e"></asp:ListItem>
									  <asp:ListItem Text="Cups and Glasses" Value="d76d8714-41e4-4d45-a0db-547776b97320"></asp:ListItem>
									  <asp:ListItem Text="Plastic Food Storage" Value="77f20ace-19fb-4293-a9aa-1ac27a5604c7"></asp:ListItem>
								  </asp:CheckBoxList>

							  
							  </div>
							</div>
							
							<div class="form-group">
								<div class="col-md-4">
									<label class="control-label" for="chkBoxLivingRoom"><h2>IMPORTANT! Help Description</h2>Please describe in DETAIL other needs!</label>
								</div>
								<div class="col-md-8">
									<asp:TextBox ID="txtHelp" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
								</div>
							</div>
								


							<div class="form-group">
								<div class="col-md-12">
								<h2 style="margin:0px;">Write a Compelling Flood Story</h2>
								<ul>
								<li>Explain how much water flooded their home.</li>
								<li>Explain where they lived after the flood.</li>
								<li>Explain their feeling coming home after the flood.</li>
								<li>List items they lost that were important to them. </li>
								<li>List at least 5 of the items they need.</li>
								</ul>
								</div>
							</div>

								
							<div class="form-group">
								<div class="col-md-4">
									<label class="control-label" for="chkBoxLivingRoom">Mininum of 3 paragraphs REQUIRED.<br />This story will become the driving force behind the campaign. Please write a compelling story that will make people want to give to this person.</label>
								</div>
								<div class="col-md-8">
									<asp:TextBox ID="txtStory" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
								</div>
							</div>

							<!-- Button -->
							<div class="form-group">
							  <div class="col-md-8"></div>
							  <div class="col-md-4">
								<asp:Button	 id="btnSubmit" runat="server" name="btnSubmit" OnClick="btnSubmit_Click" Text="Submit Help Request" class="btn btn-primary btn-block btn-lg"></asp:Button>
							  </div>
							</div>

							</fieldset>
							</div>
						</div>
						<div class="col-sm-2"></div>
					</div>
				</div>

				<div id="divResults" runat="server" visible="false">
				<div class="container-fluid bg-3 text-center">  
					<div class="row">
						<div class="col-xs-1 col-sm-4"></div>
						<div class="col-xs-10 col-sm-4">
							<br />
							Please email your homes flood photos to<br /> <a href="mailto:Support@Stability.org">Support@Stability.org</a><br /> and reference this ticket number all emails.
							<br />
							<i>(Write down this request number.)</i>
							<h1>Help Request Number</h1><br />
							<span style="font-size:150px;"><asp:Literal ID="lblNumber" runat="server"></asp:Literal></span>
							<br />
							<div class="divResults">
								<asp:Label ID="lblResults" runat="server"></asp:Label>
							</div>
							<br />
							<br />
							<asp:Label ID="lblTimeName" runat="server"></asp:Label>
							<div style="margin-top:20px;">
								<button type="button" class="btn btn-success btn-lg btn-block btn-2" runat="server" id="btnHelpNavigation">My Profile Page</button>
							</div>
						</div>
						<div class="col-xs-1 col-sm-4"></div>
					</div>
					</div>
				</div>

				<div class="divRegisterFormOuter">
				</div>
</asp:Content>

