using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class S1_Profile_MySurvivors : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSurvivors();
	}

	protected void LoadSurvivors()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var survivors = from p in dc.Profiles
						join uu in dc.UserUsers on p.UserId equals uu.AcceptingUserId
						join aspUser in dc.aspnet_Memberships on uu.AcceptingUserId equals aspUser.UserId
						where uu.RequestingUserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
						select new {p.Firstname, p.Lastname, p.PhoneNumber, p.ProfileNumber, aspUser.Email, survivorId = p.UserId};

		if(survivors.Count() > 0)
		{
			divSurvivorList.Visible = true;
			divAddSurvivorButton.Visible = false;

			rpSurvivorListTable.DataSource = survivors;
			rpSurvivorListTable.DataBind();
		}
		else
		{
			//Show a button to add a new survivor.
			divSurvivorList.Visible = false;
			divAddSurvivorButton.Visible = true;
		}
	}

	protected void rpSurvivorListTable_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Literal litDisaster = (Literal)e.Item.FindControl("litDisaster"); 
			Literal litLivingSituation = (Literal)e.Item.FindControl("litLivingSituation"); 
			Literal litRecoveryStage = (Literal)e.Item.FindControl("litRecoveryStage"); 
			HyperLink hypName = (HyperLink)e.Item.FindControl("hypName"); 
			HyperLink hypPhone = (HyperLink)e.Item.FindControl("hypPhone"); 
			HyperLink hypEmailAddress = (HyperLink)e.Item.FindControl("hypEmailAddress"); 
            HyperLink hypWishList = (HyperLink)e.Item.FindControl("hypWishList"); 

            String Firstname = (string)DataBinder.Eval(dataItem.DataItem, "Firstname");
			String Lastname = (string)DataBinder.Eval(dataItem.DataItem, "Lastname");
			int profileNumber = (int)DataBinder.Eval(dataItem.DataItem, "profileNumber");
			String email = (string)DataBinder.Eval(dataItem.DataItem, "Email");
			String phoneNumber = (string)DataBinder.Eval(dataItem.DataItem, "PhoneNumber");
			Guid survivorId = (Guid)DataBinder.Eval(dataItem.DataItem, "survivorId");

			var disaster = (from eu in dc.UserEvents
						   join d in dc.Events on eu.EventId equals d.EventId
						   where eu.UserId == survivorId
						   select new { d.Name }).Take(1).SingleOrDefault();

			if(disaster != null)
			{
				litDisaster.Text = disaster.Name;
			}
			
			var housing = (from h in dc.Housings
						   join uh in dc.UserHousings on h.HousingId equals uh.HousingId
						   where uh.UserId == survivorId
						   orderby uh.CreatedOn descending
						   select new {h.Housing1, h.Sentence }).Take(1).SingleOrDefault();
			
			if(housing != null)
			{
				litLivingSituation.Text = housing.Sentence + " " + housing.Housing1;
			}

			/*
            var itemSubTypePersonSurvey = (from istps in dc.ItemSubTypePersonSurveys
                                          where istps.UserId == survivorId
                                          select istps).SingleOrDefault();

            hypWishList.NavigateUrl = "/V1/Surveys/Survey-Items.aspx?userId=" + survivorId;
            hypWishList.Text = "Create Disaster Recovery Wishlist";
            if (itemSubTypePersonSurvey !=  null)
            {
                hypWishList.Text = "View Disaster Recovery Wishlist";
                hypWishList.NavigateUrl = "/S1/WishList.aspx?itemSubTypePersonSurveyId=" + itemSubTypePersonSurvey.ItemSubTypePersonSurveyId + "&userId=" + survivorId;
            }
			*/

            hypName.NavigateUrl = "\\sp\\" + profileNumber + "\\" + Firstname + "-" + Lastname;
			hypName.Text = Firstname + " " + Lastname;

			hypEmailAddress.NavigateUrl = "mailto:" +  email;
			hypEmailAddress.Text = email;

			hypPhone.NavigateUrl = "tel:" + phoneNumber;
			hypPhone.Text = phoneNumber;
		}
	}
}