using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroBee.Web.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBee.Web.DAL.Context.Configuration
{
	public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(ap => ap.UserName).IsRequired();
			builder.HasMany(ap => ap.CreatedItems)
				.WithOne();

			builder.HasMany(ap => ap.AcceptedItems)
				.WithOne();
		}
	}
}
