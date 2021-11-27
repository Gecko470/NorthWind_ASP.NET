using Microsoft.AspNetCore.Mvc;
using NorthWind.Models;
using Microsoft.EntityFrameworkCore;

namespace NorthWind.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly BDNorthwind _db;

        public CategoriesController(BDNorthwind db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int? resp, int? id)
        {
            List<Category> categories = await _db.Categories.ToListAsync();
            ViewBag.resp = resp;
            ViewBag.id = id;
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();

        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Category categoria)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(categoria);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return View("Create");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            Category category = await _db.Categories.FindAsync(id);
            return View(category);

        }

        public async Task<IActionResult> Editar(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(category).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
            Category category = await _db.Categories.FindAsync(id);
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

                return RedirectToAction("Index", new { resp = 0, id = id });
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", new { resp = 1 });
            }
        }
    }
}
