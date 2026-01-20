using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerİşTakip.Models;

public enum ProposalStatus
{
    Draft,
    Sent,
    Approved,
    Rejected,
    Expired
}

public class Proposal
{
    public int Id { get; set; }

    [Display(Name = "Müşteri")]
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    [Display(Name = "Başlık")]
    [Required]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Kapsam Metni")]
    public string? ScopeText { get; set; }

    [Display(Name = "Durum")]
    public ProposalStatus Status { get; set; } = ProposalStatus.Draft;

    [Display(Name = "Revizyon Hakkı")]
    public int RevisionLimit { get; set; }

    [Display(Name = "Saatlik Ücret")]
    public decimal HourlyRate { get; set; }

    [Display(Name = "Tahmini Saat")]
    public decimal EstimatedHours { get; set; }

    [Display(Name = "Önerilen Fiyat")]
    public decimal SuggestedPrice { get; set; }

    [Display(Name = "Teklif Edilen Fiyat")]
    public decimal OfferedPrice { get; set; }

    [Display(Name = "Para Birimi")]
    public string Currency { get; set; } = "TL";

    [Display(Name = "Teslim Süresi (Gün)")]
    public int DeliveryDays { get; set; }

    [Display(Name = "Gönderilme Tarihi")]
    public DateTime? SentAt { get; set; }

    [Display(Name = "Karar Tarihi")]
    public DateTime? DecisionAt { get; set; }

    [Display(Name = "Kayıp Nedeni")]
    public int? LossReasonId { get; set; }
    public LossReason? LossReason { get; set; }

    [Display(Name = "Kayıp Notu")]
    public string? LossNote { get; set; }

    [Display(Name = "Oluşturulma Tarihi")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

