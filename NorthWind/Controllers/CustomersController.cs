using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthWind.Models;

namespace NorthWind.Controllers
{
    public class CustomersController : Controller
    {
        private readonly BDNorthwind _db;
        public CustomersController(BDNorthwind db)
        {
            _db = db;
        }
        public IActionResult Index(int? resp, string? id)
        {
            List<Customer> customers = _db.Customers.ToList();
            ViewBag.resp = resp;
            ViewBag.id = id;
            return View(customers);
        }

        public IActionResult Create()
        {
            return View();

        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            Customer customer = await _db.Customers.FindAsync(id);
            return View(customer);

        }

        public async Task<IActionResult> Editar(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(customer).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                Customer customer = await _db.Customers.FindAsync(id);
                _db.Customers.Remove(customer);
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
