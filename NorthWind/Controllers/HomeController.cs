using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NorthWind.ClasesAux;
using NorthWind.Models;
using OfficeOpenXml;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.Diagnostics;
using System.Text.Json;

namespace NorthWind.Controllers
{
    public class HomeController : Controller
    {
        private readonly BDNorthwind _db;
        public string[] cabeceras = { "ID", "Product Name", "Supplier Id", "Category Id", "Quantity Per Unit", "Unit Price", "Stock", "Units on Order", "Reorder Level", "Discontinued" };
        public string[] parametrosamostrar = { "ProductId", "ProductName", "SupplierId", "CategoryId", "QuantityPerUnit", "UnitPrice", "UnitsInStock", "UnitsOnOrder", "ReorderLevel", "Discontinued" };
        public string excel = "";
        public HomeController(BDNorthwind db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(int? resp, int? id)
        {
            List<Product> lista = await _db.Products.ToListAsync();
            Object[] arrayLista = await _db.Products.ToArrayAsync();

            ViewBag.categorias = Categorias();
            ViewBag.suppliers = Suppliers();
            ViewBag.resp = resp;
            ViewBag.id = id;
            ViewBag.arrayLista = arrayLista;

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

                return RedirectToAction("Index", new { resp = 0, id = id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { resp = 1 });
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

        public string Excel(string lista)
        {
            var list = JsonSerializer.Deserialize<ProductCLS[]>(lista);
            using (MemoryStream ms = new MemoryStream())
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    ep.Workbook.Worksheets.Add("Hoja");
                    ExcelWorksheet ew = ep.Workbook.Worksheets[0];

                    for (int i = 0; i < cabeceras.Length; i++)
                    {
                        ew.Column(i + 1).Width = 30;
                        ew.Cells[1, i + 1].Value = cabeceras[i];
                    }

                    int fila = 3;
                    int columna = 1;

                    foreach (object item in list)
                    {
                        columna = 1;
                        foreach (string propiedad in parametrosamostrar)
                        {
                            ew.Cells[fila, columna].Value = item.GetType().GetProperty(propiedad).GetValue(item).ToString();
                            columna++;
                        }

                        fila++;
                    }

                    ep.SaveAs(ms);
                    byte[] buffer = ms.ToArray();
                    string base64 = Convert.ToBase64String(buffer);
                    excel = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + base64;
                    return excel;
                }
            }
        }

        public string Word(string lista)
        {
            var list = JsonSerializer.Deserialize<ProductCLS[]>(lista);

            using (MemoryStream ms = new MemoryStream())
            {
                WordDocument document = new WordDocument();

                WSection section = document.AddSection() as WSection;

                section.PageSetup.Margins.All = 7;

                IWParagraph paragraph = section.AddParagraph();

                paragraph.ApplyStyle("Normal");

                paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;

                WTextRange textRange = paragraph.AppendText("Products") as WTextRange;
                textRange.CharacterFormat.FontSize = 20f;
                textRange.CharacterFormat.FontName = "Sans-Serif";
                textRange.CharacterFormat.TextColor = Syncfusion.Drawing.Color.Blue;

                IWTable tabla = section.AddTable();
                int numCol = cabeceras.Length;
                int numFilas = list.Length;
                tabla.ResetCells(numFilas + 1, numCol);

                for (int i = 0; i < numCol; i++)
                {
                    tabla[0, i].AddParagraph().AppendText(cabeceras[i]).CharacterFormat.FontSize= 10f;
                }

                int col = 0;
                int fila = 1;
                foreach (var item in list)
                {
                    col = 0;
                    foreach (var propiedad in parametrosamostrar)
                    {
                        tabla[fila, col].AddParagraph().AppendText(item.GetType().GetProperty(propiedad).GetValue(item).ToString());
                        col++;
                    }
                    fila++;
                }

                document.Save(ms, FormatType.Docx);
                byte[] buffer = ms.ToArray();
                string base64 = Convert.ToBase64String(buffer);
                string word = "data:application/vnd.openxmlformats-officedocument.wordprocessingml.document;base64," + base64;

                return word;
            }
        }

        public string Pdf(string lista)
        {
            var list = JsonSerializer.Deserialize<ProductCLS[]>(lista);

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                using (var pdfDoc = new PdfDocument(writer))
                {

                    Document doc = new Document(pdfDoc);
                    Paragraph p1 = new Paragraph("Products");
                    p1.SetTextAlignment(TextAlignment.CENTER);
                    p1.SetFontSize(20);
                    doc.Add(p1);

                    Table tabla = new Table(cabeceras.Length);

                    Cell celda;
                    for (int i = 0; i < cabeceras.Length; i++)
                    {
                        celda = new Cell();
                        celda.Add(new Paragraph(cabeceras[i]));
                        tabla.AddHeaderCell(celda);
                    }

                    foreach (object item in list)
                    {
                        foreach (string propiedad in parametrosamostrar)
                        {
                            celda = new Cell();
                            celda.Add(new Paragraph(item.GetType().GetProperty(propiedad).GetValue(item).ToString()));
                            tabla.AddCell(celda);
                        }
                    }

                    doc.Add(tabla);

                    doc.Close();
                    writer.Close();


                    byte[] buffer = ms.ToArray();
                    string base64 = Convert.ToBase64String(buffer);
                    string pdf = "data:application/pdf;base64," + base64;

                    return pdf;
                }
            }
        }

    }
}
