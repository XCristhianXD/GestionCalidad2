using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionCalidad.Backend.DTO.Departamento;
using GestionCalidad.Backend.Data;
using GestionCalidad.Backend.Dominio;

namespace GestionCalidad.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosController : ControllerBase
    {
        private readonly GestionCalidadContext _context;

        public DepartamentosController(GestionCalidadContext context)
        {
            _context = context;
        }

        // GET: api/Departamentos
        [HttpGet]
        public async Task<IActionResult> GetDepartamento()
        {
            var lista = await (from a in _context.Departamento
                               where a.Estado != "Inactivo"
                               select new DepartamentoDTO
                               {
                                   Nombre = a.Nombre,
                                   Codigo = a.Codigo
                               }).ToListAsync();
            return Ok(lista);
        }
        // GET: api/Departamentos/5
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetDepartamento(string codigo)
        {
            var departamento = await (from a in _context.Departamento
                                      where a.Codigo == codigo && a.Estado != "Inactivo"
                                      select new DepartamentoDTO
                                      {
                                          Nombre = a.Nombre,
                                          Codigo = a.Codigo
                                      }).FirstOrDefaultAsync();

            if (departamento == null)
            {
                return NotFound();
            }

            return Ok(departamento);
        }

        // PUT: api/Departamentos/5
        [HttpPut("{codigo}")]
        public async Task<IActionResult> PutDepartamento(string nombre, string codigo)
        {
            Departamento departamento = await (from a in _context.Departamento
                                               where a.Codigo == codigo && a.Estado != "Inactivo"
                                               select a).FirstOrDefaultAsync();

            if (departamento == null)
                return NotFound($"Departamento {codigo} no encontrado");

            departamento.Nombre = nombre;

            await _context.SaveChangesAsync();

            return Ok($"Departamento {codigo} actualizado");
        }

        // POST: api/Departamentos
        [HttpPost]
        public async Task<IActionResult> PostDepartamento(string nombre, string codigo)
        {
            Departamento departamentobuscado = await (from a in _context.Departamento
                                            where a.Codigo == codigo
                                            select a).FirstOrDefaultAsync();
            if (departamentobuscado != null)
                return BadRequest("El codigo ya existe en la base de datos");
            Departamento departamento = new Departamento()
            {
                Codigo = codigo,
                Nombre = nombre,
                Estado = "Activo"
            };
            _context.Departamento.Add(departamento);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { mensaje = "Creado con exito" });
        }

        // DELETE: api/Departamentos/5
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteDepartamento(string codigo)
        {
            var departamento = await (from a in _context.Departamento
                                 where a.Codigo == codigo
                                 select a).FirstOrDefaultAsync();
            if (departamento == null)
            {
                return NotFound("El codigo no existe");
            }
            departamento.Estado = "Inactivo";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}