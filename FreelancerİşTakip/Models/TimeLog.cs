using System.ComponentModel.DataAnnotations;

namespace FreelancerİşTakip.Models;

public class TimeLog
{
    public int Id { get; set; }

    [Display(Name = "Proje")]
    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    [Display(Name = "Tarih")]
    public DateTime WorkDate { get; set; } = DateTime.Today;

    [Display(Name = "Süre (Dakika)")]
    public int Minutes { get; set; }

    [Display(Name = "Açıklama")]
    [Required]
    public string Description { get; set; } = string.Empty;

    // Helper to request duration in hours safely
    public double Hours => Minutes / 60.0;
}

