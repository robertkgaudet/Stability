using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Notifications : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [System.Web.Services.WebMethod]
    public static void MarkAsRead(string notificationId)
    {
        var dc = new CrowdReliefDBDataContext();

        var notification = dc.Notifications.FirstOrDefault(n => n.NotificationId == Guid.Parse(notificationId));
        if (notification != null)
        {
            notification.IsRead = true;
            dc.SubmitChanges();
        }
    }

    [System.Web.Services.WebMethod]
    public static string GetMoreNotifications(int page)
    {
        int itemsPerPage = 5;
        var dc = new CrowdReliefDBDataContext();
        var notificationList = dc.Notifications
            .OrderByDescending(n => n.CreatedOn)
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToList();

        notificationList.ForEach(n =>
        {
            if (n.Description.Length > 50)
            {
                n.Description = n.Description.Substring(0, 50) + "...";
            }
        });

        var notificationHtml = new StringBuilder();
        foreach (var n in notificationList)
        {
            var dataId = Convert.ToString(n.NotificationId);
            if (!string.IsNullOrEmpty(dataId))
            {
                var photoIds = (n.SenderUserId != Guid.Empty)
                                ? dc.ProfilePhotos.Where(x => x.UserId == n.SenderUserId).OrderByDescending(x => x.CreatedOn).Select(x => x.PhotoId).ToList() : new List<Guid>();

                if (photoIds != null && photoIds.Count > 1)
                {
                    var SinglePhotoId = photoIds[0];
                    var photoFilename = !string.IsNullOrEmpty(SinglePhotoId.ToString() ) ? dc.Photos.Where(x => x.PhotoId == SinglePhotoId).Select(x => x.Filename).ToList() : new List<string>();
                    var newPhoto = photoFilename[0];
                    var image = string.IsNullOrEmpty(newPhoto) ? "https://www.w3schools.com/w3images/avatar2.png" : newPhoto;

                    string imageDirectory = HttpContext.Current.Server.MapPath("/V1/Images/ProfilePhotos/");
                    string imageFilename = image;
                    string imagePath = Path.Combine(imageDirectory, imageFilename);

                    var finalImageSource = File.Exists(imagePath)
                                                                ? "/V1/Images/ProfilePhotos/" + imageFilename
                                                                : "https://www.w3schools.com/w3images/avatar2.png";

                    if (!string.IsNullOrEmpty(finalImageSource))
                    {
                        notificationHtml.AppendFormat(@"
										<div class='{0} notification-item'>
											<li class='notificationClick' data-notification-id='{1}'>
												<a href='{2}' class='notification-link'>
													<img src='{6}' class='notification-user-img' alt='logo' />
													<div class='notification-text'>
														{3}<br />
														{4}<br />
														<small class='text-muted'>{5}</small>
													</div>
												</a>
											</li>
										</div>",
                                    n.IsRead ? "read" : "unread",
                                    n.NotificationId,
                                    n.RedirectURLParameters,
                                    n.Title,
                                    n.Description,
                                    GetTimeAgo(n.CreatedOn),
                                    finalImageSource
                         );
                    }
                }
            }
        }

        return notificationHtml.ToString();
    }

    private static string GetTimeAgo(DateTime createdOn)
    {
        var timeDifference = DateTime.Now - createdOn;

        if (timeDifference.TotalMinutes <= 1)
            return string.Format("{0} minute ago", (int)timeDifference.TotalMinutes);
        if (timeDifference.TotalMinutes < 60)
            return string.Format("{0} minutes ago", (int)timeDifference.TotalMinutes);
        if (timeDifference.TotalHours <= 1)
            return string.Format("{0} hour ago", (int)timeDifference.TotalHours);
        if (timeDifference.TotalHours < 24)
            return string.Format("{0} hours ago", (int)timeDifference.TotalHours);
        if (timeDifference.TotalDays <= 1)
            return string.Format("{0} day ago", (int)timeDifference.TotalDays);
        if (timeDifference.TotalDays < 7)
            return string.Format("{0} days ago", (int)timeDifference.TotalDays);

        return string.Format("{0} weeks ago", (int)(timeDifference.TotalDays / 7));
    }


}