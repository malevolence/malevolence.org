using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Malevolence.Core.Models;
using System.Linq.Expressions;

namespace Malevolence.Data
{
	public class EFGalleryRepository : IGalleryRepository
	{
		GalleryContext context = new GalleryContext();

		public IEnumerable<Album> GetAlbums(params Expression<Func<Album, object>>[] includeProperties)
		{
			IQueryable<Album> query = context.Albums;
			foreach (var ip in includeProperties)
			{
				query = query.Include(ip);
			}
			return query.ToList();
		}

		public Album GetAlbumByID(int id)
		{
			return context.Albums.Find(id);
		}

		public Album GetAlbumBySlug(string slug)
		{
			return context.Albums.SingleOrDefault(x => x.UrlSlug == slug);
		}

		public void DeleteAlbum(int id)
		{
			var album = context.Albums.Find(id);
			context.Albums.Remove(album);
			context.SaveChanges();
		}

		public void SaveAlbum(Album album)
		{
			context.Entry(album).State = album.ID == 0 ? EntityState.Added : EntityState.Modified;
			context.SaveChanges();
		}

		public Photo GetPhoto(int id)
		{
			return context.Photos.Find(id);
		}

		public void SavePhoto(Photo photo)
		{
			context.Entry(photo).State = photo.ID == 0 ? EntityState.Added : EntityState.Modified;
			context.SaveChanges();
		}

		public void DeletePhoto(int id)
		{
			var photo = context.Photos.Find(id);
			context.Photos.Remove(photo);
			context.SaveChanges();
		}
	}
}
