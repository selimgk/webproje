using Microsoft.EntityFrameworkCore;
using FreelancerİşTakip.Models;

namespace FreelancerİşTakip.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<ProposalTemplate> ProposalTemplates { get; set; }
    public DbSet<Proposal> Proposals { get; set; }
    public DbSet<LossReason> LossReasons { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    public DbSet<TimeLog> TimeLogs { get; set; }
    public DbSet<ProjectFile> ProjectFiles { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API configurations (optional but good for precision)

        // Proposal -> Customer relationship
        modelBuilder.Entity<Proposal>()
            .HasOne(p => p.Customer)
            .WithMany(c => c.Proposals)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Project -> Customer relationship
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Customer)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.NoAction); // Avoid multiple cascade paths

        // Project -> Proposal relationship
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Proposal)
            .WithMany()
            .HasForeignKey(p => p.ProposalId)
            .OnDelete(DeleteBehavior.NoAction);

        // Seed LossReasons
        modelBuilder.Entity<LossReason>().HasData(
            new LossReason { Id = 1, Name = "Fiyat Yüksek" },
            new LossReason { Id = 2, Name = "Zamanlama Uymadı" },
            new LossReason { Id = 3, Name = "Güven Eksikliği" },
            new LossReason { Id = 4, Name = "Kapsam Belirsiz" },
            new LossReason { Id = 5, Name = "Rakip Seçildi" },
            new LossReason { Id = 6, Name = "Diğer" }
        );
    }
}

