using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Products
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            ViewBag.Categories = (await _productService.GetCategoriesAsync());
            var data = await _productService.List(page, pageSize);
            return View(data);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = (await _productService.GetCategoriesAsync()).ToList();
            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesAsync();
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.Save(product); // Ensure Save is async
                return RedirectToAction(nameof(Index));
            }

            await PopulateCategoriesAsync(); // Reload categories on validation failure
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            await PopulateCategoriesAsync();
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,CategoryId,Discount")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.Save(product); // Ensure Save is async
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productService.Includes(id))
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

            await PopulateCategoriesAsync();
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productService.Get(id);
            if (product != null)
            {
                await _productService.Delete(id); // Ensure Delete is async
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<bool> ProductExists(int id)
        {
            return await _productService.Includes(id);
        }

        public async Task PopulateCategoriesAsync()
        {
            ViewBag.Categories = new SelectList(await _productService.GetCategoriesAsync(), "Id", "Name");
        }
    }
}