<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DonationSuccess.aspx.cs" Inherits="V1_NonProfit_DonationSuccess" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Donation Successful</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f3f4f6;
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            color: #333;
        }

        .container {
            text-align: center;
            background: white;
            padding: 30px 40px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            max-width: 400px;
            width: 90%;
        }

            .container h1 {
                color: #4CAF50;
                font-size: 2em;
                margin-bottom: 10px;
            }

            .container p {
                font-size: 1.1em;
                margin: 10px 0 20px;
            }

            .container .btn {
                display: inline-block;
                padding: 10px 20px;
                background-color: #4CAF50;
                color: white;
                text-decoration: none;
                border-radius: 5px;
                font-size: 1em;
                transition: background-color 0.3s;
            }

                .container .btn:hover {
                    background-color: #45a049;
                }

        .icon {
            font-size: 3em;
            color: #4CAF50;
            margin-bottom: 15px;
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="icon">✔️</div>
        <h1>Thank You!</h1>
        <p>Your donation was successful. We greatly appreciate your support!</p>
        <p><strong> <%=TransactionId%> </strong></p>
        <a href="/" class="btn">Return to Homepage</a>
        <% if (ShowRegisterButton == false)
            { %>
        <a href="/V1/Register.aspx?role=Donor&transactionId=<%=TransactionId%>" id="btnRegister" class="btn">Register</a>
        <% } %>
    </div>
</body>
</html>
