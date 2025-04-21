using System.Web.Http;
using System.Web.Http.Cors;

namespace Stability
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API routes

            // Enable CORS for localhost:19006 with credentials support
            config.EnableCors(new EnableCorsAttribute("http://localhost:19006", "*", "*") { SupportsCredentials = true });
            // Enable attribute routing
            config.MapHttpAttributeRoutes();

			// Define default route for Web API
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			// Additional configuration settings can go here, such as:
			// - Adding custom formatters
			// - Configuring message handlers
			// - Enabling CORS (Cross-Origin Resource Sharing)
			// - Configuring dependency injection
		}
	}
}
