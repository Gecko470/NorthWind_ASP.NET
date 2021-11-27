using Microsoft.AspNetCore.Mvc;
using NorthWind.Models;
using Microsoft.EntityFrameworkCore;

namespace NorthWind.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly BDNorthwind _db;

        public SuppliersController(BDNorthwind db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int? resp, int? id)
        {
            List<Supplier> lista = await _db.Suppliers.ToListAsync();
            ViewBag.resp = resp;
            ViewBag.id = id;
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();

        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Supplier supp)
        {
            if (ModelState.IsValid)
            {
                _db.Suppliers.Add(supp);
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
            Supplier supp = await _db.Suppliers.FindAsync(id);
            return View(supp);

        }

        public async Task<IActionResult> Editar(Supplier supp)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(supp).State = EntityState.Modified;
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
                Supplier supp = await _db.Suppliers.FindAsync(id);
                _db.Suppliers.Remove(supp);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index", new { resp = 0, id = id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { resp = 1 });
            }
        }
    }
}
