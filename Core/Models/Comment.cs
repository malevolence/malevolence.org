using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Malevolence.Core.Models
{
	public class Comment
	{
		public int ID { get; set; }

		public int PostID { get; set; }
		public virtual Post Post { get; set; }

		[Required]
		[DisplayName("Email")]
		public string Author { get; set; }

		[StringLength(2000, MinimumLength = 10)]
		[Required(ErrorMessage = "You can't leave a comment without writing something.")]
		[DisplayName("Comment")]
		[DataType(DataType.MultilineText)]
		public string Text { get; set; }

		[DisplayName("Comment Added")]
		public DateTime CreatedOn { get; internal set; }

		[DisplayName("Is Approved")]
		public bool IsApproved { get; set; }

		[DisplayName("Is Featured")]
		public bool IsFeatured { get; set; }

		[DisplayName("IP Address")]
		public string IPAddress { get; set; }

		public Comment()
		{
			CreatedOn = DateTime.Now;
		}
	}
}
