using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using System.Drawing.Printing;

namespace KooliProjekt.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            ViewBag.Customers = _context.Customers.ToList();
            return View(await _context.Orders.GetPagedAsync(page, pageSize));
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var customers = new SelectList(_context.Customers, "Id", "Name");
            ViewBag.Customers = customers;

            // Siin saab määrata, et staatus on alguses näiteks "Pending"
            var orderStatuses = new List<SelectListItem>
    {
        new SelectListItem { Value = "Pending", Text = "Pending" },
        new SelectListItem { Value = "Shipped", Text = "Shipped" },
        new SelectListItem { Value = "Delivered", Text = "Delivered" },
        new SelectListItem { Value = "Cancelled", Text = "Cancelled" }
    };
            ViewBag.OrderStatuses = orderStatuses;

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,Date,Staatus")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Customers = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);

            // Kui vormi sisestamisel on vigasid, laadige staatused uuesti
            var orderStatuses = new List<SelectListItem>
    {
        new SelectListItem { Value = "Pending", Text = "Pending" },
        new SelectListItem { Value = "Shipped", Text = "Shipped" },
        new SelectListItem { Value = "Delivered", Text = "Delivered" },
        new SelectListItem { Value = "Cancelled", Text = "Cancelled" }
    };
            ViewBag.OrderStatuses = orderStatuses;

            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Laadige kliendid ja staatuse valikud redigeerimiseks
            ViewBag.Customers = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);

            var orderStatuses = new List<SelectListItem>
    {
        new SelectListItem { Value = "Pending", Text = "Pending" },
        new SelectListItem { Value = "Shipped", Text = "Shipped" },
        new SelectListItem { Value = "Delivered", Text = "Delivered" },
        new SelectListItem { Value = "Cancelled", Text = "Cancelled" }
    };
            ViewBag.OrderStatuses = orderStatuses;

            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,Date,Staatus")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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

            // Laadige staatused ja kliendid uuesti, kui on vigasid
            ViewBag.Customers = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);

            var orderStatuses = new List<SelectListItem>
    {
        new SelectListItem { Value = "Pending", Text = "Pending" },
        new SelectListItem { Value = "Shipped", Text = "Shipped" },
        new SelectListItem { Value = "Delivered", Text = "Delivered" },
        new SelectListItem { Value = "Cancelled", Text = "Cancelled" }
    };
            ViewBag.OrderStatuses = orderStatuses;

            return View(order);
        }


        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
