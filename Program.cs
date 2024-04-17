using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Serilog;

namespace Scratch;

internal class Program
{
    static async Task Main()
    {
        var logging = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(new LoggerConfiguration().WriteTo.Console().WriteTo.Debug().CreateLogger(), false);
        });

        Context ctx = new(logging);

        using var transaction = await ctx.Database.BeginTransactionAsync();

        var a = await ctx.Entities.AddAsync(new Entity());

        var b = await ctx.Entities.AddAsync(new Entity());

        var c = await ctx.Entities.AddAsync(new Entity());

        try
        {
            await ctx.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();
            _ = ex;
        }
    }
}

[PrimaryKey(nameof(Id))]
sealed class Entity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "binary(16)")]
    public EntityId Id { get; set; } = default!;

    //[Column(TypeName = "binary(16)")]
    //public Guid UUID { get; set; }
}

sealed class EntityId : IEquatable<EntityId>
{
    public Guid Value { get; set; }

    public static EntityId FromGuid(Guid id) => new() { Value = id };

    public bool Equals(EntityId? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as EntityId);
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(EntityId? lhs, EntityId? rhs) => (lhs is null && rhs is null) || lhs?.Value == rhs?.Value;
    public static bool operator !=(EntityId? lhs, EntityId? rhs) => !(lhs == rhs);
}

sealed class Context : DbContext
{
    private readonly ILoggerFactory? _logging;

    public DbSet<Entity> Entities { get; set; }

    public Context() { }
    public Context(ILoggerFactory? logging) => _logging = logging;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_logging);
        optionsBuilder.UseMySql("Server=localhost;Database=scratch;Uid=scratch;Pwd=scratch", ServerVersion.AutoDetect("Server=localhost;Database=scratch;Uid=scratch;Pwd=scratch"));
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Entity>()
            .Property(e => e.Id)
            .HasConversion(m => m.Value.ToByteArray(), p => EntityId.FromGuid(new Guid(p)))
            .HasDefaultValueSql("(UUID_TO_BIN(UUID(), 1))");

        //modelBuilder
        //    .Entity<Entity>()
        //    .Property(e => e.UUID)
        //    .HasConversion(m => m.ToByteArray(), p => new Guid(p));
    }
}