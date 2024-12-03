using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using System.Drawing.Printing;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        // GET: OrderItems
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            var products = (await _orderItemService.GetProductsAsync()).ToList();
            ViewBag.Products = products; // Otse List<Product>, mitte SelectList
            return View(await _orderItemService.List(page, pageSize));
        }

        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _orderItemService.Get(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: OrderItems/Create
        // GET: OrderItems/Create
        public async Task<IActionResult> Create()
        {
            // Saame kõik tooted ja anname need vormi jaoks
            var products = (await _orderItemService.GetProductsAsync()).ToList();
            ViewBag.Products = new SelectList(products, "Id", "Name"); // Nime ja ID järgi
            ViewBag.Orders = new SelectList((await _orderItemService.GetOrdersAsync()), "Id", "Id"); // Tellimuse ID järgi

            return View();
        }

        // POST: OrderItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                // Otsime kõik tooted, et määrata hind ja soodustus
                var products = (await _orderItemService.GetProductsAsync()).ToList();

                // Määrame toote hind ja soodustus
                orderItem.SetProductDetails(products);

                // Salvestame andmebaasi
                await _orderItemService.Save(orderItem);
                return RedirectToAction(nameof(Index));
            }

            // Kui on vigu, tagastame vaate
            ViewBag.Orders = new SelectList((await _orderItemService.GetOrdersAsync()), "Id", "Id"); // Tellimuse ID järgi
            ViewBag.Products = new SelectList((await _orderItemService.GetOrdersAsync()), "Id", "Name");
            return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _orderItemService.Get(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewBag.Products = new SelectList((await _orderItemService.GetOrdersAsync()), "Id", "Name");
            ViewBag.OrderId = new SelectList((await _orderItemService.GetOrdersAsync()), "Id", "Id", orderItem.OrderId);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,ProductId,Quantity,Price,Discount")] OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderItemService.Save(orderItem);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await _orderItemService.Includes(id)))
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
            ViewBag.Products = new SelectList((await _orderItemService.GetOrdersAsync()), "Id", "Name");
            ViewBag.OrderId = new SelectList((await _orderItemService.GetOrdersAsync()), "Id", "Id", orderItem.OrderId);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _orderItemService.Get(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _orderItemService.Get(id);
            if (orderItem != null)
            {
                await _orderItemService.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OrderItemExists(int id)
        {
            return await _orderItemService.Includes(id);
        }
    }
}
