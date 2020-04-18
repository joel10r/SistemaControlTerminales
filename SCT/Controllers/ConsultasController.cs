using Microsoft.AspNet.Identity;
using SCT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SCT.Controllers
{
    public class ConsultasController : Controller
    {
        private SCT_DBEntities db = new SCT_DBEntities();

        // GET: Consultas
        public ActionResult Index()
        {
            return View();
        
        }

       // Frontal

        [Authorize(Roles = "Frontal")]
        public ActionResult ConsultaImei()
        {

            var consulta = db.Solicitud;
            return View((consulta.ToList().Where(c => c.imei == 0)));
        }
        [Authorize(Roles = "Frontal")]
        [HttpPost]
        public ActionResult ConsultaImei(long? imei)
        {

            string usuario = User.Identity.GetUserName();
            var consulta = db.Solicitud;
            var resultado = consulta.ToList().Where(c => c.imei == imei);

            if (resultado.Count() == 0)
            {
                
                TempData["Message"] = "IMEI no registra una salida";
            }

            foreach (var result in resultado)
            {

                if (imei == result.imei && usuario == result.nombreUsuario)

                {
                    return View(resultado);
                }

                if (imei == result.imei && usuario != result.nombreUsuario)
                {
                    
                    TempData["Message"] = "No puede editar este IMEI";
                    resultado = consulta.ToList().Where(c => c.imei == imei && c.nombreUsuario == usuario);
                }

                //if (imei == null && usuario == null)
                //{
                //    ViewBag.Message = "Imei no registra una salida";
                //    resultado = consulta.ToList().Where(c => c.imei == imei && c.nombreUsuario == usuario);
                //}

            }

            return View(resultado);
        }

        [Authorize(Roles = "Frontal")]
        public ActionResult EditConsultaFrontal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud solicitud = db.Solicitud.Find(id);
            if (solicitud == null)
            {
                return HttpNotFound();
            }
            ViewBag.idFormaPago = new SelectList(db.FormaPago, "idFormaPago", "nombreFormaPago", solicitud.idFormaPago);
            ViewBag.idModelo = new SelectList(db.Modelo, "idModelo", "nombreModelo", solicitud.idModelo);
            ViewBag.idTipoTramite = new SelectList(db.TipoTramite, "idTipoTramite", "nombreTipoTramite", solicitud.idTipoTramite);
            return View(solicitud);
        }

        [Authorize(Roles = "Frontal")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConsultaFrontal([Bind(Include = "idOrden,imei,serie,imeiSustituido,fecha,nombreUsuario,datosCliente,cedulaCliente,pedido,telefono,idModelo,idFormaPago,idTipoTramite")] Solicitud solicitud)
        {
            if (ModelState.IsValid)
            {
                db.Entry(solicitud).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ConsultaImei");
            }
            ViewBag.idFormaPago = new SelectList(db.FormaPago, "idFormaPago", "nombreFormaPago", solicitud.idFormaPago);
            ViewBag.idModelo = new SelectList(db.Modelo, "idModelo", "nombreModelo", solicitud.idModelo);
            ViewBag.idTipoTramite = new SelectList(db.TipoTramite, "idTipoTramite", "nombreTipoTramite", solicitud.idTipoTramite);
            return View(solicitud);
        }

    




    // Supervisor


    [Authorize(Roles = "Supervisor")]
        public ActionResult ConsultaImeiSupervisor()
        {

            var consulta = db.Solicitud;
            return View((consulta.ToList().Where(c => c.imei == 0)));
        }
        [Authorize(Roles = "Supervisor")]

        [HttpPost]
        public ActionResult ConsultaImeiSupervisor(long? imei)
        {

            string usuario = User.Identity.GetUserName();
            var consulta = db.Solicitud;
            var resultado = consulta.ToList().Where(c => c.imei == imei);

            if (resultado.Count() == 0)
            {
                TempData["Message"] = "IMEI no registra una salida";
            }

            return View(resultado);
        }

        [Authorize(Roles = "Supervisor")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud solicitud = db.Solicitud.Find(id);
            if (solicitud == null)
            {
                return HttpNotFound();
            }
            return View(solicitud);
        }

        [Authorize(Roles = "Supervisor")]
        // POST: Solicituds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Solicitud solicitud = db.Solicitud.Find(id);
            db.Solicitud.Remove(solicitud);
            db.SaveChanges();
            return RedirectToAction("ConsultaImeiSupervisor");
        }

        [Authorize(Roles = "Supervisor")]
        public ActionResult EditConsultaSupervisor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud solicitud = db.Solicitud.Find(id);
            if (solicitud == null)
            {
                return HttpNotFound();
            }
            ViewBag.idFormaPago = new SelectList(db.FormaPago, "idFormaPago", "nombreFormaPago", solicitud.idFormaPago);
            ViewBag.idModelo = new SelectList(db.Modelo, "idModelo", "nombreModelo", solicitud.idModelo);
            ViewBag.idTipoTramite = new SelectList(db.TipoTramite, "idTipoTramite", "nombreTipoTramite", solicitud.idTipoTramite);
            return View(solicitud);
        }

        [Authorize(Roles = "Supervisor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConsultaSupervisor([Bind(Include = "idOrden,imei,serie,imeiSustituido,fecha,nombreUsuario,datosCliente,cedulaCliente,pedido,telefono,idModelo,idFormaPago,idTipoTramite")] Solicitud solicitud)
        {
            if (ModelState.IsValid)
            {
                db.Entry(solicitud).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ConsultaImeiSupervisor");
            }
            ViewBag.idFormaPago = new SelectList(db.FormaPago, "idFormaPago", "nombreFormaPago", solicitud.idFormaPago);
            ViewBag.idModelo = new SelectList(db.Modelo, "idModelo", "nombreModelo", solicitud.idModelo);
            ViewBag.idTipoTramite = new SelectList(db.TipoTramite, "idTipoTramite", "nombreTipoTramite", solicitud.idTipoTramite);
            return View(solicitud);
        }


        public ActionResult DetallesConsulta(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitud solicitud = db.Solicitud.Find(id);
            if (solicitud == null)
            {
                return HttpNotFound();
            }
            return View(solicitud);
        }
    }
}