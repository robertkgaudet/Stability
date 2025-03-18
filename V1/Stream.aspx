<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Stream.aspx.cs" Inherits="V1_Stream" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>
<%@ Register Src="~/V1/UserControls/TeamLogo.ascx" TagPrefix="uc1" TagName="TeamLogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Scripts/infinite-scroll.pkgd.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jscroll/#.#.#/jquery.jscroll.min.js"></script>
    <script>
        $(document).ready(function () {
            //$('.container').infiniteScroll({
            //  // options
            //  path: '.pagination__next',
            //  append: '.post',
            //  history: false,
            //});

            var pageNumber = 1;

            var gCommentId = "";
            var isEditComment = false;
            loadUser();

            const users = [];
            let cursorPosition = 0;

            $(document).on('click', '.commentSection', function () {
                let postId = $(this).data("item-id");
                $('#postIdForComments').val(postId);
                loadComments(postId);
            });

            function loadComments(postId) {
                $(".commentTextarea").val('');
                $('#newComments').fadeIn();
                $('#newComments').modal('show');
                $.ajax({
                    type: "POST",
                    url: "/V1/Stream.aspx/GetCommentsByPostId",
                    data: JSON.stringify({ postId: postId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $('#rptPostComments').html('');
                        var html = ""
                        if (response.d) {
                            if (response.d.length > 0) {
                                for (let i = 0; i < response.d.length; ++i) {
                                    let item = response.d[i];
                                    html += "<ul class=\"comments\" data-item-id=\"" + item.CommentId + "\">\n<li>\n<div class=\"userImage\">\n" +
                                        "<a target=\"_blank\" href=\"" + item.ProfileUrl + "\">\n<img class=\"img-rounded\" " +
                                        "src=\"" + item.ImgProfileUrl + "\" />\n</a>\n</div>\n<div class=\"commentReact\">\n" +
                                        "<span style=\"font-weight: bold; width: 70%\">\n<a target=\"_blank\" href=\"" + item.ProfileUrl + "\" " +
                                        "class=\"author-link\">" + item.Author + "</a>\n</span>\n<span style=\"float: right; width: 20%; " +
                                        "text-align: right; margin: -20px 0px 0px 0px;\">" + item.TimeAgo + "</span>\n<span style=\"margin-top: 10px\" " +
                                        "data-item-id=\"" + item.CommentId + "-comments\">" + item.Comment1 + "</span>\n\n<div class=\"reaction\">\n" +
                                        "<div class=\"ReplyPostComment\" data-item-id=\"" + item.CommentId + "\">\n<i class=\"fa fa-reply\"></i>" +
                                        "&nbsp;Reply\n</div>\n";

                                    if (item.IsEdit) {
                                        html += "<div class=\"EditPostComment\" data-item-id=\"" + item.CommentId + "\" " +
                                            "data-item-idpost=\"" + postId + "\">\n<i class=\"fa fa-edit\"></i>&nbsp;Edit\n" +
                                            "</div>";
                                    }

                                    if (item.IsDelete) {
                                        html += "<div class=\"DeletePostComment\" data-item-id=\"" + item.CommentId + "\">\n<i class=\"fa fa-trash-o\">" +
                                            "</i>&nbsp;Delete\n</div>\n";
                                    }

                                    html += "\n</div>\n</div>\n</li>\n\n<div class=\"reply-input hidden\" " +
                                        "data-item-id=\"" + item.CommentId + "-replyDiv\">\n<textarea data-item-id=\"" + item.CommentId + "-reply\" " +
                                        "rows=\"3\" class=\"replyTextarea\" placeholder=\"Write a reply...\"></textarea>\n<span class=\"hidden\" " +
                                        "data-item-id=\"" + postId + "-postReplyId\"></span>\n<button type=\"button\" class=\"addreply\" " +
                                        "data-item-id=\"" + item.CommentId + "-postReply\">\n<i class=\"fa fa-reply\"></i>&nbsp;Reply\n</button>\n" +
                                        "<div style='z-index:999' id=\"replySuggestions\" class=\"suggestions replySuggestions\"></div>\n</div>\n\n";

                                    for (let j = 0; j < item.Replies.length; ++j) {
                                        let reply = item.Replies[j];
                                        html += "<div class=\"reply\">\n<div class=\"replyImg\">\n" +
                                            "<a target=\"_blank\" href=\"" + reply.ProfileUrl + "\">\n<img class=\"img-rounded\" style=\"float: left; " +
                                            "margin-right: 10px; border-radius: 20px\" width=\"40\" src=\"" + reply.ImgProfileUrl + "\" />\n</a>\n" +
                                            "</div>\n<div class=\"replyContent\">\n<span style=\"font-weight: bold; width: 70%\">\n" +
                                            "<a target=\"_blank\" href=\"" + reply.ProfileUrl + "\" class=\"author-link\">" + reply.Author + "</a>\n</span>\n" +
                                            "<span style=\"float: right; width: 20%; text-align: right; margin: -20px 10px 0px 0px;\">" + reply.TimeAgo + "</span>\n" +
                                            "<span style=\"margin: 10px 0px 0px 40px;width: 90%;\" data-item-id=\"" + reply.CommentId + "-comments\">" + reply.Comment1 + "</span>\n" +
                                            "\n<div class=\"reaction\">\n<div class=\"ReplyPostComment\" data-item-id=\"" + reply.CommentId + "\">\n<i class=\"fa fa-reply\">" +
                                            "</i>&nbsp;Reply\n</div>\n";

                                        if (reply.IsEdit) {
                                            html += "<div class=\"EditPostReply\" data-item-id=\"" + reply.CommentId + "\" " +
                                                "data-item-postid=\"" + postId + "\">\n<i class=\"fa fa-edit\"></i>&nbsp;Edit\n</div>";
                                        }

                                        if (reply.IsDelete) {
                                            html += "<div class=\"DeletePostReply\" data-item-id=\"" + reply.CommentId + "\">\n<i class=\"fa fa-trash-o\"></i>&nbsp;Delete\n</div>";
                                        }

                                        html += "</div>\n</div>\n</div>\n";

                                        html += "<div class=\"reply-input hidden\" data-item-id=\"" + reply.CommentId + "-replyDiv\">\n" +
                                            "<textarea data-item-id=\"" + reply.CommentId + "-reply\" rows=\"3\" class=\"replyTextarea\" placeholder=\"Write a reply...\"></textarea>\n" +
                                            "<span class=\"hidden\" data-item-id=\"" + item.CommentId + "\"></span>\n" +
                                            "<button type=\"button\" class=\"addReplyReply\" data-item-id=\"" + reply.CommentId + "-postReply\">\n" +
                                            "<i class=\"fa fa-reply\"></i>&nbsp;Reply\n</button>\n<div id=\"replySuggestions\" class=\"suggestions replySuggestions\"></div>\n</div>";
                                    }
                                    html += "</ul>";
                                }
                            }
                            else {
                                html += "<div style='text-align: center'> <strong> No Comments... </strong> </div>"
                            }
                        }
                        $("#rptPostComments").html(html);
                    },
                    error: function (xhr, status, error) {
                        console.error("Error: " + error);
                        $('#rptPostComments').html('<div class="error">Error loading comments. Please try again.</div>');
                    }
                });
            }

            function loadCommentsUnderPost(postId) {
                $.ajax({
                    type: "POST",
                    url: "/V1/Stream.aspx/GetCommentsUnderPostById",
                    data: JSON.stringify({ postId: postId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var div = document.querySelector(`.comment-section-repeater-show[data-item-id="${postId}-showComments"]`);
                        var html = ""
                        if (response.d) {
                            if (response.d.length > 0) {
                                if (div) {
                                    var parentDiv = div.parentElement; // This gives you the parent div
                                    parentDiv.style.display = 'block';
                                }
                                for (let i = 0; i < response.d.length; ++i) {
                                    let item = response.d[i];
                                    html += "<ul class=\"comments\">\n<li>\n<div class=\"userImage\">\n" +
                                        "<a target=\"_blank\" href=\"" + item.ProfileUrl + "\">\n<img class=\"img-rounded\" " +
                                        "src=\"" + item.ImgProfileUrl + "\" />\n</a>\n</div>\n<div class=\"commentReact\">\n" +
                                        "<span style=\"font-weight: bold; width: 70%\">\n<a target=\"_blank\" href=\"" + item.ProfileUrl + "\" " +
                                        "class=\"author-link\">" + item.Author + "</a>\n</span>\n<span style=\"float: right; width: 12%; " +
                                        "text-align: right; margin: 0px 5px 0px 0px;\">" + item.TimeAgo + "</span>\n<span>" + item.Comment1 + "</span></div></li></ul>";
                                }
                            }
                        }
                        $(div).html(html);
                    },
                    error: function (xhr, status, error) {
                        console.error("Error: " + error);
                    }
                });
            }

            // Handle typing in the textarea
            $('.commentTextarea').on("keyup", function (e) {
                const value = $(this).val();
                const itemId = $("#postIdForComments").val();
                const caretPosition = this.selectionStart;
                cursorPosition = caretPosition;
                // Check for `@` followed by text
                const match = value.slice(0, caretPosition).match(/@\s?(\w*)$/);
                if (match) {
                    const searchText = match[1].toLowerCase();
                    // Filter users
                    const filteredUsers = users.filter((user) =>
                        user.Title.toLowerCase().startsWith(searchText)
                    );

                    // Populate commentSuggestions
                    let suggestionsHTML = "";
                    filteredUsers.forEach((user) => {
                        suggestionsHTML += `<div class="suggestion" data-id="${user.UserId}" style="padding: 5px; cursor: pointer;">${user.Title}</div>`;
                    });

                    $(".commentSuggestions").html(suggestionsHTML).show();

                    // Select suggestion
                    $('.suggestion').on('click', function () {
                        const userName = $(this).text();
                        // Insert selected user at caret position
                        const text = $(".commentTextarea").val(); //$("textarea[data-item-id='" + itemId + "-comment" + "']").val();
                        const beforeCaret = text.slice(0, cursorPosition);
                        const afterCaret = text.slice(cursorPosition);
                        const updatedText = `${beforeCaret}${userName} ${afterCaret}`;

                        $(".commentTextarea").val(updatedText);  //$("textarea[data-item-id='" + itemId + "-comment" + "']").val(updatedText);
                        cursorPosition = updatedText.length; // Update cursor position
                        $("textarea[data-item-id='" + itemId + "-comment" + "']").focus();
                        $(".commentSuggestions").hide();
                    });
                } else {
                    $(".commentSuggestions").hide();
                }
            });

            $(document).on('click', function (event) {
                if (!$(event.target).closest('.commentTextarea').length) {
                    $('.commentSuggestions').hide();
                }
            });

            $(document).on('click', function (event) {
                if (!$(event.target).closest('.replyTextarea').length) {
                    $('.replySuggestions').hide();
                }
            });

            function loadUser() {
                $.ajax({
                    type: "POST",
                    url: "/V1/Stream.aspx/GetUsers",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $.each(response.d, function (index, value) {
                            users.push(value);
                        });
                    }
                });
            }

            function reloadPage() {
                var scrollPos = $(window).scrollTop();
                $(window).on('beforeunload', function () {
                    sessionStorage.setItem('scrollPos', scrollPos);
                });
                location.reload();
            }

            $(window).scroll(function () {
                var windowHeight = $(window).scrollTop() + $(window).height();
                var documentHeight = $(document).height();
                if ((windowHeight + 200) > documentHeight) {
                    pageNumber = pageNumber + 1;
                    updateStreamPost(pageNumber);
                }
            });

            function AppendPostIntoStream(message) {
                $(".post-content").append($(message).fadeIn(1000));
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
                    let postId = $("#currentSelectedPost").val();
                    let reactionId = $(this).attr("id");
                    $.ajax({
                        type: "POST",
                        url: "/V1/Stream.aspx/UploadPostReaction",
                        data: JSON.stringify({ reactionId: reactionId, postId: postId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            $('#thankTooltip').css('display', 'none');
                            $("[data-item-rid='" + postId + "']").html(response.d);
                            if (response.d === '') {
                                $("[data-item-cid='" + postId + "']").html("&#128077; Like");
                                $("[data-item-cid='" + postId + "']").css('color', '#777');
                            }
                            else {
                                if (reactionId === '463be049-a178-4327-948c-eb3e3e7dce73') {
                                    $("[data-item-cid='" + postId + "']").html("&#128591; Like");
                                    $("[data-item-cid='" + postId + "']").css('color', '#286090');
                                }
                                else if (reactionId === 'b247efe7-3da7-44fa-9452-a331f71d337f') {
                                    $("[data-item-cid='" + postId + "']").html("&#10084; Love");
                                    $("[data-item-cid='" + postId + "']").css('color', '#FF0000');
                                }
                                else if (reactionId === '8fe324d4-3694-4b7d-b710-df277c74b1c4') {
                                    $("[data-item-cid='" + postId + "']").html("&#128171; Bump");
                                    $("[data-item-cid='" + postId + "']").css('color', '#f0ad4e');
                                }
                                else if (reactionId === '6528bbd7-501b-475b-a15f-520bb0a3ffbf') {
                                    $("[data-item-cid='" + postId + "']").html("&#128074; Be Strong");
                                    $("[data-item-cid='" + postId + "']").css('color', '#f0ad4e');
                                }
                                else if (reactionId === '43142e57-f55b-4c8d-b024-84e0e5c664e9') {
                                    $("[data-item-cid='" + postId + "']").html("&#128558; Wow");
                                    $("[data-item-cid='" + postId + "']").css('color', '#eea236');
                                }
                                else {
                                    $("[data-item-cid='" + postId + "']").html("&#128077; Like");
                                    $("[data-item-cid='" + postId + "']").css('color', '#777');
                                }
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error("Error: " + error);
                        }
                    });
                }
            );

            let tooltipTimeout; // Declare a variable to hold the timeout reference
            //$('.thankButton').hover(
            //    function () {
            //        $("#currentSelectedPost").val($(this).data("item-id"));
            //        var tooltip = $('#thankTooltip');
            //        var buttonOffset = $(this).offset(); // Get the button's position

            //        // Set tooltip text or modify as needed
            //        //tooltip.text('Tooltip for ' + $(this).text())
            //        //	.append('<button class="tooltipButton" id="closeTooltip">Close</button>'); // Adding a close button for demonstration

            //        // Calculate top position
            //        var topPosition = buttonOffset.top - tooltip.outerHeight() - 70;
            //        var leftPosition = 480;
            //        if (window.innerWidth <= 768) { // Adjust top position for mobile
            //            topPosition -= 120; // Modify by 120 pixels for mobile
            //            leftPosition -= 100;
            //        }

            //        // Position and show tooltip
            //        tooltip.css({
            //            display: 'block',
            //            top: topPosition,
            //            left: leftPosition
            //        });

            //        // Clear any existing timeout before setting a new one
            //        clearTimeout(tooltipTimeout);

            //        // Set a timeout to hide the tooltip after 8 seconds
            //        tooltipTimeout = setTimeout(function () {
            //            tooltip.css('display', 'none');
            //        }, 10000); // 8000 milliseconds = 8 seconds
            //    },
            //);

            //Hide tooltip on clicking outside or on clicking the button inside the tooltip
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

            $(document).on('click', '.closeComment', function () {
                $('#newComments').modal('hide');
            });

            $(document).on('click', '.addComment', function () {
                let postId = $("#postIdForComments").val();
                const commentText = $(".commentTextarea").val();
                debugger;
                var urlRegex = /\b((?:https?:\/\/)?(?:www\.)?[a-zA-Z0-9-]+\.[a-zA-Z]{2,}(?:\/[^\s]*)?)\b/g;
                var latestUrl = null;
                commentText.replace(urlRegex, function (url) {
                    latestUrl = url; // Store the last detected URL
                    return url; // Return the URL as is
                });

                let commentId = gCommentId;
                if (commentText.trim() === "") {
                    alert("Please enter a comment.");
                    return;
                }
                $.ajax({
                    type: "POST",
                    url: "/V1/Stream.aspx/UploadPostComment",
                    data: JSON.stringify({ postId: postId, commentId: commentId, comment: commentText, isReply: false, isEditComment: isEditComment }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        gCommentId = "";
                        isEditComment = false;
                        loadComments($("#postIdForComments").val());
                        loadCommentsUnderPost($("#postIdForComments").val());
                    },
                    error: function (xhr, status, error) {
                        console.error("Error: " + error);
                    }
                });
            });

            $(document).on('click', '.addreply', function () {
                var parentDiv = $(this).closest(".reply-input");
                var postId = parentDiv.find("span[data-item-id]").data("item-id");
                var commentId = $(this).data('item-id').slice(0, -10);
                var replyText = $("textarea[data-item-id='" + commentId + "-reply']").val();
                if (replyText.trim() === "") {
                    alert("Please enter a reply.");
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: "/V1/Stream.aspx/UploadPostComment",
                    data: JSON.stringify({ postId: postId.slice(0, -12), commentId: commentId, comment: replyText, isReply: true, isEditComment: isEditComment }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        gCommentId = "";
                        isEditComment = false;
                        loadComments($("#postIdForComments").val());
                    },
                    error: function (xhr, status, error) {
                        console.error("Error: " + error);
                    }
                });
            });

            $(document).on('click', '.addReplyReply', function () {
                var parentDiv = $(this).closest(".reply-input");
                var postId = $("#postIdForComments").val();
                var commentId = $(this).data('item-id').slice(0, -10);
                var replyText = $("textarea[data-item-id='" + commentId + "-reply']").val();
                if (replyText.trim() === "") {
                    alert("Please enter a reply.");
                    return;
                }

                if (!isEditComment) {
                    commentId = parentDiv.find("span[data-item-id]").data("item-id");
                }
                $.ajax({
                    type: "POST",
                    url: "/V1/Stream.aspx/UploadPostComment",
                    data: JSON.stringify({ postId: postId, commentId: commentId, comment: replyText, isReply: true, isEditComment: isEditComment }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        gCommentId = "";
                        isEditComment = false;
                        loadComments($("#postIdForComments").val());
                    },
                    error: function (xhr, status, error) {
                        console.error("Error: " + error);
                    }
                });
            });

            $(document).on('click', '.ReplyPostComment', function () {
                let postId = $(this).data("item-id");
                $(".reply-input").addClass("hidden");
                $("div[data-item-id='" + postId + "-replyDiv']").removeClass("hidden");
            });

            $(document).on('click', '.EditPostComment', function () {
                let postCommentId = $(this).data("item-id");
                gCommentId = postCommentId;
                isEditComment = true;
                var spanDataId = postCommentId + "-comments"; // Example: '123-comments'
                var commentText = $("span[data-item-id='" + spanDataId + "']").text();
                //var postId = $(this).data('item-idpost');
                $(".commentTextarea").val(commentText);
                //$("textarea[data-item-id='" + postId + "-comment" + "']").val(commentText);
                //$("div[data-item-id='" + postId + "-cdiv" + "']").removeClass("hidden");
            });

            $(document).on('keyup', '.replyTextarea', function () {
                const value = $(this).val();
                const itemId = $(this).data("item-id").slice(0, -6);
                const caretPosition = this.selectionStart;
                cursorPosition = caretPosition;
                // Check for `@` followed by text
                const match = value.slice(0, caretPosition).match(/@\s?(\w*)$/);
                if (match) {
                    const searchText = match[1].toLowerCase();
                    // Filter users
                    const filteredUsers = users.filter((user) =>
                        user.Title.toLowerCase().startsWith(searchText)
                    );

                    // Populate commentSuggestions
                    let suggestionsHTML = "";
                    filteredUsers.forEach((user) => {
                        suggestionsHTML += `<div class="suggestionReply" data-id="${user.UserId}" style="padding: 5px; cursor: pointer;">${user.Title}</div>`;
                    });

                    $(".replySuggestions").html(suggestionsHTML).show();

                    // Select suggestion
                    $('.suggestionReply').on('click', function () {
                        const userName = $(this).text();
                        // Insert selected user at caret position
                        const text = $("textarea[data-item-id='" + itemId + "-reply" + "']").val();
                        const beforeCaret = text.slice(0, cursorPosition);
                        const afterCaret = text.slice(cursorPosition);
                        const updatedText = `${beforeCaret}${userName} ${afterCaret}`;
                        $("textarea[data-item-id='" + itemId + "-reply" + "']").val(updatedText);
                        cursorPosition = updatedText.length; // Update cursor position
                        $("textarea[data-item-id='" + itemId + "-reply" + "']").focus();
                        $(".replySuggestions").hide();
                    });
                } else {
                    $(".replySuggestions").hide();
                }
            });

            $(document).on('click', '.EditPostReply', function () {
                let postCommentId = $(this).data("item-id");
                gCommentId = postCommentId;
                isEditComment = true;
                var spanDataId = postCommentId + "-comments"; // Example: '123-comments'
                var commentText = $("span[data-item-id='" + spanDataId + "']").text();
                //var postId = $(this).data('item-postid');
                $(".reply-input").addClass("hidden");
                $('textarea[data-item-id="' + postCommentId + '-reply"]').val(commentText);
                $('div[data-item-id="' + postCommentId + '-replyDiv"]').removeClass('hidden');

                //$("div[data-item-id='" + postCommentId + "-replyDiv']").next("textarea").val(commentText);

            });


            $(document).on('click', '.DeletePostComment', function () {
                let postCommentId = $(this).data("item-id");
                $.ajax({
                    type: "POST",
                    url: "/V1/Stream.aspx/DeletePostComment",
                    data: JSON.stringify({ postCommentId: postCommentId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        loadComments($("#postIdForComments").val());
                    },
                    error: function (xhr, status, error) {
                        console.error("Error: " + error);
                    }
                });
            });

            $(document).on('click', '.DeletePostReply', function () {
                let postCommentId = $(this).data("item-id");
                $.ajax({
                    type: "POST",
                    url: "/V1/Stream.aspx/DeletePostReply",
                    data: JSON.stringify({ postCommentId: postCommentId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        loadComments($("#postIdForComments").val());
                    },
                    error: function (xhr, status, error) {
                        console.error("Error: " + error);
                    }
                });
            });
        });

        let tooltipTimeout; // Declare a variable to hold the timeout reference
        $(document).on('mouseenter', '.thankButton', function () {
            $("#currentSelectedPost").val($(this).data("item-cid"));
            var tooltip = $('#thankTooltip');
            var buttonOffset = $(this).offset(); // Get the button's position
            // Set tooltip text or modify as needed
            //tooltip.text('Tooltip for ' + $(this).text())
            //	.append('<button class="tooltipButton" id="closeTooltip">Close</button>'); // Adding a close button for demonstration

            // Calculate top position
            var topPosition = buttonOffset.top - tooltip.outerHeight() - 70;
            var leftPosition = 480;
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
            }, 4000); // 8000 milliseconds = 8 seconds
        });


        $(document).on('click', '#commentButton', function () {
            var cDiv = $(this).data('item-id').replace('cbutton', 'cdiv');
            $("div[data-item-id='" + cDiv + "']").removeClass("hidden");
        });

    </script>
    <style>
    .suggestions {
        border: 1px solid #ccc;
        max-height: 150px;
        overflow-y: auto;
        position: absolute;
        width: 300px;
        display: none;
        background-color: white;
        z-index: 999;
    }

    span {
        word-wrap: break-word;
    }

    .suggestion-item {
        padding: 8px;
        cursor: pointer;
    }

        .suggestion-item:hover {
            background-color: #f0f0f0;
        }

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

    .tooltip {
        display: none;
        position: absolute;
        background-color: #333;
        color: #fff;
        padding: 5px;
        border-radius: 3px;
        font-size: 12px;
    }

    .hover-button {
        margin: 50px;
        padding: 10px 20px;
        background-color: #007bff;
        color: white;
        border: none;
        cursor: pointer;
    }

    .commentList {
        padding: 10px;
        height: 570px;
        max-height: 570px;
        min-height: 570px;
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
        z-index: 9999;
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


    .StreamPortalLink {
        float: right;
        text-align: right;
        color: #6A6C6F;
        font-size: 90%;
    }

        .StreamPortalLink:hover {
            color: #6A6C6F;
            text-decoration: none;
        }

        .StreamPortalLink::after {
            content: none; /* This removes the arrow if it's added via pseudo-element */
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
        position: fixed !important;
        z-index: 500;
    }

    .postrow {
        width: 100%;
        height: 120px;
        margin-left: -1px;
        margin-top: -25px;
        position: fixed;
        display: flex;
        justify-content: center;
        /*background-image: url('/V1/Images/CausePhotos/stabilityevent.png');*/ /* Path to your image */
        background-size: cover; /* Scale the image to cover the entire div */
        background-position: center; /* Center the image */
        background-repeat: no-repeat; /* Prevent the image from repeating */
        background-color: #E8D3FE;
    }

    .contentFeed {
        margin-top: 100px !important;
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
        /*background-color: #FFF !important;*/
        background-color: #5e2e91 !important;
        /* border-top-left-radius: 10px !important;
            border-top-right-radius: 10px !important;*/
        color: #fff;
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

    .reply {
        margin: 0 0 10px 50px;
        position: relative;
    }

        .reply::before {
            border-left: 2px solid #efefef;
            content: "";
            height: 78px;
            width: 1px;
            position: absolute;
            left: -35px;
            top: -65px;
        }

        .reply::after {
            border-bottom: 2px solid #efefef;
            border-radius: 0 0 0 .8rem;
            content: "";
            height: 20px;
            width: 35px;
            position: absolute;
            left: -35px;
            top: -3px;
        }

    .post-container {
        margin-top: 90px !important;
        display: flex;
        justify-content: center; /* Centers horizontally */
    }

    .post-content {
        margin-bottom: 1px !important;
        margin-top: 1px !important;
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
        width: 100%;
        resize: none;
        min-height: 50px;
        max-height: 200px;
        overflow-y: auto;
        position: relative;
        padding: 5px;
        /*border: 1px dotted #ccc;*/
        z-index: 2;
        background-color: transparent;
        color: black;
        white-space: pre-wrap;
        word-wrap: break-word;
        border: none;
        outline: none;
        margin-top: 10px;
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
        border-radius: 3px;
        border: none;
        background-color: #f0f0f0;
        min-width: fit-content;
        width: auto;
        padding: 5px;
        font-size: 12px;
        margin-left: -15px;
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

    .tooltipButton {
        margin-top: 5px;
        padding: 5px 10px;
        background-color: #f00;
        color: #fff;
        border: none;
        cursor: pointer;
    }

    .comment-section {
        width: 100%;
        max-width: 600px;
        max-height: 550px;
        min-height: 550px;
        height: 550px;
        /*overflow-y: scroll;*/
        margin: 0px auto;
        background: #fff;
        border-radius: 8px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        padding: 5px;
        position: relative;
    }

    .comment-section-show {
        width: 100%;
        max-width: 600px;
        max-height: 100px;
        min-height: auto;
        height: auto;
        margin: 0px auto;
        background: #fff;
        border-radius: 8px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        padding: 10px;
        position: relative;
    }

    .comment-section-repeater {
        width: 103%;
        max-height: 480px;
        min-height: 480px;
        height: 480px;
        overflow-x: hidden;
        overflow-y: auto;
        padding: 0px;
        position: relative;
        z-index: 0;
        scroll-behavior: smooth;
        border-top: 1px solid #f0f0f0;
    }

    .comment-section-repeater-show {
        width: 103%;
        max-height: 100px;
        min-height: auto;
        height: auto;
        overflow-x: hidden;
        overflow-y: auto;
        padding: 2px;
        position: relative;
        z-index: 0;
        scroll-behavior: smooth;
        /*border-top: 1px solid #f0f0f0;*/
    }
    /* Customize the scrollbar */
    .comment-section-repeater::-webkit-scrollbar {
        width: 8px; /* Width of the scrollbar */
    }

    .comment-section-repeater::-webkit-scrollbar-track {
        background: #f1f1f1; /* Track background */
    }

    .comment-section-repeater::-webkit-scrollbar-thumb {
        background: #888; /* Color of the thumb */
        border-radius: 4px;
    }

        .comment-section-repeater::-webkit-scrollbar-thumb:hover {
            background: #555; /* Color of the thumb when hovering */
        }

    .loadComments {
        text-decoration: underline;
        position: absolute;
        z-index: 999;
        bottom: 0;
        left: 0;
        right: 0;
        padding: 7px;
        text-align: center;
        background-color: #fff;
        color: #000;
    }

    .comment-header {
        font-size: 18px;
        font-weight: bold;
        margin-bottom: 15px;
    }

    .comment-input {
        display: flex;
        gap: 10px;
        margin-bottom: 20px;
    }

        .comment-input textarea {
            flex: 1;
            resize: none;
            padding: 5px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 12px;
            height: 40px;
            font-size: 1.4rem;
            padding: 10px;
            border-radius: 20px;
            overflow: hidden;
        }

        .comment-input button {
            padding: 0px 15px;
            background-color: #1877f2;
            color: #fff;
            border: none;
            border-radius: 15px;
            cursor: pointer;
            font-size: 14px;
            margin: 5px 0px 5px 0px;
        }

            .comment-input button:hover {
                background-color: #145db2;
            }



    .reply-input {
        display: flex;
        gap: 5px;
        margin-bottom: 20px;
        position: relative;
    }

        .reply-input textarea {
            flex: 1;
            resize: none;
            border: 1px solid #ccc;
            border-radius: 20px;
            font-size: 12px;
            height: 30px;
            margin-left: 50px;
            padding: 5px 60px 5px 5px;
        }

        .reply-input button {
            padding: 0px 10px;
            background-color: #1877f2;
            color: #fff;
            border: none;
            border-radius: 20px;
            cursor: pointer;
            font-size: 12px;
            position: absolute;
            right: 0;
            bottom: 0;
            top: 0;
        }

            .reply-input button:hover {
                background-color: #145db2;
            }

    .comments {
        list-style: none;
        padding: 0;
    }

    .comment {
        display: flex;
        align-items: flex-start;
        gap: 10px;
        margin-bottom: 15px;
    }

        .comment img {
            width: 40px;
            height: 40px;
            border-radius: 50%;
        }

    .comment-content {
        background: #f0f2f5;
        border-radius: 8px;
        padding: 10px;
        font-size: 14px;
        line-height: 1.4;
    }

    .comments li {
        width: 100%;
        display: flex;
        margin: 0 0 10px 0;
    }

    .userImage,
    .replyImg {
        width: 40px;
        float: left;
        margin-right: 0px;
    }

        .userImage img {
            border-radius: 20px;
            width: 30px;
            position: relative;
            z-index: 99;
            border: 1px solid #ccc;
        }

        .replyImg img {
            border-radius: 20px;
            width: 25px;
            position: relative;
            z-index: 99;
            border: 1px solid #ccc;
        }

    .commentReact {
        float: left;
        width: calc(100% - 50px);
    }

        .commentReact span,
        .replyContent span,
        .reaction {
            margin: 0;
            width: 100%;
            display: block;
            font-weight: normal;
        }

        .commentReact .reaction {
            /*border: 1px solid #ccc;*/
            display: inline-flex;
            margin: 5px 0 0 0;
        }

    .replyContent .reaction {
        /*border: 1px solid #ccc;*/
        display: inline-flex;
        margin: 5px 0 0 40px;
    }

    .reaction .EditPostComment,
    .reaction .DeletePostComment,
    .reaction .ReplyPostComment,
    .reaction .EditPostReply,
    .reaction .DeletePostReply,
    .reaction .ReplyPostComment {
        padding: 5px 10px;
        font-size: 10px;
        /*border-right: 1px solid #ccc;*/
        cursor: pointer;
    }

        .reaction .ReplyPostComment:hover,
        .reaction .ReplyPostComment:hover {
            color: #007bff
        }

        .reaction .EditPostComment:hover,
        .reaction .EditPostReply:hover {
            color: #17a2b8;
        }

        .reaction .DeletePostComment:hover,
        .reaction .DeletePostReply:hover {
            color: #dc3545;
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

            $('#previewButton').click(function () { });

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="postContainer">
        <div class="row postrow">
            <div class="col-lg-3">
            </div>
            <div class="col-xs-12 col-lg-6">
                <div class="hpanel post m-t-lg">
                    <div class="panel-body postOpen">
                        <div class="message">
                            <asp:HyperLink ID="lblPostMessage" NavigateUrl="/SignIn" runat="server"></asp:HyperLink>
                            <input runat="server" id="postField" type="text" class="form-control" placeholder="Create A Post" data-toggle="modal" data-target="#newPost" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-3">
            </div>
        </div>
    </div>

    <div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
        <div class="post-container">
            <div class="post-content">
                <asp:Repeater ID="rptPosts" runat="server" OnItemDataBound="rptPosts_ItemDataBound">
                    <ItemTemplate>
                        <div class="hpanel messageBody">
                            <div class="panel-body">
                                <div class="message">
                                    <div class="block-profile-image-div clearfix" style="        line-height: 1.3;">
                                        <a href="" style="float: left; margin-right: 10px; display: none;" id="linkProfile" runat="server">
                                            <img class="img-rounded" width="40" src="" runat="server" id="imgProfile" />
                                        </a>
                                        <asp:HyperLink ID="hypCreatedBy" runat="server" CssClass="StreamLink"></asp:HyperLink>
                                        <asp:HyperLink ID="hypPortalLink" runat="server" CssClass="StreamPortalLink"></asp:HyperLink>
                                        <uc1:TeamLogo runat="server" ID="ucUserNameWithBadges" />
                                        <br />
                                        <asp:Label ID="lblMessageDate" runat="server" CssClass="message-date"></asp:Label>
                                    </div>
                                    <span class="message-content">
                                        <p style="        margin-top: 10px;">
                                            <asp:Literal ID="litMessage" runat="server"></asp:Literal>
                                        </p>
                                    </span>
                                </div>
                            </div>
                            <div class="panel-footer">
                                <div class="row" style="        margin: -5px 5px -18px 5px">
                                    <span data-item-rid='<%# Eval("postId") %>'>
                                        <asp:Literal ID="litReactionCount" runat="server"></asp:Literal>
                                    </span>
                                    <span style="        float: right;
        margin-top: -23px">
                                        <div class="post-type-div commentSection" data-item-id='<%# Eval("postId") %>'>
                                            <asp:Literal ID="litCommentsCount" runat="server"></asp:Literal>
                                        </div>
                                    </span>
                                </div>
                                <hr />
                                <div class="row">
                                    <div class="col-xs-3 post-type-div thankButton text-muted" data-item-cid='<%# Eval("postId") %>'>
                                        <asp:Literal ID="litReactionTitle" runat="server"></asp:Literal>
                                    </div>
                                    <div class="col-xs-3 post-type-div commentSection" data-item-id='<%# Eval("postId") %>'><i class="fa fa-sticky-note m-r-sm nowrap"></i>Comment</div>
                                    <%--<div class="col-xs-3 post-type-div" id="helpButton"><i class="fa fa-users m-r-sm"></i>Help</div>
                                    <div class="col-xs-3 post-type-div" id="giveButton"><i class="fa fa-money m-r-sm"></i>Give</div>--%>
                                </div>

                                <div id="commentSectionShow" class="comment-section-show" runat="server">
                                    <div class="comment-section-repeater-show" data-item-id='<%# Eval("postId") %>-showComments'>
                                        <asp:Repeater ID="rptPostCommentsShow" runat="server">
                                            <ItemTemplate>
                                                <ul class="comments">
                                                    <li>
                                                        <div class="userImage">
                                                            <a target="_blank" href="<%# Eval("ProfileUrl") %>">
                                                                <img class="img-rounded" src='<%# Eval("imgProfileUrl") %>' /></a>
                                                        </div>
                                                        <div class="commentReact">
                                                            <span style="        font-weight: bold;
        width: 70%">
                                                                <a target="_blank" href="<%# Eval("ProfileUrl") %>" class="author-link"><%# Eval("author") %></a>
                                                            </span>
                                                            <span style="        float: right;
        width: 12%;
        text-align: right;
        margin: 0px 5px 0px 0px;"><%# Eval("timeAgo") %></span>
                                                            <span><%# Eval("Comment1") %></span>
                                                        </div>
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <div class="thankTooltip" id="thankTooltip">
                    <asp:PlaceHolder ID="PostReactionTypesId" runat="server"></asp:PlaceHolder>
                </div>
                <div id="currentSelectedPost" val="" class="hidden"></div>
            </div>
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
                </div>
                <div class="modal-body">
                    <div class="col-md-12">
                        <select id="AudienceType" name="AudienceType" runat="server" clientidmode="static"></select>
                    </div>
                    <div class="col-md-12 m-t-sm">
                        <select id="PortalTypes" name="PortalTypes" runat="server" clientidmode="static"></select>
                    </div>

                    <div class="textPost">
                        <textarea id="postInput" clientidmode="Static" runat="server" style="        resize: none;" name="post" rows="1" placeholder="Create A Post"></textarea>
                        <asp:Label ID="StatusLabel" runat="server" Text=""></asp:Label>
                        <div id="postContent"></div>
                        <div class="upload-div" id="thumbnails"></div>
                    </div>
                    <div class="imagePost centered-image-div" onclick="triggerFileUpload();">
                        Add Photos
                    </div>
                    <asp:FileUpload ID="FileUpload1" ClientIDMode="Static" name="files" multiple="multiple" runat="server" Style="        display: none;" />
                    <input type="file" id="fileInput" clientidmode="Static" accept="image/*" name="files" multiple="multiple" style="        display: none;" runat="server" />
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
                    <asp:Button ID="btnPost" type="button" ClientIDMode="Static" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary btn-block" Text="Post" />
                </div>
            </div>
        </div>
        <input type="hidden" clientidmode="Static" id="postURLTitle" runat="server" />
        <input type="hidden" clientidmode="Static" id="postURLDescription" runat="server" />
        <input type="hidden" clientidmode="Static" id="postURLImage" runat="server" />
        <input type="hidden" clientidmode="Static" id="postURL" runat="server" />
        <input type="hidden" clientidmode="Static" id="postTypeId" runat="server" />
    </div>

    <div class="modal fade" id="newComments" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false" style="        overflow: hidden">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="        padding: 5px">
                    <center>
                        <h4>Comments</h4>
                    </center>
                </div>
                <div class="modal-body" id="commentList" style="        padding: 10px;
        height: 570px;
        max-height: 570px;
        min-height: 570px;">
                    <div class="textPost">
                        <div class="comment-section">
                            <div class="comment-input">
                                <textarea rows="3" class="commentTextarea" placeholder="Write a comment..."></textarea>
                                <button type="button" class="addComment">Post</button>
                                <div id="commentSuggestions" class="suggestions commentSuggestions"></div>
                            </div>
                            <div class="comment-section-repeater">
                                <br />
                                <div id="rptPostComments">
                                    <strong style="        margin-left: 120px">Comments are Loading... </strong>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer" style="        padding: 5px;">
                    <button type="button" class="btn btn-danger btn-sm closeComment">Close</button>
                </div>
            </div>
        </div>

        <input type="hidden" clientidmode="Static" id="postIdForComments" runat="server" />
    </div>
</asp:Content>
