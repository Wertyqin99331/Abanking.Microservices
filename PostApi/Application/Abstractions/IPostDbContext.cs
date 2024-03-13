using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Abstractions;

public interface IPostDbContext
{
	DatabaseFacade Database { get; }
	
	DbSet<Post> Posts { get; set; }
	DbSet<Like> Likes { get; set; }
	
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}