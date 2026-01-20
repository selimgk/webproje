using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreelancerİşTakip.Data;
using FreelancerİşTakip.Models;

namespace FreelancerİşTakip.Controllers;

public class CustomersController : Controller
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Customers
    public async Task<IActionResult> Index()
    {
        return View(await _context.Customers.ToListAsync());
    }

    // GET: Customers/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var customer = await _context.Customers
            .Include(c => c.Proposals)
            .Include(c => c.Projects)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (customer == null) return NotFound();

        return View(customer);
    }

    // GET: Customers/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Customers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,ContactName,Email,Phone,CompanyName,Notes")] Customer customer)
    {
        if (ModelState.IsValid)
        {
            _context.Add(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    // GET: Customers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound();
        return View(customer);
    }

    // POST: Customers/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ContactName,Email,Phone,CompanyName,Notes,CreatedAt")] Customer customer)
    {
        if (id != customer.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    // GET: Customers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var customer = await _context.Customers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (customer == null) return NotFound();

        return View(customer);
    }

    // POST: Customers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Fetch customer with all top-level relationships
        var customer = await _context.Customers
            .Include(c => c.Proposals)
            .Include(c => c.Projects)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (customer != null)
        {
            // 1. Delete Related Proposals
            if (customer.Proposals != null && customer.Proposals.Any())
            {
                _context.Proposals.RemoveRange(customer.Proposals);
            }

            // 2. Delete Related Projects
            if (customer.Projects != null && customer.Projects.Any())
            {
                _context.Projects.RemoveRange(customer.Projects);
            }

            // Save intermediate changes (optional, but good for batching if needed, 
            // though EF Core can batch all in one SaveChanges usually. 
            // Keeping it one transaction is better).
            
            // 3. Delete Customer
            _context.Customers.Remove(customer);
            
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool CustomerExists(int id)
    {
        return _context.Customers.Any(e => e.Id == id);
    }
}

