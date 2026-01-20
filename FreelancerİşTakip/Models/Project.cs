using System.ComponentModel.DataAnnotations;

namespace FreelancerİşTakip.Models;

public enum ProjectStatus
{
    Active,
    Completed,
    OnHold
}

public class Project
{
    public int Id { get; set; }

    [Display(Name = "Müşteri")]
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    [Display(Name = "Teklif")]
    public int? ProposalId { get; set; }
    public Proposal? Proposal { get; set; }

    [Display(Name = "Proje Adı")]
    [Required]
    public string ProjectName { get; set; } = string.Empty;

    [Display(Name = "Başlangıç Tarihi")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Display(Name = "Hedef Bitiş")]
    public DateTime TargetEndDate { get; set; }

    [Display(Name = "Durum")]
    public ProjectStatus Status { get; set; } = ProjectStatus.Active;

    [Display(Name = "Revizyon Hakkı")]
    public int RevisionLimit { get; set; }

    [Display(Name = "Kullanılan Revizyon")]
    public int UsedRevisions { get; set; }

    [Display(Name = "Notlar")]
    public string? Notes { get; set; }

    // Navigation
    public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
    public ICollection<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
    public ICollection<ProjectFile> ProjectFiles { get; set; } = new List<ProjectFile>();
}

