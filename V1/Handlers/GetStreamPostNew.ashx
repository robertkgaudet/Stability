<%@ WebHandler Language="C#" Class="GetStreamPostNew" %>

using System;
using System.Web;
using System.Linq;
using System.Web.Services;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
//using System.Text.RegularExpressions;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class GetStreamPostNew : IHttpHandler, IReadOnlySessionState
{
    public string recoveryStageId = ConfigurationManager.AppSettings["recoveryStageId"].ToString();
    public string rebuildProgressSliderId = ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
    public string overallProgressSliderId = ConfigurationManager.AppSettings["overallProgressSliderId"].ToString();
    public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();

    public void ProcessRequest(HttpContext context)
    {
        string results = string.Empty;
        int pageNumber = int.Parse(context.Request.QueryString["pageNumber"]);
        int streamPostPageSize = int.Parse(ConfigurationManager.AppSettings["streamPostPageSize"].ToString());
        string eventId = (string)context.Request.QueryString["eventId"];
        try
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            var posts = (from p in dc.Posts
                         join pr in dc.Profiles on p.CreatedBy equals pr.UserId
                         where p.IsVisible == true
                         orderby p.CreatedOn descending
                         select new
                         {
                             p.CreatedBy,
                             p.PostId,
                             p.PostTypeId,
                             p.URLImage,
                             p.URLTitle,
                             p.URLDescription,
                             p.SharedURL,
                             p.CreatedOn,
                             p.Message,
                             pr.UserId,
                             fullname = pr.Firstname + " " + pr.Lastname,
                         }).Skip((streamPostPageSize + 10) + (streamPostPageSize * pageNumber)).Take(streamPostPageSize);
            foreach (var post in posts)
            {
                string username = HttpContext.Current.User.Identity.Name;
                string userId = (string)context.Request.QueryString["userId"];
                if (!string.IsNullOrEmpty(username))
                {
                    userId = dc.aspnet_Users.FirstOrDefault(f => f.UserName == username).UserId.ToString();
                }
                var imageTag = "";

                var profileImage = (from ph in dc.ProfilePhotos
                                    join p in dc.Photos on ph.PhotoId equals p.PhotoId
                                    where ph.UserId == post.CreatedBy && ph.IsCurrrent == true
                                    orderby p.CreatedOn descending
                                    select new { p.FilenameCropped }).Take(1).SingleOrDefault();

                if (profileImage != null)
                {
                    //Get the users profile image
                    imageTag = profilePhotoFolder + profileImage.FilenameCropped;
                }

                //if (post.ProfilePhoto != "")
                //{
                //    imageTag = "<img src=" + post.ProfilePhoto + " class='img-rounded' style='float: left; margin-right: 10px;' width='40'>";
                //}
                var litReactionCount = "";
                var litReactionTitle = "";
                var divSingleImage = "";
                bool URLShared = false;
                string postHtml = string.Empty;
                string URLLink = post.SharedURL;

                if (!String.IsNullOrEmpty(post.SharedURL))
                {
                    //A url was used.
                    //Make sure the URL is used to open the link.
                    postHtml = "<div class=\"post\"";
                    URLShared = true;

                    //Shorted the URL to the TLD to make the text easier to read.
                    //post.SharedURL = ExtractDomainAndTld(SharedURL);
                }
                else
                {
                    postHtml = "<div class=\"post\">";
                }

                postHtml += !String.IsNullOrEmpty(post.Message) ? "<p>" + post.Message + "</p>" : string.Empty;// If there's a message, show it.

                //
                if (URLShared)
                {
                    //Load the URL Preview DIV
                    if (!String.IsNullOrEmpty(post.URLImage))
                    {
                        postHtml += "<div onclick =window.open(" + URLLink + ", _blank) class= image-container URLPost><img class=responsive-image src=" + post.URLImage + " alt=Image></div>";
                    }

                    postHtml += "<div class=text-container URLPost><small class=text-muted>" + post.SharedURL + "</small></br>";
                    postHtml += "<b>" + post.URLTitle + "</b>";
                    postHtml += "<p>" + post.URLDescription + "</p></div>";
                }

                var postReaction = from pr in dc.PostReactions
                                   join p in dc.Profiles on pr.CreatedBy equals p.UserId
                                   where pr.PostId == post.PostId
                                   orderby pr.CreatedOn descending
                                   select new { pr.PostReactionId, pr.CreatedBy, p.Firstname, pr.ReactionTypeId };
                var postImages = from pi in dc.PostImages
                                 where pi.PostId == post.PostId
                                 select new { pi.ImageFilename, pi.PostImageId };
                Guid reactionTypeID = new Guid();

                if (postReaction != null && postReaction.Count() > 0)
                {
                    if (!string.IsNullOrEmpty(username))
                    {
                        var dataExist = postReaction.FirstOrDefault(f => f.CreatedBy == new Guid(userId));
                        if (dataExist != null)
                        {
                            reactionTypeID = (Guid)dataExist.ReactionTypeId;
                        }
                    }
                    if (postReaction.FirstOrDefault(f => f.CreatedBy == new Guid(userId)) != null)
                    {
                        litReactionCount = "You and " + (postReaction.Count() - 1).ToString() + " others";
                    }
                    else
                    {
                        litReactionCount = postReaction.FirstOrDefault().Firstname + " and " + (postReaction.Count() - 1).ToString() + " others";
                    }
                }

                if (reactionTypeID == new Guid("463be049-a178-4327-948c-eb3e3e7dce73"))
                {
                    litReactionTitle = "<span style='color: #286090'> &#128591; Thank </span>";
                }
                else if (reactionTypeID == new Guid("b247efe7-3da7-44fa-9452-a331f71d337f"))
                {
                    litReactionTitle = "<span style='color: #FF0000'> &#10084; Love </span>";
                }
                else if (reactionTypeID == new Guid("8fe324d4-3694-4b7d-b710-df277c74b1c4"))
                {
                    litReactionTitle = "<span style='color: #f0ad4e'> &#128171; Bump </span>";
                }
                else if (reactionTypeID == new Guid("6528bbd7-501b-475b-a15f-520bb0a3ffbf"))
                {
                    litReactionTitle = "<span style='color: #f0ad4e'> &#128074; Be Strong </span>";
                }
                else if (reactionTypeID == new Guid("43142e57-f55b-4c8d-b024-84e0e5c664e9"))
                {
                    litReactionTitle = "<span style='color: #eea236'> &#128558; Wow </span>";
                }
                else
                {
                    litReactionTitle = "<span style='color: #777'> &#128077; Thank </span>";
                }

                if (postImages != null && postImages.Count() > 0)
                {
                    foreach (var postImage in postImages)
                    {
                        Guid postImageId = postImage.PostImageId;
                        string imageFilename = postImage.ImageFilename;
                        if (postImages.Count() == 1)
                        {
                            divSingleImage = "<div class='single-thumbnail-container image-container-post'><img class='thumbnail' src='/V1/Images/PostImages/Thumbnails/" + imageFilename + "'></div>";
                        }
                        else
                        {
                            divSingleImage += "<div class='thumbnail-container'><img class='thumbnail' src='/V1/Images/PostImages/Thumbnails/" + imageFilename + "'></div>";
                        }
                    }
                }
                postHtml += divSingleImage;
                postHtml += "</div>";
                var postCount = dc.PostComments.Where(f => f.PostId == post.PostId).ToList().Count.ToString();
                results += "<div class='hpanel messageBody'><div class='panel-body'><div class='message'><div class='block-profile-image-div clearfix' style='line-height: 1.3;'>" +
                        "<img src=" + imageTag + " id='ContentPlaceHolder1_rptPosts_imgProfile_49' class='img-rounded' style='float: left; margin-right: 10px;' width='40'>" +
                        "<a id='ContentPlaceHolder1_rptPosts_hypCreatedBy_42' class='StreamLink' href='/V1/Profile/Profile.aspx?userId=" + post.UserId + "'>" + post.fullname + "</a><br>" +
                        "<span id='ContentPlaceHolder1_rptPosts_lblMessageDate_42' class='message-date'>" + CrowdRelief.Tools.GetElapsedTime(Convert.ToDateTime(post.CreatedOn)) + "</span></div>" +
                        "<span class='message-content'><p style='margin-top: 10px;'>" + postHtml + "</p></span></div></div><div class='panel-footer'>" +
                        "<div class='row' style='margin: -5px 5px -18px 5px'><span>" + litReactionCount + "</span><span style='float: right'>"+ postCount +" comments</span></div><hr />" +
                        "<div class='row'><div class='col-xs-3 post-type-div thankButton text-muted' data-item-id='" + post.PostId + "'>" + litReactionTitle +
                        "</div><div class='col-xs-3 post-type-div commentSection' data-item-id='"+ post.PostId +"'><i class='fa fa-sticky-note m-r-sm nowrap'></i>Comment</div><div class='col-xs-3 post-type-div' id='helpButton'>" +
                        "<i class='fa fa-users m-r-sm'></i>Help</div><div class='col-xs-3 post-type-div' id='giveButton'><i class='fa fa-money m-r-sm'></i>Give</div></div></div></div>";
            }
        }
        catch (Exception ex)
        {
            results = "<div class=\"error\"><h2>ERROR in GetStreamPostNew.aspx</h2>error:" + ex.Message + "</div>";
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write(results);
    }

    //public static string GetTimeAgo(DateTime pastDate)
    //{
    //    TimeSpan timeDifference = DateTime.Now - pastDate;
    //    double yearsDifference = timeDifference.TotalDays / 365.25;
    //    if (yearsDifference >= 1)
    //    {
    //        int years = (int)yearsDifference;
    //        return years + " y";
    //    }
    //    else if (timeDifference.TotalDays >= 1)
    //    {
    //        int days = (int)timeDifference.TotalDays;
    //        return days + " d";
    //    }
    //    else if (timeDifference.TotalHours >= 1)
    //    {
    //        int hours = (int)timeDifference.TotalHours;
    //        return hours + " h";
    //    }
    //    else if (timeDifference.TotalMinutes >= 1)
    //    {
    //        int minutes = (int)timeDifference.TotalMinutes;
    //        return minutes + " m";
    //    }
    //    else
    //    {
    //        return "Just Now";
    //    }
    //}

    //private static List<Profile> ExtractTaggedUser(string commentText, List<Profile> _users)
    //{
    //    List<Profile> taggedUsers = new List<Profile>();
    //    var regex = new Regex(@"@([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\s([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\b");
    //    foreach (Match match in regex.Matches(commentText))
    //    {
    //        var name = match.Groups[1].Value.Trim();
    //        var words = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    //        name = string.Join(" ", words.Take(2));
    //        var user = _users.FirstOrDefault(u => (u.Firstname + " " + u.Lastname).Trim().ToLower().Contains(name.ToLower()));
    //        if (user != null)
    //        {
    //            taggedUsers.Add(user);
    //        }
    //    }
    //    return taggedUsers;
    //}

    //private static string ExtractTaggedMessage(string commentText, List<Profile> _users)
    //{
    //    var regex = new Regex(@"@([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\s([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\b");
    //    var matches = regex.Matches(commentText);

    //    string processedComment = regex.Replace(commentText, match =>
    //    {
    //        var html = "";
    //        string fullName = match.Groups[1].Value.Trim();
    //        var words = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

    //        if (words.Length > 2)
    //        {
    //            fullName = string.Join(" ", words.Take(2));
    //        }

    //        var user = _users.FirstOrDefault(u => (u.Firstname + " " + u.Lastname).Trim().ToLower().Contains(fullName.ToLower()));
    //        if (user != null)
    //        {
    //            html = "<a target='_blank' href='/V1/Profile/Profile.aspx?userId=" + user.UserId + "'>" + user.Firstname + " " + user.Lastname + "</a>";
    //        }

    //        if (words.Length > 2)
    //        {
    //            html = html + " " + string.Join(" ", words.Skip(2));
    //        }

    //        return user != null ? html : match.Value;
    //    });
    //    return processedComment;
    //}

    //string ReplaceTaggedUsersWithLinks(string comment)
    //{
    //    if (string.IsNullOrEmpty(comment)) return comment;

    //    CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
    //    var regex = new Regex(@"@([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\s([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\b");

    //    return regex.Replace(comment, match =>
    //    {
    //        var html = "";
    //        string fullName = match.Groups[1].Value.Trim();
    //        var words = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    //        if (words.Length > 2)
    //        {
    //            fullName = string.Join(" ", words.Take(2));
    //        }

    //        var user = dc.Profiles.FirstOrDefault(u => (u.Firstname + " " + u.Lastname).Trim().ToLower().Contains(fullName.ToLower()));

    //        if (user != null)
    //        {
    //            html = "<a target='_blank' href='/V1/Profile/Profile.aspx?userId=" + user.UserId + "'>" + "@@" + fullName + "</a>";
    //        }

    //        return user != null ? html : match.Value;
    //        //return user != null ? "<a target='_blank' href='/V1/Profile/Profile.aspx?userId=" + user.UserId + "'>@@" + fullName + "</a>" : match.Value;
    //    });
    //}

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}