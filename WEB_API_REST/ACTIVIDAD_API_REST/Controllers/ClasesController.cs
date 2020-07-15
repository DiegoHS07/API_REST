using System;
using ConectarDatos;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Management;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity;
using System.Diagnostics;

namespace ACTIVIDAD_API_REST.Controllers
{
    public class ClasesController : ApiController
    {
        private Actividad_API_REST_Entities dbContext = new Actividad_API_REST_Entities();
        private object claseEntities;

        [HttpGet]
        public IEnumerable<tablaCuenta> Get()
        {
            using (Actividad_API_REST_Entities claseEntities =  new Actividad_API_REST_Entities())
            {
                return claseEntities.tablaCuentas.ToList();
            }
        }

        [HttpGet]
        public tablaCuenta Get(int id)
        {
            using (Actividad_API_REST_Entities claseEntities = new Actividad_API_REST_Entities())
            {
                return claseEntities.tablaCuentas.FirstOrDefault(e => e.idCuenta == id);
            }
        }

        [HttpPost]
        public IHttpActionResult AgregarCuenta([FromBody] tablaCuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                dbContext.tablaCuentas.Add(cuenta);
                dbContext.SaveChanges();
                return Ok(cuenta);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IHttpActionResult IniciarSesion(string usuario, string contraseña)
        {

            var contraseñaCuenta = dbContext.tablaCuentas.FirstOrDefault(e => e.usuarioCuenta == usuario);

            if(contraseñaCuenta != null)
            {
                if(contraseñaCuenta.accesoCuenta != "0")
                {
                    if (contraseñaCuenta.contraseñaCuenta != contraseña)
                    {
                        return Ok("La contraseña no coincide con la guardada.");
                    }
                    else
                    {
                        var idcuenta = contraseñaCuenta.idCuenta;
                        return Ok(dbContext.tablaCuentas.Find(idcuenta));
                    }
                }
                else
                {
                    return Ok("El usuario esta innactivo.");
                }
                
            }            
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public IHttpActionResult ActualizarCuenta(int id, [FromBody]tablaCuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                var CuentaExiste = dbContext.tablaCuentas.Count(c => c.idCuenta == id) > 0;

                if (CuentaExiste)
                {
                    dbContext.Entry(cuenta).State = EntityState.Modified;
                    dbContext.SaveChanges();

                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public IHttpActionResult ActualizarContraseña(int id, [FromBody] tablaCuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                var CuentaExiste = dbContext.tablaCuentas.Count(c => c.idCuenta == id) > 0;

                if (CuentaExiste)
                {
                    dbContext.Entry(cuenta).State = EntityState.Modified;
                    dbContext.SaveChanges();

                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IHttpActionResult EliminarCuenta(int id)
        {
            var cuenta = dbContext.tablaCuentas.Find(id);

            if(cuenta != null)
            {
                cuenta.accesoCuenta.Replace("1","0");
                dbContext.SaveChanges();

                return Ok(cuenta);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
