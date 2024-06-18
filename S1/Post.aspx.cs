using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Net.Mail;

public partial class S1_Post : BaseOrganizationWebForm
{
	public string successMessage = string.Empty;
	public string panelImage = string.Empty;
	public string panelImageSrc = string.Empty;
	public string pageFriendlyURL = string.Empty;
	public string articleAboutUserRole = "Survivor";
	public string survivorLinkList = string.Empty;
	public string disasterSurvivorStoryCategoryId	= ConfigurationManager.AppSettings["disasterSurvivorStoryCategoryId"].ToString();
	public string disasterHelperStoryCategoryId	= ConfigurationManager.AppSettings["disasterHelperStoryCategoryId"].ToString();

	public string totalDonations = string.Empty;
	public string donationList = string.Empty;
	public string ClientToken = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		Page.Form.DefaultButton = btnComment.UniqueID;
		string delete = Request.QueryString["d"];
	//	Master.BoxedBody = true;
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		if(!String.IsNullOrEmpty(delete))
		{
			Guid commentId = new Guid(Request.QueryString["commentId"]);
			//delete the comment.
			var comment = (from c in dc.Comments
						  where c.CommentId == commentId
						  select c).SingleOrDefault();

			comment.IsDeleted = true;
			dc.SubmitChanges();

			Response.Redirect("\\S1\\Default.aspx");
		}

		this.GenerateClientToken();

		if(IsPostBack)
        {
			string articleNumber = Request.QueryString["number"];

			if (!String.IsNullOrEmpty(articleNumber))
            {
				var article = (from a in dc.Articles
							   join u in dc.Profiles on a.CreatedBy equals u.UserId
							   where a.ArticleNumber == Int32.Parse(articleNumber)
							   && a.IsDeleted == false
							   select new { a, u }).SingleOrDefault();

				var storyUser = (from s in dc.Profiles
								 where s.UserId == article.a.UserId
								 select s).SingleOrDefault();

				UserDonation donation = this.ProcessPaymentNonce(dc, new Guid(Membership.GetUser().ProviderUserKey.ToString()), storyUser.UserId);

				if (donation != null)
                {
					var donationUser = (from s in dc.Profiles
										where s.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
										select s).SingleOrDefault();

					this.CreateRebuildPostForDonation(dc, donation);
					
					this.SendMessagesToDonorParties(donationUser.Firstname, donationUser.Lastname, Membership.GetUser().Email, storyUser.Firstname, storyUser.Lastname, Membership.GetUser(storyUser.UserId).Email, dc, donation);

					this.successMessage = "Thank you for your donation!";
					dc.UserDonations.InsertOnSubmit(donation);
					dc.SubmitChanges();
				}
				else
                {
					this.successMessage = "There was an error with your transaction.";
                }

				//donation panel
				litSurvivorNameDonate.Text = storyUser.Firstname + " " + storyUser.Lastname;
				litSurvivorNameDonate.Visible = true;

				var donationQuery = (from d in dc.UserDonations where d.RecipientId == storyUser.UserId orderby d.CreatedOn descending select d);

				float totalDonated = 0;
				System.Text.StringBuilder donationListSb = new System.Text.StringBuilder();

				foreach (var pastDonation in donationQuery)
				{
					totalDonated += pastDonation.Amount;

					var userQuery = (from p in dc.Profiles where p.UserId == pastDonation.DonorId select p);

					donationListSb.AppendLine(string.Format("{0} gave ${1}<br />", userQuery.FirstOrDefault().Firstname, Math.Round(pastDonation.Amount, 0).ToString()));
				}

				this.donationList = donationListSb.ToString();
				this.divWhoHasGiven.Visible = (totalDonated > 0);
			}


		}

