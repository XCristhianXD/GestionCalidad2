using GestionCalidad.Backend.Dominio;
using GestionCalidad.Backend.DTO.Departamento;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionCalidad.Backend.DTO.InformeCalidad;
using GestionCalidad.Backend.Data;

namespace GestionCalidad.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformeCalidadsController : ControllerBase
    {
        private readonly GestionCalidadContext _context;

        public InformeCalidadsController(GestionCalidadContext context)
        {
            _context = context;
        }

        // GET: api/InformeCalidads
        [HttpGet]
        public async Task<IActionResult> GetInformeCalidad()
        {
            var lista = await (from i in _context.InformeCalidad
                               where i.Estado != "Inactivo"
                               select new InformeCalidadDTO
                               {
                                   Calificacion = i.Calificacion,
                                   Descripcion = i.Descripcion,
                                   Fecha = i.Fecha,
                                   Codigo = i.Codigo
                               }).ToListAsync();

            return Ok(lista);
        }

        // GET: api/InformeCalidads/5
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetInformeCalidad(string codigo)
        {
            var informe = await (from i in _context.InformeCalidad
                                 where i.Codigo == codigo && i.Estado != "Inactivo"
                                 select new InformeCalidadDTO
                                 {
                                     Calificacion = i.Calificacion,
                                     Descripcion = i.Descripcion,
                                     Fecha = i.Fecha,
                                     Codigo = i.Codigo
                                 }).FirstOrDefaultAsync();

            if (informe == null)
                return NotFound();

            return Ok(informe);
        }

        // PUT: api/InformeCalidads/5
        [HttpPut("{codigo}")]
        public async Task<IActionResult> PutInformeCalidad(int calificacion, string descripcion, DateOnly fecha, string codigo)
        {
            if (calificacion < 1 || calificacion > 5)
                return BadRequest("La calificación debe estar entre 1 y 5");

            var informe = await (from i in _context.InformeCalidad
                                 where i.Codigo == codigo && i.Estado != "Inactivo"
                                 select i).FirstOrDefaultAsync();

            if (informe == null)
                return NotFound($"Informe {codigo} no encontrado");

            informe.Calificacion = calificacion;
            informe.Descripcion = descripcion;
            informe.Fecha = fecha;

            await _context.SaveChangesAsync();

            return Ok($"Informe {codigo} actualizado");
        }

        // POST: api/InformeCalidads
        [HttpPost]
        public async Task<IActionResult> PostInformeCalidad(int calificacion, string descripcion, DateOnly fecha, string codigo)
        {
            if (calificacion < 1 || calificacion > 5)
                return BadRequest("La calificación debe estar entre 1 y 5");

            var existe = await (from i in _context.InformeCalidad
                                where i.Codigo == codigo
                                select i).FirstOrDefaultAsync();

            if (existe != null)
                return BadRequest("El codigo ya existe en la base de datos");

            var informe = new InformeCalidad
            {
                Calificacion = calificacion,
                Descripcion = descripcion,
                Fecha = fecha,
                Codigo = codigo,
                Estado = "Activo"
            };

            _context.InformeCalidad.Add(informe);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { mensaje = "Creado con exito" });
        }

        // DELETE: api/InformeCalidads/5
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteInformeCalidad(string codigo)
        {
            var informe = await (from i in _context.InformeCalidad
                                 where i.Codigo == codigo
                                 select i).FirstOrDefaultAsync();

            if (informe == null)
                return NotFound("El codigo no existe");

            informe.Estado = "Inactivo";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
