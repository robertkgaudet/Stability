<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Stream.ascx.cs" Inherits="V1_UserControls_Stream" %>
<script>

    $(document).ready(function () {
        //$('.container').infiniteScroll({
        //  // options
        //  path: '.pagination__next',
        //  append: '.post',
        //  history: false,
        //});
        var pageNumber = 1;

        $(window).scroll(function () {
            var windowHeight = $(window).scrollTop() + $(window).height();
            var documentHeight = $(document).height();

            if ((windowHeight + 200) > documentHeight) {
                pageNumber = pageNumber + 1;
                updateStreamPost(pageNumber);
            }
        });

        //function uploadPostReaction(reactionId) {
        //    let postId = $("#currentSelectedPost").val();
        //    $.ajax({

        //        type: "POST",
        //        url: "/V1/UserControls/Stream.ascx.cs/UploadPostReaction",
        //        data: JSON.stringify({ userId: userId, reactionId: reactionId, postId: postId }),
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //            alert("Success");
        //        },
        //        error: function (xhr, status, error) {
        //            console.error("Error: " + error);
        //        }
        //    });
        //}

        function AppendPostIntoStream(message) {
            $(".container").append($(message).fadeIn(1000));
        }

        function updateStreamPost(pageNumber) {
            //sending commentId will cause a delete.
            $.ajax(
                {
                    type: "GET",
                    url: "/V1/Handlers/GetStreamPostNew.ashx?eventId=<%=eventId%>",
                    data: "pageNumber=" + pageNumber,
                    contentType: "text/plain; charset=utf-8",
                    dataType: "html",
                    success: function (data) {
                        if (data != "") {
                            AppendPostIntoStream(data);
                        }
                    },
                    error: function (request, status, error) {
                        request.statusText + ' - ' + error + ' - ' + status;
                    }
                });
        }

        $('.thanksReaction').click(
            function () {
                debugger;
                let postId = $("#currentSelectedPost").val();
                let reactionId = $(this).attr("id");
                $.ajax({
                    type: "POST",
                    url: "/V1/NonProfit/Stream.aspx/UploadPostReaction",
                    data: JSON.stringify({ reactionId: reactionId, postId: postId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (reactionId === '463be049-a178-4327-948c-eb3e3e7dce73') {
                            $("[data-item-id='" + postId + "']").html("&#128591; Thank");
                            $("[data-item-id='" + postId + "']").css('color', '#286090');
                        }
                        else if (reactionId === 'b247efe7-3da7-44fa-9452-a331f71d337f') {
                            $("[data-item-id='" + postId + "']").html("&#10084; Love");
                            $("[data-item-id='" + postId + "']").css('color', '#FF0000');
                        }
                        else if (reactionId === '8fe324d4-3694-4b7d-b710-df277c74b1c4') {
                            $("[data-item-id='" + postId + "']").html("&#128171; Bump");
                            $("[data-item-id='" + postId + "']").css('color', '#f0ad4e');
                        }
                        else if (reactionId === '6528bbd7-501b-475b-a15f-520bb0a3ffbf') {
                            $("[data-item-id='" + postId + "']").html("&#128074; Be Strong");
                            $("[data-item-id='" + postId + "']").css('color', '#f0ad4e');
                        }
                        else if (reactionId === '43142e57-f55b-4c8d-b024-84e0e5c664e9') {
                            $("[data-item-id='" + postId + "']").html("&#128558; Wow");
                            $("[data-item-id='" + postId + "']").css('color', '#eea236');
                        }
                        else {
                            $("[data-item-id='" + postId + "']").html("<i class='fa fa - thumbs - up m - r - sm'></i>Thank");
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error("Error: " + error);
                    }
                });
            }
        );

        let tooltipTimeout; // Declare a variable to hold the timeout reference
        $('.thankButton').hover(
            function () {
                $("#currentSelectedPost").val($(this).data("item-id"));
                var tooltip = $('#thankTooltip');
                var buttonOffset = $(this).offset(); // Get the button's position

                // Set tooltip text or modify as needed
                //tooltip.text('Tooltip for ' + $(this).text())
                //	.append('<button class="tooltipButton" id="closeTooltip">Close</button>'); // Adding a close button for demonstration

                // Calculate top position
                var topPosition = buttonOffset.top - tooltip.outerHeight() - 480;
                var leftPosition = 120;
                if (window.innerWidth <= 768) { // Adjust top position for mobile
                    topPosition -= 120; // Modify by 120 pixels for mobile
                    leftPosition -= 100;
                }

                // Position and show tooltip
                tooltip.css({
                    display: 'block',
                    top: topPosition,
                    left: leftPosition
                });

                // Clear any existing timeout before setting a new one
                clearTimeout(tooltipTimeout);

                // Set a timeout to hide the tooltip after 8 seconds
                tooltipTimeout = setTimeout(function () {
                    tooltip.css('display', 'none');
                }, 10000); // 8000 milliseconds = 8 seconds
            },
            function () {
                // Do nothing here to keep the tooltip visible until manually closed or timed out
            }
        );

        // Hide tooltip on clicking outside or on clicking the button inside the tooltip
        $(document).on('click', function (event) {
            if (!$(event.target).closest('#thankTooltip').length && !$(event.target).is('.thankButton')) {
                $('#thankTooltip').css('display', 'none');
                clearTimeout(tooltipTimeout); // Clear the timeout when hiding the tooltip manually
            }
        });

        // Close button click event inside the tooltip
        $(document).on('click', '#closeTooltip', function () {
            $('#thankTooltip').css('display', 'none');
            clearTimeout(tooltipTimeout); // Clear the timeout when closing the tooltip manually
        });
    });

</script>
<style>
    .bold-purple-star {
        font-weight: bold;
        color: red;
    }

    .large-icon:hover {
        transform: scale(1.5); /* Slightly increase the size */
    }

    .large-icon {
        font-size: 24px;
        display: inline-block; /* Ensure it responds to transforms */
        transition: transform 0.2s ease-in-out; /* Smooth transition effect */
        cursor: pointer; /* Hand cursor */
        padding: 0px 5px;
    }

    .thankTooltip {
        display: none;
        position: absolute;
        background-color: #fff; /* White background */
        color: #333; /* Dark text color */
        padding: 3px 8px 3px 8px;
        border-radius: 50px; /* Completely rounded corners */
        font-size: 12px;
        white-space: nowrap;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* Drop shadow */
        border: none; /* Remove border */
    }

    .tooltipButton {
        margin-top: 5px;
        padding: 5px 10px;
        background-color: #f00;
        color: #fff;
        border: none;
        cursor: pointer;
    }

    .checkboxlist-item {
        margin-left: 10px; /* Adjust the margin as needed */
    }

    .StreamLink {
        color: #050505;
        font-weight: bold;
    }

        .StreamLink:hover {
            color: #050505;
            text-decoration: underline;
        }

    .panel-body {
        border-top-left-radius: 10px !important;
        border-top-right-radius: 10px !important;
    }

    .panel-footer {
        border-bottom-left-radius: 10px !important;
        border-bottom-right-radius: 10px !important;
    }

    .postOpen {
        border-radius: 10px !important;
    }

    .postContainer {
        /*position:fixed !important;*/
        z-index: 500;
    }

    .postrow {
        width: 100%;
        /*height:120px;*/
        margin-left: -1px;
        margin-top: -50px;
        /* position: fixed;*/
        display: flex;
        justify-content: center;
        /*background-image: url('/V1/Images/CausePhotos/stabilityevent.png');*/ /* Path to your image */
        background-size: cover; /* Scale the image to cover the entire div */
        background-position: center; /* Center the image */
        background-repeat: no-repeat; /* Prevent the image from repeating */
        /*background-color:#E8D3FE;*/
    }

    .contentFeed {
        /*margin-top:100px !important;*/
    }

    .modal-dialog {
        display: flex;
        justify-content: center; /* Centers horizontally */
    }

    .modal-content {
        background-color: #FFF !important;
        width: 450px !important;
    }

    .modal-header {
        background-color: #FFF !important;
        border-top-left-radius: 10px !important;
        border-top-right-radius: 10px !important;
    }

    .modal-footer {
        background-color: #FFF !important;
        border-bottom-left-radius: 10px !important;
        border-bottom-right-radius: 10px !important;
    }

    .modal-body {
        text-align: left;
    }

    .modal {
        margin-top: 5px;
    }

    .image-container {
        margin-top: 25px;
        width: 100%; /* The container will take up the full width of its parent */
        max-width: 600px; /* Optional: set a maximum width for the container */
    }

    .text-container {
        border-bottom-left-radius: 5px !important;
        border-bottom-right-radius: 5px !important;
        width: 100%; /* The container will take up the full width of its parent */
        max-width: 600px; /* Optional: set a maximum width for the container */
        border: 1px solid #ccc; /* Optional: border to visualize the container */
        padding: 10px; /* Optional: padding around the image */
        box-sizing: border-box; /* Ensures padding is included in the width calculation */
        background-color: #F4F4F4;
    }

    .responsive-image {
        border-top-left-radius: 5px !important;
        border-top-right-radius: 5px !important;
        width: 100%; /* Image will take up the full width of the container */
        height: auto; /* Maintains the image's aspect ratio */
        display: block; /* Removes any inline spacing below the image */
    }

    .post {
        border-radius: 100px !important;
    }

    .URLPost:hover {
        cursor: pointer;
    }

    .post-container {
        margin-top: -60px !important;
        display: flex;
        justify-content: center; /* Centers horizontally */
    }

    .post-content {
        margin-bottom: 1px !important;
        margin-top: 100px !important;
        width: 100%;
        max-width: 100%; /* Ensure it doesn't exceed the width of the container */
        margin: 0 auto; /* Center the div */
    }

    @media only screen and (min-width: 768px) {
        .post-content {
            max-width: 610px; /* Limit width to 610px on larger screens */
            margin: 0 auto; /* Center the div on the page */
        }
    }

    .message, .message-content {
        padding: 0px !important;
        word-wrap: break-word; /* Breaks long words onto the next line */
        word-break: break-all; /* Forces a line break at any point within the word */
        overflow-wrap: break-word; /* Ensures compatibility with modern browsers */
    }

    .messageBody {
        margin-bottom: 15px !important;
    }

    .block-profile-image-div {
        clear: both; /* Prevents floating elements from wrapping around */
        width: 100%; /* Ensure the div takes up the full width */
    }

    .clearfix::after {
        content: "";
        display: table;
        clear: both;
    }

    .message-date {
        font-size: smaller;
    }

    p, div {
        /*word-wrap: break-word;*/ /* Breaks long words onto the next line */
        /*word-break: break-all;*/ /* Forces a line break at any point within the word */
        /*overflow-wrap: break-word;*/ /* Ensures compatibility with modern browsers */
    }

    #highlighted-text {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        padding: 8px;
        border: 1px solid #fff;
        background-color: white;
        color: transparent;
        white-space: pre-wrap;
        word-wrap: break-word;
        pointer-events: none;
        z-index: 1;
    }

    #postInput {
        width: 100%; /* Make sure the textarea takes the full width of its container */
        resize: none; /* Disable manual resizing */
        min-height: 50px; /* Set a minimum height */
        max-height: 200px; /* Set a maximum height after which the scroll will appear */
        overflow-y: auto; /* Hide the scrollbar */

        position: relative;
        padding: 8px;
        border: 1px solid #ccc;
        z-index: 2;
        background-color: transparent;
        color: black;
        white-space: pre-wrap;
        word-wrap: break-word;
        border: none; /* Remove the border */
        outline: none; /* Remove the outline that might appear on focus */
    }

        #postInput:focus {
            border: none; /* Remove the border */
            outline: none; /* Remove the outline that might appear on focus */
        }

    .highlighted-url {
        color: rebeccapurple;
        text-decoration: underline;
    }
    /* Custom scrollbar styles for WebKit browsers (Chrome, Safari, Edge) */
    textarea::-webkit-scrollbar {
        width: 8px; /* Adjust the width of the scrollbar */
    }

    textarea::-webkit-scrollbar-thumb {
        background-color: #888; /* Color of the scrollbar thumb */
        border-radius: 4px; /* Round the corners of the scrollbar thumb */
    }

        textarea::-webkit-scrollbar-thumb:hover {
            background-color: #555; /* Darker color when hovering over the scrollbar thumb */
        }

    textarea::-webkit-scrollbar-track {
        background-color: #f1f1f1; /* Background color of the scrollbar track */
    }

    textarea {
        border: none; /* Remove the border */
        outline: none; /* Remove the outline that might appear on focus */
    }

    select {
        border-radius: 3px; /* Round the corners of the scrollbar thumb */
        border: none; /* Remove the border */
        background-color: #f0f0f0; /* Set background color to light grey */
        min-width: fit-content; /* Make the width fit the content */
        width: auto; /* Allow the width to adjust based on content */
        padding: 5px; /* Optional: Add some padding for better appearance */
        font-size: 12px; /* Optional: Adjust the font size */
    }

        select option {
            border: none; /* Remove the border */
            padding: 8px; /* Add padding to create space inside the options */
            margin: 5px 0; /* Vertical margin between options (works in some browsers) */
        }

    .post-type-div {
        display: flex;
        justify-content: center; /* Centers horizontally */
        align-items: center; /* Centers vertically */
        padding: 4px;
        transition: background-color 0.3s ease; /* Smooth transition for hover effect */
    }

        /* Hover effect to slightly darken the background color */
        .post-type-div:hover {
            cursor: pointer;
            background-color: #e0e0e0; /* Slightly darker grey on hover */
        }

    .imagePost {
        height: 100px;
        background-color: #F1F3F6;
        padding: 5px;
        border: solid 1px #ccc;
        cursor: pointer;
    }

    .centered-image-div {
        display: flex;
        justify-content: center; /* Horizontal centering */
        align-items: center; /* Vertical centering */
    }

    .upload-div {
        width: 100%;
        display: flex;
        flex-wrap: wrap; /* Ensures the thumbnails wrap to the next line when space runs out */
        gap: 10px; /* Adds some space between the thumbnail containers */
        margin-top: 20px;
    }

    .thumbnail {
        max-width: 100%;
        max-height: 100%;
        object-fit: contain; /* Ensure the image fits within the container without being cropped */
    }

    .thumbnail-container {
        width: 120px; /* Width of the thumbnail container */
        height: 120px; /* Height of the thumbnail container */
        display: inline-flex;
        align-items: center;
        justify-content: center;
    }

    .single-thumbnail-container {
        width: 100%; /* Width of the thumbnail container */
        display: inline-flex;
        align-items: center;
        justify-content: center;
    }

    .file-input-wrapper {
        display: inline-block;
    }

    .image-container {
        width: 100%; /* Full width of the container */
        overflow: hidden; /* Hide any overflow */
    }

    .image-container-post {
        width: 100%; /* Full width of the container */
        overflow: hidden; /* Hide any overflow */
    }

    .image-container img {
        width: 100%; /* Make the image full width */
        height: 100%; /* Make the image fill the container height */
        object-fit: cover; /* Ensures the image covers the area without distortion */
    }
