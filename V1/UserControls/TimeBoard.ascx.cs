using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_TimeBoard : System.Web.UI.UserControl
{
	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	Guid _organizationEventId = Guid.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadTimeBoard();
	}

	public Guid organizationEventId
	{
		get { return _organizationEventId; }
		set { _organizationEventId = value; }
	}

	protected void LoadTimeBoard()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var peopleList = from uo in dc.UserOrganizations
						 join p in dc.Profiles on uo.UserId equals p.UserId
						 join oe in dc.OrganizationEvents on uo.OrganizationId equals oe.OrganizationId
						 where oe.OrganizationEventId == _organizationEventId && uo.Status== (int)RequestStatus.Approved && uo.Status == (int)RequestStatus.Pending
                         orderby p.Title descending
						 select p;

		rpNonProfitPeople.DataSource = peopleList;
		rpNonProfitPeople.DataBind();
	}

	protected void rpNonProfitPeople_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
			String firstname = (String)DataBinder.Eval(dataItem.DataItem, "Firstname");
			String lastname = (String)DataBinder.Eval(dataItem.DataItem, "Lastname");
			String zelloName = (String)DataBinder.Eval(dataItem.DataItem, "ZelloName");
			String title = (String)DataBinder.Eval(dataItem.DataItem, "Title");

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			//var profilePhoto = (from p in dc.ProfilePhotos
			//					join ph in dc.Photos on p.PhotoId equals ph.PhotoId
			//					where p.UserId == userId && p.IsCurrrent == true
			//					select new { ph.FilenameCropped }).Take(1).SingleOrDefault();

			Literal lblInfo = (Literal)e.Item.FindControl("lblInfo");
			//Image imgProfilePhoto = (Image)e.Item.FindControl("imgProfilePhoto");

			//imgProfilePhoto.ImageUrl = "/V1/Images/icons8-customer-64.png";
			//if (profilePhoto != null)
			//{
			//	imgProfilePhoto.ImageUrl = profilePhotoFolder + profilePhoto.FilenameCropped;
			//}

			zelloName = String.IsNullOrEmpty(zelloName) ? "none" : zelloName;
			title = String.IsNullOrEmpty(title) ? "none" : title;

			var time = (from t in dc.Timesheets
						join p in dc.Profiles on t.UserId equals p.UserId
						where t.UserId == userId
						orderby t.TimeIn descending
						select new { t.TimeIn, t.TimeOut, t.UserId }).Take(1).SingleOrDefault();

			if (time != null)
			{
				DateTime timeIn = time.TimeIn;
				DateTime timeOut = Convert.ToDateTime(time.TimeOut);
				DateTime date = time.TimeIn.Date;
				string dateInString = string.Empty;
				string timeInString = string.Empty;
				string timeOutString = string.Empty;

				if (timeIn != null && timeOut != null)
				{
					string points = string.Empty;
					int hours = 0;
					TimeSpan? span = (timeOut - timeIn);
					string totalTime = span.Value.Hours.ToString() + " hrs " + span.Value.Minutes.ToString() + " mins";
					points = span.Value.Hours.ToString();
					dateInString = timeIn.ToShortDateString();
					timeInString = timeIn.ToShortTimeString();
					timeOutString = "Out - " + timeOut.ToShortTimeString();
				}
			}


			lblInfo.Text = "<p><dl class=\"dl-horizontal\" class=\"m-l-sm\"><dd><b><a href=\"/V1/Profile/Profile.aspx?userId=" + userId.ToString() + "\"  style=\"text-decoration:underline;\">" + firstname + " " + lastname + "</a></b><br>Zello: " + zelloName + "<br>Title: " + title + "</dd></dl></p>";
		}
	}
}