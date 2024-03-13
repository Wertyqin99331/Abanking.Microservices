using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public sealed class LikeConfiguration: IEntityTypeConfiguration<Like>
{
	public void Configure(EntityTypeBuilder<Like> builder)
	{
		builder.HasKey(l => new { l.UserId, l.PostId });

		builder.HasOne(l => l.Post)
			.WithMany(p => p.Likes)
			.HasForeignKey(l => l.PostId);

		builder.ToTable("Likes");
	}
}