<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Narrow.master" AutoEventWireup="true" CodeFile="GetUserName.aspx.cs" Inherits="V1_PasswordReset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-xs-2 col-sm-3 col-lg-4"></div>
        <div class="col-xs-8 col-sm-6 col-lg-4" style="min-width:320px !important;max-width:320px !important;">
            <div class="middle-box text-center loginscreen animated fadeInDown">
                <h1 class="loginLogo">
                    <img class="img-responsive" src="/V1/Images/Logo-Horizontal-cs.png" />
                </h1>
                <div class="m-t-sm text-center">
                    <h4>Disaster-Ready Communities</h4>
                </div>
                <p>Be the lifeline your community needs.</p>
            </div>

            <p style="font-weight:bold;margin-left: 60px;">Get your UserName?</p>
            <div class="m-t justify-content-center">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
                <div class="form-group">
                    <asp:Label ID="lblEmail" runat="server" Text="Email:" CssClass="font-bold" />
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                </div>
                <asp:Button ID="btnRecover" runat="server" Text="Recover UserName" CssClass="btn btn-success btn-block full-width m-b" OnClick="btnRecover_Click" />
                <a class="btn btn-block btn-info m-t-lg" href="/SignIn">Back To Sign In</a>
            </div>
        </div>
        <div class="col-xs-2 col-sm-3 col-lg-4"></div>
    </div>
</asp:Content>
