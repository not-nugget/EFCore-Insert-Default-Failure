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

        var a = ctx.Entities.Update(new Entity
        {
            UUID = Guid.NewGuid()
        });

        var b = ctx.Entities.Update(new Entity
        {
            UUID = Guid.NewGuid()
        });

        var c = ctx.Entities.Update(new Entity
        {
            UUID = Guid.NewGuid()
        });

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
    public Guid Id { get; set; } = default!;

    [Column(TypeName = "binary(16)")]
    public Guid UUID { get; set; }
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
            .HasConversion(m => m.ToByteArray(), p => new Guid(p))
            .HasDefaultValueSql("(UUID_TO_BIN(UUID(), 1))");

        modelBuilder
            .Entity<Entity>()
            .Property(e => e.UUID)
            .HasConversion(m => m.ToByteArray(), p => new Guid(p));
    }
}