using System.Linq;
using MicroBee.Web.DAL.Context.Configuration;
using MicroBee.Web.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroBee.Web.DAL.Context
{
	public class MicroBeeDbContext : IdentityDbContext<ApplicationUser>
	{
		public MicroBeeDbContext(DbContextOptions<MicroBeeDbContext> options) : base(options)
		{
		}

		public DbSet<MicroItem> MicroItems { get; set; }
		public DbSet<ItemCategory> Categories { get; set; }
		public DbSet<ItemImage> Images { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			
			modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
			modelBuilder.ApplyConfiguration(new MicroItemConfiguration());

			modelBuilder.Entity<ItemCategory>().HasKey(c => c.Id);
		}
	}
}
