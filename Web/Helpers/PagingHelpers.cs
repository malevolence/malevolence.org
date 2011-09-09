using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Malevolence.Web.Models;
using System.Text;

namespace Malevolence.Web.Helpers
{
	public static class PagingHelpers
	{
		public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
		{
			var sb = new StringBuilder();
			TagBuilder tag;
			if (pagingInfo.CurrentPage == 1)
			{
				// First
				tag = new TagBuilder("span");
				tag.InnerHtml = "&laquo; First";
				sb.AppendLine(tag.ToString());

				// Prev
				tag = new TagBuilder("span");
				tag.InnerHtml = "&lt; Prev";
				sb.AppendLine(tag.ToString());
			}
			else
			{
				// First
				tag = new TagBuilder("a");
				tag.MergeAttribute("href", pageUrl(1));
				tag.InnerHtml = "&laquo; First";
				sb.AppendLine(tag.ToString());

				// Prev
				tag = new TagBuilder("a");
				tag.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
				tag.InnerHtml = "&lt; Prev";
				sb.AppendLine(tag.ToString());
			}

			for (int i = 1; i <= pagingInfo.TotalPages; i++)
			{
				if (i == pagingInfo.CurrentPage)
				{
					tag = new TagBuilder("span");
					tag.AddCssClass("selected");
				}
				else
				{
					tag = new TagBuilder("a");
					tag.MergeAttribute("href", pageUrl(i));
				}
				tag.InnerHtml = i.ToString();
				sb.AppendLine(tag.ToString());
			}

			if (pagingInfo.CurrentPage == pagingInfo.TotalPages)
			{
				// Next
				tag = new TagBuilder("span");
				tag.InnerHtml = "Next &gt;";
				sb.AppendLine(tag.ToString());

				// Last
				tag = new TagBuilder("span");
				tag.InnerHtml = "Last &raquo;";
				sb.AppendLine(tag.ToString());
			}
			else
			{
				// Next
				tag = new TagBuilder("a");
				tag.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));
				tag.InnerHtml = "Next &gt;";
				sb.AppendLine(tag.ToString());

				// Last
				tag = new TagBuilder("a");
				tag.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
				tag.InnerHtml = "Last &raquo;";
				sb.AppendLine(tag.ToString());
			}

			return MvcHtmlString.Create(sb.ToString());
		}
	}
}