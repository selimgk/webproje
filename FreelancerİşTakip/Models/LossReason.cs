using System.ComponentModel.DataAnnotations;

namespace FreelancerİşTakip.Models;

public class LossReason
{
    public int Id { get; set; }

    [Display(Name = "Kayıp Nedeni")]
    [Required]
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
}

