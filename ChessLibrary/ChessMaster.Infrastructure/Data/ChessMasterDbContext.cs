namespace ChessMaster.Infrastructure.Data;

public class ChessMasterDbContext : DbContext
{
    public ChessMasterDbContext(DbContextOptions<ChessMasterDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Game> Games => Set<Game>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChessMasterDbContext).Assembly);
    }
}