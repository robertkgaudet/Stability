<%@ WebHandler Language="C#" Class="UpdateMemberInfo" %>

using System;
using System.Web;
using System.IO;
using System.Linq;



public class UpdateMemberInfo : IHttpHandler {
    public void ProcessRequest(HttpContext context) {
        context.Response.ContentType = "application/json";

        try {
            // Get the user ID
            Guid userId = new Guid(context.Request.Form["userId"]);

            // Get the vetting status
            string vettingStatus = context.Request.Form["vettingStatus"];

            // Get the vetting notes
            string vettingNotes = context.Request.Form["vettingNotes"];

            // Get the stability verified status
            bool stabilityVerified = bool.Parse(context.Request.Form["stabilityVerified"]);

            // Handle the team logo file upload
            HttpPostedFile teamLogoFile = context.Request.Files["teamLogo"];
            string teamLogoPath = null;
            if (teamLogoFile != null && teamLogoFile.ContentLength > 0) {
                string fileName = Path.GetFileName(teamLogoFile.FileName);
                string filePath = "/Uploads/TeamLogos/" + fileName; // Save to a folder
                teamLogoFile.SaveAs(context.Server.MapPath(filePath));
                teamLogoPath = filePath;
            }

            // Update the database
            using (var dc = new CrowdReliefDBDataContext()) {
                var profile = dc.Profiles.SingleOrDefault(p => p.UserId == userId);
                if (profile != null) {
                    //profile.VettingStatus = vettingStatus;
                    //profile.VettingNotes = vettingNotes;
                    //profile.IsDisasterReadyCertified = stabilityVerified;
                    //if (!string.IsNullOrEmpty(teamLogoPath)) {
                    //    profile.TeamLogoPath = teamLogoPath;
                    //}

                    // Handle vetting status logic
                    switch (vettingStatus) {
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

                    dc.SubmitChanges();
                }
            }

            // Return success response
            context.Response.Write("{\"success\": true}");
        } catch (Exception ex) {
            // Return error response
            context.Response.Write("{\"success\": false, \"error\": \"" + ex.Message + "\"}");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }
}