<%@ page language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_CountyInfo, App_Web_mjkl5wor" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
							County Emergency Management Information
						</h2>
					</div>
				</div>
			</div>
		</div>
	</div>
    <div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
	    <div class="row">
		    <div class="col-lg-12">
			    <div class="hpanel form-horizontal">
				    <div class="panel-heading hbuilt">
                        <div class="pull-right">
                            <asp:HyperLink ID="hypEditCounty" Visible="false" CssClass="btn btn-xs btn-success m-r-sm" runat="server" Text="Edit"></asp:HyperLink>
                        </div> 
					    <asp:Label ID="lblStateInformation" runat="server"></asp:Label>
                        <div id="divInfoMessage" runat="server" visible="true">Detailed county information has not yet been entered.</div>
				    </div>
				    <div class="panel-body" id="divInfo" runat="server" visible="false">
					    <div class="form-group">
						    <label class="col-sm-2 control-label">Emergency Operations Center Name</label>
						    <div class="col-sm-5">
                                <asp:Literal ID="litEOCName" runat="server"></asp:Literal>
						    </div>
					    </div>
                        
					    <div class="form-group">
						    <label class="col-sm-2 control-label">Emergency Managers Name</label>
						    <div class="col-sm-5">
                                <asp:Literal ID="litEMName" runat="server"></asp:Literal>
						    </div>
					    </div>
                        
					    <div class="form-group">
						    <label class="col-sm-2 control-label">Primary Phone Number</label>
						    <div class="col-sm-5">
                                <asp:Literal ID="litPhoneNumber" runat="server"></asp:Literal>
						    </div>
					    </div>
                        
					    <div class="form-group">
						    <label class="col-sm-2 control-label">Emergency Management Website</label>
						    <div class="col-sm-5">
                                <asp:Literal ID="litWebsite" runat="server"></asp:Literal>
						    </div>
					    </div>
				    </div>
			    </div>
		    </div>
	    </div>
    </div>
</asp:Content>