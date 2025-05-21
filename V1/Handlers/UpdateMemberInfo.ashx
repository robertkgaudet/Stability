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
        Guid userId = new Guid(context.Request["userId"]);
        Guid orgId = new Guid(context.Request["organizationId"]);
        using (var dc = new CrowdReliefDBDataContext())
        {
            var profile = dc.Profiles.SingleOrDefault(p => p.UserId == userId);
                       var userOrg = dc.UserOrganizations
            .FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == orgId);
            bool isOwner = userOrg != null && userOrg.IsOwner;
            if (profile == null || userOrg == null)
            {
                throw new InvalidOperationException("User profile or organization not found.");
            }
            bool stabilityVerified = profile.IsDisasterReadyCertified;
            bool showTeamLogo = userOrg.ShowTeamLogo ?? false;
            var teamAdministratorRole = dc.aspnet_Roles.FirstOrDefault(r => r.RoleName == "Team Administrator");
            Guid teamAdministratorRoleId = teamAdministratorRole.RoleId;
            ////var teamOwnerRole = dc.aspnet_Roles.FirstOrDefault(r => r.RoleName == "Team Owner");
            //Guid teamOwnerRoleID = teamOwnerRole.RoleId;
            bool makeTeamAdministrator = dc.aspnet_UsersInRoles.Any(r => r.UserId == userId && r.RoleId == teamAdministratorRoleId);
            //bool makeTeamOwner = dc.aspnet_UsersInRoles.Any(r => r.UserId == userId && r.RoleId == teamAdministratorRoleId);

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
                makeTeamAdministrator = makeTeamAdministrator
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
     .FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == orgId);

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