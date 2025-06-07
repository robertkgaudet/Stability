<%@ Page Title="" Language="C#" MasterPageFile="~/Impactoid/MasterPages/Impactoid_Version_2_MasterPage.master" AutoEventWireup="true" CodeFile="CommunityPage.aspx.cs" Inherits="Impactoid_CommunityPage" %>
<%@ MasterType VirtualPath="~/Impactoid/MasterPages/Impactoid_Version_2_MasterPage.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>

<asp:Content ID="contentGalleryGrid" ContentPlaceHolderID="cphGalleryGrid" Runat="Server">
    <li class="active" data-filter="*"><a href="#">All</a></li>
    <asp:Repeater ID="rptGalleryGrid" runat="server" OnItemDataBound="rptGalleryGrid_ItemDataBound">
        <ItemTemplate>
                <asp:Literal id="litGalleryGridCategory" runat="server"></asp:Literal>
                <asp:Image runat="server" ID="imgGalleryImage" />
                <div class="image-overly">
                    <asp:HyperLink CssClass="pretty" runat="server" ID="hypGalleryModal"></asp:HyperLink>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

</asp:Content>
<asp:Content ID="contentGallery" ContentPlaceHolderID="cphGallery" Runat="Server">
    <li class="active" data-filter="*"><a href="#">All</a></li>
    <asp:Repeater ID="rptGalleryMenu" runat="server" OnItemDataBound="rptGalleryMenu_ItemDataBound">
        <ItemTemplate>
            <li data-filter=".cat1" runat="server" id="liControl">
            <asp:Literal ID="litIdControl" runat="server"></asp:Literal>
                <asp:HyperLink ID="hypGalleryMenu" runat="server" NavigateUrl="#"></asp:HyperLink>
            </li>
        </ItemTemplate>
    </asp:Repeater>

</asp:Content>


<asp:Content ContentPlaceHolderID="cphDisasters" ID="contentDisasters" runat="server">
        <div class="testimonial-area" id="testimonial" runat="server">
            <div class="container">
                <div class="row">
                   <div class="col-md-12">
                       <div class="sectino-intro">
                            <h2 class="section-heading">Our Community Relief Portals</h2>
                        </div>
                   </div>
                    <div class="col-md-12">
                        <div class="testimonial-wrap owl-carousel owl-theme">
                        <asp:Repeater ID="rptDisasters" runat="server" OnItemDataBound="rptDisasters_ItemDataBound">
                            <ItemTemplate>
                                <div class="single-quote">
                                    <div class="quote-thumb">
                                        <img src="aid/images/disastercolor.png" alt="">
                                    </div>
                                    <h1 style="color:white; font-weight:800;">
                                        <asp:Literal ID="litDisasterName" runat="server"></asp:Literal>
                                    </h1>
                                    <p>
                                        <asp:Literal ID="litDisasterDescription" runat="server"></asp:Literal>
                                    </p>
                                    <span>
                                        <asp:Literal ID="litDisasterDate" runat="server"></asp:Literal>
                                    </span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        </div> <!-- Testimonial Wrap End -->
                    </div>
                </div>
            </div>
        </div>
</asp:Content>

<asp:Content ID="contentPrograms" runat="server" ContentPlaceHolderID="cphPrograms">
        <asp:Repeater ID="rptPrograms" runat="server" OnItemDataBound="rptPrograms_ItemDataBound">
            <ItemTemplate>
                <div class="col-md-4 col-sm-6">
                    <div class="service-single">
                        <span class="service-icon"><i class="fa fa-connectdevelop"></i></span>
                        <div class="service-details">
                            <h4><asp:Literal ID="litProgramName" runat="server"></asp:Literal></h4>
                            <p><asp:Literal ID="litProgramDescription" runat="server"></asp:Literal></p>
                            <hr />
                            <table>
                                <tr>
                                    <td><small><asp:Literal ID="litRemote" runat="server"></asp:Literal></small>
                                    </td>
                                    <td>|</td>
                                    <td>
                                        <small><asp:Literal ID="litTraining" runat="server"></asp:Literal></small>
                                    </td>
                                    <td>|</td>
                                    <td>
                                        <small><asp:Literal ID="litDeployment" runat="server"></asp:Literal></small>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
</asp:Content>

<asp:Content ID="contentCauses" runat="server" ContentPlaceHolderID="cphCauses">
        <asp:Repeater ID="rptCauses" runat="server" OnItemDataBound="rptCauses_ItemDataBound">
            <ItemTemplate>
                <div class="col-md-3 col-sm-6">
                    <div class="cause-single">
                         <asp:Image ID="imgSlider" runat="server" />
                        <div class="cause-details">
                            <h4><asp:Literal ID="litCauseName" runat="server"></asp:Literal></h4>
                            <p><asp:Literal ID="litCauseDescription" runat="server"></asp:Literal></p>
                            <asp:HyperLink ID="hypDonate" runat="server" CssClass="btn-filled" Text="Donate"></asp:HyperLink>
                            <asp:HyperLink ID="hypVolunteer" runat="server" CssClass="btn-filled" Text="Volunteer"></asp:HyperLink>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
