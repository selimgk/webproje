using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FreelancerİşTakip.Models;

namespace FreelancerİşTakip.Controllers;

[Microsoft.AspNetCore.Authorization.AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FreelancerİşTakip.Data.AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, FreelancerİşTakip.Data.AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        // 1. Active Clients (Total Count)
        var activeClients = _context.Customers.Count();

        // 2. Active Projects
        var activeProjects = _context.Projects.Count(p => p.Status == ProjectStatus.Active);

        // 3. Pending Proposals (Draft or Sent)
        var pendingProposals = _context.Proposals.Count(p => p.Status == ProposalStatus.Draft || p.Status == ProposalStatus.Sent);

        // 4. Monthly Revenue (Approved Proposals decided in current month)
        var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var monthlyRevenue = _context.Proposals
            .Where(p => p.Status == ProposalStatus.Approved && p.DecisionAt >= startOfMonth)
            .Sum(p => p.OfferedPrice);

        // Pass to View
        ViewBag.ActiveClients = activeClients;
        ViewBag.ActiveProjects = activeProjects;
        ViewBag.PendingProposals = pendingProposals;
        ViewBag.MonthlyRevenue = monthlyRevenue;

        return View();
    }
    public IActionResult cv()
    {
        return View();
    }
    public string isim()
    {
        return "selim";
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

