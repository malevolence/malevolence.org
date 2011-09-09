using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Malevolence.Core.Models
{
	public class Album
	{
		public int ID { get; set; }

		[Required]
		[StringLength(200)]
		public string Title { get; set; }

		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		public string UrlSlug { get; set; }
		public DateTime CreateDate { get; internal set; }
		public int? CoverPhotoID { get; set; }
		public virtual Photo CoverPhoto { get; set; }
		public virtual ICollection<Photo> Photos { get; set; }

		public Album()
		{
			CreateDate = DateTime.Now;
			Photos = new List<Photo>();
		}

		public void AddPhoto(Photo photo)
		{
			photo.Album = this;
			Photos.Add(photo);
		}

		public void SetCoverPhoto(int id)
		{
			CoverPhotoID = id;
		}
	}
}
