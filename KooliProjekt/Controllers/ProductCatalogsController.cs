using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;

namespace KooliProjekt.Controllers
{
    public class ProductCatalogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductCatalogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductCatalogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductCatalogs.ToListAsync());
        }

        // GET: ProductCatalogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCatalog = await _context.ProductCatalogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCatalog == null)
            {
                return NotFound();
            }

            return View(productCatalog);
        }

        // GET: ProductCatalogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductCatalogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryName,ProductId")] ProductCatalog productCatalog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCatalog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productCatalog);
        }

        // GET: ProductCatalogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCatalog = await _context.ProductCatalogs.FindAsync(id);
            if (productCatalog == null)
            {
                return NotFound();
            }
            return View(productCatalog);
        }

        // POST: ProductCatalogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName,ProductId")] ProductCatalog productCatalog)
        {
            if (id != productCatalog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCatalog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCatalogExists(productCatalog.Id))
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
            return View(productCatalog);
        }

        // GET: ProductCatalogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCatalog = await _context.ProductCatalogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCatalog == null)
            {
                return NotFound();
            }

            return View(productCatalog);
        }

        // POST: ProductCatalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCatalog = await _context.ProductCatalogs.FindAsync(id);
            if (productCatalog != null)
            {
                _context.ProductCatalogs.Remove(productCatalog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCatalogExists(int id)
        {
            return _context.ProductCatalogs.Any(e => e.Id == id);
        }
    }
}
