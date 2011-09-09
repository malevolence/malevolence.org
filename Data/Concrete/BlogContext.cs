using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Malevolence.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Malevolence.Data
{
	public class BlogContext : DbContext
	{
		public BlogContext() : base("SiteCN")
		{
		}

		public DbSet<Post> Posts { get; set; }
		public DbSet<Category> Categories { get; set; }
		
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Post>().ToTable("blog_Posts");
			modelBuilder.Entity<Category>().ToTable("blog_Categories");
			modelBuilder.Entity<Comment>().ToTable("blog_Comments");

			//modelBuilder.Configurations.Add<Post>(new PostMap());
			//modelBuilder.Configurations.Add<Category>(new CategoryMap());
			//modelBuilder.Configurations.Add<Comment>(new CommentMap());
		}

		internal class PostMap : EntityTypeConfiguration<Post>
		{
			public PostMap()
			{
				ToTable("blog_Posts");
			}
		}

		internal class CategoryMap : EntityTypeConfiguration<Category>
		{
			public CategoryMap()
			{
				ToTable("blog_Categories");
			}
		}

		internal class CommentMap : EntityTypeConfiguration<Comment>
		{
			public CommentMap()
			{
				ToTable("blog_Comments");
			}
		}
	}

}