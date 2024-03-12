using Domain.Entities;
using Domain.ValueObjects.Post;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class PostConfiguration: IEntityTypeConfiguration<Post>
{
	public void Configure(EntityTypeBuilder<Post> builder)
	{
		builder.HasKey(e => e.Id);

		builder.Property(e => e.Title)
			.HasConversion(t => t.Value,
				value => Title.Create(value).Value)
			.HasMaxLength(Title.TITLE_MAX_LENGTH);

		builder.Property(e => e.Text)
			.HasConversion(t => t.Value,
				value => Text.Create(value).Value)
			.HasMaxLength(Text.TEXT_MAX_LENGTH);

		builder.ToTable("Posts");
	}
}