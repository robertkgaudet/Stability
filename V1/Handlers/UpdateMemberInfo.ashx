<%@ WebHandler Language="C#" Class="UpdateMemberInfo" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Linq;
using System.Web.Script.Serialization;
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
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
        string username = context.User.Identity.Name;
        Guid loginuserId = Guid.Empty;
        Guid userId = new Guid(context.Request["userId"]);
        Guid orgId = new Guid(context.Request["organizationId"]);
        using (var dc = new CrowdReliefDBDataContext())
        {
            Guid targetRoleId = new Guid("E48E49D7-392B-4C3B-A53A-62B7B2537BBF");
            bool isUserhead = false;
            bool isUserInThatRole = false;
            bool isShow = false;
            bool isTeamowner = false;
            bool iteamadmin = false;
            bool iteamadminone = false;
            bool isteamadminowner = false;
            var user = dc.aspnet_Users.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                loginuserId = user.UserId;
            }

            bool isTeamAdministratorExists = dc.UserOrganizations
           .Any(uo => uo.OrganizationId == orgId && uo.UserId == loginuserId && uo.IsTeamAdministrator == true && uo.Status== (int)RequestStatus.Approved);

            bool isTeamAdministrator = dc.UserOrganizations
            .Any(uo => uo.OrganizationId == orgId && uo.UserId == userId && uo.IsTeamAdministrator == true && uo.Status== (int)RequestStatus.Approved);

            bool userOrganizationOwner = dc.Organizations
                      .Any(o => o.OrganizationId == orgId && o.OwnerId == loginuserId);

            bool openuserOrganizationOwner = dc.Organizations
                .Any(o => o.OrganizationId == orgId && o.OwnerId == userId);

            if (openuserOrganizationOwner == true)
            {
                isUserInThatRole = dc.aspnet_UsersInRoles
           .Any(ur => ur.UserId == loginuserId && ur.RoleId == targetRoleId);
            }


            if (loginuserId == userId)
            {
                isShow = true;
            }
            if (openuserOrganizationOwner == true && isTeamAdministratorExists == true)
            {
                isteamadminowner = true;
            }
            if (isUserInThatRole == true && openuserOrganizationOwner == true && userOrganizationOwner == false)
            {
                isUserhead = true;
                if (userOrganizationOwner == true && openuserOrganizationOwner == true)
                {
                    isTeamowner = true;
                }
                if (isTeamAdministratorExists == isTeamAdministrator)
                {
                    iteamadmin = true;
                }
                if (isTeamAdministratorExists == true)
                {
                    iteamadminone = true;
                }
                if (openuserOrganizationOwner == true && isTeamAdministratorExists == true)
                {
                    isteamadminowner = true;
                }
            }
            var profile = dc.Profiles.SingleOrDefault(p => p.UserId == userId);
            var userOrg = dc.UserOrganizations
            .FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == orgId && uo.Status== (int)RequestStatus.Approved);
            bool isOwner = userOrg != null && userOrg.IsOwner;

            if (profile == null || userOrg == null)
            {
                throw new InvalidOperationException("User profile or organization not found.");
            }
            bool stabilityVerified = profile.IsDisasterReadyCertified;
            bool showTeamLogo = userOrg.ShowTeamLogo ?? false;
            var teamAdministratorRole = dc.aspnet_Roles.FirstOrDefault(r => r.RoleName == "Team Administrator");
            Guid teamAdministratorRoleId = teamAdministratorRole.RoleId;
            bool makeTeamAdministrator = userOrg.IsTeamAdministrator == true;
            if (makeTeamAdministrator == true && isTeamAdministratorExists == true)
            {
                isShow = true;
            }
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
                isOwner = isOwner,
                vettingStatus = vettingStatus,
                vettingNotes = profile.VettingNotes,
                stabilityVerified = stabilityVerified,
                showTeamLogo = showTeamLogo,
                makeTeamAdministrator = makeTeamAdministrator,
                isShow = isShow,
                isTeamowner = isTeamowner,
                isUserhead = isUserhead,
                iteamadmin = iteamadmin,
                iteamadminone = iteamadminone,
                isteamadminowner = isteamadminowner
            };
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(response));
        }
    }
    private void UpdateUserData(HttpContext context)
    {

        context.Response.ContentType = "application/json";
        try
        {
            Guid orgId = new Guid(context.Request.Form["organizationId"]);
            Guid userId = new Guid(context.Request.Form["userId"]);
            string vettingStatus = context.Request.Form["vettingStatus"];
            string vettingNotes = context.Request.Form["vettingNotes"];
            bool stabilityVerified = bool.Parse(context.Request.Form["stabilityVerified"]);
            bool showTeamLogo = bool.Parse(context.Request.Form["showTeamLogo"]);
            bool makeTeamAdministrator = bool.Parse(context.Request.Form["makeTeamAdministrator"]);

            using (var dc = new CrowdReliefDBDataContext())
            {
                var profile = dc.Profiles.SingleOrDefault(p => p.UserId == userId);
                if (profile != null)
                {
                    profile.VettingNotes = vettingNotes;
                    profile.IsDisasterReadyCertified = stabilityVerified;
                    profile.StabilityVerifiedDate = stabilityVerified ? DateTime.Now : (DateTime?)null;

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

                var userOrg = dc.UserOrganizations
     .FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == orgId && uo.Status== (int)RequestStatus.Approved);

                var role = dc.aspnet_Roles.FirstOrDefault(r => r.RoleName == "Team Administrator");

                if (makeTeamAdministrator)
                {
                    if (role != null)
                    {
                        var existingRole = dc.aspnet_UsersInRoles
                                             .FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == role.RoleId);

                        if (existingRole == null)
                        {
                            var newRoleAssignment = new aspnet_UsersInRole
                            {
                                UserId = userId,
                                RoleId = role.RoleId
                            };
                            dc.aspnet_UsersInRoles.InsertOnSubmit(newRoleAssignment);
                        }

                        if (userOrg != null)
                        {
                            userOrg.IsTeamAdministrator = true;
                        }
                    }
                }
                else
                {
                    if (role != null)
                    {
                        var existingRole = dc.aspnet_UsersInRoles
                                             .FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == role.RoleId);

                        if (existingRole != null)
                        {
                            dc.aspnet_UsersInRoles.DeleteOnSubmit(existingRole);
                        }

                        if (userOrg != null)
                        {
                            userOrg.IsTeamAdministrator = false;
                        }
                    }
                }

                if (userOrg != null)
                {
                    userOrg.ShowTeamLogo = showTeamLogo;
                    userOrg.TeamVerifiedDate = showTeamLogo ? DateTime.Now : (DateTime?)null;
                }

                dc.SubmitChanges();
            }

            var data = new { Success = true, Message = "User data updated successfully." };
            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(data);
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            var data = new { Success = false, Message = ex.Message };
            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(data);
            context.Response.Write(json);
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