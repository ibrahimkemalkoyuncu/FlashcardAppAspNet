//using FlashcardApp.Data;
//using FlashcardApp.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FlashcardApp.Controllers
//{
//    [Authorize]
//    public class FlashcardsController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public FlashcardsController(
//            ApplicationDbContext context,
//            UserManager<ApplicationUser> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//        }

//        // GET: Flashcards
//        public async Task<IActionResult> Index(string searchString, string categoryFilter)
//        {
//            var currentUser = await _userManager.GetUserAsync(User);

//            var flashcardsQuery = _context.Flashcards
//                .Include(f => f.CategoryObj)
//                .Where(f => f.UserId == currentUser.Id)
//                .AsQueryable();

//            // Arama filtresi
//            if (!string.IsNullOrEmpty(searchString))
//            {
//                flashcardsQuery = flashcardsQuery.Where(f =>
//                    f.FrontSide.Contains(searchString) ||
//                    f.BackSide.Contains(searchString) ||
//                    (f.Category != null && f.Category.Contains(searchString)) ||
//                    (f.CategoryObj != null && f.CategoryObj.Name.Contains(searchString)));
//            }

//            // Kategori filtresi
//            if (!string.IsNullOrEmpty(categoryFilter))
//            {
//                flashcardsQuery = flashcardsQuery.Where(f =>
//                    f.Category == categoryFilter ||
//                    (f.CategoryObj != null && f.CategoryObj.Name == categoryFilter));
//            }

//            // Kategorileri dropdown için hazırla
//            var categories = await _context.Categories
//                .Select(c => c.Name)
//                .Union(_context.Flashcards.Select(f => f.Category))
//                .Where(c => c != null)
//                .Distinct()
//                .OrderBy(c => c)
//                .ToListAsync();

//            ViewBag.Categories = new SelectList(categories);

//            var flashcards = await flashcardsQuery
//                .OrderByDescending(f => f.CreatedDate)
//                .ToListAsync();

//            return View(flashcards);
//        }

//        // GET: Flashcards/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var currentUser = await _userManager.GetUserAsync(User);
//            var flashcard = await _context.Flashcards
//                .Include(f => f.CategoryObj)
//                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == currentUser.Id);

//            if (flashcard == null)
//            {
//                return NotFound();
//            }

//            return View(flashcard);
//        }

//        // GET: Flashcards/Create
//        public async Task<IActionResult> Create()
//        {
//            await PrepareCategoryDropdown();
//            return View();
//        }

//        // POST: Flashcards/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,FrontSide,BackSide,Category,CategoryId")] Flashcard flashcard)
//        {
//            var currentUser = await _userManager.GetUserAsync(User);

//            if (ModelState.IsValid)
//            {
//                flashcard.UserId = currentUser.Id;
//                flashcard.CreatedDate = DateTime.Now;

//                _context.Add(flashcard);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }

//            await PrepareCategoryDropdown();
//            return View(flashcard);
//        }

//        // GET: Flashcards/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var currentUser = await _userManager.GetUserAsync(User);
//            var flashcard = await _context.Flashcards
//                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == currentUser.Id);

//            if (flashcard == null)
//            {
//                return NotFound();
//            }

//            await PrepareCategoryDropdown(flashcard.CategoryId);
//            return View(flashcard);
//        }

//        // POST: Flashcards/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,FrontSide,BackSide,Category,CategoryId,UserId,CreatedDate")] Flashcard flashcard)
//        {
//            if (id != flashcard.Id)
//            {
//                return NotFound();
//            }

//            var currentUser = await _userManager.GetUserAsync(User);
//            if (flashcard.UserId != currentUser.Id)
//            {
//                return Forbid();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(flashcard);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!FlashcardExists(flashcard.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }

//            await PrepareCategoryDropdown(flashcard.CategoryId);
//            return View(flashcard);
//        }

//        // GET: Flashcards/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var currentUser = await _userManager.GetUserAsync(User);
//            var flashcard = await _context.Flashcards
//                .Include(f => f.CategoryObj)
//                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == currentUser.Id);

//            if (flashcard == null)
//            {
//                return NotFound();
//            }

//            return View(flashcard);
//        }

//        // POST: Flashcards/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var currentUser = await _userManager.GetUserAsync(User);
//            var flashcard = await _context.Flashcards
//                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == currentUser.Id);

//            if (flashcard != null)
//            {
//                _context.Flashcards.Remove(flashcard);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        // GET: Flashcards/Review
//        public async Task<IActionResult> Review()
//        {
//            var currentUser = await _userManager.GetUserAsync(User);
//            var flashcards = await _context.Flashcards
//                .Where(f => f.UserId == currentUser.Id)
//                .ToListAsync();

//            if (!flashcards.Any())
//            {
//                TempData["Message"] = "Çalışabileceğiniz hiç flashcard yok. Önce bir flashcard oluşturun.";
//                return RedirectToAction(nameof(Index));
//            }

//            var random = new Random();
//            var flashcard = flashcards[random.Next(flashcards.Count)];

//            return View(flashcard);
//        }

//        // POST: Flashcards/Review
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Review(int id)
//        {
//            var flashcard = await _context.Flashcards.FindAsync(id);
//            if (flashcard == null)
//            {
//                return NotFound();
//            }

