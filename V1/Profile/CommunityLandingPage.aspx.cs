using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;

public partial class V1_Profile_CommunityLandingPage : BaseWebForm
{
	protected async void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			string zip = "70503"; // Example ZIP code
			string city = "Lafayette"; // Example city name
			string state = "Louisiana"; // Example city name
			string stateCode = "LA";
			double latitude = 30.2168; // Example city name
			double longitude = -92.0182; // Example city name

			if (HttpContext.Current.User.Identity.IsAuthenticated)
			{
				using (var dc = new CrowdReliefDBDataContext())
				{
					var userInfo = (from p in dc.Profiles
									where p.UserId == userId
									select new { p.City, p.State })
									.Take(1)
									.SingleOrDefault();

					if (userInfo != null)
					{
						//Get the statecode

						stateCode = (from s in dc.USStates
										 where s.Name == userInfo.State
										 select s.Code).Take(1).SingleOrDefault();

						city = userInfo.City;
						state = userInfo.State;


						var geo = GeoHelper.GetLatLonZipFromCityState(city, stateCode);
						if (geo != null)
						{
							latitude = geo.Latitude;
							longitude = geo.Longitude;
							zip = geo.Zip;
						}

						litWeatherAdvisoryLocation.Text = "Statewide Weather Advisories: " + userInfo.State;
					}

					//if (userInfo != null)
					//{
					//	city = userInfo.City;
					//	stateCode = userInfo.StateCode;
					//	state = userInfo.State;
					//	zip = userInfo.Zip;
					//	latitude = Double.Parse(userInfo.Latitude);
					//	longitude = Double.Parse(userInfo.Longitude);
					//	litWeatherAdvisoryLocation.Text = "Statewide Weather Advisories: " + userInfo.State;
					//}

					var userGroups = (from g in dc.UserOrganizations
									  join o in dc.Organizations on g.OrganizationId equals o.OrganizationId
									  where g.UserId == userId && (g.Status == (int)RequestStatus.Approved || g.Status == (int)RequestStatus.Pending)
									  orderby g.IsPrimary descending, o.Name
									  select new
									  {
										  OrganizationName = o.Name,
										  UrlFriendlyTeamName = o.URLFriendlyName,
										  o.OrganizationId
									  }).Take(1).SingleOrDefault();

					// Check if userGroups is null
					if (userGroups == null)
					{
						// Hide the team button
						hypTeam.Visible = false;
					}
					else
					{
						// Construct the URL
						string teamUrl;
						if (string.IsNullOrEmpty(userGroups.UrlFriendlyTeamName))
						{
							// Use OrganizationId as a parameter if UrlFriendlyTeamName is missing
							teamUrl = "/V1/NonProfit/Default.aspx?organizationId=" + userGroups.OrganizationId;
						}
						else
						{
							// Use UrlFriendlyTeamName in the URL
							teamUrl = "/Team/" + userGroups.UrlFriendlyTeamName;
						}

						// Set the button's URL and make it visible
						hypTeam.NavigateUrl = teamUrl;
						hypTeam.Visible = true;
					}
				}
			}
			await LoadCommunitySnapshotAsync(zip, city, state);
//			Response.Write(state  + " - " + zip + " - " + city);
			var disasterAggregatorService = new DisasterAggregatorService();
			var events = disasterAggregatorService.GetLatestDisasters(stateCode.ToUpper());

			if(events.Count == 0)
			{
				litWeatherAdvisoryLocation.Text = "No weather advisories at this time: " + state;
			}

			rptDisasterEvents.DataSource = events;
			rptDisasterEvents.DataBind();

