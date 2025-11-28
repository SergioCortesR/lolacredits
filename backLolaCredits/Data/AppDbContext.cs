using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Installment> Installments { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>()
            .HasIndex(p => p.CI)
            .IsUnique();

        modelBuilder.Entity<Person>()
            .HasMany(p => p.Loans)
            .WithOne(l => l.Person)
            .HasForeignKey(l => l.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Loan>()
            .HasMany(l => l.Installments)
            .WithOne(i => i.Loan)
            .HasForeignKey(i => i.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Installment>()
            .HasMany(i => i.Payments)
            .WithOne(p => p.Installment)
            .HasForeignKey(p => p.InstallmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Store Installment.Status as string in the DB for readability
        modelBuilder.Entity<Installment>()
            .Property(i => i.Status)
            .HasConversion<string>();
    }
}
