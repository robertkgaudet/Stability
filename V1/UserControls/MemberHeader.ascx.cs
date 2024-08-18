using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_MemberHeader : System.Web.UI.UserControl
{
	public string _coverImage;
	public string _userId = string.Empty; 
	public string _memberFullname = string.Empty;
	public string _memberProfileImageFilename = string.Empty; 
	public string _memberDescription = string.Empty;
	public string _memberTitle = string.Empty;
	public string _memberLocation = string.Empty;
	public bool _isSignedInUser = false;

	protected void Page_Load(object sender, EventArgs e)
	{
		//string causePhotoFolder			= System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		string profilePhotoFolder		= System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
		_coverImage						= profilePhotoFolder + "/profilecover.png";
		litMemberName.Text				= _memberFullname;
		imgMemberProfilePhoto.ImageUrl	= profilePhotoFolder + _memberProfileImageFilename;
		litMemberDescription.Text		= _memberDescription;
		litTitle.Text					= _memberTitle;
		litLocation.Text				= _memberLocation;

		if(HttpContext.Current.User.Identity.IsAuthenticated)
		{
			if(IsSignedInUser)
			{ 
				hypProfileEdit.Visible = true;
				divUserSettings.Visible = true;
			}


			if (HttpContext.Current.User.IsInRole("Administrator"))
			{ 
				hypProfileAdmin.Visible = true;
				hypProfileAdmin.NavigateUrl = "/V1/Profile/Profile.aspx?userId=" + _userId;
			}
		}
	}
	public bool IsSignedInUser
	{
		get { return _isSignedInUser; }
		set { _isSignedInUser = value; }
	}
	public String UserId
	{
		get { return _userId; }
		set { _userId = value; }
	}
	public string MemberFullname
	{
		get { return _memberFullname; }
		set { _memberFullname = value; }
	}
	public string MemberProfileImageFilename
	{
		get { return _memberProfileImageFilename; }
		set { _memberProfileImageFilename = value; }
	}
	public string MemberDescription
	{
		get { return _memberDescription; }
		set { _memberDescription = value; }
	}
	public string MemberTitle
	{
		get { return _memberTitle; }
		set { _memberTitle = value; }
	}
	public string MemberLocation
	{
		get { return _memberLocation; }
		set { _memberLocation = value; }
	}
}