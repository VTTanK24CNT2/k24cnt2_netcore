
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VttLesson11.Models;

public class Vtt2410900069Controller : Controller
{
    private readonly VttEmployee2410900069Context _context;

    public Vtt2410900069Controller(VttEmployee2410900069Context context)
    {
        _context = context;
    }

    // GET: VTT2410900069S
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Vtt2410900069s.ToListAsync());
    }

    // GET: VTT2410900069S/Details/5
    public async Task<IActionResult> Details(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vtt2410900069 = await _context.Vtt2410900069s
            .FirstOrDefaultAsync(m => m.Id == id);
        if (vtt2410900069 == null)
        {
            return NotFound();
        }

        return View(vtt2410900069);
    }

    // GET: VTT2410900069S/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: VTT2410900069S/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,VttName,VttGender,VttBirthDay,VttEmail,VttPhone,VttActive")] Vtt2410900069 vtt2410900069)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vtt2410900069);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(vtt2410900069);
    }

    // GET: VTT2410900069S/Edit/5
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vtt2410900069 = await _context.Vtt2410900069s.FindAsync(id);
        if (vtt2410900069 == null)
        {
            return NotFound();
        }
        return View(vtt2410900069);
    }

    // POST: VTT2410900069S/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long? id, [Bind("Id,VttName,VttGender,VttBirthDay,VttEmail,VttPhone,VttActive")] Vtt2410900069 vtt2410900069)
    {
        if (id != vtt2410900069.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(vtt2410900069);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Vtt2410900069Exists(vtt2410900069.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(vtt2410900069);
    }

    // GET: VTT2410900069S/Delete/5
    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var vtt2410900069 = await _context.Vtt2410900069s
            .FirstOrDefaultAsync(m => m.Id == id);
        if (vtt2410900069 == null)
        {
            return NotFound();
        }

        return View(vtt2410900069);
    }

    // POST: VTT2410900069S/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long? id)
    {
        var vtt2410900069 = await _context.Vtt2410900069s.FindAsync(id);
        if (vtt2410900069 != null)
        {
            _context.Vtt2410900069s.Remove(vtt2410900069);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool Vtt2410900069Exists(long? id)
    {
        return _context.Vtt2410900069s.Any(e => e.Id == id);
    }
}
