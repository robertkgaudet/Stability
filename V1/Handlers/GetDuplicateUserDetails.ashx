<%@ WebHandler Language="C#" Class="GetDuplicateUserDetails" %>

using System;
using System.Web;
using System.Linq;
using System.Web.Services;
using System.Web.SessionState;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class GetDuplicateUserDetails : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string control = (string)context.Request.QueryString["control"];
        string value = (string)context.Request.QueryString["value"];
        string results = string.Empty;

        if (GetDuplicateUserDetailsByName(control, value))
        {
            results = "Yes";
        }
        else
        {
            results = "No";
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write(results);
    }

    protected bool GetDuplicateUserDetailsByName(string control, string value)
    {
        try
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            Object user = new Object();
           
            if (control == "Email")
            {
                user = dc.aspnet_Memberships.FirstOrDefault(f => f.Email.ToLower() == value.ToLower());
            }
            else
            {
                user = dc.aspnet_Users.FirstOrDefault(f => f.UserName.ToLower() == value.ToLower());
            }
            if (user == null)
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
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