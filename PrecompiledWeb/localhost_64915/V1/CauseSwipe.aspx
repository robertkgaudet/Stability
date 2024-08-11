<%@ page language="C#" masterpagefile="~/V1/MasterPages/Swipe.master" autoeventwireup="true" inherits="V1_CauseSwipe, App_Web_mjkl5wor" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Swipe.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://code.jquery.com/mobile/1.4.4/jquery.mobile-1.4.4.min.css">
    <script src="https://code.jquery.com/jquery-1.11.1.min.js"></script>
    <script src="https://code.jquery.com/mobile/1.4.4/jquery.mobile-1.4.4.min.js"></script>
    <script>
        $(document).ready(function () {

            $(".cause").on("swiperight", function (event) {

                $(this).addClass('rotate-left').delay(900).fadeOut(1);
                $(this).append('<div class="like">FOLLOW</div>');
                
                if ($(this).is(':first-child')) {
                    $("#container").append('<div class="like">THAT WAS THE LAST CAUSE</div>');
                } else {
                    $(this).next().removeClass('rotate-left rotate-right').fadeIn(400);
                    $(this).next().remove();
                }
            });

            $(".cause").on("swipeleft", function () {
                $(this).addClass('rotate-right').delay(900).fadeOut(1);
                $(this).append('<div class="dislike">NOT NOW</div>');

                if ($(this).is(':first-child')) {
                    $("#container").append('<div class="like">THAT WAS THE LAST CAUSE</div>');
                } else {
                    $(this).next().removeClass('rotate-left rotate-right').fadeIn(400);
                    $(this).next().remove();
                }
            });
        });
    </script>

    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&display=swap" rel="stylesheet">

    <style>
        body {
            background-color:#662A85;
            font-family: 'Poppins', sans-serif;
	        text-shadow: none;
        }

    .rotate-left {
	    transform: rotate(30deg) scale(0.8);
	    transition: 1s;
	    margin-left: 400px;
	    cursor: e-resize;
	    /*opacity: 0;*/
	    z-index: 10;
    }

    .rotate-right {
	    transform: rotate(-30deg) scale(0.8);
	    transition: 1s;
	    /*opacity: 0;*/
	    margin-left: -400px;
	    cursor: w-resize;
	    z-index: 10;
    }

    .like {
	    border-radius: 5px;
	    padding: 5px 10px;
	    border: 5px solid #000000;
	    color: #000000;
	    text-transform: uppercase;
	    font-size: 30px;
	    position: absolute;
	    top: 10px;
	    right:10px;
	    text-shadow: none;
    }

    .dislike {
	    border-radius: 5px;
	    padding: 5px 10px;
	    border: 5px solid white;
	    color: white;
	    text-transform: uppercase;
	    font-size: 30px;
	    position: absolute;
	    top: 10px;
	    left: 10px;
	    text-shadow: none;
    }

    #container {
        aspect-ratio: 9/16;
        width:80%;
        max-width:400px;
        height: 80vh;
        margin: auto !important;
        position: relative;
        display: block;
        list-style-type: none;
        -webkit-touch-callout: none;
        -webkit-user-select: none;
        -khtml-user-select: none;
        -moz-user-select: none;
        -ms-user-select: none;
        user-select: none;
    }
    
    .cause {
	    top: 20px;
        width:100%;
        height:100%;
        font-family: 'Poppins', sans-serif;
	    display: none;
	    background: #66b24f;
	    border-radius: 10px;
	    box-shadow: 0 0 10px rgba(0,0,0,0.2);
	    color: #66b24f;
	    position: absolute;
	    cursor: pointer;
        background: linear-gradient(45deg, #1c5c09, #74c15d, #66b24f);
    }
   /* .cause {
	    top: 20px;
        width:100%;
        height:100%;
        font-family: 'Poppins', sans-serif;
	    display: none;
	    background: #fff;
	    border-radius: 10px;
	    box-shadow: 0 0 10px rgba(0,0,0,0.2);
	    color: #fff;
	    position: absolute;
	    cursor: pointer;
        background: linear-gradient(45deg, #FFFFFF, #FFFFFF, #74c15d);
    }*/

    .causeContent{
        width:90%;
        height:80%;
        margin:15px
    }

    .causeTitle {
        letter-spacing:1px;
        line-height:1.6;
        font-family:Helvetica;
        font-weight:800;
        font-size:30px;
        color:#fff;
        display: flex;
        justify-content: center;
        align-items: center;
        width: 90%; /* You can adjust the width as needed. */
        height: 30%; /* You can adjust the height as needed. */
        position:absolute;
        text-shadow: none;
    }

    .causeDescription{
    letter-spacing:1px;
    line-height:1.6;
      font-family:Helvetica;
      font-weight:400;
      font-size:14px;
      color:#ffffff;
      bottom:0px;
      padding:10px;
      display: flex;
      justify-content: center;
      align-items: center;
      left:-1px;
      right:-1px;
      height: 60%; /* You can adjust the height as needed. */
      position:absolute;
      background: linear-gradient(to top, rgba(0, 0, 0, .4), rgba(0, 0, 0, .4)); // 0.5 represents the opacity (50%) 
      z-index: 0; /* You can adjust the z-index as needed */
	  text-shadow: none;
    }

    .causeVolunteerInstructions{
        font-weight:400;
        font-size:14px;
        color:#fff;
        bottom:0px;
        display: flex;
        justify-content: center;
        align-items: center;
        width: 90%; /* You can adjust the width as needed. */
        height: 100%; /* You can adjust the height as needed. */
        bottom:100px;
        position:absolute;
        letter-spacing:1px;
        line-height:1.6;
        font-family:Helvetica;
        text-shadow: none;
    }

    .noCause
    {
       z-index:1;
    }

    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="container">
        <asp:Repeater ID="rptCauseSwipe" runat="server" OnItemDataBound="rptCauseSwipe_ItemDataBound">
            <ItemTemplate>
                    <div class="cause" style="display:block;" id="divCause" runat="server">
                        <div class="causeContent">
                            <span class="causeTitle"><asp:Literal ID="litCauseTitle" runat="server"></asp:Literal></span>
                                   
                            <div class="causeDescription">
                                <div style="top:0px;">
                                    <asp:Literal ID="litDescription" runat="server"></asp:Literal>
                                </div>
                            </div>
                          <%--  <div class="causeVolunteerInstructions">
                                <div>
                                    <asp:Literal ID="litVolunteerInstructions" runat="server"></asp:Literal>
                                </div>
                            </div>--%>
                                   
                        </div>
                    </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>

