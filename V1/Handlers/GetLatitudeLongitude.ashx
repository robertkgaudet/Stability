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
	public void ProcessRequest (HttpContext context)
	{
		string address  = (string)context.Request.QueryString["address"];
		string userId  = (string)context.Request.QueryString["userId"];
		string mapDomain = ConfigurationManager.AppSettings["mapDomain"].ToString();
		double latitude = 0.0;
		double longitude = 0.0;
		string message = string.Empty;
		string results = string.Empty;
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

		if(GetLatitudeLongitudeFromGoogle(address, out latitude, out longitude, out message, out isPartialMatch, out street_number, out street, out city, out state, out country, out postal_code, out county, out googlePlaceId, out formattedAddress))
		{

			//Check database for duplicate address.
			//If it doesn't exist add it.

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var duplicateAddress = from a in dc.Addresses
								   where a.GooglePlaceId == googlePlaceId
								   select a;

			if(duplicateAddress.Count() > 0)
			{
				//Address already exists.
				duplicate = true;
				addressId = duplicateAddress.Take(1).SingleOrDefault().AddressId.ToString();
			}
			else
			{
				//Add it.
				duplicate = false;
				Guid AddressId = Guid.NewGuid();
				Address newAddress = new Address();
				newAddress.AddressId = AddressId;
				newAddress.Address1 = street_number + " " + street;
				newAddress.City = city;
				newAddress.Country = country;
				newAddress.County = county;
				newAddress.CreatedBy = new Guid(userId);
				newAddress.CreatedOn = DateTime.Now;
				newAddress.FormattedAddress = formattedAddress;
				newAddress.GooglePlaceId = googlePlaceId;
				newAddress.IsActive = true;
				newAddress.Latitude = latitude.ToString();
				newAddress.Longitude = longitude.ToString();
				newAddress.State = state;
				newAddress.StreetName = street;
				newAddress.StreetNumber = street_number;
				newAddress.Zip = postal_code;
				dc.Addresses.InsertOnSubmit(newAddress);
				dc.SubmitChanges();
				addressId = AddressId.ToString();

				var rebuildStatus = (from rb in dc.RebuildStatus
									 where rb.Status == "House"
									 select new { rb.RebuildStatusId }).SingleOrDefault();

				var HomeRelationshipOwnRentType = (from rb in dc.RebuildStatus
												   where rb.Status == "Home Owner"
												   select new { rb.RebuildStatusId }).SingleOrDefault();

				var profile = (from p in dc.Profiles
							   where p.UserId == Guid.Parse(userId)
							   select new { p.ProfileId }).SingleOrDefault();

				//Add Address ID to Survivor Profile.
				ProfileAddress profileAddress = new ProfileAddress();
				profileAddress.AddressId = AddressId;
				profileAddress.HasFloodInsurance = false;
				profileAddress.HasHomeownersInsurance = false;
				profileAddress.ProfileAddressId = Guid.NewGuid();
				profileAddress.ProfileId = profile.ProfileId;
				profileAddress.HomeTypeId = rebuildStatus.RebuildStatusId;
				profileAddress.HomeRelationshipOwnRentTypeId = HomeRelationshipOwnRentType.RebuildStatusId;
				dc.ProfileAddresses.InsertOnSubmit(profileAddress);
				dc.SubmitChanges();
			}
			results = isPartialMatch  + "|" + latitude + "|" + longitude + "|" + street_number + "|" + street  + "|" + city  + "|" + state  + "|" + country  + "|" + postal_code  + "|" + county  + "|" + googlePlaceId  + "|" + formattedAddress + "|" + duplicate + "|" + addressId;
		}
		else
		{
			results = " Error retrieving Google Maps API information from " + address + " Error:" + message;
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
			var requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", Uri.EscapeDataString(address.Replace(" ","+")), mapApiKey);

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
			county = locationInfo.results[0]
								.address_components
								.FirstOrDefault(o => o.types.Contains(address_type))
								.long_name;

			address_type = "street_number";
			street_number = locationInfo.results[0]
								.address_components
								.FirstOrDefault(o => o.types.Contains(address_type))
								.long_name;

			address_type = "route";
			street = locationInfo.results[0]
								.address_components
								.FirstOrDefault(o => o.types.Contains(address_type))
								.long_name;

			address_type = "locality";
			city = locationInfo.results[0]
								.address_components
								.FirstOrDefault(o => o.types.Contains(address_type))
								.long_name;

			address_type = "administrative_area_level_1";
			state = locationInfo.results[0]
								.address_components
								.FirstOrDefault(o => o.types.Contains(address_type))
								.long_name;

			address_type = "country";
			country = locationInfo.results[0]
								.address_components
								.FirstOrDefault(o => o.types.Contains(address_type))
								.long_name;

			address_type = "postal_code";
			postal_code = locationInfo.results[0]
								.address_components
								.FirstOrDefault(o => o.types.Contains(address_type))
								.long_name;

			foreach(var location in locationInfo.results)
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
			message = ex.Message + "<br>DATA:" + ex.Data + "<br>InnerException: " + ex.InnerException + "<br>Source: " + ex.Source + "<br>StackTrace: " + ex.StackTrace  + "<br>TargetSite: " + ex.TargetSite;
		}
		return status;//return false if failed.
	}

	public bool IsReusable {
		get {
			return false;
		}
	}
}