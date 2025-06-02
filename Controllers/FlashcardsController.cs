using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlashcardApp.Data;
using FlashcardApp.Models;

namespace FlashcardApp.Controllers
{
    public class FlashcardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlashcardsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Flashcards
        public async Task<IActionResult> Index(string searchString, string categoryFilter)
        {
            var flashcards = from f in _context.Flashcards
                             select f;

            if (!string.IsNullOrEmpty(searchString))
            {
                flashcards = flashcards.Where(f => f.FrontSide.Contains(searchString)
                                        || f.BackSide.Contains(searchString)
                                        || (f.Category != null && f.Category.Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(categoryFilter))
            {
                flashcards = flashcards.Where(f => f.Category == categoryFilter);
            }

            // Kategorileri filtreleme dropdown'ı için ViewBag'e ekliyoruz
            ViewBag.Categories = await _context.Flashcards
                .Where(f => f.Category != null)
                .Select(f => f.Category)
                .Distinct()
                .ToListAsync();

            return View(await flashcards.ToListAsync());
        }

        // GET: Flashcards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flashcard = await _context.Flashcards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flashcard == null)
            {
                return NotFound();
            }

            return View(flashcard);
        }

// GET: Flashcards/Create
public IActionResult Create()
{
    // Boş bir model ile view'a gidiyoruz
    return View(new Flashcard());
}

        // POST: Flashcards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FrontSide,BackSide,Category,CreatedDate,LastReviewedDate")] Flashcard flashcard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flashcard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flashcard);
        }

// GET: Flashcards/Edit/5
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var flashcard = await _context.Flashcards.FindAsync(id);
    if (flashcard == null)
    {
        return NotFound();
    }
    return View(flashcard); // Modeli view'a gönderiyoruz
}

        // POST: Flashcards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FrontSide,BackSide,Category,CreatedDate,LastReviewedDate")] Flashcard flashcard)
        {
            if (id != flashcard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flashcard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlashcardExists(flashcard.Id))
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
            return View(flashcard);
        }

        // GET: Flashcards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flashcard = await _context.Flashcards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flashcard == null)
            {
                return NotFound();
            }

            return View(flashcard);
        }

        // POST: Flashcards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flashcard = await _context.Flashcards.FindAsync(id);
            if (flashcard != null)
            {
                _context.Flashcards.Remove(flashcard);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Flashcards/Review
        // Flashcard'ları gözden geçirme modu için action
        public async Task<IActionResult> Review()
        {
            var flashcards = await _context.Flashcards.ToListAsync();

            // Eğer hiç flashcard yoksa Index'e yönlendir
            if (!flashcards.Any())
            {
                TempData["Message"] = "No flashcards available for review. Please create some flashcards first.";
                return RedirectToAction(nameof(Index));
            }

            // Rastgele bir flashcard seç
            var random = new Random();
            var flashcard = flashcards[random.Next(flashcards.Count)];

            return View(flashcard);
        }

        // POST: Flashcards/Review
        // Flashcard gözden geçirme işlemini tamamlayan action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int id)
        {
            var flashcard = await _context.Flashcards.FindAsync(id);

            if (flashcard == null)
            {
                return NotFound();
            }

            // Son gözden geçirme tarihini güncelle
            flashcard.LastReviewedDate = DateTime.Now;
            _context.Update(flashcard);
            await _context.SaveChangesAsync();

            // Yeni bir flashcard ile tekrar Review sayfasına yönlendir
            return RedirectToAction(nameof(Review));
        }

        private bool FlashcardExists(int id)
        {
            return _context.Flashcards.Any(e => e.Id == id);
        }
    }
}
