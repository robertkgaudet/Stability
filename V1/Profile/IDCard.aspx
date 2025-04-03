<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="IDCard.aspx.cs" Inherits="V1_Profile_IDCard" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>
<%@ Register Src="~/V1/UserControls/TeamLogo.ascx" TagPrefix="uc1" TagName="TeamLogo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .id-card-container {
            width: 3.375in;
            height: 2.125in;
            background: white;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            overflow: hidden;
            position: relative;
            margin: 0 auto;
            font-family: Arial, sans-serif;
        }
          .watermark-bg {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 110px;
            height: 110px;
            background-repeat: no-repeat;
            background-size: 70% 70%;
            background-position: center;
            opacity: 0.15;
            z-index: 1;
            border-radius: 50%;
            pointer-events: none;
            background-clip: content-box;
            padding: 15px;
            box-sizing: border-box;
            overflow: visible;
        }
        .id-card-header {
            background: #DD1447;
            color: white;
            padding: 6px 0;
            text-align: center;
            font-size: 12px;
            font-weight: bold;
            position: relative;
            z-index: 2;
        }

        /* Organization Banner */
        .id-card-org {
            background: #C0113A;
            color: white;
            padding: 4px 0;
            text-align: center;
            font-size: 11px;
            font-weight: bold;
            position: relative;
            z-index: 2;
        }

        /* Card Body */
        .id-card-body {
            display: flex;
            padding: 8px;
            height: calc(100% - 70px);
            position: relative;
            z-index: 2;
        }

        /* Photo Section */
        .id-card-photo {
            width: 30%;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
            z-index: 2;
        }
            .id-card-photo img {
                width: 100%;
                max-height: 100%;
                border: 1px solid #EEE;
                border-radius: 3px;
                background-color: white;
            }
        /* Info Section */
        .id-card-info {
            width: 70%;
            padding-left: 8px;
            font-size: 9px; /* Smaller base font size */
            position: relative;
            z-index: 2;
        }
        .id-card-name {
            font-weight: bold;
            font-size: 14px; /* Larger font size for name */
            margin-bottom: 4px;
        }
        .id-card-detail {
            margin-bottom: 2px;
            font-size: 10px; /* Smaller font size for details */
        }
        /* Footer Section */
        .id-card-footer {
            background: #DD1447;
            color: white;
            padding: 4px 0;
            text-align: center;
            font-size: 10px;
            font-weight: bold;
            position: absolute;
            bottom: 0;
            width: 100%;
            z-index: 2;
        }
        /* Certification Badge */
        .certification-badge {
            position: absolute;
            top: 5px;
            right: 5px;
            background: #28a745;
            color: white;
            padding: 2px 5px;
            border-radius: 3px;
            font-size: 8px;
            font-weight: bold;
            z-index: 3;
        }
        /* Print-specific styles */
        @media print {
            body {
                background: white !important;
            }
            .no-print {
                display: none !important;
            }
            .id-card-container {
                box-shadow: none;
                page-break-after: always;
            }
            .watermark-bg {
                opacity: 0.1; /* More subtle for printing */
            }
        }
        .icon{
            margin-right:2px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="id-card-container">
        <div id="watermarkBg" runat="server" class="watermark-bg"></div>
        <div class="id-card-header">
            <i class="fa fa-id-card-o"></i>
            <asp:Literal ID="litTitle" runat="server"></asp:Literal>
        </div>
        <div class="id-card-org">
           <asp:Literal ID="TeamName" runat="server"></asp:Literal>
        </div>
        <div class="id-card-body">
            <div class="id-card-photo">
                <asp:Image runat="server" ID="imgProfile" AlternateText="Volunteer Photo" />
            </div>
            <div class="id-card-info">
                <div class="id-card-name">
                    <uc1:TeamLogo ID="ucTeamLogo" runat="server" />
                </div>
                <div class="id-card-detail">
                     <%=locationDD %>
                </div>
                <div class="id-card-detail">
                      <asp:Literal ID="litNumber" runat="server"></asp:Literal>
                </div>
                <div class="id-card-detail">
                    Print Date:
                    <asp:Literal ID="litPrintDate" runat="server"></asp:Literal><br />
                    <asp:Label ID="lblStabilityVerifiedDate" runat="server" Text="Stability Verified Date:"
                        Visible="false"></asp:Label>
                    <asp:Literal ID="litStabilityVerifiedDate" runat="server"></asp:Literal><br />

                    <asp:Label ID="lblTeamVerifiedDate" runat="server" Text="Team Verified Date:" Visible="false"></asp:Label>
                    <asp:Literal ID="litTeamVerifiedDate" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
        <!-- Footer -->
        <div class="id-card-footer">
           <asp:Literal ID="TeamRole" runat="server"></asp:Literal>
        </div>
    </div>
</asp:Content>