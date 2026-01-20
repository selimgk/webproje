using System.ComponentModel.DataAnnotations;

namespace FreelancerİşTakip.Models;

public class Customer
{
    public int Id { get; set; }

    [Display(Name = "Müşteri Adı")]
    [Required(ErrorMessage = "Müşteri adı zorunludur.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Yetkili Kişi")]
    public string? ContactName { get; set; }

    [Display(Name = "E-posta")]
    [EmailAddress]
    public string? Email { get; set; }

    [Display(Name = "Telefon")]
    [Phone]
    public string? Phone { get; set; }

    [Display(Name = "Şirket Adı")]
    public string? CompanyName { get; set; }

    [Display(Name = "Notlar")]
    public string? Notes { get; set; }

    [Display(Name = "Oluşturulma Tarihi")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}

