using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Malevolence.Infrastructure.Logging
{
	public class LogUtility
	{
		public static string BuildExceptionMessage(Exception exc)
		{
			Exception logException = exc;
			if (exc.InnerException != null)
				logException = exc.InnerException;

			var sb = new StringBuilder();

			// Get querystring + virtual path
			sb.AppendFormat("Error in Path: {0}", HttpContext.Current.Request.Path);
			sb.AppendLine();
			sb.AppendFormat("Raw URL: {0}", HttpContext.Current.Request.RawUrl);
			sb.AppendLine();
			sb.AppendFormat("Message: {0}", logException.Message);
			sb.AppendLine();
			sb.AppendFormat("Source: {0}", logException.Source);
			sb.AppendLine();
			sb.AppendFormat("Stack Trace: {0}", logException.StackTrace);
			sb.AppendLine();
			sb.AppendFormat("TargetSite: {0}", logException.TargetSite);
			return sb.ToString();
		}
	}
}