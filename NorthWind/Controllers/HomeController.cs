using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NorthWind.Models;
using System.Diagnostics;

namespace NorthWind.Controllers
{
    public class HomeController : Controller
    {
        private readonly BDNorthwind _db;

        public HomeController(BDNorthwind db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(int? resp, int? id)
        {
            List<Product> lista = await _db.Products.ToListAsync();

            ViewBag.categorias = Categorias();
            ViewBag.suppliers = Suppliers();
            ViewBag.resp = resp;
            ViewBag.id = id;

            return View(lista);
        }

        public async Task<IActionResult> Filtrar(string termino)
        {
            if (termino == null)
            {
                List<Product> lista = await _db.Products.ToListAsync();
                ViewBag.categorias = Categorias();
                ViewBag.suppliers = Suppliers();
                return View("Index", lista);
            }
            else
            {
                List<Product> lista = await _db.Products.Where(p => p.ProductName.Contains(termino)).ToListAsync();
                ViewBag.categorias = Categorias();
                ViewBag.suppliers = Suppliers();
                return View("Index", lista);
            }
        }

        public async Task<IActionResult> FiltrarCat(string categoria)
        {
            if (categoria == null)
            {
                List<Product> lista = await _db.Products.ToListAsync();
                ViewBag.categorias = Categorias();
                ViewBag.suppliers = Suppliers();
                return View("Index", lista);
            }
            else
            {
                List<Product> lista = await _db.Products.Where(p => p.CategoryId.ToString() == categoria).ToListAsync();
                ViewBag.categorias = Categorias();
                ViewBag.suppliers = Suppliers();
                return View("Index", lista);
            }
        }

        public async Task<IActionResult> FiltrarSup(string supplier)
        {
            if (supplier == null)
            {
                List<Product> lista = await _db.Products.ToListAsync();
                ViewBag.categorias = Categorias();
                ViewBag.suppliers = Suppliers();
                return View("Index", lista);
            }
            else
            {
                List<Product> lista = await _db.Products.Where(p => p.SupplierId.ToString() == supplier).ToListAsync();
                ViewBag.categorias = Categorias();
                ViewBag.suppliers = Suppliers();
                return View("Index", lista);
            }
        }

        public IActionResult Create()
        {
            return View();

        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Product producto)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(producto);
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
            Product producto = await _db.Products.FindAsync(id);
            return View(producto);

        }

        public async Task<IActionResult> Editar(Product producto)
        {
            if (ModelState.IsValid)
            {
            _db.Entry(producto).State = EntityState.Modified;
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
            Product producto = await _db.Products.FindAsync(id);
            _db.Products.Remove(producto);
            await _db.SaveChangesAsync();

                return RedirectToAction("Index", new {resp = 0, id = id});
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new {resp = 1});
            }
        }

        public List<SelectListItem> Categorias()
        {
            List<SelectListItem> categorias = _db.Categories.Select(p => new SelectListItem()
            {
                Value = p.CategoryId.ToString(),
                Text = p.CategoryName

            }).ToList();

            return categorias;

        }

        public List<SelectListItem> Suppliers()
        {
            List<SelectListItem> suppliers = _db.Suppliers.Select(p => new SelectListItem()
            {
                Value = p.SupplierId.ToString(),
                Text = p.CompanyName

            }).ToList();

            return suppliers;

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}