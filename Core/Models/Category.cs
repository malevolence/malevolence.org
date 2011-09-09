using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Malevolence.Core.Models
{
	public class Category
	{
		public int ID { get; set; }

		[Required]
		public string Title { get; set; }

		[DisplayName("Url Slug")]
		public string UrlSlug { get; set; }

		[DisplayName("Sort Order")]
		public int SortOrder { get; set; }
		public virtual ICollection<Post> Posts { get; set; }

		public Category()
		{
			Posts = new List<Post>();
		}
	}
}
