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
    public class UsuarioController : Controller
    {
        private readonly DbpruebatecnicabackendContext _context;

        public UsuarioController(DbpruebatecnicabackendContext context)
        {
            _context = context;
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            var dbpruebatecnicabackendContext = _context.Usuarios.Include(u => u.IdAutoNavigation).Include(u => u.IdtipoNavigation);
            return View(await dbpruebatecnicabackendContext.ToListAsync());
        }


        public async Task SetAcumuladoToZero(string patente)
        {
            int autoId = GetAutoIdByPatente(patente);

            var usuario = await _context.Usuarios.FindAsync(autoId);

            if (usuario != null)
            {
                usuario.Acumulado = 0;
                await _context.SaveChangesAsync();
            }
        }
        private int GetAutoIdByPatente(string patente)
        {
            var auto = _context.Autos.FirstOrDefault(a => a.Patente == patente);

            if (auto != null)
            {
                return auto.ID;
            }

            return -1; 
        }

        public async Task<Double> GetAcumuladoByPatente(string patente)
        {
            int autoId = GetAutoIdByPatente(patente);

            if (autoId != -1)
            {
                var usuario = await _context.Usuarios.FindAsync(autoId);

                if (usuario != null)
                {
                    return (double)usuario.Acumulado;
                }
            }

            return -1; 
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdAutoNavigation)
                .Include(u => u.IdtipoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        public IActionResult Create()
        {
            ViewData["IdAuto"] = new SelectList(_context.Autos, "Patente", "Patente");
            ViewData["Idtipo"] = new SelectList(_context.Tipos, "Idtipo", "Idtipo");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Idtipo")] Usuario usuario, string patente)
        {
            if (ModelState.IsValid)
            {
                var nuevoAuto = new Auto()
                {
                    Patente = patente,
                };

                _context.Add(nuevoAuto);
                await _context.SaveChangesAsync();

                int autoId = nuevoAuto.ID;

                usuario.IdAuto = autoId.ToString();

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAuto"] = new SelectList(_context.Autos, "Patente", "Patente", usuario.IdAuto);
            ViewData["Idtipo"] = new SelectList(_context.Tipos, "Idtipo", "Idtipo", usuario.Idtipo);
            return View(usuario);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdAuto"] = new SelectList(_context.Autos, "Patente", "Patente", usuario.IdAuto);
            ViewData["Idtipo"] = new SelectList(_context.Tipos, "Idtipo", "Idtipo", usuario.Idtipo);
            return View(usuario);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Acumulado,Idtipo,IdAuto")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            ViewData["IdAuto"] = new SelectList(_context.Autos, "Patente", "Patente", usuario.IdAuto);
            ViewData["Idtipo"] = new SelectList(_context.Tipos, "Idtipo", "Idtipo", usuario.Idtipo);
            return View(usuario);
        }

      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdAutoNavigation)
                .Include(u => u.IdtipoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
