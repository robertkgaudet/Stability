<%@ Page Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Donation.aspx.cs" Inherits="V1_NonProfit_Donation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Oswald:wght@200..700&display=swap" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css2?family=Oswald:wght@200..700&family=Public+Sans:ital,wght@0,100..900;1,100..900&display=swap" rel="stylesheet">
    <link href="https://pagecdn.io/lib/easyfonts/fonts.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="summary">
            <%=DefaultCampaign.Summary%>
        </div>
    </div>

    <div class="container">
        <% if (ShowDonationButton)
            {
                if (DefaultCampaign == null || DefaultCampaign.DonationCampaignId == Guid.Empty)
                { %>
        <a class="donate-button" style="pointer-events: none;">
            <span class="heart-icon">&#10084;</span> DONATE NOW
        </a>
        <% }
            else
            { %>
        <a href="DonationDetails.aspx?organizationId=<%=orgId%>&donationCampaignId=<%=DefaultCampaign.DonationCampaignId%>" class="donate-button">
            <span class="heart-icon">&#10084;</span> DONATE NOW
        </a>
        <% }
            } %>
    </div>

    <div class="address-with-divider">
        <h3 class="addressHeader">SEND A CHECK TO THIS ADDRESS</h3>
        <div class="divider"></div>
    </div>

    <div class="container">
        <%=DefaultCampaign.Address%>
    </div>

    <div class="module_row themify_builder_row tb_pflf545 tf_w" style="margin-top: 140px;">

        <input runat="server" type="hidden" id="transactionDetailId" name="TransactionDetailId" value="" />
        <div class="row_inner col_align_top gutter-none tb_col_count_3 tf_box tf_rel"
            style="display: flex; justify-content: center; align-items: center; gap: 30px;">
            <input type="hidden" id="organizationId" runat="server" value="" />
            <!-- PayPal Column -->
            <div class="module_column tb-column col3-1 tb_iux6546 donate_payment_item first"
                style="flex: 1; max-width: 300px; text-align: center; border: 2px solid #ccc; border-radius: 10px; padding: 20px; transition: transform 0.3s ease-in-out;">
                <div class="module module-image tb_aa3388 image-center donate_new_img tf_mw">
                    <div class="image-wrap tf_rel tf_mw">
                        <a href="<%=paypalLink%>">
                            <!-- Reduced Image Size -->
                            <img fetchpriority="high" decoding="async" width="150" height="100" src="https://groundforce.ngo/wp-content/uploads/2023/06/paypal-1.png"
                                class="wp-post-image wp-image-5497" title="paypal" alt="paypal">
                        </a>
                    </div>
                </div>
                <div class="module module-buttons tb_mq1g936 buttons-horizontal solid" style="margin-top: 10px;">
                    <a href="<%=paypalLink%>"
                        class="ui builder_button tf_in_flx transparent"
                        style="display: inline-block; padding: 10px 20px; background-color: #0070ba; color: white; border-radius: 5px; text-decoration: none; font-weight: bold;">Donate with PayPal
                    </a>
                </div>
            </div>

            <!-- Venmo Column -->
            <div class="module_column tb-column col3-1 tb_eqq2546 donate_payment_item"
                style="flex: 1; max-width: 300px; text-align: center; border: 2px solid #ccc; border-radius: 10px; padding: 20px; transition: transform 0.3s ease-in-out;">
                <div class="module module-image tb_pkwp17 image-center donate_new_img tf_mw">
                    <div class="image-wrap tf_rel tf_mw">
                        <a href="<%=venmoLink%>">
                            <!-- Reduced Image Size -->
                            <img decoding="async" width="150" height="100"
                                src="https://groundforce.ngo/wp-content/uploads/2023/06/venmo-1.png"
                                class="wp-post-image wp-image-5498" title="venmo" alt="venmo">
                        </a>
                    </div>
                </div>
                <div class="module module-buttons tb_4q4g88 buttons-horizontal solid" style="margin-top: 10px;">
                    <a href="<%=venmoLink%>"
                        class="ui builder_button tf_in_flx transparent"
                        style="display: inline-block; padding: 10px 20px; background-color: #3d95ce; color: white; border-radius: 5px; text-decoration: none; font-weight: bold;">Donate with Venmo
                    </a>
                </div>
            </div>

            <!-- Cash App Column -->
            <div class="module_column tb-column col3-1 tb_47tr546 donate_payment_item last"
                style="flex: 1; max-width: 300px; text-align: center; border: 2px solid #ccc; border-radius: 10px; padding: 20px; transition: transform 0.3s ease-in-out;">
                <div class="module module-image tb_viie799 image-center donate_new_img tf_mw">
                    <div class="image-wrap tf_rel tf_mw">
                        <a href="<%=cashAppLink%>">
                            <!-- Reduced Image Size -->
                            <img decoding="async" width="150" height="100" src="https://groundforce.ngo/wp-content/uploads/2023/06/cashapp-1.png"
                                class="wp-post-image wp-image-5496" title="cashapp" alt="cashapp">
                        </a>
                    </div>
                </div>

                <div class="module module-buttons tb_4ln1180 buttons-horizontal solid" style="margin-top: 10px;">
                    <a href="<%=cashAppLink%>"
                        class="ui builder_button tf_in_flx transparent"
                        style="display: inline-block; padding: 10px 20px; background-color: #28c101; color: white; border-radius: 5px; text-decoration: none; font-weight: bold;">Donate with Cash App
                    </a>
                </div>
            </div>
        </div>
    </div>
    <div class="container" style="display: inherit;">
        <h2>Donate To Current Deployment</h2>
        <% 
            if (DonationCampaigns != null)
            {
                foreach (var campaign in DonationCampaigns.Where(x => !x.IsDefault))
                {
                    if (campaign.DonationCampaignId == null)
                    {
                        Response.Redirect("/V1/NonProfit/Donation.aspx");
                    }
				%>
				<h3>
					<a href="<%= ShowDonationButton ? "DonationDetails.aspx?organizationId=" + orgId + "&donationCampaignId=" + campaign.DonationCampaignId : "javascript:void(0);" %>">
						<%= campaign.CampaignName %>
					</a>
				</h3>
				<% 
                }
            }
            else
            {
                //Response.Redirect("/V1/NonProfit/Default.aspx");
            }
        %>
    </div>

    <!-- Add the following CSS in your head or external stylesheet -->
    <style>
        .donate_payment_item:hover {
            transform: scale(1.1);
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.2);
        }

        .donate_payment_item .image-wrap:hover {
            border-color: #0070ba; /* Border color on hover */
        }

        .donate_payment_item .module-buttons a:hover {
            background-color: #005a8b; /* Change background color on hover */
        }

        .container {
            display: flex;
            justify-content: center;
            align-items: center;
            margin-bottom: 40px;
        }

        .donate-button {
            background-color: #ee2312;
            color: #fff;
            font-family: 'Arial', sans-serif;
            font-weight: bold;
            font-size: 16px;
            padding: 18px 50px 18px 50px;
            border: none;
            border-radius: 10px;
            display: flex;
            align-items: center;
            gap: 10px;
            cursor: pointer;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.2);
            transition: transform 0.2s ease, background-color 0.3s ease;
            text-decoration: none;
            margin-top: 40px;
        }

            .donate-button:hover {
                background-color: #000000; /* Darker purple on hover */
                transform: translateY(-2px); /* Slight upward movement on hover */
                color: white;
                text-decoration: none;
            }

        .heart-icon {
            font-size: 25px; /* Adjust the size of the heart */
            color: #fff !important;
        }

        .summary {
            padding-top: 25px;
        }

        .addressHeader {
            font-family: "Oswald";
            font-weight: 600;
            font-size: 26px;
            line-height: 39px;
            text-transform: uppercase;
            font-style: normal;
        }

        .address-with-divider {
            text-align: center; /* Centers the text and divider */
        }

            .address-with-divider h3 {
                margin-bottom: 10px; /* Adds space between text and divider */
            }

            .address-with-divider .divider {
                width: 80px; /* Width of the divider */
                height: 2px; /* Thickness of the divider */
                background-color: #000; /* Color of the divider */
                margin: 0 auto; /* Centers the divider */
                margin-bottom: 10px;
            }

        .link-style {
            color: blue;
            text-decoration: underline;
            cursor: pointer;
        }

            .link-style:hover {
                color: darkblue;
            }
    </style>
</asp:Content>








