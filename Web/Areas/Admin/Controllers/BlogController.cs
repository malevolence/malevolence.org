using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Malevolence.Data;
using Malevolence.Infrastructure.Logging;
using Malevolence.Core.Models;

namespace Malevolence.Web.Areas.Admin.Controllers
{
    public class BlogController : Controller
    {
		private IBlogRepository db;
		private ILogger logger;

		public BlogController(IBlogRepository repo, ILogger logger)
		{
			this.db = repo;
			this.logger = logger;
		}

        public ActionResult Index(int page = 1)
        {
			var posts = db.GetPosts(d => d.Category).OrderByDescending(x => x.ID);
			return View(posts);
		}

		public ActionResult Create()
		{
			AddCategoriesToViewBag();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Post post)
		{
			post.Author = User.Identity.Name;

			if (ModelState.IsValid)
			{
				db.SavePost(post);
				logger.Info(string.Format("New album added by {0} width ID = {1}", User.Identity.Name, post.ID));
				TempData["message"] = string.Format("New blog post saved with ID = {0}", post.ID);
				return RedirectToAction("Index");
			}

			AddCategoriesToViewBag();
			return View(post);
		}

		private void AddCategoriesToViewBag()
		{
			ViewBag.Categories = db.GetCategories().ToList();
		}
    }
}