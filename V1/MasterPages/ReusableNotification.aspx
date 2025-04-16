<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReusableNotification.aspx.cs" Inherits="V1_MasterPages_ReusableNotification" ValidateRequest="false" %>



<style>
    .removeStyle:hover {
        background: none;
        border: none;
    }

    .removeStyle {
        background: none;
        border: none;
    }

    #notificationDropdown {
        position: relative;
    }


        #notificationDropdown .fa-bell {
            font-size: 22px;
            color: #fff;
            position: absolute;
            margin-left: 50px;
        }

        #notificationDropdown .badge {
            position: absolute;
            top: 18px;
            right: -39px;
            background-color: red;
            color: white;
            font-size: 11px;
            font-weight: bold;
            padding: 3px 1px;
            border-radius: 80%;
            line-height: 1;
            min-width: 16px;
            text-align: center;
            box-shadow: 0px 0px 5px rgba(0, 0, 0, 0.3);
            transform: translate(50%, -50%);
        }

    .dropdown-menu.notification {
        background: #fff;
        border-radius: 10px;
        box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
        padding: 0;
        margin-top: 6px !important;
        border: none;
        font-family: Arial, sans-serif;
        width: 320px;
        max-height: 450px;
        overflow: hidden;
        position: absolute !important;
        right: 0;
        left: auto;
    }

        .dropdown-menu.notification .title {
            font-size: 16px;
            font-weight: bold;
            padding: 12px;
            background: #f5f5f5;
            border-bottom: 1px solid #ddd;
            text-align: center;
        }

    #notificationList {
        max-height: 350px;
        overflow-y: auto;
    }

        #notificationList::-webkit-scrollbar {
            width: 6px;
        }

        #notificationList::-webkit-scrollbar-track {
            background: #f1f1f1;
            border-radius: 10px;
        }

        #notificationList::-webkit-scrollbar-thumb {
            background: #b0b0b0;
            border-radius: 10px;
        }

            #notificationList::-webkit-scrollbar-thumb:hover {
                background: #999;
            }

    #notificationContainer {
        list-style: none;
        padding: 0;
    }

        #notificationContainer li {
            padding: 12px;
            border-bottom: 1px solid #eee;
            display: flex;
            align-items: center;
        }

            #notificationContainer li:hover {
                background: #f0f2f5;
                cursor: pointer;
            }

            #notificationContainer li:last-child {
                border-bottom: none;
            }

            #notificationContainer li img {
                width: 40px;
                height: 40px;
                border-radius: 50%;
                margin-right: 10px;
            }

    #loading, #noloading {
        font-size: 14px;
        color: #777;
        text-align: center;
        padding: 10px;
    }


    .summary {
        padding: 10px;
        text-align: center;
        background: #f5f5f5;
        border-top: 1px solid #ddd;
        border-radius: 0 0 10px 10px;
    }

        .summary a {
            color: #1877f2;
            font-weight: bold;
            text-decoration: none;
        }

            .summary a:hover {
                text-decoration: none;
            }

    .notification-user-img {
        width: 100px;
        height: 100px;
        border-radius: 50%;
        object-fit: cover;
    }

    .notification-link {
        text-decoration: none !important;
        display: flex;
        align-items: center;
    }

        .notification-link:hover {
            text-decoration: none !important;
            color: inherit; /* Prevent color change */
        }

    .notification-item.read .notification-text {
        color: gray;
    }

    .notification-item.unread .notification-text {
        font-weight: bold;
        color: black;
    }

    .notification-item.read {
        all: unset;
    }

    .notification-item.unread {
        font-weight: bold;
        color: black;
    }
</style>
<%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>--%>

<script>
    $(document).ready(function () {
        var page = 1;
        var loading = false;

        function loadMoreNotifications() {
            if (loading) return;
            loading = true;

            $('#loading').show();
            $('#noloading').hide();

            $.ajax({
                url: "/V1/MasterPages/ReusableNotification.aspx/GetMoreNotifications",
                type: "POST",
                data: JSON.stringify({ page: page }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d && response.d.trim() !== "") {
                        $("#notificationContainer").append(response.d);
                        page++;
                    } else {
                        $("#noloading").show();
                        console.log("No more notifications to load.");
                    }

                    $("#loading").hide();
                    loading = false;
                },
                error: function () {
                    console.error("Failed to load notifications.");
                    $("#loading").hide();
                    loading = false;
                }
            });
        }

        loadMoreNotifications();

        $("#notificationList").scroll(function () {
            var scrollHeight = $(this)[0].scrollHeight;
            var scrollTop = $(this).scrollTop();
            var elementHeight = $(this).height();

            if (scrollTop + elementHeight >= scrollHeight - 50) {
                loadMoreNotifications();
            }
        });


        $('#notificationList').on('click', '.notification-item', function () {

            var notificationId = $(this).find('.notificationClick').data('notification-id');
            var self = $(this);
            
            $.ajax({
                url: "/V1/MasterPages/ReusableNotification.aspx/MarkAsRead",
                type: "POST",
                data: JSON.stringify({ notificationId: notificationId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    console.log(res);
                    if (res.d === "Success") {

                    }
                },
                error: function () {
                    console.error('Failed to mark notification as read.');
                }
            });

            if ($(this).hasClass('unread')) {
                self.closest('.notification-item').removeClass('unread').addClass('read');
                var countElem = $('#notificationCounts');
                var count = parseInt(countElem.text()) || 0; // Get the current count (default to 0 if empty)
                if (count > 0) {
                    count -= 1;
                    countElem.text(count);
                    if (count == 0) {
                        $('.badge.badge-danger').hide();
                    }
                }
            }
        });

    });
</script>



<ul class="dropdown-menu hdropdown notification animated flipInX" style="width: 300px; max-height: 400px; overflow-y: auto;" id="notificationList">
    <li class="title">Notifications</li>
    <li id="notificationContainer" class="decreaseCount">
        <!-- Notifications will be dynamically loaded here -->
    </li>
    <li id="loading" style="display: none; text-align: center; padding: 10px; font-style: italic;">Loading...</li>
    <li id="noloading" style="display: none; text-align: center; padding: 10px; font-style: italic;">No notifications to load.</li>
    <li class="summary text-center">
        <a href="/V1/Notifications.aspx">See All Notifications</a>
    </li>
</ul>

