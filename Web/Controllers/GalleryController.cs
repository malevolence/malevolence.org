using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Malevolence.Data;
using Malevolence.Core.Models;

namespace Malevolence.Web.Controllers
{
	public class GalleryController : Controller
	{
		private IGalleryRepository db;
		public GalleryController(IGalleryRepository repo)
		{
			this.db = repo;
		}

		public ActionResult Index()
		{
			return View(db.GetAlbums().OrderByDescending(x => x.CreateDate).ToList());
		}

		public ActionResult Details(string slug)
		{
			var album = db.GetAlbumBySlug(slug);
			if (album == null)
				return HttpNotFound("No album found at this url");

			album.Photos = album.Photos.OrderBy(x => x.SortOrder).ThenBy(x => x.ID).ToList();
			return View(album);
		}

		public ActionResult ShowPhoto(int id)
		{
			var photo = db.GetPhoto(id);
			if (photo == null || photo.ID == 0)
				return HttpNotFound("No photo found with that ID");

			return View(photo);
		}
	}
}