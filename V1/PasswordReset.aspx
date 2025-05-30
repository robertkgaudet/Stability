<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Narrow.master" AutoEventWireup="true" CodeFile="PasswordReset.aspx.cs" Inherits="V1_PasswordReset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-xs-2 col-sm-3 col-lg-4"></div>
        <div class="container">
            <div class="row justify-content-center">

                <div class="col-xs-8 col-sm-6 col-lg-4" style="min-width: 320px !important; max-width: 320px !important;">
                    <div class="middle-box text-center loginscreen animated fadeInDown">
                        <h1 class="loginLogo" style="margin-left: 25px;">
                            <img class="img-responsive" src="/V1/Images/Logo-Horizontal-cs.png" />
                        </h1>
                        <div class="m-t-sm text-center">
                            <h4>Disaster-Ready Communities
                            </h4>
                        </div>
                        <p>
                            Be the lifeline your community needs.
                        </p>
                    </div>
                    <div class="m-t justify-content-center" role="form">
                        <div class="form-group">
                            <asp:PasswordRecovery
                                ID="prPasswordRecovery"
                                runat="server"
                                HeaderText=""
                                InstructionText=""
                                InstructionTextStyle-CssClass="m-l"
                                TextBoxStyle-CssClass="form-control"
                                TitleTextStyle-CssClass="font-bold"
                                LabelStyle-CssClass="font-bold"
                                BackColor="White"
                                SubmitButtonText="Recover Password"
                                UserNameLabelText="Username: "
                                OnSendingMail="PasswordRecovery2_SendingMail"
                                SuccessText="A temporary password has been emailed to you.">
                                <SubmitButtonStyle CssClass="btn btn-success btn-block full-width m-b"></SubmitButtonStyle>
                            </asp:PasswordRecovery>
                        </div>
                        <a class="btn btn-block btn-info m-t-lg" href="/SignIn">Back To Sign In</a>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-xs-2 col-sm-3 col-lg-4"></div>
</asp:Content>

