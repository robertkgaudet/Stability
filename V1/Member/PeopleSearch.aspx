<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="PeopleSearch.aspx.cs" Inherits="V1_Member_PeopleSearch" %>
<%@ Register Src="~/V1/UserControls/TeamLogo.ascx" TagPrefix="uc1" TagName="TeamLogo" %>
<%@ Register Src="~/V1/UserControls/MemberNavigation.ascx" TagPrefix="uc1" TagName="MemberNavigation" %>
<%@ Register Src="~/V1/UserControls/PeopleSearch.ascx" TagPrefix="uc1" TagName="PeopleSearch" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('.btnFriend').click(function () {
                RequestConnection('<%=_receiverUserId%>', '<%=_sendingUserId%>');
                return false;
            });
            function RequestConnection(receiverUserIdString, requestorUserIdString) {
                $.ajax({
                    url: '/V1/Member/Default.aspx/RequestConnection',
                    method: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify({ receiverUserIdString: receiverUserIdString, requestorUserIdString: requestorUserIdString }),
                    success: function (response) {

                        //Update friend button style.
                        $('.btnFriend').text("Connection Request Sent").removeClass("btnFriend").removeClass("btn-primary").addClass("btn-default").attr("id", "btnUpdated");

                    },
                    error: function (error) {
                        console.error('Error loading events:', error);
                    }
                });
            }
            $("body").tooltip({ selector: '[data-toggle=tooltip]' });
        });
    </script>

    <style>
        .hpanel {
            margin-bottom: 5px !important;
        }
        .user-name{
                color: #337ab7 !important;
                text-decoration: none;
               cursor: pointer;
        }
        .Userlink{
            display:inline !important;
        }
        .user-name:hover{
                   color: #23527c !important;
                  text-decoration: underline;
        }
        .member-panel-body {
            border-radius: 10px !important;
            margin-bottom: 0px !important;
        }
        /* Custom gutter class for rows */
        .row.no-gutter {
            margin-left: 0;
            margin-right: 0;
        }

            .row.no-gutter [class*="col-"] {
                padding-left: 1px; /* Reduced gutter padding */
                padding-right: 1px;
            }

        .friendsGrid {
            display: grid;
            grid-template-columns: repeat(2, 1fr); /* Two columns layout */
            gap: 10px;
            width: 100%; /* Ensures the DataList stretches to 100% of the container */
            padding: 0;
            list-style: none;
        }

        .friendItem {
            padding: 10px;
            border: 1px solid #ccc;
            text-align: center;
        }

        /* Responsive styling for smaller screens */
        @media screen and (max-width: 768px) {
            .friendsGrid {
                grid-template-columns: 1fr;
            }
        }

        p {
            margin: 0px !important;
        }

        .fa-pending-color {
            color: gainsboro;
        }

        .fa-approved-color {
            color: #63CB31;
        }

        .fa:hover {
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container" style="padding-bottom: 100px !important;">
        <div class="row justify-content-center" style="margin-top: 40px;">
            <div class="col-sm-8 col-lg-9">
                <div class="hpanel">
                    <div class="panel-body member-panel-body">
                        <div class="row">
                            <div class="col-lg-4">
                                <div class="hpanel">
                                    <div class="panel-body" style="border: none;">
                                        <h4>Search For Connections
                                        </h4>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="hpanel">
                                    <div class="panel-body" style="border: none;">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtSearchBox" runat="server" placeholder="Search People..." CssClass="form-control"></asp:TextBox>
                                            <div class="input-group-btn">
                                                <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" CssClass="btn btn-primary">
													<i class="fa fa-search"></i>
												</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <asp:Repeater ID="ConnectionsDataList" OnItemDataBound="ConnectionsDataList_ItemDataBound" runat="server">
                            <ItemTemplate>
                                <div class="col-xs-12">
                                    <div class="hpanel">
                                        <div class="panel-body member-panel-body">
                                            <div class="clearfix">
                                                <img class="img-circle img-small pull-left m-r-md" src="<%=profilePhotoFolder%><%# string.IsNullOrEmpty(Eval("ProfileImage") as string) ? "icons8-customer-64.png" : Eval("ProfileImage")%>" />
                                                <div class="media-body">
                                                    <p style="font-size: 16px;">
                                                        <strong>
															<uc1:TeamLogo runat="server" ID="ucTeamLogo" UserId='<%# Eval("UserId") %>' PageName="people" />
															<asp:Literal ID="litPassedVetting" runat="server"></asp:Literal>
                                                        </strong>
                                                        <div class="text-muted"><%# Eval("CityState") %></div>
                                                        <div class="text-muted"><%# Eval("TeamName") %>  <%# Eval("ProfileTitle") %></div>
                                                        
                                                        <button data-id="<%#Eval("UserId")%>" data-action="remove" onclick="UpdateConnection(this); return false;" class="btn btn-default pull-right add-button" <%=_hideConnectionButton%>>Add Connection</button>
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
                    </div>
                </div>
            </div>


            <!--NAVIGATION-->
            <div class="col-sm-4 col-lg-3">
                <uc1:MemberNavigation runat="server" ID="ucMemberNavigation" />
            </div>
        </div>
    </div>

</asp:Content>