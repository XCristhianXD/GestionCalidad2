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
    public class InformeCalidad_DepartamentoController : ControllerBase
    {
        private readonly GestionCalidadContext _context;

        public InformeCalidad_DepartamentoController(GestionCalidadContext context)
        {
            _context = context;
        }

        [HttpGet("reporte/todos")]
        public async Task<IActionResult> GetTodosLosInformes()
        {
            var lista = await (
                from icd in _context.InformeCalidad_Departamento
                join d in _context.Departamento on icd.id_Departamento equals d.id_Departamento
                join i in _context.InformeCalidad on icd.id_InformeCalidad equals i.Id_InformeCalidad
                where icd.Estado != "Inactivo"
                select new
                {
                    Departamento = d.Nombre,
                    Codigo = i.Codigo,
                    Calificacion = i.Calificacion,
                    Descripcion = i.Descripcion,
                    Fecha = i.Fecha
                }
            ).ToListAsync();

            return Ok(lista);
        }

        [HttpGet("reporte/promedio-sistema")]
        public async Task<IActionResult> GetPromedioSistema()
        {
            var promedio = await _context.InformeCalidad
                .Where(i => i.Estado != "Inactivo")
                .AverageAsync(i => i.Calificacion);

            return Ok(new { PromedioSistema = promedio });
        }

        //=====================================================

        [HttpGet("reporte/promedio/general")]
        public async Task<IActionResult> GetPromedioGeneral()
        {
            var reporte = await (
                from icd in _context.InformeCalidad_Departamento
                join d in _context.Departamento on icd.id_Departamento equals d.id_Departamento
                join i in _context.InformeCalidad on icd.id_InformeCalidad equals i.Id_InformeCalidad
                where icd.Estado != "Inactivo"
                group i by d.Nombre into g
                select new
                {
                    Departamento = g.Key,
                    Promedio = g.Average(x => x.Calificacion),
                    Total = g.Count()
                }
            )
            .OrderByDescending(x => x.Promedio)
            .ToListAsync();

            return Ok(reporte);
        }
        [HttpGet("reporte/promedio/anio/{anio}")]
        public async Task<IActionResult> GetPromedioPorAnio(int anio)
        {
            var reporte = await (
                from icd in _context.InformeCalidad_Departamento
                join d in _context.Departamento on icd.id_Departamento equals d.id_Departamento
                join i in _context.InformeCalidad on icd.id_InformeCalidad equals i.Id_InformeCalidad
                where i.Fecha.Year == anio && icd.Estado != "Inactivo"
                group i by d.Nombre into g
                select new
                {
                    Departamento = g.Key,
                    Promedio = g.Average(x => x.Calificacion),
                    Total = g.Count()
                }
            )
            .OrderByDescending(x => x.Promedio)
            .ToListAsync();

            return Ok(reporte);
        }

        [HttpGet("reporte/promedio/anio/{anio}/mes/{mes}")]
        public async Task<IActionResult> GetPromedioPorMes(int anio, int mes)
        {
            if (mes < 1 || mes > 12)
                return BadRequest("Mes inválido");

            var reporte = await (
                from icd in _context.InformeCalidad_Departamento
                join d in _context.Departamento on icd.id_Departamento equals d.id_Departamento
                join i in _context.InformeCalidad on icd.id_InformeCalidad equals i.Id_InformeCalidad
                where i.Fecha.Year == anio
                      && i.Fecha.Month == mes
                      && icd.Estado != "Inactivo"
                group i by d.Nombre into g
                select new
                {
                    Departamento = g.Key,
                    Promedio = g.Average(x => x.Calificacion),
                    Total = g.Count()
                }
            )
            .OrderByDescending(x => x.Promedio)
            .ToListAsync();

            return Ok(reporte);
        }


        //==============================================================================

        [HttpGet("reporte/departamento/{codigoDepartamento}")]
        public async Task<IActionResult> GetPorDepartamento(string codigoDepartamento)
        {
            var lista = await (
                from icd in _context.InformeCalidad_Departamento
                join d in _context.Departamento on icd.id_Departamento equals d.id_Departamento
                join i in _context.InformeCalidad on icd.id_InformeCalidad equals i.Id_InformeCalidad
                where d.Codigo == codigoDepartamento
                      && icd.Estado != "Inactivo"
                select new
                {
                    Departamento = d.Nombre,
                    i.Codigo,
                    i.Calificacion,
                    i.Descripcion,
                    Fecha = i.Fecha
                }
            ).ToListAsync();

            return Ok(lista);
        }

        [HttpGet("reporte/departamento/{codigoDepartamento}/anio/{anio}")]
        public async Task<IActionResult> GetPorDepartamentoAnio(string codigoDepartamento, int anio)
        {
            var lista = await (
                from icd in _context.InformeCalidad_Departamento
                join d in _context.Departamento on icd.id_Departamento equals d.id_Departamento
                join i in _context.InformeCalidad on icd.id_InformeCalidad equals i.Id_InformeCalidad
                where d.Codigo == codigoDepartamento
                      && i.Fecha.Year == anio
                      && icd.Estado != "Inactivo"
                select new
                {
                    Departamento = d.Nombre,
                    i.Codigo,
                    i.Calificacion,
                    i.Descripcion,
                    Fecha = i.Fecha
                }
            ).ToListAsync();

            return Ok(lista);
        }

        [HttpGet("reporte/departamento/{codigoDepartamento}/anio/{anio}/mes/{mes}")]
        public async Task<IActionResult> GetPorDepartamentoAnioMes(string codigoDepartamento, int anio, int mes)
        {
            if (mes < 1 || mes > 12)
                return BadRequest("Mes inválido");

            var lista = await (
                from icd in _context.InformeCalidad_Departamento
                join d in _context.Departamento on icd.id_Departamento equals d.id_Departamento
                join i in _context.InformeCalidad on icd.id_InformeCalidad equals i.Id_InformeCalidad
                where d.Codigo == codigoDepartamento
                      && i.Fecha.Year == anio
                      && i.Fecha.Month == mes
                      && icd.Estado != "Inactivo"
                select new
                {
                    Departamento = d.Nombre,
                    i.Codigo,
                    i.Calificacion,
                    i.Descripcion,
                    Fecha = i.Fecha
                }
            ).ToListAsync();

            return Ok(lista);
        }




        [HttpPost("crear")]
        public async Task<IActionResult> PostInformeCalidad_Departamento(string codigoDepartamento, string codigoInforme, string codigoAtencion)
        {
            var departamento = await (from d in _context.Departamento
                                      where d.Codigo == codigoDepartamento && d.Estado != "Inactivo"
                                      select d).FirstOrDefaultAsync();

            var informe = await (from i in _context.InformeCalidad
                                 where i.Codigo == codigoInforme && i.Estado != "Inactivo"
                                 select i).FirstOrDefaultAsync();

            if (departamento == null || informe == null)
                return BadRequest("Departamento o Informe no encontrado");

            var informeYaUsado = await (from icd in _context.InformeCalidad_Departamento
                                        where icd.id_InformeCalidad == informe.Id_InformeCalidad
                                        select icd).FirstOrDefaultAsync();

            if (informeYaUsado != null)
                return BadRequest("Este informe ya está asignado a un departamento");

            var nuevo = new InformeCalidad_Departamento()
            {
                id_Departamento = departamento.id_Departamento,
                id_InformeCalidad = informe.Id_InformeCalidad,
                CodigoAtencion = codigoAtencion,
                Estado = "Activo"
            };

            _context.InformeCalidad_Departamento.Add(nuevo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Relación creada correctamente" });
        }

        [HttpPut("actualizar-atencion/{codigoInforme}")]
        public async Task<IActionResult> PutCodigoAtencion(string codigoInforme, string codigoAtencion)
        {
            var relacion = await (
                from icd in _context.InformeCalidad_Departamento
                join i in _context.InformeCalidad on icd.id_InformeCalidad equals i.Id_InformeCalidad
                where i.Codigo == codigoInforme && icd.Estado != "Inactivo"
                select icd
            ).FirstOrDefaultAsync();

            if (relacion == null)
                return NotFound("Relación no encontrada");

            relacion.CodigoAtencion = codigoAtencion;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Código de atención actualizado correctamente" });
        }


        [HttpDelete("borrar/{codigoDepartamento}/{codigoInforme}")]
        public async Task<IActionResult> DeleteInformeCalidad_Departamento(string codigoDepartamento, string codigoInforme)
        {
            var relacion = await (
                from icd in _context.InformeCalidad_Departamento
                join d in _context.Departamento on icd.id_Departamento equals d.id_Departamento
                join i in _context.InformeCalidad on icd.id_InformeCalidad equals i.Id_InformeCalidad
                where d.Codigo == codigoDepartamento && i.Codigo == codigoInforme
                select icd
            ).FirstOrDefaultAsync();

            if (relacion == null)
                return NotFound();

            _context.InformeCalidad_Departamento.Remove(relacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
