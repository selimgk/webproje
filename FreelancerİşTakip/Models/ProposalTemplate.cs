using System.ComponentModel.DataAnnotations;

namespace FreelancerİşTakip.Models;

public class ProposalTemplate
{
    public int Id { get; set; }

    [Display(Name = "Şablon Adı")]
    [Required(ErrorMessage = "Şablon adı zorunludur.")]
    public string TemplateName { get; set; } = string.Empty;

    [Display(Name = "Varsayılan Kapsam Metni")]
    public string? DefaultScopeText { get; set; }

    [Display(Name = "Varsayılan Teslim Süresi (Gün)")]
    public int DefaultDeliveryDays { get; set; }

    [Display(Name = "Varsayılan Revizyon Hakkı")]
    public int DefaultRevisionLimit { get; set; }

    [Display(Name = "Varsayılan Saatlik Ücret")]
    public decimal DefaultHourlyRate { get; set; }

    [Display(Name = "Varsayılan Tahmini Saat")]
    public decimal DefaultEstimatedHours { get; set; }
}

