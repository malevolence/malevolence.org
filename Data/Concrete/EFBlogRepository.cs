using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Malevolence.Core.Models;

namespace Malevolence.Data
{
	public class EFBlogRepository : IBlogRepository
	{
		BlogContext context = new BlogContext();

		public IQueryable<Post> GetPosts(params Expression<Func<Post, object>>[] includeProperties)
		{
			IQueryable<Post> query = context.Posts;

			foreach (var ip in includeProperties)
			{
				query = query.Include(ip);
			}

			return query;
		}

		public Post GetPostByID(int id)
		{
			return context.Posts.Find(id);
		}

		public Post GetPostBySlug(string slug)
		{
			return context.Posts.Where(x => x.UrlSlug == slug).FirstOrDefault();
		}

		public void DeletePost(int id)
		{
			var post = context.Posts.Find(id);
			context.Posts.Remove(post);
			context.SaveChanges();
		}

		public void SavePost(Post post)
		{
			if (post.ID == default(int))
			{
				context.Posts.Add(post);
			}
			else
			{
				context.Entry(post).State = EntityState.Modified;
			}

			context.SaveChanges();
		}

		public IQueryable<Category> GetCategories()
		{
			return context.Categories;
		}
	}
}