</style>

<script>
    function triggerFileUpload() {
        // Trigger click event on the hidden FileUpload control
        document.getElementById('<%=FileUpload1.ClientID%>').click();
    }

    var postTypeId = "8D8CDB63-28D9-4265-AC78-AF06BA6AC582"; //Message
    $('#postTypeId').val(postTypeId);
    $(document).ready(function () {
        $('.imagePost').hide();
        $('#btnImagePost').hide();

        $('#previewButton').click(function () {
        });

        // Infinite scroll feature
        $('#postContent').on('scroll', function () {
            if ($('#postContent').scrollTop() + $('#postContent').innerHeight() >= $('#postContent')[0].scrollHeight) {
                // Load more content (here you can implement a call to the server to get more data)
                $('#postContent').append('<div class="post">More content loaded...</div>');
            }
        });

        // Regular expression to detect URLs
        var urlRegex = /\b((?:https?:\/\/)?(?:www\.)?[a-zA-Z0-9-]+\.[a-zA-Z]{2,}(?:\/[^\s]*)?)\b/g;

        var previousUrls = []; // Array to store previously detected URLs
        var debounceTimer; // Timer to handle the debounce

        $('#postInput').on('input', function () {
            var textarea = $(this); // Store the reference to the textarea

            clearTimeout(debounceTimer); // Clear the timer to reset the debounce

            // Set a new debounce timer
            debounceTimer = setTimeout(function () {
                var text = textarea.val(); // Get the current value of the textarea

                // Clear previous highlights
                //			$('#highlighted-text').empty();

                var latestUrl = null;

                // Find the latest URL
                text.replace(urlRegex, function (url) {
                    latestUrl = url; // Store the last detected URL
                    return url; // Return the URL as is
                });

                if (latestUrl) {
                    if (!previousUrls.includes(latestUrl)) {
                        previousUrls.push(latestUrl); // Add the new URL to the list
                        triggerAjaxCall(latestUrl); // Trigger the AJAX call
                    }
                }
            }, 300); // Delay in milliseconds (e.g., 300ms)
        });

        // Function to trigger the AJAX call
        function triggerAjaxCall(url) {

            //var url = $('#postInput').val();
            //alert(url);
            // Regular expression to check if the URL starts with "http://" or "https://"
            var httpsRegex = /^(https?:\/\/)/i;

            // If the URL does not start with "http://" or "https://", prepend "https://"
            if (!httpsRegex.test(url)) {
                url = "https://" + url;
            }

            $.ajax({
                url: '/api/previewlink',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(url),
                success: function (data) {

                    //Hide the image upload feature on the post modal.
                    //document.getElementById('#postType').style.display = 'none';
                    $('.imagePost').hide();
                    $('#btnImagePost').hide();
                    $('#btnPost').show();
                    postTypeId = "6614579C-BD4F-45E1-9DD4-7AF88DA1C151"; //LINK POSTTYPEID
                    $('#postTypeId').val(postTypeId);

                    $('#postContent').empty();
                    var postHtml = '<div class="post">';

                    if (data.imageUrl) {
                        postHtml += '<div class="image-container"><img class="responsive-image" src="' + data.imageUrl + '" alt="Image"></div>';
                    }

                    var truncatedDescription = truncateText(data.description, 28);

                    postHtml += '<div class="text-container"><small class="text-muted">' + url + '</small></br>';
                    postHtml += '<b>' + data.title + '</b>';
                    postHtml += '<p>' + truncatedDescription + '</p></div>';

                    postHtml += '</div>';
                    $('#postContent').append(postHtml);

                    postHtml = '';

                    $('#postURL').val(url);
                    $('#postURLTitle').val(data.title);
                    $('#postURLDescription').val(truncatedDescription);
                    $('#postURLImage').val(data.imageUrl);
                    thumbnailDiv.empty(); // Clear the div for new images
                }
            });
        }

        // Function to automatically resize the textarea
        function autoResizeTextarea(textarea) {
            textarea.style.height = 'auto'; // Reset height
            textarea.style.height = (textarea.scrollHeight) + 'px'; // Set height based on scroll height

            // If the height exceeds the max-height, revert to max-height and allow scrolling
            if (textarea.scrollHeight > parseInt(window.getComputedStyle(textarea).maxHeight)) {
                textarea.style.height = window.getComputedStyle(textarea).maxHeight;
            }
        }

        // Attach the event listener to the textarea
        document.getElementById('postInput').addEventListener('input', function () {
            autoResizeTextarea(this);
        });

        // Initialize the textarea height based on initial content
        document.addEventListener('DOMContentLoaded', function () {
            var textarea = document.getElementById('postInput');
            autoResizeTextarea(textarea);
        });

        // JavaScript to clear the content when the button is clicked
        document.getElementById('buttonCancel').addEventListener('click', function () {
            // Clear the content of the div
            document.getElementById('postContent').innerHTML = '';
            document.getElementById('postInput').value = '';
            previousUrls.length = 0;
            $('.postType').show();
            $('#btnImagePost').hide();
            $('#btnPost').show();
            thumbnailDiv.empty(); // Clear the div for new images
        });

        document.getElementById('postTypeImageDiv').addEventListener('click', function () {
            //PHOTO POST TYPE
            $('.imagePost').show();
            $('#btnImagePost').show();
            $('#btnPost').hide();
        });

        document.getElementById('postTypeTextDiv').addEventListener('click', function () {
            //TEXT POST TYPE
            $('.imagePost').hide();
            $('#btnImagePost').hide();
            $('#btnPost').show();
        });

        function truncateText(text, wordLimit) {
            // Split the text into an array of words
            var words = text.split(' ');

            // Check if the word count exceeds the limit
            if (words.length > wordLimit) {
                // Join the first 'wordLimit' words and add "..."
                return words.slice(0, wordLimit).join(' ') + '...';
            } else {
                // If text is within the limit, return it unchanged
                return text;
            }
        }

        $('#FileUpload1').on('change', function (e) {
            const files = e.target.files;

            const thumbnailDiv = $('#thumbnails');
            thumbnailDiv.empty(); // Clear the div for new images

            // Limit to 4 images
            const maxImages = Math.min(files.length, 4);

            for (let i = 0; i < maxImages; i++) {
                const file = files[i];

                // Ensure the file is an image
                if (file.type.startsWith('image/')) {
                    postTypeId = "00D6E5A4-362E-40F0-8E95-1320EB1767F3"; //PHOTO POSTTYPEID
                    $('#postTypeId').val(postTypeId);
                    const reader = new FileReader();

                    reader.onload = function (event) {
                        const imgElement = $('<img>', {
                            src: event.target.result,
                            class: 'thumbnail',
                        });

                        const container = files.length == 1 ? $('<div>', { class: 'single-thumbnail-container image-container' }) : $('<div>', { class: 'thumbnail-container' });
                        container.append(imgElement);
                        thumbnailDiv.append(container);
                    };

                    reader.readAsDataURL(file); // Read the image as a data URL
                }
            }

            // Hide the file input after images are selected
            $('.imagePost').hide();
        });
    });

