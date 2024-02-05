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
    public class EstanciaController : Controller
    {
        private readonly DbpruebatecnicabackendContext _context;

        public EstanciaController(DbpruebatecnicabackendContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dbpruebatecnicabackendContext = _context.Estancia.Include(e => e.IdAutoNavigation);
            return View(await dbpruebatecnicabackendContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estancium = await _context.Estancia
                .Include(e => e.IdAutoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estancium == null)
            {
                return NotFound();
            }

            return View(estancium);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string patente, [Bind("HsEntrada")] Estancium estancium)
        {
            if (ModelState.IsValid)
            {
                var auto = await _context.Autos.FirstOrDefaultAsync(a => a.Patente == patente);
                int AUTOID = auto.ID;
                if (auto == null)
                {
                    return NotFound();
                }

                estancium.IdAuto = AUTOID.ToString();

                _context.Add(estancium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdAuto"] = new SelectList(_context.Autos, "Patente", "Patente", estancium.IdAuto);
            return View(estancium);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task FinalizarEstancia(string Patente, DateTime hsSalida)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var ultimaEstancia = await _context.Estancia
                        .OrderByDescending(e => e.Id)
                        .FirstOrDefaultAsync(e => e.IdAuto == Patente);

                    if (ultimaEstancia == null)
                    {
                        throw new Exception("No se encontró la última estancia del auto.");
                    }

                    var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdAuto == Patente);

                    if (usuario == null)
                    {
                        throw new Exception("No se encontró el usuario asociado al auto.");
                    }

                    TimeSpan diferenciaTiempo = (TimeSpan)(hsSalida - ultimaEstancia.HsEntrada);

                    int diferenciaMinutos = (int)Math.Round(diferenciaTiempo.TotalMinutes);

                    decimal costo = 0;

                    if (usuario.Idtipo == 1)
                    {
                        // Tipo oficial, costo = 0
                    }
                    else if (usuario.Idtipo == 2)
                    {
                        // Tipo residente
                        costo = diferenciaMinutos * 0.05m;
                        usuario.Acumulado += Convert.ToDouble(costo);
                    }
                    else
                    {
                        // Otros tipos
                        costo = diferenciaMinutos * 0.5m;
                    }

                    ultimaEstancia.HsSalida = hsSalida;
                    ultimaEstancia.Costo = (double?)costo;
                    _context.Update(ultimaEstancia);

                    if (usuario.Idtipo== 2)
                    {
                        _context.Update(usuario);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        public async Task<IActionResult> GetEstancias(string patente)
        {
            var auto = await _context.Autos.FirstOrDefaultAsync(a => a.Patente == patente);

            if (auto == null)
            {
                return NotFound();
            }

            var estancias = await _context.Estancia
                .Where(e => e.IdAuto == auto.ID.ToString())
                .OrderByDescending(e => e.HsEntrada)
                .ToListAsync();

            return Ok(estancias);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estancium = await _context.Estancia.FindAsync(id);
            if (estancium == null)
            {
                return NotFound();
            }
            ViewData["IdAuto"] = new SelectList(_context.Autos, "Patente", "Patente", estancium.IdAuto);
            return View(estancium);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HsEntrada,HsSalida,Costo,IdAuto")] Estancium estancium)
        {
            if (id != estancium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estancium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstanciumExists(estancium.Id))
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
            ViewData["IdAuto"] = new SelectList(_context.Autos, "Patente", "Patente", estancium.IdAuto);
            return View(estancium);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estancium = await _context.Estancia
                .Include(e => e.IdAutoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estancium == null)
            {
                return NotFound();
            }

            return View(estancium);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estancium = await _context.Estancia.FindAsync(id);
            if (estancium != null)
            {
                _context.Estancia.Remove(estancium);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstanciumExists(int id)
        {
            return _context.Estancia.Any(e => e.Id == id);
        }
    }
}
