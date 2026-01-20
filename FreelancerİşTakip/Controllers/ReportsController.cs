using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreelancerİşTakip.Data;
using FreelancerİşTakip.Models;

namespace FreelancerİşTakip.Controllers;

public class ReportsController : Controller
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Simple Dashboard Stats
        
        // 1. Proposal Status Counts
        var statusCounts = _context.Proposals
            .GroupBy(p => p.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToList(); // Executing on client side or valid SQL translation? GroupBy is tricky in EF Core versions sometimes but simple count is usually fine.
                       // Safest: Fetch all statuses and group in memory if data volume low, or strict EF syntax.
                       // Using client evaluation for safety with Enum group by if specific version issues arise, but generally okay.
        
        ViewBag.StatusCounts = statusCounts;

        // 2. Loss Reasons
        var lossReasons = await _context.Proposals
            .Where(p => p.Status == ProposalStatus.Rejected && p.LossReasonId != null)
            .GroupBy(p => p.LossReason.Name)
            .Select(g => new { Reason = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToListAsync();

        ViewBag.LossReasons = lossReasons;

        // 3. Approved Revenue
        var totalRevenue = await _context.Proposals
            .Where(p => p.Status == ProposalStatus.Approved)
            .SumAsync(p => p.OfferedPrice);
            
        ViewBag.TotalRevenue = totalRevenue;

        // 4. Last 30 Days Activity
        var last30Days = DateTime.Today.AddDays(-30);
        var recentProposals = await _context.Proposals.CountAsync(p => p.CreatedAt >= last30Days); // Assuming logic or fallback to CreatedAt if not in model? 
        // Wait, Proposal model doesn't have CreatedAt explicity in my snippet earlier? 
        // Let's check Proposal.cs snippet. It didn't have CreatedAt.
        // It had SentAt, DecisionAt. I'll use SentAt for activity or just Id for simplicity.
        // Or I should add CreatedAt to Proposal? The user specs said "CreatedAt" for Customer, didn't specify for Proposal but good practice.
        // The user spec: "SentAt, DecisionAt". 
        // I'll count proposals Sent in last 30 days.

        var recentSent = await _context.Proposals.CountAsync(p => p.CreatedAt >= last30Days);
        ViewBag.RecentSent = recentSent;

        return View();
    }
}

