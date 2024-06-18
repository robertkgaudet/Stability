using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Business_Business : System.Web.UI.Page
{
	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	public string donateLink = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
        if (String.IsNullOrEmpty(Request.QueryString["businessId"]))
		{
			Response.Write("No business Id provided.");
			Response.End();
        }

        string businessId = Request.QueryString["businessId"];


		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var peopleList =	from uo in dc.BusinessUsers
							join p in dc.Profiles on uo.UserId equals p.UserId
							where uo.BusinessId == new Guid(businessId)
							orderby p.Title descending
							select new { p.Firstname, p.Lastname, p.UserId, p.Title, p.ZelloName };

		rpNonProfitPeople.DataSource = peopleList;
		rpNonProfitPeople.DataBind();

		var business = (from o in dc.Businesses
						   where o.BusinessId == new Guid(businessId)
						   select o).SingleOrDefault();

		if(business != null)
        {
            if (User.IsInRole("Administrator"))
            {
                divUnpublishedInformation.Visible = true;
                divUploadLogoCover.Visible = true;
                dtEIN.Visible = true;
                ddEIN.Visible = true;

                if (!String.IsNullOrEmpty(Request.QueryString["ownerId"]))
                {
                    //Set the new ownerId
                    business.OwnerId = new Guid(Request.QueryString["ownerId"]);
                    dc.SubmitChanges();
                    litAlertMessage.Text = "A new non-profit page owner has been set.";
                    divAlertMessage.Visible = true;
                }
            }

            Master.PageTitle			= business.Name + " - CrowdRelief";
			Master.PageDescription		= business.Description;
			Master.FbDescription		= business.Description;
			//Master.FbImage				= business.CoverImage;
			Master.FbImageType			= "image/jpg";
			Master.FbSite_name			= business.Name + " - CrowdRelief";
			Master.FbURL				= Request.Url.AbsoluteUri;

			litBusinessName.Text		= business.Name;
			lblOrgName.Text				= business.Name;
			litMission.Text				= business.PurposeMission;
			litDescription.Text			= business.Description;
			litYearFounded.Text			= business.YearFounded;
			hypAddress.Text				= business.Address + "<br/>" + business.City + ", " + business.State + " " + business.Zip;
			hypAddress.NavigateUrl		= "http://maps.google.com/maps?q=" + business.Address.Replace(" ","+") + "," + business.City.Replace(" ","+") + "," + business.State.Replace(" ","+") + "," + business.Zip;			
			
			lblPointOfContactPerson.Text = business.PointOfContactName;
			if(!String.IsNullOrEmpty(business.PointOfContactPhoneNumber))
			{
				hypPointOfContactPhone.Text				= Regex.Replace(business.PointOfContactPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hypPointOfContactPhone.NavigateUrl			= "tel:" + business.PointOfContactPhoneNumber;
				hypPointOfContactPhone.Font.Underline		= true;
			}
			
			if(!String.IsNullOrEmpty(business.PointOfContactEmail))
			{
				hypPointOfContactEmail.Text				= business.PointOfContactEmail;
				hypPointOfContactEmail.NavigateUrl		= "mailto:" + business.PointOfContactEmail;
				hypPointOfContactEmail.Font.Underline	= true;
			}
			
			
			if(!String.IsNullOrEmpty(business.FacebookURL))
			{
				hypFacebookPage.Text			= business.Name + " Facebook Page";
				hypFacebookPage.NavigateUrl		= business.FacebookURL;
				hypFacebookPage.Font.Underline	= true;
			}

			if(!String.IsNullOrEmpty(business.FacebookGroupURL))
			{
				hypFacebookGroup.Text			= business.Name + " Facebook Group";
				hypFacebookGroup.NavigateUrl	= business.FacebookGroupURL;
				hypFacebookGroup.Font.Underline	= true;
			}
			
			if(!String.IsNullOrEmpty(business.TwitterURL))
			{
				hypTwitter.Text					= "Visit " + business.TwitterURL;
				hypTwitter.NavigateUrl			= "https://www.Twitter.com/" + business.TwitterURL;
				hypTwitter.Font.Underline		= true;
			}
			
			if(!String.IsNullOrEmpty(business.InstagramURL))
			{
				hypInstagram.Text				= "Instagram";
				hypInstagram.NavigateUrl		= "https://www.Instagram.com/" + business.InstagramURL;
				hypInstagram.Font.Underline		= true;
			}
			
			if(!String.IsNullOrEmpty(business.YouTubeURL))
			{
				hypYouTube.Text					= business.Name + " YouTube Channel";
				hypYouTube.NavigateUrl			= business.YouTubeURL;
				hypYouTube.Font.Underline		= true;
			}
			

			
			






			if(!String.IsNullOrEmpty(business.EIN))
			{
				lblEIN.Text		= business.EIN;
				dtEIN.Visible	= true;
				ddEIN.Visible	= true;
			}
			
			if(!String.IsNullOrEmpty(business.PrimaryPhone))
			{
				dtPrimaryPhone.Visible				= true;
				ddPrimaryPhone.Visible				= true;
				hypPrimaryPhone.Text				= Regex.Replace(business.PrimaryPhone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hypPrimaryPhone.NavigateUrl			= "tel:" + business.PrimaryPhone;
				hypPrimaryPhone.Font.Underline		= true;
			}
			
			if(!String.IsNullOrEmpty(business.SecondaryPhone))
			{
				dtSecondaryPhone.Visible				= true;
				ddSecondaryPhone.Visible			= true;
				hypSecondaryPhone.Text				= Regex.Replace(business.SecondaryPhone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hypSecondaryPhone.NavigateUrl		= "tel:" + business.SecondaryPhone;
				hypSecondaryPhone.Font.Underline	= true;
			}


			if(!String.IsNullOrEmpty(business.PublicPhoneNumber))
			{
				ddPublicPhone.Visible				= true;
				hyoPublicPhoneNumber.Text			= Regex.Replace(business.PublicPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
				hyoPublicPhoneNumber.NavigateUrl	= "tel:" + business.PublicPhoneNumber;
				hyoPublicPhoneNumber.Font.Underline = true;
			}
			if(!String.IsNullOrEmpty(business.PublicEmail))
			{
				ddPublicEmail.Visible					= true;
				hypPublicEmailAddress.Text				= business.PublicEmail;
				hypPublicEmailAddress.NavigateUrl		= "mailto:" + business.PublicEmail;
				hypPublicEmailAddress.Font.Underline	= true;
			}
			if(!String.IsNullOrEmpty(business.Website))
			{
				ddWebsite.Visible			= true;
				hypWebsite.Text				= business.Website;
				hypWebsite.NavigateUrl		= business.Website;
				hypWebsite.Font.Underline	= true;
			}
			
			if(!String.IsNullOrEmpty(business.BlogURL))
			{
				ddBlog.Visible			= true;
				hypBlog.Text			= business.BlogURL;
				hypBlog.NavigateUrl		= business.BlogURL;
				hypBlog.Font.Underline	= true;
			}
			
			if(!String.IsNullOrEmpty(business.DonationURL))
			{
				lbDonate.Visible = true;
				donateLink = business.DonationURL;
			}
		}
	}
    
    protected void rpNonProfitPeople_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
        //List disasters
        //Show their photo
        //Show their skills

        
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
			String firstname	= (String)DataBinder.Eval(dataItem.DataItem, "Firstname");
			String lastname		= (String)DataBinder.Eval(dataItem.DataItem, "Lastname");
			String zelloName		= (String)DataBinder.Eval(dataItem.DataItem, "ZelloName");
			String title	= (String)DataBinder.Eval(dataItem.DataItem, "Title");

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var profilePhoto = (from p in dc.ProfilePhotos
							   join ph in dc.Photos on p.PhotoId equals ph.PhotoId
							   where p.UserId == userId && p.IsCurrrent == true
							   select new {ph.FilenameCropped }).Take(1).SingleOrDefault();
            
            Literal lblInfo = (Literal)e.Item.FindControl("lblInfo");
			Image imgProfilePhoto = (Image)e.Item.FindControl("imgProfilePhoto");
            HyperLink hypMakeOwner = (HyperLink)e.Item.FindControl("hypMakeOwner");

            hypMakeOwner.NavigateUrl = "NonProfit.aspx?businessId=" + Request.QueryString["businessId"] +"&ownerId=" + userId.ToString();
            hypMakeOwner.Text = "Set '" + firstname + "' As Owner";
            if (User.IsInRole("Administrator"))
            {
                hypMakeOwner.Visible = true;
            }

                imgProfilePhoto.ImageUrl = "/V1/Images/icons8-customer-64.png";
			if(profilePhoto != null)
			{
				imgProfilePhoto.ImageUrl = profilePhotoFolder + profilePhoto.FilenameCropped;
			}

			zelloName = String.IsNullOrEmpty(zelloName) ? "none" : zelloName;
			title = String.IsNullOrEmpty(title) ? "none" : title;
				
			lblInfo.Text = "<p><dl class=\"dl-horizontal\" class=\"m-l-sm\"><dd><b><a href=\"/V1/Profile/Profile.aspx?userId=" + userId.ToString() + "\"  style=\"text-decoration:underline;\">" + firstname + " "+  lastname + "</a></b><br>Zello: " + zelloName + "<br>Title: " + title + "</dd></dl></p>";
		}
	}
}