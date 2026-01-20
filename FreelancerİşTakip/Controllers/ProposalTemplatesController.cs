using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreelancerİşTakip.Data;
using FreelancerİşTakip.Models;

namespace FreelancerİşTakip.Controllers;

public class ProposalTemplatesController : Controller
{
    private readonly AppDbContext _context;

    public ProposalTemplatesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: ProposalTemplates
    public async Task<IActionResult> Index()
    {
        return View(await _context.ProposalTemplates.ToListAsync());
    }

    // GET: ProposalTemplates/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var proposalTemplate = await _context.ProposalTemplates
            .FirstOrDefaultAsync(m => m.Id == id);

        if (proposalTemplate == null) return NotFound();

        return View(proposalTemplate);
    }

    // GET: ProposalTemplates/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ProposalTemplates/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,TemplateName,DefaultScopeText,DefaultDeliveryDays,DefaultRevisionLimit,DefaultHourlyRate,DefaultEstimatedHours")] ProposalTemplate proposalTemplate)
    {
        if (ModelState.IsValid)
        {
            _context.Add(proposalTemplate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(proposalTemplate);
    }

    // GET: ProposalTemplates/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var proposalTemplate = await _context.ProposalTemplates.FindAsync(id);
        if (proposalTemplate == null) return NotFound();
        return View(proposalTemplate);
    }

    // POST: ProposalTemplates/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,TemplateName,DefaultScopeText,DefaultDeliveryDays,DefaultRevisionLimit,DefaultHourlyRate,DefaultEstimatedHours")] ProposalTemplate proposalTemplate)
    {
        if (id != proposalTemplate.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(proposalTemplate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProposalTemplateExists(proposalTemplate.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(proposalTemplate);
    }

    // GET: ProposalTemplates/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var proposalTemplate = await _context.ProposalTemplates
            .FirstOrDefaultAsync(m => m.Id == id);

        if (proposalTemplate == null) return NotFound();

        return View(proposalTemplate);
    }

    // POST: ProposalTemplates/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var proposalTemplate = await _context.ProposalTemplates.FindAsync(id);
        if (proposalTemplate != null)
        {
            _context.ProposalTemplates.Remove(proposalTemplate);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool ProposalTemplateExists(int id)
    {
        return _context.ProposalTemplates.Any(e => e.Id == id);
    }
}

