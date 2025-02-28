<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="People.aspx.cs" Inherits="V1_NonProfit_People" %>

<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
    <link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
    <link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
    <link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
    <link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
    <script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>
    <script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>
    <script src="/Homer/vendor/jquery-validation/jquery.validate.min.js"></script>
  
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/css/bootstrap-multiselect.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/js/bootstrap-multiselect.min.js"></script>

    <style>
        .website {
            background-color: #5E2E91;
            padding: 17px;
            color: white;
        }

            .website:hover {
                background-color: #902F91;
                color: white;
                cursor: pointer;
            }

        .modal-dialog {
            margin-top: 100px;
        }
        .model-search{
            margin-top: 10px;
                margin-bottom: 11px;
                    margin-left: 8px;
        }
        .text-line{
                 margin-left:-10px;  
        }

    .multiselect-container input[type="checkbox"] {
        display: inline-block !important;
        margin-right: 5px;
    }
</style>

  


    <script>
        // Step 1: Select all the buttons in the table

        var recipientsName;
        var recipientsEmail;
        var message;
        var teamName;
        var divMessageSuccess;
        var divMessageTextBox;
        var divMessageError;
        var divErrorMessage
        var btnSend;
        var messageModelTitle;
        var txtMessage;

        $(document).ready(function () {


            $(function () {
               
                $('#StartDate').datepicker({
                    format: 'dd/mm/yyyy',
                    autoclose: true,
                });
                $('#EndDate').datepicker({
                    format: 'dd/mm/yyyy',
                    autoclose: true,
                });
            });

            // Initialize Example 1
            $('#tblVolunteers').footable();


            messageModelTitle = document.getElementById('messageModelTitle');
            txtMessage = document.getElementById('<%=txtMessage.ClientID%>');
            divMessageTextBox = document.getElementById('messageTextBox');
            btnSend = document.getElementById('btnSend');
            divMessageError = document.getElementById('divMessageError');
            divErrorMessage = document.getElementById('divErrorMessage');

            divMessageSuccess = document.getElementById('divMessageSuccess');
            divMessageSuccess.style.visibility = 'visible';
            divMessageSuccess.hidden = true;
            divMessageTextBox.hidden = false;
            btnSend.hidden = false;

            divMessageError.style.visibility = 'hidden';
            divErrorMessage.style.visibility = 'hidden';
            divMessageError.hidden = true;
            divErrorMessage.hidden = true;

            $("#nonProfit.dropdown-menu li").click(function () {
                window.location.href = "/V1/NonProfit/Default.aspx?organizationId=" + $(this).attr('id');
            });
        });

        function sendClick(object) {
            message = txtMessage.value;
            teamName = '<%=teamName%>';
            signedInUserFullName = '<%=signedInUserFullName%>';
            organizationId = '<%=organizationId%>';
            sendMessage(signedInUserFullName, organizationId, recipientsName, recipientsEmail, teamName, message);
        }

        function btnClick(object) {
            txtMessage.value = "";
            divMessageSuccess.style.visibility = 'hidden';
            divMessageTextBox.style.visibility = 'visible';
            btnSend.style.visibility = 'visible';
            divMessageSuccess.hidden = true;
            divMessageTextBox.hidden = false;
            btnSend.hidden = false;

            divMessageError.style.visibility = 'hidden';
            divErrorMessage.style.visibility = 'hidden';
            divMessageError.hidden = true;
            divErrorMessage.hidden = true;

            recipientsName = object.getAttribute('data-name');
            recipientsEmail = object.getAttribute('data-email');
            messageModelTitle.innerHTML = "This will send " + recipientsName + " an email from the Stability platform.";
            return false;
        }

        function sendMessage(signedInUserFullName, organizationId, recipientsName, recipientsEmail, teamName, message) {
            $.ajax
                (
                    {
                        type: "POST",
                        url: "/V1/Handlers/MessageMember.ashx",
                        data: JSON.stringify({ senderName: signedInUserFullName, organizationId: organizationId, recipientsName: recipientsName, recipientsEmail: recipientsEmail, teamName: teamName, message: message }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            //alert(divMessageSuccess);
                            //alert(divTextBody);
                            divMessageSuccess.style.visibility = 'visible';
                            divMessageTextBox.style.visibility = 'hidden';
                            btnSend.style.visibility = 'hidden';
                            divMessageSuccess.hidden = false;
                            divMessageTextBox.hidden = true;
                            btnSend.hidden = true;
                        },
                        error: function (xhr, status, error) {
                            alert(xhr.responseText);
                            /*var err = eval("(" + xhr.responseText + ")");*/
                            divMessageError.style.visibility = 'visible';
                            divErrorMessage.style.visibility = 'visible';
                            divMessageError.hidden = false;
                            divErrorMessage.hidden = false;
                            divErrorMessage.innerHTML += xhr.responseText;
                        }
                    }
                );
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <uc1:TeamHeader runat="server" ID="ucTeamHeader" />

    <div class="panel-heading">
        <asp:HyperLink ID="hypInviteTeamMembers" runat="server" Visible="false" Text="Invite Team Members" CssClass="btn btn-sm btn-info"></asp:HyperLink>
        <asp:HyperLink ID="hypPrintableTeamList" runat="server" Visible="false" Target="_blank" Text="Printable List" CssClass="btn btn-sm btn-info"></asp:HyperLink>
    </div>

    <div class="panel-body">
        <div id="divUpdateMessage" runat="server" class="alert alert-warning text-center" style="margin-bottom: 20px;" visible="false">
            <asp:Literal ID="litMessage" runat="server"></asp:Literal>
        </div>
        <div id="divFilterMessage" runat="server" class="alert alert-info text-center" style="margin-bottom: 20px;" visible="false">
            <asp:Literal ID="litFilterMessage" runat="server"></asp:Literal>
        </div>
        <div class="" data-child="hpanel" data-effect="fadeInDown" runat="server" id="hpanelMembers" visible="false">
            <div class="hpanel" runat="server" id="hpanelJoin" visible="true">
                <a href="/V1/Profile/EditNonProfits.aspx">Join This Team</a>
            </div>
           <div class="container mt-4">
    <div class="row">
        <div class="col-md-4  mb-3">
            <b>Search To Filter Your Team :</b>
            <input type="text" class="form-control input-sm" id="filter" placeholder="Search in table">
        </div>
        <div class="col-md-4  mb-3">
            <b>Location :</b>
             <input type="text" class="form-control input-sm" id="txtlocation" placeholder="Enter Location">
        </div>
    </div>

    <div class="row">
        <div class="col-md-4 mb-3">
            <label class="form-label model-search"></label>
              <b class="text-line">Select Skills :</b>
            <asp:ListBox ID="ddlSkills" runat="server" CssClass="form-control multiselect" SelectionMode="Multiple" AppendDataBoundItems="true"></asp:ListBox>
        </div>

        <div class="col-md-4 mb-3">
            <label class="form-label model-search"></label>
               <b class="text-line">Select Resources :</b>
            <asp:ListBox ID="ddlResources" runat="server" CssClass="form-control multiselect" SelectionMode="Multiple" AppendDataBoundItems="true"></asp:ListBox>
        </div>
    </div>

    <div class="row">
        <div class="col-md-4 mb-3">
            <label  class="form-label model-search"></label>
               <b class="text-line">Available From :</b>
            <asp:TextBox ID="StartDate" runat="server" CssClass="form-control datepicker" placeholder="Start Date" autocomplete="off" ClientIDMode="Static" />
        </div>
        <div class="col-md-4 mb-3">
            <label  class="form-label model-search"></label>
              <b class="text-line">Available To :</b>
            <asp:TextBox ID="EndDate" runat="server" CssClass="form-control datepicker" placeholder="End Date" autocomplete="off" ClientIDMode="Static" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="row">
                <div class="col-sm-2">
                    <div class="form-check">
                         <label class="form-check-label model-search">
                        <asp:CheckBox ID="txtIsVetted" runat="server" CssClass="form-check-input" />
                            Is Vetted
                        </label>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="form-check">
                         <label class="form-check-label model-search">
                        <asp:CheckBox ID="txtOptedSMS" runat="server" CssClass="form-check-input" />                     
                            Opted SMS
                        </label>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="form-check">
                         <label class="form-check-label model-search">
                        <asp:CheckBox ID="txtEmailconnect" runat="server" CssClass="form-check-input" />                       
                            Email Connected
                        </label>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="form-check">
                        <label class="form-check-label model-search">
                        <asp:CheckBox ID="txtIsVerified" runat="server" CssClass="form-check-input" />
                         Is Verified
                        </label>
                    </div>                
                </div>
            </div>
        </div>
    </div>
    <div class="row mt-4">
        <div class="col-md-8 text-right">
            <asp:Button ID="SearchButton" runat="server" CssClass="btn btn-info btn-sm mr-2 model-search" Text="Search" OnClick="SearchButton_Click" />
            <asp:Button ID="ClearButton" runat="server" CssClass="btn btn-danger btn-sm model-search" Text="Clear" OnClick="ClearButton_Click" />
        </div>
    </div>
</div>

           
            <table id="tblVolunteers" class="footable" data-page-size="20" data-filter="#filter">
                <tbody>
                    <asp:Repeater ID="rptVolunteers" runat="server" OnItemDataBound="rptVolunteers_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td style="background-color: white;">
                                    <div class="hpanel">
                                        <div class="panel-body">
                                            <h5 class="m-b-xs">
                                                <asp:HyperLink ID="hypName" runat="server"></asp:HyperLink>
                                            </h5>
                                            <p>
                                                <asp:Literal ID="litMemberInfo" runat="server"></asp:Literal>
                                                <asp:Literal ID="litDescription" runat="server"></asp:Literal>
                                            </p>
                                            <div class="pull-right">
                                                <asp:Button ID="btnContact" runat="server" Text="Message" Visible="false" CssClass="btn btn-success messageButton" data-toggle="modal" data-target="#messageMemberModal"></asp:Button>
                                            </div>
                                            <asp:Literal ID="litSkills" runat="server"></asp:Literal>
                                            <asp:Literal ID="litResources" runat="server"></asp:Literal>
                                        </div>
                                        <div class="panel-footer" id="divFooter" runat="server" visible="false">
                                            <div class="text-muted small">
                                                <asp:Literal ID="litVettingInfo" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td>
                            <br />
                            <ul class="pagination pull-right"></ul>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
    <div class="modal fade" id="messageMemberModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="color-line"></div>
                <div class="modal-header text-center">
                    <h5 class="modal-title">Send a Message</h5>
                    <div id="divMessageSuccess" class="alert alert-success text-uppercase">
                        <i class="fa fa-envelope"></i>Your message has been sent.
                    </div>
                    <div id="divMessageError" class="alert alert-warning text-uppercase">
                        <i class="fa fa-envelope"></i>
                        <div id="divErrorMessage"></div>
                    </div>
                </div>
                <div class="modal-body" id="messageTextBox">
                    <p>
                        <div id="messageModelTitle"></div>
                        <asp:TextBox TextMode="MultiLine" Width="100%" Rows="5" runat="server" ID="txtMessage"></asp:TextBox>
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" id="btnSend" onclick="return sendClick(this);">Send</button>
                </div>
            </div>
        </div>
    </div>
     <script>
         $(document).ready(function () {
             $('.multiselect').multiselect({
                 includeSelectAllOption: true,
                 enableFiltering: true,
                 enableCaseInsensitiveFiltering: true,
                 buttonWidth: '100%',
                 maxHeight: 200,
                 nonSelectedText: 'Select Options',
                 allSelectedText: 'All Selected',
                 numberDisplayed: 2
             });
         });
     </script>
    <uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>
