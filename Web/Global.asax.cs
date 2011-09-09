using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Malevolence.Infrastructure.Logging;
using System.Web.Security;

namespace Malevolence.Web
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("favicon.ico");

			routes.MapRoute(
				"BlogDetail",
				"blog/post/{slug}",
				new { controller = "Blog", action = "Details" }
			);

			routes.MapRoute(
				"GalleryDetail",
				"gallery/show/{slug}",
				new { controller = "Gallery", action = "Details" }
			);

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
			ControllerBuilder.Current.DefaultNamespaces.Add("Malevolence.Web.Controllers");
			Logger.Info("Application is starting up...");
		}

		protected void Application_End()
		{
			Logger.Info("Application is shutting down...");
		}

		public ILogger Logger
		{
			get { return DependencyResolver.Current.GetService<ILogger>(); }
		}

		// To solve a problem with flash and authentication cookies
		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			try
			{
				string sessionParamName = "ASPSESSID";
				string sessionCookieName = "ASP.NET_SessionId";
				if (HttpContext.Current.Request.Form[sessionParamName] != null)
				{
					UpdateCookie(sessionCookieName, HttpContext.Current.Request.Form[sessionParamName]);
				}
				else if (HttpContext.Current.Request.QueryString[sessionParamName] != null)
				{
					UpdateCookie(sessionCookieName, HttpContext.Current.Request.QueryString[sessionParamName]);
				}
			}
			catch
			{
			}

			try
			{
				string authParamName = "AUTHID";
				string authCookieName = FormsAuthentication.FormsCookieName;

				if (HttpContext.Current.Request.Form[authParamName] != null)
				{
					UpdateCookie(authCookieName, HttpContext.Current.Request.Form[authParamName]);
				}
				else if (HttpContext.Current.Request.QueryString[authParamName] != null)
				{
					UpdateCookie(authCookieName, HttpContext.Current.Request.QueryString[authParamName]);
				}
			}
			catch
			{
			}
		}

		private void UpdateCookie(string cookieName, string cookieValue)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookieName);
			if (cookie == null)
			{
				cookie = new HttpCookie(cookieName);
			}

			cookie.Value = cookieValue;
			HttpContext.Current.Request.Cookies.Set(cookie);
		}
	}
}