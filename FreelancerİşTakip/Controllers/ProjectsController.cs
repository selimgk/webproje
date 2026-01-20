using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreelancerİşTakip.Data;
using FreelancerİşTakip.Models;

namespace FreelancerİşTakip.Controllers;

public class ProjectsController : Controller
{
    private readonly AppDbContext _context;

    public ProjectsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Projects
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.Projects.Include(p => p.Customer).Include(p => p.Proposal);
        return View(await appDbContext.ToListAsync());
    }

    // GET: Projects/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var project = await _context.Projects
            .Include(p => p.Customer)
            .Include(p => p.Proposal)
            .Include(p => p.Milestones.OrderBy(m => m.OrderNo))
            .Include(p => p.TimeLogs.OrderByDescending(t => t.WorkDate))
            .Include(p => p.ProjectFiles.OrderByDescending(f => f.UploadedAt))
            .FirstOrDefaultAsync(m => m.Id == id);

        if (project == null) return NotFound();

        return View(project);
    }

    // GET: Projects/CreateFromProposal/5
    public async Task<IActionResult> CreateFromProposal(int proposalId)
    {
        var proposal = await _context.Proposals.FindAsync(proposalId);
        if (proposal == null) return NotFound();

        // Check if project already exists for this proposal? (Optional)

        var project = new Project
        {
            CustomerId = proposal.CustomerId,
            ProposalId = proposal.Id,
            ProjectName = proposal.Title,
            RevisionLimit = proposal.RevisionLimit,
            StartDate = DateTime.Today,
            TargetEndDate = DateTime.Today.AddDays(proposal.DeliveryDays),
            Status = ProjectStatus.Active,
            Notes = "Tekliften oluşturuldu."
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Add Default Milestones
        var milestones = new List<Milestone>
        {
            new Milestone { ProjectId = project.Id, Title = "Analiz", Status = MilestoneStatus.Planned, DueDate = DateTime.Today.AddDays(1), OrderNo = 1 },
            new Milestone { ProjectId = project.Id, Title = "Tasarım", Status = MilestoneStatus.Planned, DueDate = DateTime.Today.AddDays(3), OrderNo = 2 },
            new Milestone { ProjectId = project.Id, Title = "Geliştirme", Status = MilestoneStatus.Planned, DueDate = DateTime.Today.AddDays(10), OrderNo = 3 },
            new Milestone { ProjectId = project.Id, Title = "Test & Teslim", Status = MilestoneStatus.Planned, DueDate = DateTime.Today.AddDays(15), OrderNo = 4 },
        };
        _context.Milestones.AddRange(milestones);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = project.Id });
    }

    // GET: Projects/Create
    public IActionResult Create()
    {
        ViewData["CustomerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Customers, "Id", "Name");
        return View();
    }

    // POST: Projects/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CustomerId,ProjectName,StartDate,TargetEndDate,Status,RevisionLimit,Notes")] Project project)
    {
        // Remove Proposal validation since it's optional for manual creation
        ModelState.Remove("Proposal");
        ModelState.Remove("Customer"); // EF Core relation handling

        if (ModelState.IsValid)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CustomerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Customers, "Id", "Name", project.CustomerId);
        return View(project);
    }

    // GET: Projects/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var project = await _context.Projects.FindAsync(id);
        if (project == null) return NotFound();
        return View(project);
    }

    // POST: Projects/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,ProposalId,ProjectName,StartDate,TargetEndDate,Status,RevisionLimit,UsedRevisions,Notes")] Project project)
    {
        if (id != project.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Details), new { id = project.Id });
        }
        return View(project);
    }

    // Sub-Resource Actions (Milestones)

    [HttpPost]
    public async Task<IActionResult> AddMilestone(int projectId, string title, DateTime dueDate, string status)
    {
        var milestone = new Milestone
        {
            ProjectId = projectId,
            Title = title,
            DueDate = dueDate,
            Status = Enum.Parse<MilestoneStatus>(status),
            OrderNo = 99 // Default
        };
        _context.Milestones.Add(milestone);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = projectId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteMilestone(int id, int projectId)
    {
        var m = await _context.Milestones.FindAsync(id);
        if (m != null)
        {
            _context.Milestones.Remove(m);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Details), new { id = projectId });
    }

    // Sub-Resource Actions (TimeLogs)

    [HttpPost]
    public async Task<IActionResult> AddTimeLog(int projectId, string description, int minutes, DateTime workDate)
    {
        var log = new TimeLog
        {
            ProjectId = projectId,
            Description = description,
            Minutes = minutes,
            WorkDate = workDate
        };
        _context.TimeLogs.Add(log);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = projectId });
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteTimeLog(int id, int projectId)
    {
        var t = await _context.TimeLogs.FindAsync(id);
        if (t != null)
        {
            _context.TimeLogs.Remove(t);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Details), new { id = projectId });
    }

    // Use Revision Logic
    [HttpPost]
    public async Task<IActionResult> UseRevision(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            project.UsedRevisions++;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Details), new { id });
    }

    // File Management Actions
    [HttpPost]
    public async Task<IActionResult> UploadFile(int projectId, IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            // Ensure directory exists
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            // Generate unique filename
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var projectFile = new ProjectFile
            {
                ProjectId = projectId,
                FileName = file.FileName,
                FilePath = "/uploads/" + uniqueFileName,
                UploadedBy = User.Identity?.Name ?? "Admin",
                UploadedAt = DateTime.Now
            };

            _context.ProjectFiles.Add(projectFile);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Details), new { id = projectId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFile(int id, int projectId)
    {
        var file = await _context.ProjectFiles.FindAsync(id);
        if (file != null)
        {
            // Optional: Delete physical file if needed
            // var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FilePath.TrimStart('/'));
            // if (System.IO.File.Exists(physicalPath)) System.IO.File.Delete(physicalPath);

            _context.ProjectFiles.Remove(file);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Details), new { id = projectId });
    }

    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.Id == id);
    }
}

