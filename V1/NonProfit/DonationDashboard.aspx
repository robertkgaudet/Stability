<%@ Page Title="Donation  Details" Language="C#" EnableViewState="true" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="DonationDashboard.aspx.cs" Inherits="V1_NonProfit_DonationDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link href="../../Bigshop/bigshop-2.3/src/css/default/DashboardStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
    <div class="dashboard">
        <div class="headers">
            <h1>Dashboard</h1>
            <div class="total-donated">
                <p>Total Amount Donated</p>
                <h2>$<%= totalDonation %></h2>
            </div>
           
        </div>

        <div class="donation-details">
            <div class="card">
                <h2>$<%= todayCount %></h2>
                <p>Today</p>
            </div>
            <div class="card">
                <h2>$<%= thisWeekCount %></h2>
                <p>This Week</p>
            </div>
            <div class="card">
                <h2>$<%= thisMonthCount %></h2>
                <p>This Month</p>
            </div>
            <div class="card">
                <h2>$<%= thisYearCount %></h2>
                <p>This Year</p>
            </div>
        </div>

<div class="recent-donors-container" style="display: flex; gap: 20px; width: 100%;"> 
    <!-- Left Section -->
    <div style="flex: 1;">
        <h3>Recent Donors</h3>
        <div class="recent-donors" style="flex: 1;">    
             <asp:Label ID="RecentDonorsRepeaterNoData" runat="server"></asp:Label>
            <asp:Repeater ID="RecentDonorsRepeater" runat="server">
                <HeaderTemplate>
                    <table class="table table-bordered" style="width: 100%; table-layout: fixed;">
                        <thead>
                            <tr>
                                <th>Donor Name</th>
                                <th>Donation Date</th>
                                <th>Total Amount</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <div style="display: flex; align-items: center;">
                                <div style="width: 40px; height: 40px; border-radius: 50%; background-color: #4a4e69; color: #fff; display: flex; justify-content: center; align-items: center; margin-right: 10px; font-weight: bold;">
                                    <%# !string.IsNullOrEmpty(Eval("FirstName").ToString()) ? Eval("FirstName").ToString().Substring(0, 1) : "" %>
                                </div>
                                <%# Eval("FirstName") %> <%# Eval("LastName") %>
                            </div>
                        </td>
                        <td>
                            <div style="display: flex; align-items: center;">
                                <%# Eval("CreatedAt") %>
                            </div>
                        </td>
                        <td>
                            <div style="display: flex; align-items: center;">
                                $<%# Eval("Amount") %>
                            </div>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>

    <!-- Right Section -->
    <div style="flex: 1;">
        <h3> Deployment's Donations       <asp:Button ID="Donationlist" type="submit" class="save-btn" runat="server"  Text="Donation List" OnClick="btnDonationsList_Click" />
</h3>
        <div class="recent-donors" style="flex: 1;">
             <asp:Label ID="AdditionalInfoRepeaterNoData" runat="server"></asp:Label>
            <asp:Repeater ID="AdditionalInfoRepeater" runat="server">
                <HeaderTemplate>
                    <table class="table table-bordered" style="width: 100%; table-layout: fixed;">
                        <thead>
                            <tr>
                                <th>Deployment Name</th>
                                <th> Total Amount</th>
                                <th>Latest Donation Date</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>   
                        <td>
                             <div style="display: flex; align-items: center;">
                              <div style="width: 40px; height: 40px; border-radius: 50%; background-color: #4a4e69; color: #fff; display: flex; justify-content: center; align-items: center; margin-right: 10px; font-weight: bold;">
                                  <%# !string.IsNullOrEmpty(Eval("CampaignName").ToString()) ? Eval("CampaignName").ToString().Substring(0, 1) : "" %>
                                 
                            </div>
                                  <%# Eval("CampaignName") %>
                            </div>
                        </td>
                        <td>$<%#Eval("Amount") %></td>
                        <td><%# Eval("CreatedAt") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
   </div>
        </div>
   </asp:Content>         



