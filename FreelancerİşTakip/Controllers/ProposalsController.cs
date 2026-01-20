using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreelancerİşTakip.Data;
using FreelancerİşTakip.Models;

namespace FreelancerİşTakip.Controllers;

public class ProposalsController : Controller
{
    private readonly AppDbContext _context;

    public ProposalsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Proposals
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.Proposals.Include(p => p.Customer);
        return View(await appDbContext.ToListAsync());
    }

    // GET: Proposals/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var proposal = await _context.Proposals
            .Include(p => p.Customer)
            .Include(p => p.LossReason)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (proposal == null) return NotFound();

        return View(proposal);
    }

    // GET: Proposals/Contract/5
    public async Task<IActionResult> Contract(int? id)
    {
        if (id == null) return NotFound();

        var proposal = await _context.Proposals
            .Include(p => p.Customer)
            .Include(p => p.LossReason)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (proposal == null) return NotFound();

        return View(proposal);
    }

    // GET: Proposals/Create
    // Optional: templateId to pre-fill
    public async Task<IActionResult> Create(int? customerId, int? templateId)
    {
        var proposal = new Proposal();
        
        if (customerId.HasValue)
        {
            proposal.CustomerId = customerId.Value;
        }

        if (templateId.HasValue)
        {
            var template = await _context.ProposalTemplates.FindAsync(templateId.Value);
            if (template != null)
            {
                proposal.ScopeText = template.DefaultScopeText;
                proposal.HourlyRate = template.DefaultHourlyRate;
                proposal.EstimatedHours = template.DefaultEstimatedHours;
                proposal.RevisionLimit = template.DefaultRevisionLimit;
                proposal.DeliveryDays = template.DefaultDeliveryDays;
                // Auto calc suggested price
                proposal.SuggestedPrice = proposal.HourlyRate * proposal.EstimatedHours;
                proposal.OfferedPrice = proposal.SuggestedPrice; // Default to suggested
            }
        }

        ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", proposal.CustomerId);
        ViewData["TemplateId"] = new SelectList(_context.ProposalTemplates, "Id", "TemplateName", templateId);
        return View(proposal);
    }

    // POST: Proposals/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CustomerId,Title,ScopeText,Status,RevisionLimit,HourlyRate,EstimatedHours,SuggestedPrice,OfferedPrice,Currency,DeliveryDays")] Proposal proposal)
    {
        // Re-calculate suggested price to ensure integrity
        proposal.SuggestedPrice = proposal.HourlyRate * proposal.EstimatedHours;

        if (ModelState.IsValid)
        {
            _context.Add(proposal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", proposal.CustomerId);
        ViewData["TemplateId"] = new SelectList(_context.ProposalTemplates, "Id", "TemplateName"); 
        return View(proposal);
    }

    // GET: Proposals/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var proposal = await _context.Proposals.FindAsync(id);
        if (proposal == null) return NotFound();
        
        ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", proposal.CustomerId);
        ViewData["LossReasonId"] = new SelectList(_context.LossReasons, "Id", "Name", proposal.LossReasonId);
        return View(proposal);
    }

    // POST: Proposals/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,Title,ScopeText,Status,RevisionLimit,HourlyRate,EstimatedHours,SuggestedPrice,OfferedPrice,Currency,DeliveryDays,SentAt,DecisionAt,LossReasonId,LossNote")] Proposal proposal)
    {
        if (id != proposal.Id) return NotFound();

        proposal.SuggestedPrice = proposal.HourlyRate * proposal.EstimatedHours;

        if (ModelState.IsValid)
        {
            try
            {
                // Status logic updates
                if (proposal.Status == ProposalStatus.Sent && proposal.SentAt == null)
                {
                    proposal.SentAt = DateTime.Now;
                }
                if ((proposal.Status == ProposalStatus.Approved || proposal.Status == ProposalStatus.Rejected) && proposal.DecisionAt == null)
                {
                    proposal.DecisionAt = DateTime.Now;
                }

                _context.Update(proposal);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProposalExists(proposal.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", proposal.CustomerId);
        ViewData["LossReasonId"] = new SelectList(_context.LossReasons, "Id", "Name", proposal.LossReasonId);
        return View(proposal);
    }

    // GET: Proposals/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var proposal = await _context.Proposals
            .Include(p => p.Customer)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (proposal == null) return NotFound();

        return View(proposal);
    }

    // POST: Proposals/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var proposal = await _context.Proposals.FindAsync(id);
        if (proposal != null)
        {
            _context.Proposals.Remove(proposal);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool ProposalExists(int id)
    {
        return _context.Proposals.Any(e => e.Id == id);
    }
}

