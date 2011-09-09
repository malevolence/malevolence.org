﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Malevolence.Data;
using Malevolence.Infrastructure.Logging;
using Malevolence.Core.Models;

namespace Malevolence.Web.Controllers
{
    public class BlogController : Controller
    {
		private IBlogRepository db;

		public BlogController(IBlogRepository repo)
		{
			db = repo;
		}

        public ActionResult Index(int page = 1)
        {
            var posts = db.GetPosts().OrderByDescending(x => x.PublishedOn).ThenByDescending(x => x.ID);
			return View(posts);
        }

		public ActionResult Details(string slug)
		{
			var post = db.GetPostBySlug(slug);
			if (post == null)
				return HttpNotFound("No blog post found at this url");

			return View(post);
		}
    }
}
