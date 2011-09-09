using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Malevolence.Core.Models
{
	public class Post
	{
		public int ID { get; set; }

		[Display(Name="Category")]
		public int CategoryID { get; set; }

		[Required]
		[StringLength(200)]
		public string Title { get; set; }

		[Display(Name="Url Slug")]
		public string UrlSlug { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		[AllowHtml]
		public string Body { get; set; }

		[DataType(DataType.MultilineText)]
		[AllowHtml]
		public string Excerpt { get; set; }

		[Display(Name="Published On")]
		public DateTime? PublishedOn { get; set; }

		[Display(Name="Is Published")]
		public bool IsPublished { get; set; }

		[Display(Name="Allow Comments")]
		public bool AllowComments { get; set; }

		[Display(Name="Is Featured")]
		public bool IsFeatured { get; set; }

		[ScaffoldColumn(false)]
		public string Author { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
		public virtual Category Category { get; set; }

		public Post()
		{
			Comments = new List<Comment>();
		}

		public void AddComment(Comment comment)
		{
			comment.Post = this;
			Comments.Add(comment);
		}
	}
}