</asp:Content>

<asp:Content ID="contentTicker" ContentPlaceHolderID="cphTicker" Runat="Server">
    <ul>
        <asp:Repeater ID="rptTicker" runat="server" OnItemDataBound="rptTicker_ItemDataBound">
            <ItemTemplate>
                <li>
                    <asp:HyperLink ID="hypOrganizationName" runat="server"></asp:HyperLink>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</asp:Content>

<asp:Content ID="contentTopContent" ContentPlaceHolderID="cphTopContent" runat="server">
    <div class="top-info">
        <div class="single-info hidden-sm">
            <p><i class="fa fa-envelope-o"></i><a href="mailto:<%=publicEmailAddress%>"><%=publicEmailAddress%></a></p>
        </div>
        <div class="single-info hidden-sm">
            <p> <i class="fa fa-phone"></i><a href="tel:<%=publicPhoneNumber%>"><%=publicPhoneNumber%></a></p>
        </div>
        <div class="single-info">
            <ul>
                <li><a href="<%=facebookURL%>"><i class="fa fa-facebook"></i></a></li>
                <li><a href="<%=facebookGroupURL%>"><i class="fa fa-facebook"></i></a></li>
                <li><a href="<%=instagramURL%>"><i class="fa fa-instagram"></i></a></li>
                <li><a href="<%=twitterURL%>"><i class="fa fa-twitter"></i></a></li>
                <li><a href="<%=youtubeURL%>"><i class="fa fa-youtube"></i></a></li>
            </ul>
        </div>
    </div>
</asp:Content>

<asp:Content ID="contentSlider" ContentPlaceHolderID="cphSlider" runat="server">
        <div id="slider-area" class="owl-carousel owl-theme home-3">
            <div class="single-slide home3-overly">
                <asp:Image ID="imgCover" runat="server" />
                <div class="slide-txt">
                    <h2><%=pageTitle%></h2>
                    <p><%=pageDescription%></p>
                    <a href="#" class="btn-filled btndonate">learn more</a>
                </div>
            </div>
            <div class="single-slide home3-overly">
                <img src="/V1/Images/CausePhotos/slide11.png" alt="">
                <div class="slide-txt">
                    <h2>Every Minute Matters</h2>
                    <h3 style="color:white;">Volunteer time can double the help communities receive!<br /><br />So we capture every donated minute and morsel.</h3>
                    <a href="#" class="btn-filled btndonate">learn more</a>
                </div>
            </div>
            <asp:Repeater ID="rptCauseSlider" runat="server" OnItemDataBound="rptCauseSlider_ItemDataBound">
                <ItemTemplate>
                    <div class="single-slide home3-overly">
                        <asp:Image ID="imgSlider" runat="server" />
                        <div class="slide-txt">
                            <h2><asp:Literal ID="litCauseName" runat="server"></asp:Literal> </h2>
                            <p>
                                <asp:Literal ID="litCauseDescription" runat="server"></asp:Literal>
                            </p>
                            <asp:HyperLink ID="hypDonate" runat="server" CssClass="btn-filled" Text="Donate"></asp:HyperLink>
                            <asp:HyperLink ID="hypVolunteer" runat="server" CssClass="btn-filled" Text="Volunteer"></asp:HyperLink>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
</asp:Content>
<asp:Content ID="contentTeam" runat="server" ContentPlaceHolderID="cphTeam">
            <asp:Repeater ID="rptTeam" runat="server" OnItemDataBound="rptTeam_ItemDataBound">
                <ItemTemplate>
                    <!-- Team Member -->
                    <div class="col-md-3 col-sm-6">
                        <div class="team-member">
                            <div class="member-thumb">
                                <asp:Image ID="imgTeamMember" runat="server" />
                            </div>
                            <div class="member-detail">
                                <h4 class="member-name"><asp:Literal ID="litName" runat="server"></asp:Literal></h4>
                                <p><asp:Literal ID="litTitle" runat="server"></asp:Literal></p>
                               <%-- <ul class="member-social">
                                    <li><a href="#"><i class="fa fa-facebook"></i></a></li>
                                    <li><a href="#"><i class="fa fa-twitter"></i></a></li>
                                    <li><a href="#"><i class="fa fa-google-plus"></i></a></li>
                                    <li><a href="#"><i class="fa fa-linkedin"></i></a></li>
                                </ul>--%>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphDescription" ID="contentDescription">
       <asp:Literal ID="litDescription" runat="server"></asp:Literal>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphHistory" ID="contentHistory">
       <asp:Literal ID="litHistory" runat="server"></asp:Literal>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphAboutUs" ID="contentAboutUs">
       <asp:Literal ID="litAboutUs" runat="server"></asp:Literal>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphProgramOverview" ID="contentProgramOverview">
       <asp:Literal ID="litProgramOverview" runat="server"></asp:Literal>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphOrgName" ID="contentOrgName">
       <asp:Literal ID="litOrgName" runat="server"></asp:Literal>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMission" ID="contentMission">
       <asp:Literal ID="litMission" runat="server"></asp:Literal>
</asp:Content>