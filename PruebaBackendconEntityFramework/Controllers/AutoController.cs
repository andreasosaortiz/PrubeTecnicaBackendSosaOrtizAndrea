using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaBackendconEntityFramework.Models;

namespace PruebaBackendconEntityFramework.Controllers
{
    public class AutoController : Controller
    {
        private readonly DbpruebatecnicabackendContext _context;

        public AutoController(DbpruebatecnicabackendContext context)
        {
            _context = context;
        }

        // GET: Auto
        public async Task<IActionResult> Index()
        {
            return View(await _context.Autos.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auto = await _context.Autos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (auto == null)
            {
                return NotFound();
            }

            return View(auto);
        }

        public async Task<List<Auto>> ListarAutosPorTipoUsuario(string descripcion)
        {
            int idTipoUsuario = GetIdTipoUsuarioByDescripcion(descripcion);

            if (idTipoUsuario != -1)
            {
                return await _context.Autos
                    .Include(a => a.Usuarios)
                    .Where(a => a.Usuarios.Any(u => u.Idtipo == idTipoUsuario))
                    .ToListAsync();
            }

            return new List<Auto>(); 
        }
        private int GetIdTipoUsuarioByDescripcion(string descripcion)
        {
            var tipoUsuario = _context.Tipos.FirstOrDefault(t => t.Descripcion == descripcion);

            if (tipoUsuario != null)
            {
                return tipoUsuario.Idtipo;
            }

            return -1;
        }


        // GET: Auto/Create
        public IActionResult Create()
        {
            return View();
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Patente")] Auto auto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(auto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(auto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auto = await _context.Autos.FindAsync(id);
            if (auto == null)
            {
                return NotFound();
            }
            return View(auto);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Patente")] Auto auto)
        {
            if (id != auto.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutoExists(auto.ID))
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
            return View(auto);
        }

        // GET: Auto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auto = await _context.Autos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (auto == null)
            {
                return NotFound();
            }

            return View(auto);
        }

        // POST: Auto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auto = await _context.Autos.FindAsync(id);
            if (auto != null)
            {
                _context.Autos.Remove(auto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutoExists(int id)
        {
            return _context.Autos.Any(e => e.ID == id);
        }
    }
}
