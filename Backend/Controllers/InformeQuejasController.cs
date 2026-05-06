using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionCalidad.Backend.Dominio;
using GestionCalidad.Backend.DTO.InformeQueja;
using GestionCalidad.Backend.Data;

namespace GestionCalidad.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformeQuejasController : ControllerBase
    {
        private readonly GestionCalidadContext _context;

        public InformeQuejasController(GestionCalidadContext context)
        {
            _context = context;
        }

        // GET: api/InformeQuejas
        [HttpGet]
        public async Task<IActionResult> GetInformeQueja()
        {
            var lista = await (from i in _context.InformeQueja
                               where i.Estado != "Inactivo"
                               select new InformeQuejaDTO
                               {
                                   Descripcion = i.Descripcion,
                                   Fecha = i.Fecha,
                                   Codigo = i.Codigo
                               }).ToListAsync();

            return Ok(lista);
        }

        // GET: api/InformeQuejas/5
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetInformeQueja(string codigo)
        {
            var informe = await (from i in _context.InformeQueja
                                 where i.Codigo == codigo && i.Estado != "Inactivo"
                                 select new InformeQuejaDTO
                                 {
                                     Descripcion = i.Descripcion,
                                     Fecha = i.Fecha,
                                     Codigo = i.Codigo
                                 }).FirstOrDefaultAsync();

            if (informe == null)
                return NotFound();

            return Ok(informe);
        }

        // PUT: api/InformeQuejas/5
        [HttpPut("{codigo}")]
        public async Task<IActionResult> PutInformeQueja(string descripcion, DateOnly fecha, string codigo)
        {
            var informe = await (from i in _context.InformeQueja
                                 where i.Codigo == codigo && i.Estado != "Inactivo"
                                 select i).FirstOrDefaultAsync();

            if (informe == null)
                return NotFound($"Informe {codigo} no encontrado");

            informe.Descripcion = descripcion;
            informe.Fecha = fecha;

            await _context.SaveChangesAsync();

            return Ok($"Informe {codigo} actualizado");
        }

        // POST: api/InformeQuejas  
        [HttpPost]
        public async Task<IActionResult> PostInformeQueja(string descripcion, DateOnly fecha, string codigo)
        {
            var existe = await (from i in _context.InformeQueja
                                where i.Codigo == codigo
                                select i).FirstOrDefaultAsync();

            if (existe != null)
                return BadRequest("El codigo ya existe en la base de datos");

            var informe = new InformeQueja
            {
                Descripcion = descripcion,
                Fecha = fecha,
                Codigo = codigo,
                Estado = "Activo"
            };

            _context.InformeQueja.Add(informe);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { mensaje = "Creado con exito" });
        }

        // DELETE: api/InformeQuejas/5
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteInformeQueja(string codigo)
        {
            var informe = await (from i in _context.InformeQueja
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