</script>
<div class="postContainer">
    <div class="row postrow">
        <div class="col-xs-12">
            <div class="hpanel post m-t-lg">
                <div class="panel-body postOpen">
                    <div class="message">
                        <asp:HyperLink ID="lblPostMessage" NavigateUrl="/SignIn" runat="server"></asp:HyperLink>
                        <input runat="server" id="postField" type="text" class="form-control" placeholder="Create A Post" data-toggle="modal" data-target="#newPost" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="post-container">
    <div class="post-content">
        <asp:Repeater ID="rptPosts" runat="server" OnItemDataBound="rptPosts_ItemDataBound">
            <ItemTemplate>
                <div class="hpanel messageBody">
                    <div class="panel-body">
                        <div class="message">
                            <div class="block-profile-image-div clearfix" style="line-height: 1.3;">
                                <img class="img-rounded" style="float: left; margin-right: 10px;" width="40" src="" runat="server" id="imgProfile" />
                                <asp:HyperLink ID="hypCreatedBy" runat="server" CssClass="StreamLink"></asp:HyperLink><br />
                                <asp:Label ID="lblMessageDate" runat="server" CssClass="message-date"></asp:Label>
                            </div>
                            <span class="message-content">
                                <p style="margin-top: 10px;">
                                    <asp:Literal ID="litMessage" runat="server"></asp:Literal>
                                </p>
                            </span>
                        </div>
                    </div>
                    <div class="panel-footer">
                        <div class="row">
                            <div class="col-xs-3 post-type-div thankButton text-muted" data-item-id='<%# Eval("postId") %>'>
                                <%--<i class="fa fa-thumbs-up m-r-sm"></i>Thank--%>
                                <asp:Literal ID="litReactionTitle" runat="server"></asp:Literal>
                            </div>
                            <div class="col-xs-3 post-type-div commentButton"><i class="fa fa-sticky-note m-r-sm nowrap"></i>Comment</div>
                            <div class="col-xs-3 post-type-div helpButton"><i class="fa fa-users m-r-sm"></i>Help</div>
                            <div class="col-xs-3 post-type-div giveButton"><i class="fa fa-money m-r-sm"></i>Give</div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
       <%-- <div id="postReactionType" runat="server">
        </div>--%>
        <div class="thankTooltip" id="thankTooltip">
            <asp:PlaceHolder ID="PostReactionTypesId" runat="server"></asp:PlaceHolder>
           <%-- <span id="463BE049-A178-4327-948C-EB3E3E7DCE73" class="large-icon thanksReaction" data-toggle="tooltip" data-placement="top" title="Thank">&#128591;</span>
            <!-- Thank -->
            <span id="B247EFE7-3DA7-44FA-9452-A331F71D337F" class="large-icon text-danger thanksReaction" data-toggle="tooltip" data-placement="top" title="Love">&#10084;</span>
            <!-- Love -->
            <span id="8FE324D4-3694-4B7D-B710-DF277C74B1C4" class="large-icon bold-purple-star thanksReaction" data-toggle="tooltip" data-placement="top" title="Bump">&#128171;</span>
            <!-- Bump -->
            <span id="6528BBD7-501B-475B-A15F-520BB0A3FFBF" class="large-icon thanksReaction" data-toggle="tooltip" data-placement="top" title="Be Strong">&#128074;</span>
            <!-- Connect -->
            <span id="43142E57-F55B-4C8D-B024-84E0E5C664E9" class="large-icon thanksReaction" data-toggle="tooltip" data-placement="top" title="Wow">&#128558;</span>
            <!-- Be Strong -->--%>
        </div>
        <div id="currentSelectedPost" val="" class="hidden"></div>
    </div>
