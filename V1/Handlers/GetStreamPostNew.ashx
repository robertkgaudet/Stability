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
using System.Text.RegularExpressions;

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
                             p.EventId,
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
                    imageTag = profilePhotoFolder + profileImage.FilenameCropped;
                }

                var hypPortalLink = "";
                if (post.EventId != null)
                {
                    var sEvent = dc.Events.FirstOrDefault(f => f.EventId == post.EventId);
                    if (sEvent != null)
                    {
                        hypPortalLink = sEvent.Name;
                    }
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


                var showReaction = "";
                var BeStrong = "<span class='m-r-n-xs' title='Be Strong'>&#128074;</span>";
                var Wow = "<span class='m-r-n-xs' title='Wow'>&#128558;</span>";
                var Love = "<span class='m-r-n-xs text-danger' title='Love'>&#10084;</span>";
                var Bump = "<span class='m-r-n-xs' title='Bump'>&#128171;</span>";
                var Like = "<span class='m-r-n-xs' title='Like'>&#128591;</span>";

                var thanksCount = postReaction.Count(f => f.ReactionTypeId == new Guid("463be049-a178-4327-948c-eb3e3e7dce73"));
                var loveCount = postReaction.Count(f => f.ReactionTypeId == new Guid("b247efe7-3da7-44fa-9452-a331f71d337f"));
                var bumpCount = postReaction.Count(f => f.ReactionTypeId == new Guid("8fe324d4-3694-4b7d-b710-df277c74b1c4"));
                var beStrongCount = postReaction.Count(f => f.ReactionTypeId == new Guid("6528bbd7-501b-475b-a15f-520bb0a3ffbf"));
                var wowCount = postReaction.Count(f => f.ReactionTypeId == new Guid("43142e57-f55b-4c8d-b024-84e0e5c664e9"));

                var reactionCounts = new[]
                {
                new { Reaction = "Like", Count = thanksCount },
                new { Reaction = "Love", Count = loveCount },
                new { Reaction = "Bump", Count = bumpCount },
                new { Reaction = "Be Strong", Count = beStrongCount },
                new { Reaction = "Wow", Count = wowCount }
                };
                var top3Counts = reactionCounts.Where(f => f.Count > 0).OrderByDescending(r => r.Count).Take(3).ToList();

                // Display the top 3 reactions
                foreach (var reaction in top3Counts)
                {
                    if (reaction.Reaction == "Like")
                    {
                        showReaction += Like;
                    }
                    else if (reaction.Reaction == "Love")
                    {
                        showReaction += Love;
                    }
                    else if (reaction.Reaction == "Be Strong")
                    {
                        showReaction += BeStrong;
                    }
                    else if (reaction.Reaction == "Wow")
                    {
                        showReaction += Wow;
                    }
                    else if (reaction.Reaction == "Bump")
                    {
                        showReaction += Bump;
                    }
                }


                if (postReaction.FirstOrDefault(f => f.CreatedBy == new Guid(userId)) != null)
                {
                    if ((postReaction.Count() - 1) > 0)
                    {
                        litReactionCount = "<div>" + showReaction + "<span> &nbsp; You + " + (postReaction.Count() - 1).ToString() + "</span></div>";
                    }
                    else
                    {
                        litReactionCount = "<div>" + showReaction + "<span> &nbsp; You </span></div>";
                    }
                }
                else
                {
                    if ((postReaction.Count() - 1) > 0)
                    {
                        litReactionCount = "<div>" + showReaction + "<span> &nbsp; " + (postReaction.Count() - 1).ToString() + "</span></div>";
                    }
                    else
                    {
                        litReactionCount = "<div>" + showReaction + "</div>";
                    }
                }

                if (reactionTypeID == new Guid("463be049-a178-4327-948c-eb3e3e7dce73"))
                {
                    litReactionTitle = "<span style='color: #286090'> &#128591; Like </span>";
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
                    litReactionTitle = "<span style='color: #777'> &#128077; Like </span>";
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
                //results += "<div class='hpanel messageBody'><div class='panel-body'><div class='message'><div class='block-profile-image-div clearfix' style='line-height: 1.3;'>" +
                //        "<img src=" + imageTag + " id='ContentPlaceHolder1_rptPosts_imgProfile_49' class='img-rounded' style='margin-right: 10px;' width='40'>" +
                //        "<a id='ContentPlaceHolder1_rptPosts_hypCreatedBy_42' class='StreamLink' href='/V1/Member/Default.aspx?userid=" + post.UserId + "'>" + post.fullname + "</a><br>" +
                //        "<a id='hypPortalLink' class='StreamPortalLink'>" + hypPortalLink + "</a>" +
                //        "<span id='ContentPlaceHolder1_rptPosts_lblMessageDate_42' class='message-date'>" + CrowdRelief.Tools.GetElapsedTime(Convert.ToDateTime(post.CreatedOn)) + "</span></div>" +
                //        "<span class='message-content'><p style='margin-top: 10px;'>" + postHtml + "</p></span></div></div><div class='panel-footer'>" +
                //        "<div class='row' style='margin: -5px 5px -18px 5px'><span data-item-rid='" + post.PostId + "'>" + litReactionCount + "</span><span style='float: right; margin-top: -23px'>" +
                //        "<div class='post-type-div commentSection' data-item-id='" + post.PostId + "'>" + postCount + " comments</div></span></div><hr />" +
                //        "<div class='row' style='margin-top: -18px !important'><div class='col-xs-3 post-type-div thankButton text-muted' data-item-cid='" + post.PostId + "'>" + litReactionTitle +
                //        "</div><div class='col-xs-3 post-type-div commentSection' data-item-id='" + post.PostId + "'><i class='fa fa-sticky-note m-r-sm nowrap'></i>Comment</div></div>";
                //+
                //"<div class='col-xs-3 post-type-div' id='helpButton'>" +
                //"<i class='fa fa-users m-r-sm'></i>Help</div><div class='col-xs-3 post-type-div' id='giveButton'><i class='fa fa-money m-r-sm'></i>Give</div></div> " +
                //"";

                var postComments = (from pc in dc.PostComments
                                    join c in dc.Comments on pc.CommentId equals c.CommentId
                                    where pc.PostId == post.PostId
                                          && c.ParentId == null
                                          && c.IsDeleted == false
                                    orderby c.CreatedOn descending
                                    select new
                                    {
                                        Comment1 = ReplaceTaggedUsersWithLinks(c.Comment1),
                                        timeAgo = GetTimeAgo(c.CreatedOn),
                                        author = dc.Profiles.FirstOrDefault(f => f.UserId == c.CreatedBy).Firstname,
                                        ProfileUrl = "/V1/Member/Default.aspx?userid=" + c.CreatedBy,
                                        ImgProfileUrl = profilePhotoFolder + (
                                           (from rph in dc.ProfilePhotos
                                            join rp in dc.Photos on rph.PhotoId equals rp.PhotoId
                                            where rph.UserId == c.CreatedBy
                                            orderby rp.CreatedOn descending
                                            select rp.FilenameCropped).FirstOrDefault() ?? "profilepicture.png")
                                    }).Take(2).ToList();

                //if (postComments.Count > 0)
                //{
                //    results += "<div id='commentSectionShow' class='comment-section-show'><div class='comment-section-repeater-show' data-item-id='" + post.PostId + "-showComments>";

                //    foreach (var item in postComments)
                //    {
                //        //results += "<ul class='comments'><li><div class='userImage'><a target='_blank' href='" + item.ProfileUrl + "'>" +
                //        //    "<img class='img-rounded' src='" + item.ImgProfileUrl + "'/></a></div><div class='commentReact'>" +
                //        //    "<span style='font-weight: bold; width: 70%'>" +
                //        //    "<a target='_blank' href='" + item.ProfileUrl + "' class='author-link'>" + item.author + "</a>" +
                //        //    "</span><span style='float: right; width: 12%; text-align: right; margin: 0px 5px 0px 0px;'>" + item.timeAgo + "</span>" +
                //        //    "<span>" + item.Comment1 + "</span></div></li></ul>";

                //        results += "<ul class='comments'><li><div class='userImage'>" +
                //                "<a target='_blank' href=\"" + item.ProfileUrl + "'><img class='img-rounded'" +
                //                "src=" + item.ImgProfileUrl + "/></a></div><div class='commentReact'>" +
                //                "<span style='font-weight: bold; width: 70%'>\n<a target='_blank' href='" + item.ProfileUrl + "'" +
                //                "class='author-link'>" + item.author + "</a></span><span style='float: right; width: 12%; " +
                //                "text-align: right; margin: 0px 5px 0px 0px;'>" + item.timeAgo + "</span><span>" + item.Comment1 + "</span></div></li></ul>";
                //    }
                //    results += "</div></div>";
                //}
                //results += "</div></div>";


                string htmlContent = "";
                htmlContent += "<div class=\"hpanel messageBody\">";
                htmlContent += "    <div class=\"panel-body\">";
                htmlContent += "        <div class=\"message\">";
                htmlContent += "            <div class=\"block-profile-image-div clearfix\" style=\"line-height: 1.3;\">";
                htmlContent += "                <a id=\"ContentPlaceHolder1_rptPosts_linkProfile_9\" style=\"float: left; margin-right: 10px; display: none;\">";
                htmlContent += "                    <img src=" + imageTag + " id=\"ContentPlaceHolder1_rptPosts_imgProfile_9\" class=\"img-rounded\" width=\"40\">";
                htmlContent += "                </a>";
                htmlContent += "                <a id=\"ContentPlaceHolder1_rptPosts_ucUserNameWithBadges_9_hypName_9\" class=\"StreamLink\" href=\"/V1/Member/Default.aspx?userId=" + post.UserId + "><span id=\"ContentPlaceHolder1_rptPosts_ucUserNameWithBadges_9_lblprofileusername_9\" class=\"user-name\">" + post.fullname + "</span></a>";
                htmlContent += "                <a id=\"ContentPlaceHolder1_rptPosts_hypPortalLink_9\" class=\"StreamPortalLink\">" + hypPortalLink + "</a>";
                htmlContent += "                <br>";
                htmlContent += "                <span id=\"ContentPlaceHolder1_rptPosts_lblMessageDate_9\" class=\"message-date\">" + CrowdRelief.Tools.GetElapsedTime(Convert.ToDateTime(post.CreatedOn)) + "</span>";
                htmlContent += "            </div>";
                htmlContent += "            <span class=\"message-content\">";
                htmlContent += "                <p style=\"margin-top: 10px;\">";
                htmlContent += "                </p><div class=\"post\"><p>" + postHtml + "</p></div>";
                htmlContent += "                <p></p>";
                htmlContent += "            </span>";
                htmlContent += "        </div>";
                htmlContent += "    </div>";
                htmlContent += "    <div class=\"panel-footer\">";
                htmlContent += "        <div class=\"row\" style=\"margin: -5px 5px -18px 5px\">";
                htmlContent += "            <span data-item-rid=" + post.PostId + ">";
                htmlContent += "                <div>" + litReactionCount + "</div>";
                htmlContent += "            </span>";
                htmlContent += "            <span style=\"float: right; margin-top: -23px\">";
                htmlContent += "                <div class=\"post-type-div commentSection\" data-item-id=" + post.PostId + ">";
                htmlContent += "                    " + postCount + " Comments";
                htmlContent += "                </div>";
                htmlContent += "            </span>";
                htmlContent += "        </div>";
                htmlContent += "        <hr>";
                htmlContent += "        <div class=\"row\" style=\"margin-top: -18px !important\">";
                htmlContent += "            <div class=\"col-xs-3 post-type-div thankButton text-muted\" data-item-cid=" + post.PostId + ">";
                htmlContent += "                " + litReactionTitle + "";
                htmlContent += "            </div>";
                htmlContent += "            <div class=\"col-xs-3 post-type-div commentSection\" data-item-id=" + post.PostId + "><i class=\"fa fa-sticky-note m-r-sm nowrap\"></i>Comment</div>";
                htmlContent += "        </div>";

                if (postComments.Count > 0)
                {
                    htmlContent += "        <div id=\"ContentPlaceHolder1_rptPosts_commentSectionShow_9\" class=\"comment-section-show\">";
                }
                else
                {
                    htmlContent += "        <div id=\"ContentPlaceHolder1_rptPosts_commentSectionShow_9\" class=\"comment-section-show\" style=\"display:none;\">";
                }
                htmlContent += "            <div class=\"comment-section-repeater-show\" data-item-id=" + post.PostId + "-showComments>";
                foreach (var item in postComments)
                {
                    htmlContent += "                <ul class=\"comments\">";
                    htmlContent += "                    <li>";
                    htmlContent += "                        <div class=\"userImage\">";
                    htmlContent += "                            <a target=\"_blank\" href=" + item.ProfileUrl + ">";
                    htmlContent += "                                <img class=\"img-rounded\" src=" + item.ImgProfileUrl + "></a>";
                    htmlContent += "                        </div>";
                    htmlContent += "                        <div class=\"commentReact\">";
                    htmlContent += "                            <span style=\"font-weight: bold; width: 70%; height: 22px;\">";
                    htmlContent += "                                <a id=\"ContentPlaceHolder1_rptPosts_rptPostCommentsShow_9_ucTeamLogo_0_hypName_0\" class=\"StreamLink\" href=" + item.ProfileUrl + "><span id=\"ContentPlaceHolder1_rptPosts_rptPostCommentsShow_9_ucTeamLogo_0_lblprofileusername_0\" class=\"user-name\">" + item.author + "</span></a>";
                    htmlContent += "                            </span>";
                    htmlContent += "                            <span style=\"float: right; width: 12%; text-align: right; margin: 0px 5px 0px 0px;\">" + item.timeAgo + "</span>";
                    htmlContent += "                            <span>" + item.Comment1 + "</span>";
                    htmlContent += "                        </div>";
                    htmlContent += "                    </li>";
                    htmlContent += "                </ul>";
                }
                htmlContent += "            </div>";
                htmlContent += "        </div>";
                htmlContent += "    </div>";
                htmlContent += "</div>";

                results = htmlContent;
            }
        }
        catch (Exception ex)
        {
            results = "<div class=\"error\"><h2>ERROR in GetStreamPostNew.aspx</h2>error:" + ex.Message + "</div>";
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write(results);
    }

    private static string ReplaceTaggedUsersWithLinks(string comment)
    {
        if (string.IsNullOrEmpty(comment)) return comment;

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var regex = new Regex(@"@([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\s([A-Za-z0-9]+(?:[-\s][A-Za-z0-9]+)*)\b");

        return regex.Replace(comment, match =>
        {
            var html = "";
            string fullName = match.Groups[1].Value.Trim();
            var words = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 2)
            {
                fullName = string.Join(" ", words.Take(2));
            }

            var user = dc.Profiles.FirstOrDefault(u => (u.Firstname + " " + u.Lastname).Trim().ToLower().Contains(fullName.ToLower()));

            if (user != null)
            {
                html = "<a target='_blank' href='/V1/Member/Default.aspx?userid=" + user.UserId + "'>" + "@@" + fullName + "</a>";
            }

            return user != null ? html : match.Value;
            //return user != null ? "<a target='_blank' href='/V1/Member/Default.aspx?userid=" + user.UserId + "'>@@" + fullName + "</a>" : match.Value;
        });
    }

    public static string GetTimeAgo(DateTime pastDate)
    {
        TimeSpan timeDifference = DateTime.Now - pastDate;
        double yearsDifference = timeDifference.TotalDays / 365.25;
        if (yearsDifference >= 1)
        {
            int years = (int)yearsDifference;
            return years + " y";
        }
        else if (timeDifference.TotalDays >= 1)
        {
            int days = (int)timeDifference.TotalDays;
            return days + " d";
        }
        else if (timeDifference.TotalHours >= 1)
        {
            int hours = (int)timeDifference.TotalHours;
            return hours + " h";
        }
        else if (timeDifference.TotalMinutes >= 1)
        {
            int minutes = (int)timeDifference.TotalMinutes;
            return minutes + " m";
        }
        else
        {
            return "Just Now";
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}