			var service = new DisasterAlertService();
			//var cards = service.GetDisasterCardsWithGuidance(state);
			var cards = service.GetDisasterCardsWithGuidance(latitude, longitude);
			if(cards.Count > 0)
			{ 
				rptDisasterCards.DataSource = cards;
				rptDisasterCards.DataBind();
			}

		}
	}

	public string GetCardCssClass(string severity, string urgency)
	{
		severity = severity.ToLower();
		urgency = urgency.ToLower();

		if (severity == "extreme" || urgency == "immediate") return "panel-danger";
		if (severity == "severe") return "panel-warning";
		if (severity == "moderate") return "panel-info";
		return "panel-default";
	}

	private string GetWeatherIcon(string condition)
	{
		condition = condition.ToLower();

		if (condition.Contains("sun")) return "☀️";
		if (condition.Contains("clear")) return "🌤️";
		if (condition.Contains("cloud")) return "☁️";
		if (condition.Contains("rain")) return "🌧️";
		if (condition.Contains("thunder")) return "⛈️";
		if (condition.Contains("snow")) return "❄️";
		if (condition.Contains("fog") || condition.Contains("mist")) return "🌫️";

		return "🌡️"; // default/fallback
	}
	public class ForecastDay
	{
		public string Day { get; set; }
		public string Temp { get; set; }
		public string Icon { get; set; }
		public string Condition { get; set; }
	}

	private async Task LoadCommunitySnapshotAsync(string zip, string city, string state)
	{
		string demonym;

		CivicNarrator.CommunityDemonyms.TryGetValue(city, out demonym);

		try
		{
			var snapshot = await StabilityCommunityFeed.GetCommunitySnapshotAsync(city);
			var demographics = await StabilityCommunityFeed.GetElderlyByZipAsync(zip);
			var forecast = await StabilityCommunityFeed.GetFiveDayForecastAsync(city);

			lblWeatherIcon.Text = GetWeatherIcon(snapshot.Weather.Condition);

			rptForecast.DataSource = forecast;
			rptForecast.DataBind();

			// 🔧 Convert to CivicNarrator-compatible types
			var weatherForNarrator = new CivicNarrator.WeatherInfo
			{
				WeatherInfoId = snapshot.Weather.WeatherInfoId,
				City = snapshot.Weather.City,
				Temperature = snapshot.Weather.Temperature,
				Condition = snapshot.Weather.Condition,
				RetrievedAt = snapshot.Weather.RetrievedAt
			};

			var demographicsForNarrator = new CivicNarrator.ElderlyDemographics
			{
				Name = demographics.Name,
				ElderlyCount = demographics.ElderlyCount,
				ZCTA = demographics.ZCTA,
				ElderlyPercentage = demographics.ElderlyPercentage,
				TotalPopulation = demographics.TotalPopulation

			};
			var forecastForNarrator = forecast.Select(f => new CivicNarrator.ForecastDay
			{
				Day = f.Day,
				Temp = f.Temp,
				Icon = f.Icon,
				Condition = f.Condition
			}).ToList();

			// Basic UI

			// Forecast binding
			rptForecast.DataSource = forecast;
			rptForecast.DataBind();

			// New narrative
			string narrative = await CivicNarrator.GenerateWeatherAndDemographicsNarrativeAsync(weatherForNarrator, demographicsForNarrator, forecastForNarrator, demonym);
			txtNarrative.Text = narrative;


			string timeWord = GetTimeOfDayLabel(DateTime.Now);
			string briefWord = GetRandomBriefWord();

			lblHello.Text = timeWord + " " + briefWord;
			lblCity.Text = city + ", " + state;
			lblTemp.Text = Math.Round(snapshot.Weather.Temperature).ToString();
			lblElderlyCount.Text = demographics.ElderlyCount.ToString("N0");
			lblTotalPop.Text = demographics.TotalPopulation.ToString("N0");
			lblElderlyPct.Text = demographics.ElderlyPercentage.ToString("0.0") + "%";
			lblZip.Text = "For Zip " + zip;
		}
		catch (Exception ex)
		{
			txtNarrative.Text = "We’re having trouble fetching today’s update.";
		}

		await Task.CompletedTask;
	}

	private string GetRandomBriefWord()
	{
		string[] options = { "Brief", "Update", "Recap", "Outlook", "Snapshot", "Wrap", "Digest", "Summary" };
		Random rand = new Random();
		return options[rand.Next(options.Length)];
	}

	private static readonly Random _random = new Random();

	private string GetTimeOfDayLabel(DateTime now)
	{
		int hour = now.Hour;
		int variation = _random.Next(1, 6); // 1 to 5

		switch (variation)
		{
			case 1: return GetTimeOfDay_Variant1(hour);
			case 2: return GetTimeOfDay_Variant2(hour);
			case 3: return GetTimeOfDay_Variant3(hour);
			case 4: return GetTimeOfDay_Variant4(hour);
			default: return GetTimeOfDay_Variant5(hour);
		}
	}

	private string GetTimeOfDay_Variant1(int hour)
	{
		if (hour < 1) return "After Midnight";
		if (hour < 3) return "Still Up?";
		if (hour < 5) return "Crack of Dawn";
		if (hour < 9) return "Morning";
		if (hour < 12) return "Mid Morning";
		if (hour < 15) return "Lunch Hour";
		if (hour < 17) return "Tea Time";
		if (hour < 21) return "Sunset Hours";
		if (hour < 23) return "Almost Bedtime";
		return "Late Night";
	}

	private string GetTimeOfDay_Variant2(int hour)
	{
		if (hour < 1) return "Midnight Watch";
		if (hour < 3) return "Night Owl Zone";
		if (hour < 5) return "Dawn Patrol";
		if (hour < 9) return "Fresh Start";
		if (hour < 12) return "Pre-Lunch";
		if (hour < 15) return "Post-Lunch Dip";
		if (hour < 17) return "Wrapping Up";
		if (hour < 21) return "Prime Time";
		if (hour < 23) return "Winding Down";
		return "Nightcap Hour";
	}

	private string GetTimeOfDay_Variant3(int hour)
	{
		if (hour < 1) return "Witching Hour";
		if (hour < 3) return "Nocturnal Zone";
		if (hour < 5) return "Rooster's Call";
		if (hour < 9) return "Early Rush";
		if (hour < 12) return "Midday Build";
		if (hour < 15) return "Afternoon Glide";
		if (hour < 17) return "Late Afternoon";
		if (hour < 21) return "Evening Ease";
		if (hour < 23) return "Late Chill";
		return "Dark Hours";
	}

	private string GetTimeOfDay_Variant4(int hour)
	{
		if (hour < 1) return "Zero Hour";
		if (hour < 3) return "Moonlight Time";
		if (hour < 5) return "Before Sunrise";
		if (hour < 9) return "New Day";
		if (hour < 12) return "Morning Rush";
		if (hour < 15) return "Quiet Period";
		if (hour < 17) return "Late Day";
		if (hour < 21) return "Evening Light";
		if (hour < 23) return "Dimming Light";
		return "Final Hour";
	}

	private string GetTimeOfDay_Variant5(int hour)
	{
		if (hour < 1) return "Graveyard Shift";
		if (hour < 3) return "Dreamland";
		if (hour < 5) return "Sunrise Seekers";
		if (hour < 9) return "Bright Morning";
		if (hour < 12) return "Golden Hours";
		if (hour < 15) return "High Noon";
		if (hour < 17) return "Chores Hour";
		if (hour < 21) return "Family Time";
		if (hour < 23) return "Evening Wrap";
		return "Sleep Zone";
	}



}