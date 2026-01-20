using System.ComponentModel.DataAnnotations;

namespace FreelancerİşTakip.Models
{
    public class ProjectFile
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required]
        [Display(Name = "Dosya Adı")]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Dosya Yolu")]
        public string FilePath { get; set; } = string.Empty;

        [Display(Name = "Yükleyen")]
        public string UploadedBy { get; set; } = string.Empty;

        [Display(Name = "Yüklenme Tarihi")]
        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}

