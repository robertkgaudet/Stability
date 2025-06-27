using CrowdRelief;
using GoogleMapsAPI.Places;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Metadata;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Member_Connections : BaseWebForm
{
	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	public string divider = String.Empty;
	public string hideDeleteButton = String.Empty;
	public string hideConfirmButton = String.Empty;
	public string hideFriendControls = String.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (User.Identity.IsAuthenticated)
            {
                ConnectionsDataList.ItemDataBound += ConnectionsDataList_ItemDataBound;

                string received = Request.QueryString["received"];
                String _userId = String.IsNullOrEmpty(Request.QueryString["userId"]) ? userId.ToString() : Request.QueryString["userId"];
                String status = String.IsNullOrEmpty(Request.QueryString["status"]) ? "Connected" : Request.QueryString["status"];
                litPageName.Text = status;

                BindFriendsDataList(_userId, status, received);

                // Visibility setup
                linkConnectionsReceived.Visible = false;
                linkConnectionsSent.Visible = false;
                hideFriendControls = "style='display:none;'";
                if (userId == new Guid(_userId))
                {
                    hideFriendControls = "";
                    divider = "|";
                    linkConnectionsReceived.Visible = true;
                    linkConnectionsSent.Visible = true;
                    linkConnectionsReceived.NavigateUrl = "/V1/Member/Connections.aspx?received=true&status=Pending&userId=" + userId;
                    linkConnectionsSent.NavigateUrl = "/V1/Member/Connections.aspx?status=Sent&userId=" + userId;
                }

                ucMemberNavigation.UserId = userId.ToString();
            }
            else
            {
                Response.Redirect("/Signin");
            }
        }
    }

    protected void ConnectionsDataList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var dataItem = e.Item.DataItem as Tools.FriendInfo;

            var ucTeamLogo = e.Item.FindControl("ucTeamLogo") as V1_UserControls_TeamLogo;
            if (ucTeamLogo != null && dataItem != null)
            {
                ucTeamLogo.UserId = dataItem.UserId;
                ucTeamLogo.LoadNameWithBadges();
            }
        }
    }

    [WebMethod]
	public static string UpdateConnection(string userId, string friendId, string action)
	{
		string result = "success";
		Guid UserId = new Guid(userId);
		Guid FriendId = new Guid(friendId);

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		//Check for the combination of current and user and friend request.
		UserUser friend = (from uu in dc.UserUsers
							where 
							(uu.RequestingUserId == UserId && uu.AcceptingUserId == FriendId)
							||
							(uu.RequestingUserId == FriendId && uu.AcceptingUserId == UserId)
							select uu).Take(1).SingleOrDefault();

		if (friend != null)
		{ 
			if(action == "remove")
			{
				dc.UserUsers.DeleteOnSubmit(friend);
				dc.SubmitChanges();
			}
			else //action = "confirm"
			{
				var statusType = (from uus in dc.UserUserStatus
								  where uus.Status == "Connected"
								  select new { uus.UserUserStatusId }).SingleOrDefault();

				//Change the status to 'connected'
				friend.UserUserStatusId = statusType.UserUserStatusId;
				friend.AcceptedOn = DateTime.Now;
				friend.IsActive = true;
				dc.SubmitChanges();
			}
		}

		return result;
	}

	public void BindFriendsDataList(string _userId, string status, string requestsIReceived)
	{
		status = status == "Sent" ? "Pending" : status;

		Guid UserId = new Guid(_userId);
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var statusType = (from uus in dc.UserUserStatus
						  where uus.Status == status
						  select new { uus.UserUserStatusId }).SingleOrDefault();

		bool isActive = status == "Pending" ? false : true;

		if(status == "Connected")
		{
			hideConfirmButton = "style='display:none;'";
			//MY ACCEPTED CONNECTIONS
			List<Tools.FriendInfo> MyConnections = Tools.MyConnections(UserId, 0);
			ConnectionsDataList.DataSource = MyConnections;
			ConnectionsDataList.DataBind();
			litConnectionCount.Text = MyConnections.Count.ToString() + " Connections";
      
        }
		else if (!String.IsNullOrEmpty(requestsIReceived))
		{
			//CONNECT REQUESTS I RECEIVED
			List<Tools.FriendInfo> MyReceivedConnectionRequests = Tools.MyReceivedConnections(UserId, 0);
			ConnectionsDataList.DataSource = MyReceivedConnectionRequests;
			ConnectionsDataList.DataBind();
			litConnectionCount.Text = MyReceivedConnectionRequests.Count.ToString() + " connect requests to review.";
		}
		else
		{
			hideConfirmButton = "style='display:none;'";
			//CONNECT REQUESTS I SENT
			List<Tools.FriendInfo> MySentConnectionRequests = Tools.MySentConnections(UserId, 0);
			ConnectionsDataList.DataSource = MySentConnectionRequests;
			ConnectionsDataList.DataBind();
			litConnectionCount.Text = MySentConnectionRequests.Count.ToString() + " requests waiting acceptance.";
		}
	}
}