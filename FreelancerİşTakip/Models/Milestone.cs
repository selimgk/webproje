using System.ComponentModel.DataAnnotations;

namespace FreelancerİşTakip.Models;

public enum MilestoneStatus
{
    Planned,
    InProgress,
    Done
}

public class Milestone
{
    public int Id { get; set; }

    [Display(Name = "Proje")]
    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    [Display(Name = "Başlık")]
    [Required]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Display(Name = "Son Tarih")]
    public DateTime DueDate { get; set; }

    [Display(Name = "Durum")]
    public MilestoneStatus Status { get; set; } = MilestoneStatus.Planned;

    [Display(Name = "Sıra No")]
    public int OrderNo { get; set; }
}

