using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Malevolence.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Malevolence.Data
{
	public class GalleryContext : DbContext
	{
		public GalleryContext() : base("SiteCN")
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Configurations.Add<Album>(new AlbumMap());
			modelBuilder.Configurations.Add<Photo>(new PhotoMap());
		}

		public DbSet<Album> Albums { get; set; }
		public DbSet<Photo> Photos { get; set; }

		internal class AlbumMap : EntityTypeConfiguration<Album>
		{
			public AlbumMap()
			{
				ToTable("gallery_Albums");
				HasOptional(p => p.CoverPhoto).WithMany().HasForeignKey(x => x.CoverPhotoID).WillCascadeOnDelete(false);
			}
		}

		internal class PhotoMap : EntityTypeConfiguration<Photo>
		{
			public PhotoMap()
			{
				ToTable("gallery_Photos");
				HasRequired(p => p.Album).WithMany(g => g.Photos).HasForeignKey(p => p.AlbumID);
			}
		}
	}
}