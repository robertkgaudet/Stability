<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ImpactedCommunities.ascx.cs" Inherits="V1_UserControls_ImpactedCommunities" %>
					
			        <style>
			            .LinkDescription { color:#365899; font-weight:normal !important;}
                        .EventLink  { color:#365899; font-weight:bold !important; text-decoration:underline;}

			        </style>
                    <div class="col-lg-12">
                        <div class="hpanel hviolet panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                        <asp:Repeater ID="rptStates" runat="server" OnItemDataBound="rptStates_ItemDataBound">
			                <ItemTemplate>
                                <div class="panel-heading hbuilt m-b-xs">
                                <h4>
                                    <asp:HyperLink ID="hypState" runat="server" aria-expanded="true" data-toggle="collapse" data-parent="#accordion">
                                        <asp:Literal ID="litState" runat="server"></asp:Literal>
                                    </asp:HyperLink>
                                </h4>
                                    </div>
                                    <!--panel body-->
                                    <asp:Literal ID="litDivControl" runat="server"></asp:Literal>
                                        <div class="panel-body ">
                                            <div class="btn-group pull-right">
                                                <asp:HyperLink ID="hypEditState" Visible="false" CssClass="btn btn-xs btn-success m-r-sm" runat="server" Text="Edit"></asp:HyperLink>
                                                <asp:HyperLink ID="hypEditCounties" Visible="false" CssClass="btn btn-xs btn-success m-r-sm" runat="server" Text="Edit Counties"></asp:HyperLink>
                                                <span class="label label-success">MULTI-AGENCY ACTIVE</span>
                                            </div> 
                                            <div class="row">
                                                <div class="col-sm-7">
                                                    <h4>IMPACTED COUNTIES</h4>
                                                    <div class="project-people">
                                                        <asp:Repeater ID="rptCounties" runat="server" OnItemDataBound="rptCounties_ItemDataBound">
                                                            <ItemTemplate>
                                                                    <asp:HyperLink ID="hypCounty" runat="server" Target="_blank"></asp:HyperLink>, 
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                    <p>
                                                        <br /><span class="text-info font-bold m-b-xs">Emergency Management Office Name:</span> <asp:Literal ID="lblEOCName" runat="server"></asp:Literal>
                                                        <br /><span class="text-info font-bold m-b-xs">State Emergency Manager:</span> <asp:Literal ID="lblEMName" runat="server"></asp:Literal>
                                                        <br /><span class="text-info font-bold m-b-xs">Phone Number:</span> <asp:HyperLink ID="hypPhoneNumber" runat="server"></asp:HyperLink>
                                                        <br /><span class="text-info font-bold m-b-xs">Website:</span> <asp:Literal ID="lblWebsite" runat="server"></asp:Literal>
                                                    </p>
                                                </div>
                                                <div class="col-sm-5" runat="server" id="divFEMAOffset" visible="false">
                                                        <div class="contact-stat"><span>FEMA Offset $$ Recouped</span>
                                                        <h2 class="text-success">
                                                            $1,206,400.00
                                                        </h2></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="panel-footer contact-footer">
                                            <div class="row" runat="server" id="divPayWall" visible="false">
                                                <div class="col-md-3 border-right">
                                                    <div class="contact-stat"><span>Volunteer Hours: </span> <strong>29,922</strong></div>
                                                </div>
                                                <div class="col-md-3 border-right">
                                                    <div class="contact-stat"><span>Recouped Volunteer Funds: </span> <strong class="text-success">$1,206,400.00</strong></div>
                                                </div>
                                                <div class="col-md-3 border-right">
                                                    <div class="contact-stat"><span>In-Kind Donations (lbs): </span> <strong>30,000</strong></div>
                                                </div>
                                                <div class="col-md-3">
                                                    <div class="contact-stat"><span>Recouped In-Kind Donations ($$): </span> <strong class="text-success">$400,000</strong></div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <div class="contact-stat"><span>Active Non-Profits: </span> <strong> <asp:Literal ID="litActiveNonprofitCount" runat="server"></asp:Literal> </strong></div>
                                                </div> 
                                                <div class="col-md-3 border-right">
                                                    <div class="contact-stat"><span>Active Shelters: </span> <strong><asp:Literal ID="litActiveShelterCount" runat="server"></asp:Literal></strong></div>
                                                </div>
                                                <div class="col-md-3 border-right">
                                                    <div class="contact-stat"><span>Active Feeding Locations: </span> <strong><asp:Literal ID="litActiveFoodSites" runat="server"></asp:Literal></strong></div>
                                                </div>
                                                <div class="col-md-3 border-right">
                                                    <div class="contact-stat"><span>Active Distribution Locations: </span> <strong><asp:Literal ID="litActiveDistributionLocations" runat="server"></asp:Literal></strong></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div> 
			                </ItemTemplate>
		                </asp:Repeater>
                        </div>
                    </div>