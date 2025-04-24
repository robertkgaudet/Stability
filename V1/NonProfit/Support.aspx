<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Support.aspx.cs" Inherits="V1_NonProfit_Support" %>

<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        body {
            font-family: Arial, sans-serif;
        }

        .container {
            width: 80%;
            margin: 0 auto;
        }

        h1 {
            color: #333;
        }

        .support-info {
            margin: 20px 0;
        }

        .contact-form {
            margin-top: 30px;
            padding: 20px;
            background-color: #f9f9f9;
            border-radius: 5px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
        }

            .contact-form input, .contact-form textarea {
                width: 100%;
                padding: 10px;
                margin-bottom: 15px;
                border: 1px solid #ccc;
                border-radius: 4px;
            }

            .contact-form button {
                padding: 10px 15px;
                background-color: #4CAF50;
                color: white;
                border: none;
                border-radius: 4px;
                cursor: pointer;
            }

                .contact-form button:hover {
                    background-color: #45a049;
                }

        .response-message {
            margin-top: 20px;
            color: green;
            font-weight: bold;
            display: none;
        }

        .error-message {
            margin-top: 20px;
            color: red;
            font-weight: bold;
            display: none;
        }
    </style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:TeamHeader runat="server" ID="ucTeamHeader" />
    <div class="panel-body">


        <div class="support-info">
            <p>If you need assistance, please contact us directly at:</p>
            <p>Email: <a href="mailto:help@stability.org">help@stability.org</a></p>
        </div>

        <div class="contact-form">
            <h3>Contact Us for Help</h3>
            <form id="contactForm">
                <label for="name">Your Name:</label>
                <input type="text" id="name" name="name" required>

                <label for="email">Your Email:</label>
                <input type="email" id="email" name="email" required>

                <label for="subject">Subject:</label>
                <input type="text" id="subject" name="subject" required />

                <label for="message">Your Message:</label>
                <textarea id="message" name="message" rows="5" required></textarea>

                <button type="button" id="btnContactForm">Send Message</button>
            </form>
            <div class="response-message">Your message has been sent successfully!</div>
            <div class="error-message">Oops, there was an error. Please try again.</div>
        </div>


    </div>
    <uc1:TeamFooter runat="server" ID="ucTeamFooter" />

<script>
    $(document).ready(function () {
        $('#btnContactForm').click(function () {
            debugger;
            let name = $("#name").val();
            let email = $("#email").val();
            let subject = $("#subject").val();
            let message = $("#message").val();
            if (name === null || name === "" || name === undefined
                || email === null || email === "" || email === undefined
                || subject === null || subject === "" || subject === undefined
                || message === null || message === "" || message === undefined) {
                alert("Please enter all required fields!");
                return;
            }

            $.ajax({
                type: "POST",
                url: "/V1/NonProfit/Support.aspx/SendEmail",
                data: JSON.stringify({ name: name, email: email, subject: subject, message: message }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === 'success') {
                        $(".response-message").show();
                        $(".error-message").hide();
                        $("#name").val('');
                        $("#email").val('');
                        $("#subject").val('');
                        $("#message").val('');
                    } else {
                        $(".response-message").hide();
                        $(".error-message").show();
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error: " + error);
                }
            });
        });
    });
</script>

</asp:Content>
