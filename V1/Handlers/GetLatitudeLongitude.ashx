<%@ WebHandler Language="C#" Class="GetLatitudeLongitude" %>

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
public class GetLatitudeLongitude : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string results = string.Empty;

        try
        {
            string address = (string)context.Request.QueryString["address"];
            string userId = (string)context.Request.QueryString["userId"];
            string mapDomain = ConfigurationManager.AppSettings["mapDomain"].ToString();
            double latitude = 0.0;
            double longitude = 0.0;
            string message = string.Empty;
            string street_number = string.Empty;
            string street = string.Empty;
            string city = string.Empty;
            string state = string.Empty;
            string country = string.Empty;
            string postal_code = string.Empty;
            string county = string.Empty;
            string googlePlaceId = string.Empty;
            string formattedAddress = string.Empty;
            bool? isPartialMatch = false;
            bool? duplicate = false;
            string addressId = string.Empty;

            if (GetLatitudeLongitudeFromGoogle(address, out latitude, out longitude, out message, out isPartialMatch, out street_number, out street, out city, out state, out country, out postal_code, out county, out googlePlaceId, out formattedAddress))
            {
                CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

                var duplicateAddress = from a in dc.Addresses
                                       where a.GooglePlaceId == googlePlaceId
                                       select a;

                if (duplicateAddress.Any())
                {
                    duplicate = true;
                    addressId = duplicateAddress.First().AddressId.ToString();
                }
                else
                {
                    duplicate = false;
                    Guid AddressId = Guid.NewGuid();
                    Address newAddress = new Address
                    {
                        AddressId = AddressId,
                        Address1 = street_number + " " + street,
                        City = city,
                        Country = country,
                        County = county,
                        CreatedBy = new Guid(userId),
                        CreatedOn = DateTime.Now,
                        FormattedAddress = formattedAddress,
                        GooglePlaceId = googlePlaceId,
                        IsActive = true,
                        Latitude = latitude.ToString(),
                        Longitude = longitude.ToString(),
                        State = state,
                        StreetName = street,
                        StreetNumber = street_number,
                        Zip = postal_code
                    };

                    dc.Addresses.InsertOnSubmit(newAddress);
                    dc.SubmitChanges();

                    var matchingCounty = (from c in dc.Counties
                                          where (c.Name == county.Replace(" County", "").Replace(" Parish", ""))
                                             && c.State == state
                                          select c).FirstOrDefault();

                    if (matchingCounty != null)
                    {
                        var insertedAddress = dc.Addresses.SingleOrDefault(a => a.AddressId == AddressId);
                        if (insertedAddress != null)
                        {
                            insertedAddress.CountyId = matchingCounty.CountyId;
                            dc.SubmitChanges();
                        }
                    }

                    addressId = AddressId.ToString();

                    var rebuildStatus = dc.RebuildStatus.FirstOrDefault(rb => rb.Status == "House");
                    var homeRelationshipOwnRentType = dc.RebuildStatus.FirstOrDefault(rb => rb.Status == "Home Owner");

                    var profile = dc.Profiles.FirstOrDefault(p => p.UserId == Guid.Parse(userId));

                    ProfileAddress profileAddress = new ProfileAddress
                    {
                        AddressId = AddressId,
                        HasFloodInsurance = false,
                        HasHomeownersInsurance = false,
                        ProfileAddressId = Guid.NewGuid(),
                        ProfileId = profile.ProfileId,
                        HomeTypeId = rebuildStatus.RebuildStatusId,
                        HomeRelationshipOwnRentTypeId = homeRelationshipOwnRentType.RebuildStatusId
                    };

                    dc.ProfileAddresses.InsertOnSubmit(profileAddress);
                    dc.SubmitChanges();
                }

                results = isPartialMatch + "|" + latitude + "|" + longitude + "|" + street_number + "|" + street + "|" + city + "|" + state + "|" + country + "|" + postal_code + "|" + county + "|" + googlePlaceId + "|" + formattedAddress + "|" + duplicate + "|" + addressId;
            }
            else
            {
                results = "Error retrieving Google Maps API information from " + address + ". Error: " + message;
            }
        }
        catch (Exception ex)
        {
            // You can log the exception here to a file or DB
            results = "Unhandled error: " + ex.Message;
        }

        context.Response.ContentType = "text/plain";
        context.Response.Write(results);
    }

    protected bool GetLatitudeLongitudeFromGoogle(string address, out double latitude, out double longitude, out string message, out bool? isPartialMatch, out string street_number, out string street, out string city, out string state, out string country, out string postal_code, out string county, out string googlePlaceId, out string formattedAddress)
    {
        message = string.Empty;
        latitude = 0.0;
        longitude = 0.0;
        bool status = true;
        isPartialMatch = false;
        googlePlaceId = string.Empty;
        formattedAddress = string.Empty;
        county = string.Empty;
        street_number = string.Empty;
        street = string.Empty;
        city = string.Empty;
        state = string.Empty;
        country = string.Empty;
        postal_code = string.Empty;
        string mapApiKey = ConfigurationManager.AppSettings["mapApiKey"].ToString();

        try
        {
            status = true;

            var requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", Uri.EscapeDataString(address.Replace(" ", "+")), mapApiKey);

            WebRequest request = WebRequest.Create(requestUri);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader reader = new StreamReader(data);

            //latitude = locationElement.Element("lat").Value;
            //longitude = locationElement.Element("lng").Value;
            message = "success";
            string responseFromServer = reader.ReadToEnd();

            response.Close();
            GoogleMapsAPI.Places.RootObject locationInfo = JsonConvert.DeserializeObject<GoogleMapsAPI.Places.RootObject>(responseFromServer);
            var address_type = "administrative_area_level_2";
            var component = locationInfo != null &&
                            locationInfo.results != null &&
                            locationInfo.results[0].address_components != null
                            ? locationInfo.results[0].address_components.FirstOrDefault(o => o.types != null && o.types.Contains(address_type))
                            : null;
            county = component != null ? component.long_name : null;
            address_type = "street_number";
            component = locationInfo != null &&
                        locationInfo.results != null &&
                        locationInfo.results[0].address_components != null
                        ? locationInfo.results[0].address_components.FirstOrDefault(o => o.types != null && o.types.Contains(address_type))
                        : null;
            street_number = component != null ? component.long_name : null;


            address_type = "route";
            component = locationInfo != null &&
                        locationInfo.results != null &&

                        locationInfo.results[0].address_components != null
                        ? locationInfo.results[0].address_components.FirstOrDefault(o => o.types != null && o.types.Contains(address_type))
                        : null;
            street = component != null ? component.long_name : null;

            address_type = "locality";
            component = locationInfo != null &&
                        locationInfo.results != null &&

                        locationInfo.results[0].address_components != null
                        ? locationInfo.results[0].address_components.FirstOrDefault(o => o.types != null && o.types.Contains(address_type))
                        : null;
            city = component != null ? component.long_name : null;

            address_type = "administrative_area_level_1";
            component = locationInfo != null &&
                        locationInfo.results != null &&

                        locationInfo.results[0].address_components != null
                        ? locationInfo.results[0].address_components.FirstOrDefault(o => o.types != null && o.types.Contains(address_type))
                        : null;
            state = component != null ? component.long_name : null;

            address_type = "country";
            component = locationInfo != null &&
                        locationInfo.results != null &&

                        locationInfo.results[0].address_components != null
                        ? locationInfo.results[0].address_components.FirstOrDefault(o => o.types != null && o.types.Contains(address_type))
                        : null;
            country = component != null ? component.long_name : null;

            address_type = "postal_code";
            component = locationInfo != null &&
                        locationInfo.results != null &&

                        locationInfo.results[0].address_components != null
                        ? locationInfo.results[0].address_components.FirstOrDefault(o => o.types != null && o.types.Contains(address_type))
                        : null;
            postal_code = component != null ? component.long_name : null;

            foreach (var location in locationInfo.results)
            {
                isPartialMatch = location.partial_match.HasValue ? location.partial_match : false;
                latitude = location.geometry.location.lat;
                longitude = location.geometry.location.lng;
                googlePlaceId = location.place_id;
                formattedAddress = location.formatted_address;
            }
        }
        catch (Exception ex)
        {
            status = false;
            message = ex.Message + "<br>DATA:" + ex.Data + "<br>InnerException: " + ex.InnerException + "<br>Source: " + ex.Source + "<br>StackTrace: " + ex.StackTrace + "<br>TargetSite: " + ex.TargetSite;
        }
        return status;//return false if failed.
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}