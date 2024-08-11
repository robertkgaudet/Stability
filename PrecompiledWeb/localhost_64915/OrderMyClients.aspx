<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Basic.master" autoeventwireup="true" inherits="OrderMyClients, App_Web_h4gntnip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript">

        function randomArrayShuffle(array)
        {
            var currentIndex = array.length, temporaryValue, randomIndex;
            while (0 !== currentIndex) {
                randomIndex = Math.floor(Math.random() * currentIndex);
                currentIndex -= 1;
                temporaryValue = array[currentIndex];
                array[currentIndex] = array[randomIndex];
                array[randomIndex] = temporaryValue;
            }
            return array;
        }

        $(".btnSubmit").click(function (event)
        {
            alert("test");
            separateString();
        });

        function separateString()
        {
            originalString = $("#textCustomerlist").val();
            separatedArray = originalString.split(',');

            $("#btnSubmit").val() = randomArrayShuffle(separatedArray);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<textarea id="textCustomerlist" name="textCustomerlist" rows="20" cols="60" style="margin-top:40px;"></textarea>
<div id="divClients"></div>
<button id="btnSubmit" type="button" name="btnSubmit" onclick="event.preventDefault();">Randomize my Clients</button>
    <script src="https://js.braintreegateway.com/web/3.48.0/js/client.min.js"></script>
    <script src="https://js.braintreegateway.com/web/3.48.0/js/hosted-fields.min.js"></script>
</asp:Content>