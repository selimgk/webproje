namespace FreelancerİşTakip.Models;

public class HomeViewModel
{
    public int TotalProjects { get; set; }
    public int ActiveProjects { get; set; }
    public int TotalProposals { get; set; }
    public int PendingProposals { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<Project> RecentProjects { get; set; } = new();
}

