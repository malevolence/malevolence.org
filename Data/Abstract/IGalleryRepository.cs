using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Malevolence.Core.Models;
using System.Linq.Expressions;

namespace Malevolence.Data
{
	public interface IGalleryRepository
	{
		IEnumerable<Album> GetAlbums(params Expression<Func<Album, object>>[] includeProperties);
		Album GetAlbumByID(int id);
		Album GetAlbumBySlug(string slug);
		void DeleteAlbum(int id);
		void SaveAlbum(Album album);
		Photo GetPhoto(int id);
		void SavePhoto(Photo photo);
		void DeletePhoto(int id);
	}
}