		if (!IsPostBack)
		{ 
			string storyPhotoFolder		= System.Configuration.ConfigurationManager.AppSettings["storyPhotoFolder"].ToString();
			string articleNumber = Request.QueryString["number"];
			this.Master.HideCategoryList = true;

			if(!String.IsNullOrEmpty(articleNumber))
			{
				var article = (from a in dc.Articles
								join u in dc.Profiles on a.CreatedBy equals u.UserId
								where a.ArticleNumber == Int32.Parse(articleNumber)
								&& a.IsDeleted == false
								select new {a, u }).SingleOrDefault();
			
				if(article == null)
				{
					Response.Redirect("/stories");
				}


				var storyPhoto = (from sp in dc.ArticlePhotos
								 join p in dc.Photos on sp.PhotoId equals p.PhotoId
								 where sp.ArticleId == article.a.ArticleId
								 orderby sp.CreatedOn descending
								 select new {p.FilenameCropped }).Take(1).SingleOrDefault();

				
				
				Guid loggedInUserId = Guid.Empty;
				if(User.Identity.IsAuthenticated)
				{
					loggedInUserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
					
					if(User.IsInRole("Administrator") || article.u.UserId == loggedInUserId)
					{
						divArticleAdmin.Visible = true;
						hypAddPhoto.NavigateUrl = "/S1/Profile/BlogUploadPhoto.aspx?userId=" + article.u.UserId + "&articleId=" + article.a.ArticleId;
						hypEdit.NavigateUrl		= "/S1/Profile/EditBlogPost.aspx?userId=" + article.u.UserId + "&articleId=" + article.a.ArticleId;
						hypDelete.NavigateUrl	= "/S1/Default.aspx?d=t&userId=" + article.u.UserId + "&articleId=" + article.a.ArticleId;
					}
				}

				string storyTitle = article.a.Title;
				string articlePhoto = string.Empty;
				hidArticleId.Value = article.a.ArticleId.ToString();
				hidTitle.Value = article.a.Title.Replace(" ","-");

				if(storyPhoto != null)
				{
					panelImage = "panel-image";
					panelImageSrc = "<img class=\"img-responsive\" src=\"" + storyPhotoFolder + storyPhoto.FilenameCropped + "\" \\>";
					//storyTitle = string.Empty;
				}

				//BEGIN FACEBOOK META TAGS
				string facebookArticleImage = "/S1/Images/CrowdReliefFBPost.jpg";

				pageFriendlyURL = "http://www.Stability.org/a/" + article.a.ArticleNumber + "/" + article.a.Title.Replace(" ","-");

				Master.FbURL			= pageFriendlyURL;//Request.Url.AbsoluteUri;
				Master.FbDescription	= StripHTML(article.a.Text.Length > 501 ? article.a.Text.Substring(0, 500) + "..." : article.a.Text);
				Master.PageDescription	= StripHTML(article.a.Text.Length > 501 ? article.a.Text.Substring(0, 500) + "..." : article.a.Text);

				if(storyPhoto != null)
				{
					facebookArticleImage = storyPhotoFolder + storyPhoto.FilenameCropped;
				}

				Master.FbImage			= facebookArticleImage;
				Master.FbImageType		= "image/jpg";
				Master.FbSite_name		= "Stability Disaster Aid Platform";
				//END FACEBOOK META TAGS

				string articleDate = article.a.CreatedOn.ToString("MM/dd/yyyy h:mm tt");

				int? countInit = 0;
				if(article.a.ViewCount != null)
				{
					countInit = article.a.ViewCount;
				}
				article.a.ViewCount = countInit + 1;
				dc.SubmitChanges();

				int? viewCount = 0;
				if(article.a.ViewCount != null)
				{
					viewCount = (article.a.ViewCount + 248);
				}

				var commentCount = (from c in dc.ArticleComments
								   where c.ArticleId == article.a.ArticleId
								   select c).Count();
				
				litTitle.Text			= storyTitle;
				litViews.Text			= viewCount.ToString();
				litCommentCount.Text	= commentCount.ToString();
			
				var storyUser = (from s in dc.Profiles
							   where s.UserId == article.a.UserId
							   select s).SingleOrDefault();

				string headerSubTitle = string.Empty;
				string survivorInfo = string.Empty;
				if(storyUser != null)
				{
					if(article.a.CategoryId != null && new Guid(disasterSurvivorStoryCategoryId) == article.a.CategoryId)
					{
						//SURVIVOR
						articleAboutUserRole = "Survivor";
						dtRecoveryStage.Visible = true;
						litRecoveryStage.Visible = true;
						dtHousingTitle.Visible = true;
						litHousingSituation.Visible = true;
						dtQualifiersTitle.Visible = true;
						litQualifiers.Visible = true;

						
						var survivorHousing = (from sh in dc.UserHousings
											  join h in dc.Housings on sh.HousingId equals h.HousingId
											  where sh.UserId == article.a.UserId
											  orderby sh.CreatedOn descending
											  select new {h }).Take(1).SingleOrDefault();

						if(survivorHousing != null)
						{
							dtHousingTitle.Visible		= true;
							litHousingSituation.Visible = true;
							litHousingSituation.Text	= survivorHousing.h.Sentence + " " + survivorHousing.h.Housing1;
						}

						if(!String.IsNullOrEmpty(storyUser.AmazonWishListURL))
						{
							litHelpSurvivorMessage.Text = "<p>Help " + storyUser.Firstname + " " + storyUser.Lastname +  " by purchasing much needed items from their Amazon Wish List.</p>";

							hypAmazon.Text = "Donate From My Amazon Wish List";
							hypAmazon.NavigateUrl = storyUser.AmazonWishListURL;
						}
						else
						{
							hypAmazon.Visible = false;
						}
						
						
						var recoveryStage = (from ust in dc.UserSliderTicks
											join s in dc.SliderTicks on ust.SliderTickId equals s.SliderTickId
											where ust.SurvivorId == article.a.UserId
											orderby ust.CreatedOn descending
											select new {s.Lable }).Take(1).SingleOrDefault();

						if(recoveryStage != null)
						{
							litRecoveryStage.Visible = true;
							dtRecoveryStage.Visible = true;
							litRecoveryStage.Text = recoveryStage.Lable;
						}

						string qualifierList = string.Empty;

						var qualifiers = from rs in dc.RebuildStatus
										join uq in dc.UserQualifiers on rs.RebuildStatusId equals uq.QualifierId
										where rs.StatusType == 2 && uq.UserId == article.a.UserId
										orderby rs.Status
										select new { rs };

						if(qualifiers.Count() > 0)
						{
							foreach(var qualifier in qualifiers)
							{
								qualifierList += "<span class=\"btn btn-default m-xs btn-m\">" + qualifier.rs.Status + "</span>";
							}
							litQualifiers.Visible	= true;
							litQualifiers.Text		= qualifierList;
							dtQualifiersTitle.Visible	= true;
						}
						
						Master.PageTitle		= storyTitle + " - Stability Disaster Survivors Story";
						headerSubTitle = "This Stability Disaster Survivors Story was created for <a href=\"\\sp\\" +  storyUser.ProfileNumber + "\\" + storyUser.Firstname + "-" + storyUser.Lastname + "\"\">" + storyUser.Firstname + " " + storyUser.Lastname + "</a> by <a href=\"\\hp\\" +  article.u.ProfileNumber + "\\" + article.u.Firstname + "-" + article.u.Lastname + "\"\">" +  article.u.Firstname + " " + article.u.Lastname + "</a> who is helping them rebuild their lives. " + articleDate;
					}
					else if(article.a.CategoryId != null && new Guid(disasterHelperStoryCategoryId) == article.a.CategoryId)
					{
						//HELPER
						hypAmazon.Visible = false;
						LoadSurvivors(article.a.UserId);
						articleAboutUserRole = "Helper";
						dtRecoveryStage.Visible = false;
						litRecoveryStage.Visible = false;
						dtHousingTitle.Visible = false;
						litHousingSituation.Visible = false;
						dtQualifiersTitle.Visible = false;
						litQualifiers.Visible = false;
						
						Master.PageTitle		= storyTitle + " - Stability Disaster Volunteer Story";
						headerSubTitle = "This Stability Disaster Helpers Story was created for helper <a href=\"\\hp\\" +  storyUser.ProfileNumber + "\\" + storyUser.Firstname + "-" + storyUser.Lastname + "\"\">" + storyUser.Firstname + " " + storyUser.Lastname + "</a>. " + articleDate;
					}
					litHeaderSubTitle.Text = headerSubTitle;
					
					var survivorEvent = (from d in dc.Events
										 join ue in dc.UserEvents on d.EventId equals ue.EventId
										 where ue.UserId == article.a.UserId
										 select new {d.Name, d.URLFriendlyName}).Take(1).SingleOrDefault();

					if(survivorEvent != null)
					{
						dtDisasterTitle.Visible		= true;
						hypDisaster.Visible			= true;
						hypDisaster.Text			= survivorEvent.Name;
						hypDisaster.NavigateUrl		= "/Disaster/" + survivorEvent.URLFriendlyName;
						hypDisaster.Font.Underline	= true;
						//litDisasterName.Visible		= true;
						//litDisasterName.Text		= survivorEvent.Name + " Disaster " + articleAboutUserRole;

						//donation panel
						litDisasterNameDonate.Visible = true;
						litDisasterNameDonate.Text = survivorEvent.Name + " Disaster " + articleAboutUserRole;
					}


					divSurvivorInfo.Visible		= true;
					litText.Text				= article.a.Text;
					litText.Visible				= true;
					//litSurvivorName.Text		= storyUser.Firstname + " " + storyUser.Lastname;
					//litSurvivorName.Visible		= true;
					hypAllArticles.NavigateUrl	= "\\s\\" + storyUser.ProfileNumber + "\\" +  storyUser.Firstname + "-" + storyUser.Lastname;
					hypAllArticles.Visible		= true;
					hypAllArticles.Text			= "More Articles...";

					//donation panel
					litSurvivorNameDonate.Text = storyUser.Firstname + " " + storyUser.Lastname;
					litSurvivorNameDonate.Visible = true;

					var donationQuery = (from d in dc.UserDonations where d.RecipientId == storyUser.UserId orderby d.CreatedOn descending select d);

					float totalDonated = 0;
					System.Text.StringBuilder donationListSb = new System.Text.StringBuilder();

					foreach (var pastDonation in donationQuery)
					{
						totalDonated += pastDonation.Amount;

						var userQuery = (from p in dc.Profiles where p.UserId == pastDonation.DonorId select p);

						donationListSb.AppendLine(string.Format("{0} gave ${1}<br />", userQuery.FirstOrDefault().Firstname, Math.Round(pastDonation.Amount, 0).ToString()));
					}

					this.donationList = donationListSb.ToString();
					this.divWhoHasGiven.Visible = (totalDonated > 0);


				}
				else
				{
					divSurvivorInfo.Visible		= false;
					litTextNoSurvivor.Visible	= true;
					litTextNoSurvivor.Text		= article.a.Text;
				}

				LoadCategories(article.a.ArticleId);
				LoadComments(article.a.ArticleId);
			}
		}
	}

	protected void GenerateClientToken()
	{
		bool testMode = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["brainTreeTestMode"].ToString());

		Braintree.BraintreeGateway gateway = new Braintree.BraintreeGateway();

		if (testMode)
		{
			gateway.Environment = Braintree.Environment.SANDBOX;
			gateway.MerchantId = "bzzxdb3b496mcmg8";
			gateway.PublicKey = "8pt4km256dh5d8xm";
			gateway.PrivateKey = "44c448397615c0dbad22baf364ab1885";
		}
		else
		{
			gateway.Environment = Braintree.Environment.PRODUCTION;
			gateway.MerchantId = "2fpmr2kcw3km3q6y";
			gateway.PublicKey = "5xf6jgccdtvzykqq";
			gateway.PrivateKey = "3f036ed0d48a2337b0c702043e32b224";
		}

		this.ClientToken = gateway.ClientToken.Generate();
	}

	protected UserDonation ProcessPaymentNonce(CrowdReliefDBDataContext dc, Guid donorUserId, Guid recipientUserId)
	{
		if (this.hidNonce.Value.Length > 0 && this.hidPrice.Value.Length > 0)
		{
			bool testMode = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["brainTreeTestMode"].ToString());

			Braintree.BraintreeGateway gateway = new Braintree.BraintreeGateway();

			if (testMode)
			{
				gateway.Environment = Braintree.Environment.SANDBOX;
				gateway.MerchantId = "bzzxdb3b496mcmg8";
				gateway.PublicKey = "8pt4km256dh5d8xm";
				gateway.PrivateKey = "44c448397615c0dbad22baf364ab1885";
			}
			else
			{
				gateway.Environment = Braintree.Environment.PRODUCTION;
				gateway.MerchantId = "2fpmr2kcw3km3q6y";
				gateway.PublicKey = "5xf6jgccdtvzykqq";
				gateway.PrivateKey = "3f036ed0d48a2337b0c702043e32b224";
			}

			var donorQuery = (from p in dc.Profiles where p.UserId == donorUserId select p);
			var recipientQuery = (from p in dc.Profiles where p.UserId == recipientUserId select p);

			Guid donationId = Guid.NewGuid();
			Dictionary<string, string> customFields = new Dictionary<string, string>();
			customFields["DonationId"] = donationId.ToString();

			customFields["DonorId"] = donorUserId.ToString();
			customFields["DonorName"] = donorQuery.FirstOrDefault().Firstname + " " + donorQuery.FirstOrDefault().Lastname;

			customFields["RecipientId"] = recipientUserId.ToString();
			customFields["RecipientName"] = recipientQuery.FirstOrDefault().Firstname + " " + recipientQuery.FirstOrDefault().Lastname;

			var txnRequest = new Braintree.TransactionRequest
			{
				Amount = Convert.ToDecimal(this.hidPrice.Value),
				PaymentMethodNonce = hidNonce.Value,
				//CustomFields = customFields
			};

			Braintree.Result<Braintree.Transaction> result = gateway.Transaction.Sale(txnRequest);

			if (result.IsSuccess())
			{
				Braintree.Transaction transaction = result.Target;

				UserDonation donation = new UserDonation();
				donation.DonationId = donationId;
				donation.CreatedOn = DateTime.Now;
				donation.Amount = float.Parse(this.hidPrice.Value);
				donation.Status = transaction.Status.ToString();
				donation.BrainTreeId = transaction.Id;
				donation.RecipientId = recipientUserId;
				donation.DonorId = donorUserId;

				//this.txtAmount.Text = "";

				return donation;
			}
			else
            {
				return null;
            }
		}

		return null;
	}

	protected void SendMessagesToDonorParties(
		string donorFirstName, 
		string donorLastName, 
		string donorEmail,
		string recipientFirstName, 
		string recipientLastName, 
		string recipientEmail,
		CrowdReliefDBDataContext dc, UserDonation userDonation)
    {
		this.SendToDonor(donorFirstName, donorLastName, donorEmail, recipientLastName);
		this.SendToRecipient(recipientFirstName, recipientLastName, recipientEmail, donorFirstName, donorLastName);
	}

	protected void SendToDonor(string donorFirstName, string donorLastName, string donorEmail, string recipientLastName)
    {
		try
		{
			string emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
			string emailFromDisplayName = ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();

			ListDictionary ldEmailBodyReplacements = new ListDictionary();
			ldEmailBodyReplacements.Add("<% DonorFirstName %>", donorFirstName);
			ldEmailBodyReplacements.Add("<% DonorLastName %>", donorLastName);
			ldEmailBodyReplacements.Add("<% RecipLastName %>", recipientLastName);

			MailAddress fromAddress = new MailAddress(emailFrom, emailFromDisplayName);

			MailDefinition mailDefinition = new MailDefinition();

			mailDefinition.BodyFileName = HttpContext.Current.Server.MapPath("~\\EmailTemplates\\DonorLetter.html");
			mailDefinition.Subject = "Thank You For Your Donation!";
			mailDefinition.IsBodyHtml = true;

			MailMessage newUserMailMessage = mailDefinition.CreateMailMessage(donorEmail, ldEmailBodyReplacements, this);

			SmtpClient smtp = new SmtpClient();
			//smtp.EnableSsl = true;
			smtp.Send(newUserMailMessage);
		}
		catch (Exception ex)
		{ }
	}

	protected void SendToRecipient(string recipientFirstName, string recipientLastName, string recipientEmail, string donorFirstName, string donorLastName)
    {
		try
		{
			string emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
			string emailFromDisplayName = ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();

			ListDictionary ldEmailBodyReplacements = new ListDictionary();
			ldEmailBodyReplacements.Add("<% DonorFirstName %>", donorFirstName);
			ldEmailBodyReplacements.Add("<% DonorLastName %>", donorLastName);
			ldEmailBodyReplacements.Add("<% RecipFirstName %>", recipientFirstName);
			ldEmailBodyReplacements.Add("<% RecipLastName %>", recipientLastName);

			MailAddress fromAddress = new MailAddress(emailFrom, emailFromDisplayName);

			MailDefinition mailDefinition = new MailDefinition();

			mailDefinition.BodyFileName = HttpContext.Current.Server.MapPath("~\\EmailTemplates\\RecipLetter.html");
			mailDefinition.Subject = "Thank You For Your Donation!";
			mailDefinition.IsBodyHtml = true;

			MailMessage newUserMailMessage = mailDefinition.CreateMailMessage(recipientEmail, ldEmailBodyReplacements, this);

			SmtpClient smtp = new SmtpClient();
			//smtp.EnableSsl = true;
			smtp.Send(newUserMailMessage);
		}
		catch (Exception ex)
		{ }
	}


	protected void CreateRebuildPostForDonation(CrowdReliefDBDataContext dc, UserDonation userDonation)
    {
		try
        {
			var rebuildsByUser = (from r in dc.Rebuilds where r.SurvivorId == userDonation.RecipientId select r);

			Rebuild foundRebuild = rebuildsByUser.FirstOrDefault();

			if (foundRebuild != null)
			{
				var donorQuery = (from p in dc.Profiles where p.UserId == userDonation.DonorId select p);
				var recipientQuery = (from p in dc.Profiles where p.UserId == userDonation.RecipientId select p);

				RebuildPost rebuildPost = new RebuildPost();
				rebuildPost.RebuildPostId = Guid.NewGuid(); ;
				rebuildPost.CreatedOn = DateTime.Now;
				rebuildPost.IsVisible = true;
				rebuildPost.Post = donorQuery.FirstOrDefault().Firstname + " just donated $" + userDonation.Amount + " to " + recipientQuery.FirstOrDefault().Firstname;// "Just created a new rebuild in " + city + ", " + state + " for " + firstname + " " + lastname + ". The targeted start date is <i>" + targetStartDate + "</i>.";
				rebuildPost.RebuildId = foundRebuild.RebuildId;
				rebuildPost.UserId = userDonation.DonorId;
				dc.RebuildPosts.InsertOnSubmit(rebuildPost);
				dc.SubmitChanges();
			}
		}
		catch (Exception ex)
        {
			ex.ToString();
        }
	}

	protected string StripHTML(string input)
	{
		if (!string.IsNullOrEmpty(input))
		{
			input = Regex.Replace(input, "<.*?>", String.Empty);
		}
		return input;
	}
	
	
	protected void LoadSurvivors(Guid? profileUserId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var survivorProfiles = (from uu in dc.UserUsers
								join p in dc.Profiles on uu.AcceptingUserId equals p.UserId
								where uu.RequestingUserId == profileUserId
								orderby p.Lastname
								select new {p}).Distinct();

		if(survivorProfiles != null && survivorProfiles.Count() > 0)
		{
			dtSurvivorLinkList.Visible = true;
		}

		foreach(var survivorProfile in survivorProfiles)
		{
			survivorLinkList = survivorLinkList + "<li><a href=\"\\sp\\" + survivorProfile.p.ProfileNumber + "\\" + survivorProfile.p.Firstname + "-" + survivorProfile.p.Lastname  + "\">" + survivorProfile.p.Firstname + " " + survivorProfile.p.Lastname +  "</a></li>" + Environment.NewLine;
		}
	}



	protected void LoadComments(Guid articleId)
	{
		string profilePhotoFolder	= System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var articleComments = from c in dc.Comments
							  join ac in dc.ArticleComments on c.CommentId equals ac.CommentId
							  join p in dc.Profiles on ac.UserId equals p.UserId
							  where ac.ArticleId == articleId && c.IsDeleted == false
							  orderby c.CreatedOn descending
							  select new {c, ac, p};

		string comments = string.Empty;

		foreach(var articleComment in articleComments)
		{
			string profilePhoto = string.Empty;

			var profileImage = (from ph in dc.ProfilePhotos
							   join p in dc.Photos on ph.PhotoId equals p.PhotoId
							   where ph.UserId == articleComment.p.UserId && ph.IsCurrrent == true
							   orderby p.CreatedOn descending
							   select new { p.FilenameCropped }).Take(1).SingleOrDefault();

			if(profileImage != null)
			{
				//Get the users profile image
				profilePhoto = profilePhotoFolder + profileImage.FilenameCropped;
			}

			string createdOn = GetElapsedTime(articleComment.ac.CreatedOn); //articleComment.ac.CreatedOn.ToString("MM/dd/yyyy h:mm tt");
			string comment = articleComment.c.Comment1;
			string fullname = articleComment.p.Firstname + " " + articleComment.p.Lastname;

			string delete = string.Empty;
			if(User.IsInRole("Administrator"))
			{
				delete = "<div><a href=\"Post.aspx?articleId=" + articleId + "&d=true&commentId=" + articleComment.c.CommentId + "\">Delete</a></div>";
			}

			comments += "<div class=\"social-talk\">" +  Environment.NewLine +
							"<div class=\"media social-profile clearfix\">" +  Environment.NewLine +
								"<a class=\"pull-left\">" +  Environment.NewLine +
									"<img src=\"" + profilePhoto + "\" alt=\"post-picture\">" +  Environment.NewLine +
								"</a>" +  Environment.NewLine +
								"<div class=\"media-body\">" +  Environment.NewLine +
									"<span class=\"font-bold\">" + fullname + "</span>" +  Environment.NewLine +
									"<small class=\"text-muted\">" + createdOn + "</small>" +  Environment.NewLine +
									"<div class=\"social-content\">" +  Environment.NewLine +
										comment +  Environment.NewLine +
									"</div>" +  Environment.NewLine +
								"</div>" +  Environment.NewLine +
							"</div>" +  Environment.NewLine +
							delete + 
						"</div>";
		}
		litComments.Text = comments;
	}

	protected void LoadCategories(Guid articleId)
	{
		string categoryList = string.Empty;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var categories = from c in dc.Categories
						join ac in dc.ArticleCategories on c.CategoryId equals ac.CategoryId
						where ac.ArticleId == articleId
						orderby c.Category1
						select new { c.Category1 };

		if(categories.Count() > 0)
		{
			foreach(var category in categories)
			{
				categoryList += "<a href=\"\\c\\" + category.Category1.Replace(" ","-").Replace("/","_") + "\" class=\"btn btn-default m-xs btn-m\">" + category.Category1 + "</a>";
			}
			litCategory.Text = categoryList;
		}
	}

	protected void imgAmazonButton_Click(object sender, ImageClickEventArgs e)
	{
		ImageButton btn = (ImageButton)(sender);
		Response.Redirect(btn.CommandArgument);
	}

	protected void btnComment_Click(object sender, EventArgs e)
	{
		string commentText = txtComment.Text;
		Guid commentId = Guid.NewGuid();
		Guid articleId = new Guid(hidArticleId.Value);
		string title =  hidTitle.Value;

		commentText = Linkify(commentText);


		Comment comment = new Comment();
		comment.Comment1 = commentText;
		comment.CommentId = commentId;
		comment.CreatedBy = new Guid(Membership.GetUser().ProviderUserKey.ToString());
		comment.CreatedOn = DateTime.Now;
		comment.IsDeleted = false;
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		dc.Comments.InsertOnSubmit(comment);
		dc.SubmitChanges();

		ArticleComment articleComment = new ArticleComment();
		articleComment.ArticleCommentId = Guid.NewGuid();
		articleComment.ArticleId = articleId;
		articleComment.CreatedOn = DateTime.Now;
		articleComment.CommentId = commentId;
		articleComment.IsDeleted = false;
		articleComment.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

		dc.ArticleComments.InsertOnSubmit(articleComment);
		dc.SubmitChanges();
		
		string articleNumber = Request.QueryString["number"];

		Response.Redirect("\\a\\" + articleNumber + "\\" + title);
	}
	protected string Linkify( string SearchText ) {
	// this will find links like:
	// http://www.mysite.com
	// as well as any links with other characters directly in front of it like:
	// href="http://www.mysite.com"
	// you can then use your own logic to determine which links to linkify
	Regex regx = new Regex( @"\b(((\S+)?)(@|mailto\:|(news|(ht|f)tp(s?))\://)\S+)\b", RegexOptions.IgnoreCase );
	SearchText = SearchText.Replace( "&nbsp;", " " );
	MatchCollection matches = regx.Matches( SearchText );

	foreach ( Match match in matches ) {
		if ( match.Value.StartsWith( "http" ) ) { // if it starts with anything else then dont linkify -- may already be linked!
			SearchText = SearchText.Replace( match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>" );
		}
	}

	return SearchText;
}
}