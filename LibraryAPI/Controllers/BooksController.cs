﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly APIDbContext _context;

        public BooksController(APIDbContext context)
        {
            _context = context;
        }

        // GET: Books
        [HttpGet]
        public async Task<IActionResult> Index()
        {
              return _context.Books != null ? 
                          View(await _context.Books.ToListAsync()) :
                          Problem("Entity set 'APIDbContext.Books'  is null.");
        }

        // GET: Books/Details/5
        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }*/

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }


        // GET: Books/Create
        internal IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookID }, book);
        }



        /*        // POST: Books/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("BookID,Name,Year,Author,IsAvailable,ClientID")] Book book)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(book);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(book);
                }

                // GET: Books/Edit/5
                public async Task<IActionResult> Edit(int? id)
                {
                    if (id == null || _context.Books == null)
                    {
                        return NotFound();
                    }

                    var book = await _context.Books.FindAsync(id);
                    if (book == null)
                    {
                        return NotFound();
                    }
                    return View(book);
                }

                // POST: Books/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, [Bind("BookID,Name,Year,Author,IsAvailable,ClientID")] Book book)
                {
                    if (id != book.BookID)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(book);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!BookExists(book.BookID))
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
                    return View(book);
                }

                // GET: Books/Delete/5
                public async Task<IActionResult> Delete(int? id)
                {
                    if (id == null || _context.Books == null)
                    {
                        return NotFound();
                    }

                    var book = await _context.Books
                        .FirstOrDefaultAsync(m => m.BookID == id);
                    if (book == null)
                    {
                        return NotFound();
                    }

                    return View(book);
                }

                // POST: Books/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(int id)
                {
                    if (_context.Books == null)
                    {
                        return Problem("Entity set 'APIDbContext.Books'  is null.");
                    }
                    var book = await _context.Books.FindAsync(id);
                    if (book != null)
                    {
                        _context.Books.Remove(book);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                private bool BookExists(int id)
                {
                  return (_context.Books?.Any(e => e.BookID == id)).GetValueOrDefault();
                }*/
    }
}
