using CrowdRelief;
using Stability;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
	public string _teamName = string.Empty;
	public string _teamId = string.Empty;
	public bool _isSignedInUser = false;
	public string _deploymentCount = string.Empty;
	public int _connectionCount = 0;
	public Tools.FriendStatus _friendStatus = Tools.FriendStatus.AddConnection;  //Default
	public string _badgeVettingStatus = "fa-pending-color";
	public string _badgeCertificationStatus = "fa-pending-color";
	public string _badgeDeployedStatus = "fa-pending-color";
	public string _badgeHoursRecordedStatus = "fa-pending-color";
	public string _badgeTOPStatus = "fa-pending-color";
	public string faIdBadgeClick = string.Empty;
    public string _teamLogo = string.Empty;
    public bool _isShowTeamLogo = false;
    public bool _isDisasterReadyCertified = false;
    public string _purplebadge = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
	{
		//string causePhotoFolder			= System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		string profilePhotoFolder		= System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
		string coverPhotoFolder			= System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
        string teamLogo = System.Configuration.ConfigurationManager.AppSettings["logoFolder"].ToString();
        _coverImage = coverPhotoFolder + "Stability_Cover_V3.jpg";
        _purplebadge = teamLogo + "purplebadge.png";

       
        if (_isShowTeamLogo)
        {
            litMemberLogo.ImageUrl = teamLogo + _teamLogo;
        }
        else
        {
            litMemberLogo.ImageUrl = string.Empty;
        }



        if (_isDisasterReadyCertified)
        {
            litMemberName.Text = _memberFullname;
            litpurplebadge.ImageUrl = teamLogo + "purplebadge.png";
        }
        else
        {
            litMemberName.Text = _memberFullname;
        }

        litMemberName.Text = _memberFullname;

        imgMemberProfilePhoto.ImageUrl	= profilePhotoFolder + _memberProfileImageFilename;
		litMemberDescription.Text		= _memberDescription;
		litTitle.Text					= !String.IsNullOrEmpty(_memberTitle) ? _memberTitle + "<br />" : string.Empty;
		litLocation.Text				= _memberLocation;
		litDeploymentCount.Text			= _deploymentCount;
		hypConnections.Text				= _connectionCount + " Connections";
		hypConnections.NavigateUrl		= "/V1/Member/Connections.aspx?userId=" + UserId;

		if(_badgeVettingStatus == "fa-approved-color")
		{
			faIdBadgeClick = "faIdBadgeClick";
		}

		if (!String.IsNullOrEmpty(_teamId))
		{
			litTeamBreak.Text =         "<br />";
			hypTeam.Visible				= true;
			hypTeam.Text				= _teamName;
			hypTeam.NavigateUrl			= "/V1/NonProfit/Default.aspx?organizationId=" + _teamId;
			hypMyTeam.NavigateUrl		= "/V1/NonProfit/Default.aspx?organizationId=" + _teamId;
		}

		if (HttpContext.Current.User.Identity.IsAuthenticated)
		{
			UpdateAddConnectionButton(_friendStatus, IsSignedInUser);

			if(IsSignedInUser)
			{
				linkCamera.Visible = true;
				hypProfileEdit.Visible = true;
				divUserSettings.Visible = true;
			}

			if (HttpContext.Current.User.IsInRole("Administrator"))
			{
				idKwest.Visible = true;
				hypProfileAdmin.Visible = true;
				hypProfileAdmin.NavigateUrl = "/V1/Profile/Profile.aspx?userId=" + _userId;
			}
		}
		else
		{
			//Disable the friend button if user is not signed in.
			btnFriend.Enabled = false;
			btnFriend.Text = "Sign In To Connect";
			btnFriend.CssClass = "btn btn-primary pull-left m-t-sm";
			btnFriend.Attributes.Add("disabled", "disabled");
			btnFriend.ID = ".btnDisabled";
		}
	}

	public void UpdateAddConnectionButton(Tools.FriendStatus friendStatus, bool isSignedInUser)
	{
		if(isSignedInUser)
		{ 
			//SIGNED IN USER SHOW 
			// - NO BUTTON
			btnFriend.Visible = false;
		}
		else
		{
			if(friendStatus == Tools.FriendStatus.AddConnection)
			{ 
				//VISITING USER SHOW
				// - Add Connection (blue)
				btnFriend.CssClass = "btn btn-primary pull-left m-t-sm btnFriend";
				btnFriend.Text = "Add Connection";
			}

			if (friendStatus == Tools.FriendStatus.Delete)
			{
				// - Remove Connection (white)
				btnFriend.CssClass = "btn btn-default pull-left m-t-sm";
				btnFriend.Text = "Remove Connection";
				btnFriend.ID = ".btnDisabled";
			}

			if (friendStatus == Tools.FriendStatus.Pending)
			{
				// - Pending (white)
				btnFriend.CssClass = "btn btn-default pull-left m-t-sm";
				btnFriend.Text = "Connection Pending";
				btnFriend.ID = ".btnDisabled";
				btnFriend.Attributes.Add("disabled", "disabled");
			}

			if (friendStatus == Tools.FriendStatus.Connected)
			{
				// - Connected (white)
				btnFriend.CssClass = "btn btn-default pull-left m-t-sm";
				btnFriend.Text = "Connected";
				btnFriend.ID = ".btnDisabled";
				btnFriend.Attributes.Add("disabled", "disabled");
			}

			if (friendStatus == Tools.FriendStatus.Blocked)
			{
				// - Blocked (white)
				btnFriend.CssClass = "btn btn-default pull-left m-t-sm";
				btnFriend.Text = "Blocked";
				btnFriend.ID = ".btnDisabled";
			}
		}
	}

    public string TeamLogo
    {
        get { return _teamLogo; }
        set { _teamLogo = value; }
    }
    public string BadgeVettingStatus
	{
		get { return _badgeVettingStatus; }
		set { _badgeVettingStatus = value; }
	}
	public string BadgeCertificationStatus
	{
		get { return _badgeCertificationStatus; }
		set { _badgeCertificationStatus = value; }
	}
	public string BadgeDeployedStatus
	{
		get { return _badgeDeployedStatus; }
		set { _badgeDeployedStatus = value; }
	}
	public string BadgeHoursRecordedStatus
	{
		get { return _badgeHoursRecordedStatus; }
		set { _badgeHoursRecordedStatus = value; }
	}
    public bool IsDisasterReadyCertified
    {
        get { return _isDisasterReadyCertified; }
        set { _isDisasterReadyCertified = value; }
    }
    public bool IsShowTeamLogo
    {
        get { return _isShowTeamLogo; }
        set { _isShowTeamLogo = value; }
    }
    public string BadgeTOPStatus
	{
		get { return _badgeTOPStatus; }
		set { _badgeTOPStatus = value; }
	}

	public int ConnectionCount
	{
		get { return _connectionCount; }
		set { _connectionCount = value; }
	}
	public Tools.FriendStatus FriendStatus
	{
		get { return _friendStatus; }
		set { _friendStatus = value; }
	}
	public string DeploymentCount
	{
		get { return _deploymentCount; }
		set { _deploymentCount = value; }
	}
	public string TeamName
	{
		get { return _teamName; }
		set { _teamName = value; }
	}
	public string TeamId
	{
		get { return _teamId; }
		set { _teamId = value; }
	}
	public bool IsSignedInUser
	{
		get { return _isSignedInUser; }
		set { _isSignedInUser = value; }
	}
	public string UserId
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