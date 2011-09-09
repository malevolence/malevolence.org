using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Malevolence.Core.Models
{
	public class Photo
	{
		public int ID { get; set; }
		public int AlbumID { get; set; }
		public virtual Album Album { get; set; }

		[ScaffoldColumn(false)]
		[StringLength(100)]
		public string FileName { get; set; }

		[StringLength(200)]
		public string Caption { get; set; }

		[Display(Name="Sort Order")]
		public int SortOrder { get; set; }
	}
}