</div>

<div class="modal fade" id="newPost" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <center>
                    <h4>Create post</h4>
                    <button type="button" class="btn btn-default pull-right" data-dismiss="modal" id="buttonCancel">Cancel</button>
                </center>
                <b>
                    <asp:Literal ID="litFullName" runat="server"></asp:Literal>
                </b>
                <select name="account" id="AudienceType" runat="server" clientidmode="static">
                    <%--<option style="font-size:13px;" value="FA66D7CD-4B31-4A52-B15B-4E76FD2030C2"> Public</option>
						<option style="font-size:13px;" value="3499A4F5-08AE-4869-9DAC-8B2BE4E3B206"> Friends</option>
						<option style="font-size:13px;" value="6EB17B41-1FCC-4739-BB09-94AE7044A1F9"> My Team</option>
						<option style="font-size:13px;" value="82D78B09-A57B-4674-A14E-CD3EECBB6650"> Only Me</option>--%>
                </select>
            </div>
            <div class="modal-body">
                <div class="textPost">
                    <textarea id="postInput" clientidmode="Static" runat="server" style="resize: none;" name="post" rows="1" placeholder="Create A Post"></textarea>
                    <asp:Label ID="StatusLabel" runat="server" Text=""></asp:Label>
                    <div id="postContent"></div>
                    <div class="upload-div" id="thumbnails"></div>
                </div>
                <div class="imagePost centered-image-div" onclick="triggerFileUpload();">
                    Add Photos
                </div>
                <asp:FileUpload ID="FileUpload1" ClientIDMode="Static" name="files" multiple="multiple" runat="server" Style="display: none;" />
                <input type="file" id="fileInput" clientidmode="Static" accept="image/*" name="files" multiple="multiple" style="display: none;" runat="server" />
            </div>
            <div class="modal-footer">
                <div class="postType">
                    <div class="row">
                        <div class="col-lg-3"></div>
                        <div id="postTypeTextDiv" class="col-lg-3 post-type-div"><i class="fa fa-align-left m-r-sm"></i>TEXT</div>
                        <div id="postTypeImageDiv" class="col-lg-3 post-type-div"><i class="fa fa-image m-r-sm"></i>PHOTO</div>
                        <div class="col-lg-3"></div>
                    </div>
                </div>
                <asp:Button ID="btnImagePost" ClientIDMode="Static" runat="server" CssClass="btn btn-primary btn-block" Text="Post" OnClick="Button1_Click" />
                <asp:Button ID="btnPost" ClientIDMode="Static" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary btn-block" Text="Post" />
            </div>
        </div>
    </div>
    <input type="hidden" clientidmode="Static" id="postURLTitle" runat="server" />
    <input type="hidden" clientidmode="Static" id="postURLDescription" runat="server" />
    <input type="hidden" clientidmode="Static" id="postURLImage" runat="server" />
    <input type="hidden" clientidmode="Static" id="postURL" runat="server" />
    <input type="hidden" clientidmode="Static" id="postTypeId" runat="server" />
</div>
