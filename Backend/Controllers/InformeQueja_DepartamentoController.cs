using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionCalidad.Backend.Dominio;
using GestionCalidad.Backend.Data;

namespace GestionCalidad.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformeQueja_DepartamentoController : ControllerBase
    {
        private readonly GestionCalidadContext _context;

        public InformeQueja_DepartamentoController(GestionCalidadContext context)
        {
            _context = context;
        }



        [HttpGet("reporte/todos")]
        public async Task<IActionResult> GetTodosLosInformesQueja()
        {
            var lista = await (
                from iqd in _context.InformeQueja_Departamento
                join d in _context.Departamento on iqd.id_Departamento equals d.id_Departamento
                join q in _context.InformeQueja on iqd.id_InformeQueja equals q.id_InformeQueja
                where iqd.Estado != "Inactivo"
                select new
                {
                    Departamento = d.Nombre,
                    Codigo = q.Codigo,
                    Descripcion = q.Descripcion,
                    Fecha = q.Fecha,
                    CodigoPaciente = iqd.CodigoPaciente
                }
            ).ToListAsync();

            return Ok(lista);
        }

        [HttpGet("reporte/paciente/{codigoPaciente}/departamento/{codigoDepartamento}")]
        public async Task<IActionResult> GetQuejasPorPacienteDepartamento(string codigoPaciente, string codigoDepartamento)
        {
            var lista = await (
                from iqd in _context.InformeQueja_Departamento
                join d in _context.Departamento on iqd.id_Departamento equals d.id_Departamento
                join q in _context.InformeQueja on iqd.id_InformeQueja equals q.id_InformeQueja
                where iqd.CodigoPaciente == codigoPaciente
                      && d.Codigo == codigoDepartamento
                      && iqd.Estado != "Inactivo"
                orderby q.Fecha descending
                select new
                {
                    Departamento = d.Nombre,
                    Codigo = q.Codigo,
                    Descripcion = q.Descripcion,
                    Fecha = q.Fecha,
                    CodigoPaciente = iqd.CodigoPaciente
                }
            ).ToListAsync();

            return Ok(lista);
        }

        [HttpGet("reporte/quejas/anio/{anio}")]
        public async Task<IActionResult> GetCantidadQuejasPorDepartamentoAnio(int anio)
        {
            var reporte = await (
                from iqd in _context.InformeQueja_Departamento
                join d in _context.Departamento on iqd.id_Departamento equals d.id_Departamento
                join q in _context.InformeQueja on iqd.id_InformeQueja equals q.id_InformeQueja
                where q.Fecha.Year == anio
                      && iqd.Estado != "Inactivo"
                group q by d.Nombre into g
                select new
                {
                    Departamento = g.Key,
                    TotalQuejas = g.Count()
                }
            )
            .OrderByDescending(x => x.TotalQuejas)
            .ToListAsync();

            return Ok(reporte);
        }


        [HttpGet("reporte/departamento/{codigoDepartamento}")]
        public async Task<IActionResult> GetQuejasPorDepartamento(string codigoDepartamento)
        {
            var lista = await (
                from iqd in _context.InformeQueja_Departamento
                join d in _context.Departamento on iqd.id_Departamento equals d.id_Departamento
                join q in _context.InformeQueja on iqd.id_InformeQueja equals q.id_InformeQueja
                where d.Codigo == codigoDepartamento
                      && iqd.Estado != "Inactivo"
                select new
                {
                    Departamento = d.Nombre,
                    Codigo = q.Codigo,
                    Descripcion = q.Descripcion,
                    Fecha = q.Fecha,
                    CodigoPaciente = iqd.CodigoPaciente
                }
            ).ToListAsync();

            return Ok(lista);
        }

        [HttpGet("reporte/departamento/{codigoDepartamento}/anio/{anio}")]
        public async Task<IActionResult> GetQuejasPorDepartamentoAnio(string codigoDepartamento, int anio)
        {
            var lista = await (
                from iqd in _context.InformeQueja_Departamento
                join d in _context.Departamento on iqd.id_Departamento equals d.id_Departamento
                join q in _context.InformeQueja on iqd.id_InformeQueja equals q.id_InformeQueja
                where d.Codigo == codigoDepartamento
                      && q.Fecha.Year == anio
                      && iqd.Estado != "Inactivo"
                select new
                {
                    Departamento = d.Nombre,
                    Codigo = q.Codigo,
                    Descripcion = q.Descripcion,
                    Fecha = q.Fecha,
                    CodigoPaciente = iqd.CodigoPaciente
                }
            ).ToListAsync();

            return Ok(lista);
        }

        [HttpGet("reporte/departamento/{codigoDepartamento}/anio/{anio}/mes/{mes}")]
        public async Task<IActionResult> GetQuejasPorDepartamentoAnioMes(string codigoDepartamento, int anio, int mes)
        {
            if (mes < 1 || mes > 12)
                return BadRequest("Mes inválido");

            var lista = await (
                from iqd in _context.InformeQueja_Departamento
                join d in _context.Departamento on iqd.id_Departamento equals d.id_Departamento
                join q in _context.InformeQueja on iqd.id_InformeQueja equals q.id_InformeQueja
                where d.Codigo == codigoDepartamento
                      && q.Fecha.Year == anio
                      && q.Fecha.Month == mes
                      && iqd.Estado != "Inactivo"
                select new
                {
                    Departamento = d.Nombre,
                    Codigo = q.Codigo,
                    Descripcion = q.Descripcion,
                    Fecha = q.Fecha,
                    CodigoPaciente = iqd.CodigoPaciente
                }
            ).ToListAsync();

            return Ok(lista);
        }




        [HttpPost("crear")]
        public async Task<IActionResult> PostInformeQueja_Departamento(string codigoDepartamento, string codigoQueja, string codigoPaciente)
        {
            var departamento = await (
                from d in _context.Departamento
                where d.Codigo == codigoDepartamento && d.Estado != "Inactivo"
                select d
            ).FirstOrDefaultAsync();

            var queja = await (
                from q in _context.InformeQueja
                where q.Codigo == codigoQueja && q.Estado != "Inactivo"
                select q
            ).FirstOrDefaultAsync();

            if (departamento == null || queja == null)
                return BadRequest("Departamento o Informe de queja no encontrado");

            var quejaYaUsada = await (
                from iqd in _context.InformeQueja_Departamento
                where iqd.id_InformeQueja == queja.id_InformeQueja
                select iqd
            ).FirstOrDefaultAsync();

            if (quejaYaUsada != null)
                return BadRequest("Este informe de queja ya está asignado a un departamento");

            var nuevo = new InformeQueja_Departamento()
            {
                id_Departamento = departamento.id_Departamento,
                id_InformeQueja = queja.id_InformeQueja,
                CodigoPaciente = codigoPaciente,
                Estado = "Activo"
            };

            _context.InformeQueja_Departamento.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Relación creada correctamente" });
        }

        [HttpPut("actualizar-paciente/{codigoQueja}")]
        public async Task<IActionResult> PutCodigoPaciente(string codigoQueja, string codigoPaciente)
        {
            var relacion = await (
                from iqd in _context.InformeQueja_Departamento
                join q in _context.InformeQueja on iqd.id_InformeQueja equals q.id_InformeQueja
                where q.Codigo == codigoQueja && iqd.Estado != "Inactivo"
                select iqd
            ).FirstOrDefaultAsync();

            if (relacion == null)
                return NotFound("Relación no encontrada");

            relacion.CodigoPaciente = codigoPaciente;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Código de paciente actualizado correctamente" });
        }



        [HttpDelete("borrar/{codigoDepartamento}/{codigoQueja}")]
        public async Task<IActionResult> DeleteInformeQueja_Departamento(string codigoDepartamento, string codigoQueja)
        {
            var relacion = await (
                from iqd in _context.InformeQueja_Departamento
                join d in _context.Departamento on iqd.id_Departamento equals d.id_Departamento
                join q in _context.InformeQueja on iqd.id_InformeQueja equals q.id_InformeQueja
                where d.Codigo == codigoDepartamento && q.Codigo == codigoQueja
                select iqd
            ).FirstOrDefaultAsync();

            if (relacion == null)
                return NotFound();

            _context.InformeQueja_Departamento.Remove(relacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
