using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Malevolence.Data;
using Malevolence.Infrastructure.Logging;
using Malevolence.Core.Models;
using PagedList;

namespace Malevolence.Web.Areas.Admin.Controllers
{
    [Authorize]
	public class GalleryController : Controller
    {
		private IGalleryRepository db;
		private ILogger logger;
		private int pageSize = 10;

		public GalleryController(IGalleryRepository repo, ILogger logger)
		{
			db = repo;
			this.logger = logger;
		}

		public ActionResult Index(int page = 1)
        {
			var albums = db.GetAlbums().OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
			return View(albums);
        }

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(Album album)
		{
			if (ModelState.IsValid)
			{
				db.SaveAlbum(album);
				logger.Info(string.Format("New album added by {0} width ID = {1}", User.Identity.Name, album.ID));
				TempData["message"] = string.Format("New album saved with ID = {0} at {1:g}", album.ID, album.CreateDate);
				return RedirectToAction("Index");
			}

			return View(album);
		}

		public ActionResult Edit(int id)
		{
			var album = db.GetAlbumByID(id);
			if (album == null || album.ID == 0)
				return HttpNotFound("No album found with that ID");

			album.Photos = album.Photos.OrderBy(x => x.SortOrder).ThenBy(x => x.ID).ToList();
			return View(album);
		}

		[HttpPost]
		public ActionResult Edit(int id, FormCollection form)
		{
			var album = db.GetAlbumByID(id);
			var whitelist = new string[] { "Title", "Description" };
			if (TryUpdateModel(album, whitelist, form))
			{
				db.SaveAlbum(album);
				logger.Info(string.Format("Changes were made by {0} to album with ID = {1}", User.Identity.Name, id));
				TempData["message"] = string.Format("Changes to album with ID = {0} saved successfully at {1:g}", id, DateTime.Now);
				return RedirectToAction("Edit", new { id = id });
			}

			return View(album);
		}

		public ActionResult AddPhoto(int id)
		{
			return View(id);
		}

		[HttpPost]
		public ActionResult AddPhoto(int id, HttpPostedFileBase file)
		{
			if (id > 0 && file != null)
			{
				string extension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
				string fileName = string.Format("{0}.{1}", Guid.NewGuid(), extension);
				string fullPath = string.Format(@"{0}\content\images\photos\orig\{1}", Request.PhysicalApplicationPath, fileName);
				string origPath = string.Format("/content/images/photos/orig/{0}", fileName);
				string resizePath = string.Format("/content/images/photos/{0}", fileName);
				string thumbPath = string.Format("/content/images/photos/thumbs/{0}", fileName);

				string mimeType = file.ContentType;

				file.SaveAs(fullPath);

				ImageUtils.CreateZoomedThumbnail(origPath, thumbPath, 150);
				ImageUtils.ResizeImage(origPath, resizePath, 720, false, "");

				var album = db.GetAlbumByID(id);
				var photo = new Photo();
				photo.FileName = fileName;
				if (album.Photos.Count > 0)
					photo.SortOrder = album.Photos.Max(x => x.SortOrder) + 1;
				album.AddPhoto(photo);
				db.SaveAlbum(album);
				TempData["message"] = string.Format("New photo added to album with filename = {0}", fileName);
				return RedirectToAction("Edit", new { id = id });
			}

			return View();
		}

		[HttpPost]
		public string AddPhotoAjax(int id, HttpPostedFileBase file)
		{
			if (id > 0 && file != null)
			{
				string extension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
				string fileName = string.Format("{0}.{1}", Guid.NewGuid(), extension);
				string fullPath = string.Format(@"{0}\content\images\photos\orig\{1}", Request.PhysicalApplicationPath, fileName);
				string origPath = string.Format("/content/images/photos/orig/{0}", fileName);
				string resizePath = string.Format("/content/images/photos/{0}", fileName);
				string thumbPath = string.Format("/content/images/photos/thumbs/{0}", fileName);

				string mimeType = file.ContentType;

				file.SaveAs(fullPath);

				ImageUtils.CreateZoomedThumbnail(origPath, thumbPath, 150);
				ImageUtils.ResizeImage(origPath, resizePath, 720, false, "");

				var album = db.GetAlbumByID(id);
				var photo = new Photo();
				photo.FileName = fileName;
				if (album.Photos.Count > 0)
					photo.SortOrder = album.Photos.Max(x => x.SortOrder) + 1;
				album.AddPhoto(photo);
				db.SaveAlbum(album);
				return "Photo saved";
			}
			return "No file found";
		}

		[HttpPost]
		public ActionResult SetCoverPhoto(int albumID, int photoID)
		{
			var album = db.GetAlbumByID(albumID);
			album.SetCoverPhoto(photoID);
			db.SaveAlbum(album);
			logger.Info(string.Format("Cover photo set by {0} on album with ID = {1}", User.Identity.Name, albumID));
			return Json(new { status = string.Format("Photo with id = {0} set as cover photo for album", photoID) });
		}

		[HttpPost]
		public ActionResult SaveSortOrder(int[] photos)
		{
			string result = "pre-processing";
			var list = photos.ToList();
			try
			{
				foreach (int p in list)
				{
					var photo = db.GetPhoto(p);
					photo.SortOrder = list.IndexOf(p);
					db.SavePhoto(photo);
				}

				logger.Info(string.Format("Sort order updated by {0} on album containing photo with ID = {1}", User.Identity.Name, photos[0]));
				result = "Photo sort order was saved successfully";
			}
			catch (Exception exc)
			{
				result = exc.Message;
			}

			return Json(new { status = result });
		}

		[HttpPost]
		public ActionResult SaveCaption(int id, string caption)
		{
			var photo = db.GetPhoto(id);
			photo.Caption = caption;
			db.SavePhoto(photo);
			logger.Info(string.Format("Caption updated by {0} on photo with ID = {1}", User.Identity.Name, id));
			return Json(new { status = "success" });
		}

		[HttpPost]
		public ActionResult DeletePhoto(int id)
		{
			string fileName = "";
			var photo = db.GetPhoto(id);
			if (photo != null && photo.ID > 0)
				fileName = photo.FileName;

			db.DeletePhoto(id);

			// Delete the actual files (orig, normal, and thumb)
			string origPath = string.Format(@"{0}\content\images\photos\orig\{1}", Request.PhysicalApplicationPath, fileName);
			string normalPath = string.Format(@"{0}\content\images\photos\{1}", Request.PhysicalApplicationPath, fileName);
			string thumbPath = string.Format(@"{0}\content\images\photos\thumbs\{1}", Request.PhysicalApplicationPath, fileName);

			try
			{
				System.IO.File.Delete(origPath);
				System.IO.File.Delete(normalPath);
				System.IO.File.Delete(thumbPath);
				logger.Info("File deleted: " + fileName);
			}
			catch (Exception exc)
			{
				logger.Error(exc.Message);
			}

			return Json(new { status = "success" });
		}
    }
}