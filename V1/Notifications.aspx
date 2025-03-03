<%@ Page Title="Notifications" Language="C#" EnableViewState="true" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Notifications.aspx.cs" Inherits="V1_Notifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
             <style>
        #header{
            margin-top:-500px !important;
        }
        body {
            overflow:unset;
            height: 0%;
        }
            </style>
<style>

    .notification-item{
        justify-content: center;
        align-items: center;
    }

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
            position: relative;
        }

        #notificationDropdown .badge {
            position: absolute;
            top: 18px; 
            right: 30px;
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

    .dropdown-menus.notifications {
        background: #fff;
        border-radius: 10px; 
        box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
        padding: 0;
        margin-top: 15px !important;
        border: none;
        font-family: Arial, sans-serif;
        width: 400px;
        max-height: 450px;
        overflow: hidden;
        position: absolute !important; 
        right: auto;
        left: 500px;
        top:-450px;
        justify-content: center;
        align-items: center;
    }

        .dropdown-menus.notifications .title {
            font-size: 16px;
            font-weight: bold;
            padding: 12px;
            background: #f5f5f5;
            border-bottom: 1px solid #ddd;
            text-align: center;
        }

    #notificationLists {
        max-height: 500px;
        overflow-y: auto;
    }

        #notificationLists::-webkit-scrollbar {
            width: 6px;
        }

        #notificationLists::-webkit-scrollbar-track {
            background: #f1f1f1;
            border-radius: 10px;
        }

        #notificationLists::-webkit-scrollbar-thumb {
            background: #b0b0b0;
            border-radius: 10px;
        }

            #notificationLists::-webkit-scrollbar-thumb:hover {
                background: #999;
            }

    #notificationContainers {
        list-style: none;
        padding: 0;
    }

        #notificationContainers li {
            padding: 12px;
            border-bottom: 1px solid #eee;
            display: flex;
            align-items: center;
        }

            #notificationContainers li:hover {
                background: #f0f2f5;
                cursor: pointer;
            }

            #notificationContainers li:last-child {
                border-bottom: none;
            }

            #notificationContainers li img {
                width: 40px;
                height: 40px;
                border-radius: 50%;
                margin-right: 10px;
            }

    #loadings, #noloadings {
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
        /
    }


    .notification-link {
        text-decoration: none !important;
        display: flex;
        align-items: center;
    }

        .notification-link:hover {
            text-decoration: none !important;
            color: inherit;
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
        var loadings = false;

        function loadMoreNotifications() {
            if (loadings) return;
            loadings = true;

            $('#loadings').show();
            $('#noloadings').hide(); 

            $.ajax({
                url: "/V1/Notifications.aspx/GetMoreNotifications",
                type: "POST",
                data: JSON.stringify({ page: page }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d && response.d.trim() !== "") {
                        $("#notificationContainers").append(response.d); 
                        page++; 
                    } else {
                        $("#noloadings").show(); 
                        console.log("No more notifications to load.");
                    }

                    $("#loadings").hide();
                    loadings = false;
                },
                error: function () {
                    console.error("Failed to load notifications.");
                    $("#loadings").hide();
                    loadings = false;
                }
            });
        }

        loadMoreNotifications();

        $("#notificationLists").scroll(function () {
            var scrollHeight = $(this)[0].scrollHeight;
            var scrollTop = $(this).scrollTop();
            var elementHeight = $(this).height();

            if (scrollTop + elementHeight >= scrollHeight - 50) {
                loadMoreNotifications();
            }
        });

        $('#notificationContainers').on('click', '.notification-item', function () {
            var notificationId = $(this).find('.notificationClick').data('notification-id');
            $.ajax({
                url: "/V1/Notifications.aspx/MarkAsRead",
                type: "POST",
                data: JSON.stringify({ notificationId: notificationId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    $(this).closest('li').removeClass('unread').addClass('read');
                },
                error: function () {
                    console.error('Failed to mark notification as read.');
                }
            });
        });
    });
</script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



            <ul class="dropdown-menus hdropdown notifications animated flipInX"  id="notificationLists">
                <li class="title">Notifications</li>
                <li id="notificationContainers">
                    <!-- Notifications will be dynamically loaded here -->
                </li>
                <li id="loadings" style="display: none; text-align: center; padding: 10px; font-style: italic;">Loading...</li>
                <li id="noloadings" style="display: none; text-align: center; padding: 10px; font-style: italic;">No notifications to load.</li>
                <li class="summary text-center">
                    <a href="/V1/MasterPages/ReusableNotification.aspx">See All Notifications</a>
                </li>
            </ul>	
	
  
</asp:Content>


