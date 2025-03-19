<%@ WebHandler Language="C#" Class="UpdateMemberInfo" %>
using System;
using System.Web;
using System.Linq;
using System.Web.SessionState;

public class UpdateMemberInfo : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        string action = context.Request["action"];
        if (action == "fetch")
        {
            FetchUserData(context);
        }
        else if (action == "update")
        {
            UpdateUserData(context);
        }
        else
        {
            throw new ArgumentException("Invalid action specified.");
        }
    }
    private void FetchUserData(HttpContext context)
    {
        Guid userId = new Guid(context.Request["userId"]);

        using (var dc = new CrowdReliefDBDataContext())
        {
            var profile = dc.Profiles.SingleOrDefault(p => p.UserId == userId);
            var userOrg = dc.UserOrganizations.FirstOrDefault(uo => uo.UserId == userId);
            if (profile == null || userOrg == null)
            {
                throw new InvalidOperationException("User profile or organization not found.");
            }
            bool stabilityVerified = profile.IsDisasterReadyCertified;
            bool showTeamLogo = userOrg.ShowTeamLogo ?? false;
            string vettingStatus = "";
            if (profile.VettingActive == true)
            {
                vettingStatus = "VettingStarted";
            }
            else if (profile.VettingComplete == true)
            {
                vettingStatus = profile.PassedVetting == true ? "VettingComplete_Passed" : "VettingComplete_Failed";
            }
            var response = new
            {
                success = true,
                vettingStatus = vettingStatus,
                vettingNotes = profile.VettingNotes,
                stabilityVerified = stabilityVerified,
                showTeamLogo = showTeamLogo
            };
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }
    }
    private void UpdateUserData(HttpContext context)
    {
        Guid userId = new Guid(context.Request.Form["userId"]);
        string vettingStatus = context.Request.Form["vettingStatus"];
        string vettingNotes = context.Request.Form["vettingNotes"];
        bool stabilityVerified = bool.Parse(context.Request.Form["stabilityVerified"]);
        bool showTeamLogo = bool.Parse(context.Request.Form["showTeamLogo"]);

        using (var dc = new CrowdReliefDBDataContext())
        {
            var profile = dc.Profiles.SingleOrDefault(p => p.UserId == userId);
            if (profile != null)
            {
                profile.VettingNotes = vettingNotes;
                profile.IsDisasterReadyCertified = stabilityVerified;

                switch (vettingStatus)
                {
                    case "VettingStarted":
                        profile.DateVettingStarted = DateTime.Now;
                        profile.VettingActive = true;
                        break;
                    case "VettingComplete_Failed":
                        profile.DateVettingCompleted = DateTime.Now;
                        profile.VettingActive = false;
                        profile.VettingComplete = true;
                        profile.PassedVetting = false;
                        break;
                    case "VettingComplete_Passed":
                        profile.DateVettingCompleted = DateTime.Now;
                        profile.VettingActive = false;
                        profile.VettingComplete = true;
                        profile.PassedVetting = true;
                        break;
                }
            }

            var userOrg = dc.UserOrganizations.FirstOrDefault(uo => uo.UserId == userId);
            if (userOrg != null)
            {
                userOrg.ShowTeamLogo = showTeamLogo;
            }

            dc.SubmitChanges();
        }

        context.Response.Write("{\"success\": true}");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}