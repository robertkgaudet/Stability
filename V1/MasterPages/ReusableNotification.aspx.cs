using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class V1_MasterPages_ReusableNotification : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [System.Web.Services.WebMethod]
    public static string MarkAsRead(string notificationId)
    {
        var dc = new CrowdReliefDBDataContext();


        var notification = dc.Notifications.FirstOrDefault(n => n.NotificationId == Guid.Parse(notificationId));
        if (notification != null)
        {
            notification.IsRead = true;
            dc.SubmitChanges();
            return "Success";
        }
        return "false";
    }

    [System.Web.Services.WebMethod]
    public static string GetMoreNotifications(int page)
    {
		if(HttpContext.Current.User.Identity.IsAuthenticated)
		{ 
			int itemsPerPage = 5;
			var dc = new CrowdReliefDBDataContext();
			var notificationList = (from n in dc.Notifications
									join f in dc.FeatureTypes on n.FeatureTypeId equals f.FeatureTypeId
									where n.RecipientUserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
									orderby n.CreatedOn descending
									select new
									{
										n.NotificationId,
										n.Title,
										n.Description,
										NotificationRedirectUrl = f.RedirectURL + n.RedirectURLParameters,
										n.NotificationType,
										n.CreatedOn,
										n.CreatedBy,
										n.IsRead,
										n.SenderUserId,
										n.RecipientUserId,
										n.FeatureTypeId,
										n.PostToStream,
										n.OrganizationId    
									})
							.Skip((page - 1) * itemsPerPage)
							.Take(itemsPerPage)
							.ToList();

			var notificationHtml = new StringBuilder();
			foreach (var n in notificationList)
			{

				var dataId = Convert.ToString(n.NotificationId);
				if (!string.IsNullOrEmpty(dataId))
				{
					Guid photoIds = (n.SenderUserId != Guid.Empty)
									? dc.ProfilePhotos.Where(x => x.UserId == n.SenderUserId).OrderByDescending(x => x.CreatedOn).Select(x => x.PhotoId).FirstOrDefault() : Guid.Empty;

					string photoFilename = photoIds != null ? dc.Photos.Where(x => x.PhotoId == photoIds).Select(x => x.Filename).FirstOrDefault() : null;
					string image = string.IsNullOrEmpty(photoFilename) ? "/V1/Images/ProfilePhotos/avatar2.png" : photoFilename;

					string imageDirectory = HttpContext.Current.Server.MapPath("/V1/Images/ProfilePhotos/");
					string imageFilename = image;
					string imagePath = Path.Combine(imageDirectory, imageFilename);

					string finalImageSource = File.Exists(imagePath)
																? "/V1/Images/ProfilePhotos/" + imageFilename
																: "/V1/Images/ProfilePhotos/avatar2.png";
					string shortDescription = n.Description.Length > 30 ? n.Description.Substring(0, 30) + "..." : n.Description;
					string RedirectUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + n.NotificationRedirectUrl;

					if (!string.IsNullOrEmpty(finalImageSource))
					{
						notificationHtml.AppendFormat(@"
											<div class='{0} notification-item'>
												<li class='notificationClick' data-notification-id='{1}'>
													<a href='{2}' target='_blank' class='notification-link'>
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
									RedirectUrl,
									n.Title,
									shortDescription,
									GetTimeAgo(n.CreatedOn),
									finalImageSource
						 );
					}
				}
			}

			return notificationHtml.ToString();
		}
		else
		{ return string.Empty; }
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