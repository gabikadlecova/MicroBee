using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBee.Web.DAL.Context.Configuration
{
	public class MicroItemConfiguration : IEntityTypeConfiguration<MicroItem>
	{
		public void Configure(EntityTypeBuilder<MicroItem> builder)
		{
			builder.HasKey(m => m.Id);
			builder.HasOne(m => m.Category)
				.WithMany();

			builder.HasOne<ApplicationUser>()
				.WithMany(ap => ap.CreatedItems)
				.HasForeignKey(m => m.OwnerId);

			builder.HasOne<ApplicationUser>()
				.WithMany(ap => ap.AcceptedItems)
				.HasForeignKey(m => m.WorkerId);

			builder.Property(m => m.Price).HasColumnType("Money");

		}
	}
}
