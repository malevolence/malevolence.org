using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Malevolence.Core.Models;

namespace Malevolence.Data
{
	public interface IBlogRepository
	{
		// Posts
		IQueryable<Post> GetPosts(params Expression<Func<Post, object>>[] includeProperties);
		Post GetPostByID(int id);
		Post GetPostBySlug(string slug);
		void DeletePost(int id);
		void SavePost(Post post);

		// Categories
		IQueryable<Category> GetCategories();
	}
}
