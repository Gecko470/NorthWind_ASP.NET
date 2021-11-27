using Microsoft.AspNetCore.Mvc;
using NorthWind.Models;
using Microsoft.EntityFrameworkCore;

namespace NorthWind.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly BDNorthwind _db;

        public EmployeesController(BDNorthwind db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int? resp, int? id)
        {
            List<Employee> lista = await _db.Employees.ToListAsync();
            ViewBag.resp = resp;
            ViewBag.id = id;
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();

        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Employee emp)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Add(emp);
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
            Employee employee = await _db.Employees.FindAsync(id);
            return View(employee);

        }

        public async Task<IActionResult> Editar(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(employee).State = EntityState.Modified;
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
                Employee employee = await _db.Employees.FindAsync(id);
                _db.Employees.Remove(employee);
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
