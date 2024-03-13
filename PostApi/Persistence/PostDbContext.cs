using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class PostDbContext: DbContext, IPostDbContext
{
	public PostDbContext() { }
	public PostDbContext(DbContextOptions<PostDbContext> options): base(options) {}
	
	public DbSet<Post> Posts { get; set; } = null!;
	public DbSet<Like> Likes { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.HasDefaultSchema("post");

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostDbContext).Assembly);
	}
}