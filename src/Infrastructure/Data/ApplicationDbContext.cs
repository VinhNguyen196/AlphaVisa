using System.Reflection;
using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Domain.Entities;
using AlphaVisa.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlphaVisa.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<ServiceItem> ServiceItems => Set<ServiceItem>();

    public DbSet<NewItem> NewItems => Set<NewItem>();

    public DbSet<ContactItem> ContactItems => Set<ContactItem>();

    public DbSet<AttachmentItem> AttachmentItems => Set<AttachmentItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
