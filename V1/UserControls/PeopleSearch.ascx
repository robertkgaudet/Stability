<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PeopleSearch.ascx.cs" Inherits="V1_UserControls_PeopleSearch" %>
<asp:Repeater ID="rptPeopleSearch" runat="server">
    <ItemTemplate>
        <div class="col-xs-12">
            <div class="hpanel">
                <div class="panel-body member-panel-body">
                    <div class="clearfix">
                        <img class="img-circle img-small pull-left m-r-md"
                             src='<%# string.IsNullOrEmpty(Eval("ProfileImage") as string) ? "/images/profilepicture.png" : "/UserData/Profile/" + Eval("ProfileImage") %>' />
                        <div class="media-body">
                            <p style="font-size: 16px;">
                                <strong><%# Eval("FullName") ?? "No Name" %></strong>
                                <div class="text-muted"><%# Eval("CityState") %></div>
                                <div class="text-muted"><%# Eval("TeamName") %> <%# Eval("ProfileTitle") %></div>
                            </p>
                        </div>
                    </div>
                    <div class="social-content m-t-sm">
                        <div class="text-muted"><%# Eval("ProfileDescription") %></div>
                    </div>
                </div>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>