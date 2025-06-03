<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Child.master" AutoEventWireup="true" CodeFile="NonProfitCampaign.aspx.cs" Inherits="V1_NonProfit_NonProfitCampaign" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/1-Column-Child.master" %>
<%@ Register Src="~/V1/UserControls/TimeBoard.ascx" TagPrefix="uc1" TagName="TimeBoard" %>
<%@ Register Src="~/V1/UserControls/PositionNavigation.ascx" TagPrefix="uc1" TagName="PostionNavigation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/summernote@0.8.18/dist/summernote-bs4.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/summernote@0.8.18/dist/summernote-bs4.min.js"></script>
    <link href="../../Homer/vendor/sweetalert/lib/sweet-alert.css" rel="stylesheet" />
    <script src="../../Homer/vendor/sweetalert/lib/sweet-alert.min.js"></script>
    <style>


            .modal-header-line {
    display: flex;
    align-items: center;
    margin-bottom: 20px;
    flex-wrap: wrap; 
    gap: 15px;
}

.modal-title {
    font-size: 1.8rem;
    font-weight: 600;
    margin: 0;
}

.radio-option {
    font-size: 1.5rem;
    display: ruby-text;
    align-items: center;
}

.radio-option input[type="radio"] {
    margin-right: 5px;
    width: 18px; /* Default is ~13-15px */
    height: 18px;
    cursor: pointer;
    transform: translateY(3px);

}
          .btn {
    margin: 0 5px;          
    float: right;           
}
          #txtsms{
                 min-height: min-content
          }
       

    </style>
    <script type="text/javascript">

        $(document).ready(function () {

            $('.donateButton').click(function () {
                window.location.href = '<%=donateLink%>';
                return false;
            });

            $('.volunteerButton').click(function () {
                window.location.href = '<%=volunteerLink%>';
                return false;
            });

            $('.getHelpButton').click(function () {
                window.location.href = '<%=getHelpLink%>';
                return false;
            });

        });
        function toggleMessageType() {
            const selected = document.querySelector('input[name="sendOption"]:checked').value;
            const isEmail = selected === 'Email';
            document.getElementById('smsContainer').style.display = isEmail ? 'none' : 'block';
            document.getElementById('emailContainer').style.display = isEmail ? 'block' : 'none';
        }
        document.addEventListener('DOMContentLoaded', toggleMessageType);
        function openInviteModal() {
            document.getElementById('inviteModal').style.display = 'block';
            document.getElementById('modalOverlay').style.display = 'block';
        }
        function closeInviteModal() {
            document.getElementById('<%= txtEmail.ClientID %>').value = ''; 
            document.getElementById('<%= txtsms.ClientID %>').value = '';
            $('#<%= txtEmail.ClientID %>').summernote('code', '');
            $("input[name='sendOption'][value='SMS']").prop('checked', true);
            document.getElementById('inviteModal').style.display = 'none';
            document.getElementById('modalOverlay').style.display = 'none';
       const validators = document.querySelectorAll('.text-danger');
            validators.forEach(v => v.style.display = 'none');
            toggleMessageType();
   }

        function validateBeforeSend() {
            var selectedOption = document.querySelector('input[name="sendOption"]:checked').value;
            ValidatorEnable(document.getElementById('<%= rfvEmail.ClientID %>'), false);
            ValidatorEnable(document.getElementById('<%= rfvSMS.ClientID %>'), false);
            if (selectedOption === "Email") {
                ValidatorEnable(document.getElementById('<%= rfvEmail.ClientID %>'), true);
        } else {
            ValidatorEnable(document.getElementById('<%= rfvSMS.ClientID %>'), true);
            }
            return Page_ClientValidate();
        }

        function toggleMessageType() {
            const selected = document.querySelector('input[name="sendOption"]:checked').value;
            document.getElementById('smsContainer').style.display = (selected === 'SMS') ? 'block' : 'none';
            document.getElementById('emailContainer').style.display = (selected === 'Email') ? 'block' : 'none';
        }
        $(document).ready(function () {
            $('#<%= txtEmail.ClientID %>').summernote({
                toolbar: [
                    ['style', ['bold', 'italic']],
                    ['alignment', ['ul', 'ol', 'paragraph']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['height', ['height']],
                    ['insert', ['picture', 'link', 'table']],
                ],
                height: 100
            });
        });
        function sendInvitationAjax() {
            if (!validateBeforeSend()) return false;

            var eventId = new URLSearchParams(window.location.search).get("organizationEventId");
            var messageType = $("input[name='sendOption']:checked").val(); 
            var messageBody = messageType === "Email" ? $("#<%= txtEmail.ClientID %>").val() : $("#<%= txtsms.ClientID %>").val();
    $.ajax({
        type: "POST",
        url: messageType === "SMS"
            ? "/V1/NonProfit/NonProfitCampaign.aspx/SendSms"
            : "/V1/NonProfit/NonProfitCampaign.aspx/SendEmail",  
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            eventId: eventId,
            smsMessage: messageBody
        }),
        success: function (response) {
            document.getElementById('<%= txtEmail.ClientID %>').value = '';  
            document.getElementById('<%= txtsms.ClientID %>').value = ''; 
            $('#<%= txtEmail.ClientID %>').summernote('code', '');
            $("input[name='sendOption'][value='SMS']").prop('checked', true);
            document.getElementById('inviteModal').style.display = 'none';
            document.getElementById('modalOverlay').style.display = 'none';
            toggleMessageType();
            if (parseInt(response.d) > 0) {
                swal({
                    title: "Success",
                    text: "Invitation sent successfully for " + response.d + " members.",
                    icon: "success"
                });
            }
            else {
                swal({
                    title: "Error",
                    text: "No member found to receive invite.",
                    icon: "error"
                });
            }
        },
        error: function (xhr, status, error) {
            swal({
                title: "Error",
                text: "An error occurred while sending the invitation.",
                icon: "error"
            });
        }
    });

            return false; 
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:PostionNavigation runat="server" ID="ucPostionNavigation" />
    <div class="hpanel">
        <div class="panel-body">
            <div class="row">
                <div class="col-lg-8">
                    <div>
                        <b>Mission</b>
                        <p>
                            <asp:Literal ID="litCampaignMission" runat="server"></asp:Literal>
                        </p>
                        <div class="m-t-lg">
                            If you would like to help you can
                            <asp:HyperLink ID="lbVolunteer" runat="server" CssClass="volunteerButton" Text="view open volunteer positions" Visible="false"></asp:HyperLink>, <a href="/V1/Administration/TeamName.aspx?userActionModal=false">create your own team</a> or
                            <asp:HyperLink ID="linkDeploymentMap" runat="server" CssClass="mapButton" Text="view the deployment map"></asp:HyperLink>
                            showing areas that need assistance.
							
							<asp:Button ID="btnActiveVolunteer" runat="server" CssClass="btn btn-light" Visible="false" />
                            <asp:LinkButton ID="lbGetHelp" runat="server" CssClass="btn btn-warning btn-dark getHelpButton" Text="Get Help" Visible="false"></asp:LinkButton>
                            <asp:Button Visible="false" CssClass="btn w-xs btn-light" ID="hypCauseIsNotActive" runat="server"></asp:Button>
                            <br />
                            <br />
                            <div class="col-md-3 m-t-lg">
                                <asp:LinkButton ID="lbDonate" runat="server" CssClass="btn btn-success btn-block donateButton" Text="Donate Here" Visible="false"></asp:LinkButton>
                            </div>
                            <div class="col-md-3 m-t-lg">
                                <asp:HyperLink ID="hypViewCases" runat="server" CssClass="btn btn-info btn-block" Text="View Cases" Visible="false"></asp:HyperLink>
                            </div>
                            <div class="col-md-3 m-t-lg">
                                <asp:HyperLink ID="hypAddCase" runat="server" CssClass="btn btn-info btn-block" Text="Add A Case" Visible="false"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="hpanel stats">
                        <div class="panel-body h-200 list">
                            <div>
                                <div class="font-bold no-margins">
                                    Deployment Activity
                                </div>
                                <small>Time/monetary value of this deployment.
                                </small>
                            </div>
                            <div class="row m-t-sm bg-info">
                                <div class="col-xs-2"></div>
                                <div class="col-xs-2"><small class="stats-label">Vols</small></div>
                                <div class="col-xs-2"><small class="stats-label">Hours</small></div>
                                <div class="col-xs-6"><small class="stats-label">Total</small></div>
                            </div>
                            <div class="row m-t-sm bg-success">
                                <div class="col-xs-2">
                                    <small class="stats-label">Today</small>
                                </div>
                                <div class="col-xs-2">
                                    <span class="no-margins"><%=_todayVolunteerCount%></span>
                                </div>
                                <div class="col-xs-2">
                                    <span class="no-margins"><%=_todayVolunteerHours%></span>
                                </div>
                                <div class="col-xs-6">
                                    <span class="no-margins"><%=_todayVolunteerValue%>*</span>
                                </div>
                            </div>
                            <div class="row m-t-sm bg-success">
                                <div class="col-xs-2">
                                    <small class="stats-label">Total</small>
                                </div>
                                <div class="col-xs-2">
                                    <span class="no-margins"><%=_totalVolunteerCount%></span>
                                </div>
                                <div class="col-xs-2">
                                    <span class="no-margins"><%=_totalVolunteerHours%></span>
                                </div>
                                <div class="col-xs-6">
                                    <span class="no-margins"><%=_totalVolunteerValue%>*</span>
                                </div>
                            </div>
                            <div class="stats-icon pull-right m-t-sm">
                                <span class="label label-success pull-left" style="color: #ffffff; background-color: #62CB31;">LIVE DATA</span>
                            </div>
                            <div class="stats-title pull-left">
                                <h4>Volunteer Rate <span style="color: orangered;"><%=_volunteerHourlyRate%>/HR</span></h4>
                            </div>
                        </div>
                        <div class="panel-footer">
                            <small>*This number is calcuted based on volunteer hours. It does not reflect a cash donation.
                                <br />
                                Latest Update: <%=DateTime.Now.ToShortDateString() %> <%=DateTime.Now.ToShortTimeString() %></small>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer">
            <div class="alert alert-success" runat="server" id="divAlertMessage" visible="false">
                <i class="fa fa-bolt"></i>
                <asp:Literal ID="litAlertMessage" runat="server"></asp:Literal>
            </div>
        </div>
    </div>


    <div class="content">
        <div class="row">
            <div class="col-xs-12">
                <div class="hpanel hgreen">
                    <div class="panel-heading hbuilt">
                        Social Media Links
                    </div>
                    <div class="panel-body">
                        <dl class="dl-horizontal">
                            <dt runat="server" id="dtWebsite" visible="false">Website
                            </dt>
                            <dd runat="server" id="ddWebsite" visible="false" class="m-b-sm">
                                <asp:HyperLink ID="hypWebsite" runat="server" Target="new"></asp:HyperLink>
                            </dd>
                            <dt>Facebook Page
                            </dt>
                            <dd class="m-b-sm">
                                <asp:HyperLink ID="hypFacebookPage" runat="server" Target="new"></asp:HyperLink>
                            </dd>
                            <dt>Facebook Group
                            </dt>
                            <dd class="m-b-sm">
                                <asp:HyperLink ID="hypFacebookGroup" runat="server" Target="new"></asp:HyperLink>
                            </dd>
                            <dt>Blog
                            </dt>
                            <dd class="m-b-sm">
                                <asp:HyperLink ID="hypBlog" runat="server" Target="new"></asp:HyperLink>
                            </dd>
                            <dt>YouTube Channel
                            </dt>
                            <dd class="m-b-sm">
                                <asp:HyperLink ID="hypYouTube" runat="server" Target="new"></asp:HyperLink>
                            </dd>
                            <dt>Twitter
                            </dt>
                            <dd class="m-b-sm">
                                <asp:HyperLink ID="hypTwitter" runat="server" Target="new"></asp:HyperLink>
                            </dd>
                        </dl>
                    </div>
                    <div class="panel-footer"></div>
                </div>
                <div class="hpanel hgreen">
                    <div class="panel-heading hbuilt">
                        This Campaign Created By:
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-sm-6">
                                <dl class="dl-vertical">
                                    <dt>Point of Contact
                                    </dt>
                                    <dd class="m-b-sm">
                                        <asp:Label ID="lblPointOfContactPerson" runat="server"></asp:Label>
                                    </dd>

                                    <dt id="dtZelloChannel" runat="server" visible="false">Zello Channel
                                    </dt>
                                    <dd id="ddZelloChannel" runat="server" class="m-b-sm" visible="false">
                                        <asp:Label ID="lblZelloChannel" runat="server"></asp:Label>
                                    </dd>

                                    <dt id="dtPhone" runat="server">Phone Number
                                    </dt>
                                    <dd id="ddPhone" runat="server" class="m-b-sm">
                                        <asp:HyperLink ID="hypPointOfContactPhone" runat="server"></asp:HyperLink>
                                    </dd>

                                    <dt id="dtEmail" runat="server">Email
                                    </dt>
                                    <dd id="ddEmail" runat="server" class="m-b-sm">
                                        <asp:HyperLink ID="hypPointOfContactEmail" runat="server"></asp:HyperLink>
                                    </dd>

                                    <dt id="dtPrimaryPhone" runat="server">Phone
                                    </dt>
                                    <dd id="ddPrimaryPhone" runat="server" class="m-b-sm">
                                        <asp:HyperLink ID="hypPrimaryPhone" runat="server" Target="new"></asp:HyperLink>
                                    </dd>

                                </dl>
                            </div>
                            <div class="col-sm-6">
                                <dl class="dl-verticle">
                                    <dd>
                                        <asp:Label ID="lblParentOrgName" runat="server"></asp:Label>
                                    </dd>
                                    <dd id="ddParentAddress" runat="server" class="m-b-sm">
                                        <asp:HyperLink ID="hypParentAddress" runat="server" Target="new"></asp:HyperLink>
                                    </dd>
                                    <dd id="ddParentPhone" runat="server" visible="false">
                                        <asp:HyperLink ID="hypParentPhone" runat="server" Target="new"></asp:HyperLink>
                                    </dd>
                                    <dd id="ddParentEmail" runat="server" visible="false">
                                        <asp:HyperLink ID="hypParentEmail" runat="server" Target="new"></asp:HyperLink>
                                    </dd>
                                    <dd id="ddParentWebsite" runat="server" visible="false">
                                        <asp:HyperLink ID="hypParentWebsite" runat="server" Target="new"></asp:HyperLink>
                                    </dd>
                                </dl>
                                <dl class="dl-horizontal">
                                    <dt>VOAD Member
                                    </dt>
                                    <dd>
                                        <asp:Label ID="lblVoadMember" runat="server"></asp:Label>
                                    </dd>
                                    <dt>501c3
                                    </dt>
                                    <dd class="m-b-sm">
                                        <asp:Label ID="lbl501c3" runat="server"></asp:Label>
                                    </dd>
                                </dl>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer"></div>
                </div>
            </div>
            <div class="col-xs-12 l-lg-6">
                <div class="hpanel hgreen">
                    <div class="panel-heading hbuilt">
                        Volunteers for this Cause (Active within the past 30 days.)
                    </div>
                    <div class="panel-body">
                        <asp:Repeater ID="rpNonProfitPeople" runat="server" OnItemDataBound="rpNonProfitPeople_ItemDataBound">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="lblInfo" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <!-- Invite Link -->
    <!-- Modal Overlay -->
    <div id="modalOverlay" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.5); z-index: 999;" onclick="closeInviteModal()"></div>

    <!-- Modal Window -->
    <div id="inviteModal" style="display: none; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%); background: #fff; padding: 20px; border-radius: 8px; width: 50%; box-shadow: 0 2px 10px rgba(0,0,0,0.3); z-index: 1000; position: fixed;">

        <div class="modal-header-line">
            <h3 class="modal-title">Send Invitation</h3>
            <label class="radio-option">
                <input type="radio" name="sendOption" value="SMS" checked onchange="toggleMessageType()">
                SMS
            </label>
            <label class="radio-option">
                <input type="radio" name="sendOption" value="Email" onchange="toggleMessageType()">
                Email
            </label>
        </div>


        <div id="smsContainer">
            <textarea id="txtsms" runat="server" clientidmode="Static" class="form-control"
                placeholder="Enter SMS Text (Max 450 characters)" rows="6" cols="95"></textarea>

            <asp:RequiredFieldValidator
                ID="rfvSMS"
                runat="server"
                ControlToValidate="txtsms"
                ErrorMessage="SMS text is required."
                CssClass="text-danger"
                Display="Dynamic"
                EnableClientScript="true"
                Enabled="false">
            </asp:RequiredFieldValidator>
        </div>
        <div id="emailContainer" style="display: none;">
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" ClientIDMode="Static" placeholder="Enter Email Text"></asp:TextBox>
            <asp:RequiredFieldValidator
                ID="rfvEmail"
                runat="server"
                ControlToValidate="txtEmail"
                ErrorMessage="Text is required."
                CssClass="text-danger"
                Display="Dynamic">
            </asp:RequiredFieldValidator>
        </div>



        <div class="text-end btn">
            <button class="btn btn-secondary" onclick="event.preventDefault(); closeInviteModal();">Cancel</button>
            <asp:Button ID="btnSendInvitation" runat="server" CssClass="btn btn-primary me-2" Text="Send" OnClientClick="return sendInvitationAjax();" />
        </div>
    </div>
</asp:Content>