//            flashcard.LastReviewedDate = DateTime.Now;
//            _context.Update(flashcard);
//            await _context.SaveChangesAsync();

//            return RedirectToAction(nameof(Review));
//        }

//        private bool FlashcardExists(int id)
//        {
//            return _context.Flashcards.Any(e => e.Id == id);
//        }

//        private async Task PrepareCategoryDropdown(int? selectedCategoryId = null)
//        {
//            var categories = await _context.Categories
//                .OrderBy(c => c.Name)
//                .Select(c => new { c.Id, c.Name })
//                .ToListAsync();

//            ViewBag.CategoryList = new SelectList(categories, "Id", "Name", selectedCategoryId);
//        }
//    }
//}

using FlashcardApp.Data;
using FlashcardApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FlashcardApp.Controllers
{
    [Authorize]
    public class FlashcardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FlashcardsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Flashcards
        public async Task<IActionResult> Index(string? searchString, string? categoryFilter)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var flashcardsQuery = _context.Flashcards
                .Include(f => f.CategoryObj)
                .Where(f => f.UserId == currentUser.Id)
                .AsQueryable();

            // Arama filtresi
            if (!string.IsNullOrEmpty(searchString))
            {
                flashcardsQuery = flashcardsQuery.Where(f =>
                    (f.FrontSide != null && f.FrontSide.Contains(searchString)) ||
                    (f.BackSide != null && f.BackSide.Contains(searchString)) ||
                    (f.Category != null && f.Category.Contains(searchString)) ||
                    (f.CategoryObj != null && f.CategoryObj.Name != null &&
                     f.CategoryObj.Name.Contains(searchString)));
            }

            // Kategori filtresi
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                flashcardsQuery = flashcardsQuery.Where(f =>
                    (f.Category != null && f.Category == categoryFilter) ||
                    (f.CategoryObj != null && f.CategoryObj.Name == categoryFilter));
            }

            // Kategorileri dropdown için hazırla
            var categories = await _context.Categories
                .Where(c => c.Name != null)
                .Select(c => c.Name!)
                .Union(_context.Flashcards
                    .Where(f => f.Category != null)
                    .Select(f => f.Category!))
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            ViewBag.Categories = new SelectList(categories);

            var flashcards = await flashcardsQuery
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();

            return View(flashcards);
        }

        // GET: Flashcards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var flashcard = await _context.Flashcards
                .Include(f => f.CategoryObj)
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == currentUser.Id);

            if (flashcard == null)
            {
                return NotFound();
            }

            return View(flashcard);
        }

        // GET: Flashcards/Create
        public async Task<IActionResult> Create()
        {
            await PrepareCategoryDropdown();
            return View(new Flashcard());
        }

        // POST: Flashcards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FrontSide,BackSide,Category,CategoryId")] Flashcard flashcard)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            if (ModelState.IsValid)
            {
                flashcard.UserId = currentUser.Id;
                flashcard.CreatedDate = DateTime.Now;

                _context.Add(flashcard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PrepareCategoryDropdown();
            return View(flashcard);
        }

        // GET: Flashcards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var flashcard = await _context.Flashcards
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == currentUser.Id);

            if (flashcard == null)
            {
                return NotFound();
            }

            await PrepareCategoryDropdown(flashcard.CategoryId);
            return View(flashcard);
        }

        // POST: Flashcards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FrontSide,BackSide,Category,CategoryId,UserId,CreatedDate")] Flashcard flashcard)
        {
            if (id != flashcard.Id)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || flashcard.UserId != currentUser.Id)
            {
                return Forbid();
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
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PrepareCategoryDropdown(flashcard.CategoryId);
            return View(flashcard);
        }

        // GET: Flashcards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var flashcard = await _context.Flashcards
                .Include(f => f.CategoryObj)
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == currentUser.Id);

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
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var flashcard = await _context.Flashcards
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == currentUser.Id);

            if (flashcard != null)
            {
                _context.Flashcards.Remove(flashcard);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Flashcards/Review
        public async Task<IActionResult> Review()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var flashcards = await _context.Flashcards
                .Where(f => f.UserId == currentUser.Id)
                .ToListAsync();

            if (!flashcards.Any())
            {
                TempData["Message"] = "Çalışabileceğiniz hiç flashcard yok. Önce bir flashcard oluşturun.";
                return RedirectToAction(nameof(Index));
            }

            var random = new Random();
            var flashcard = flashcards[random.Next(flashcards.Count)];

            return View(flashcard);
        }

        // POST: Flashcards/Review
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int id)
        {
            var flashcard = await _context.Flashcards.FindAsync(id);
            if (flashcard == null)
            {
                return NotFound();
            }

            flashcard.LastReviewedDate = DateTime.Now;
            _context.Update(flashcard);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Review));
        }

        private bool FlashcardExists(int id)
        {
            return _context.Flashcards.Any(e => e.Id == id);
        }

        private async Task PrepareCategoryDropdown(int? selectedCategoryId = null)
        {
            var categories = await _context.Categories
                .Where(c => c.Name != null)
                .OrderBy(c => c.Name)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            ViewBag.CategoryList = new SelectList(categories, "Id", "Name", selectedCategoryId);
        }
    }
}