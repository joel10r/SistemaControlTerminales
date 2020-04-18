using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SCT.Models;
using PagedList;
using System.Data.Entity.Infrastructure;

namespace SCT.Controllers
{
    public class AnotacionsController : Controller
    {
        private SCT_DBEntities db = new SCT_DBEntities();

        // GET: Anotacions
        public ActionResult Index(int? page)
        {
            var anotacion = db.Anotacion.Include(a => a.TipoAnotacion);
            return View(anotacion.ToList().OrderBy(a => a.idTipoAnotacion)
                .Where(a => a.TipoAnotacion.nombreTipoAnotacion == "Abierto" ||
                    a.TipoAnotacion.nombreTipoAnotacion == "Escalado GTI").OrderByDescending(a => a.idAnotacion).ToPagedList(page ?? 1, 10));
        }


        // GET: Anotacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anotacion anotacion = db.Anotacion.Find(id);
            if (anotacion == null)
            {
                return HttpNotFound();
            }
            return View(anotacion);
        }

        // GET: Anotacions/Create
        public ActionResult Create()
        {
            ViewBag.idTipoAnotacion = new SelectList(db.TipoAnotacion, "idTipoAnotacion", "nombreTipoAnotacion");
            return View();
        }

        // POST: Anotacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idAnotacion,nombreUsuario,datosCliente,observacion,imei,idTipoAnotacion")] Anotacion anotacion)
        {
            if (ModelState.IsValid)
            {
                db.Anotacion.Add(anotacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idTipoAnotacion = new SelectList(db.TipoAnotacion, "idTipoAnotacion", "nombreTipoAnotacion", anotacion.idTipoAnotacion);
            return View(anotacion);
        }

        // GET: Anotacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anotacion anotacion = db.Anotacion.Find(id);
            if (anotacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.idTipoAnotacion = new SelectList(db.TipoAnotacion, "idTipoAnotacion", "nombreTipoAnotacion", anotacion.idTipoAnotacion);

            return View(anotacion);
        }

        // POST: Anotacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idAnotacion,nombreUsuario,datosCliente,observacion,imei,idTipoAnotacion")] Anotacion anotacion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(anotacion).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("IndexFrontalAnotacion");
                }
            } 
            catch (Exception e)
            {
                TempData["Message"] = e.Message.ToString();
            }
            ViewBag.idTipoAnotacion = new SelectList(db.TipoAnotacion, "idTipoAnotacion", "nombreTipoAnotacion", anotacion.idTipoAnotacion);
            return View(anotacion);
        }


        public ActionResult EditarConsulta(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anotacion anotacion = db.Anotacion.Find(id);
            if (anotacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.idTipoAnotacion = new SelectList(db.TipoAnotacion, "idTipoAnotacion", "nombreTipoAnotacion", anotacion.idTipoAnotacion);
            return View(anotacion);
        }

        // POST: Anotacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarConsulta([Bind(Include = "idAnotacion,nombreUsuario,datosCliente,observacion,imei,idTipoAnotacion")] Anotacion anotacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(anotacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ConsultaAnotacionFrontal");
            }
            ViewBag.idTipoAnotacion = new SelectList(db.TipoAnotacion, "idTipoAnotacion", "nombreTipoAnotacion", anotacion.idTipoAnotacion);
            return View(anotacion);
        }







        // GET: Anotacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Anotacion anotacion = db.Anotacion.Find(id);
            if (anotacion == null)
            {
                return HttpNotFound();
            }
            return View(anotacion);
        }

        // POST: Anotacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Anotacion anotacion = db.Anotacion.Find(id);
            db.Anotacion.Remove(anotacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // Frontal 

        [Authorize(Roles = "Frontal")]
        public ActionResult IndexFrontalAnotacion(int? page)
        {
            string usuario = User.Identity.GetUserName();
            string nombre = db.TipoAnotacion.ToString();
            var anotacion = db.Anotacion.Include(a => a.TipoAnotacion);

            return View(anotacion.ToList().OrderByDescending(a => a.idAnotacion).Where(
                a => a.nombreUsuario == usuario && (a.TipoAnotacion.nombreTipoAnotacion.ToString() ==
                    "Abierto" || a.TipoAnotacion.nombreTipoAnotacion.ToString() ==
                    "Escalado GTI")).ToPagedList(page ?? 1, 10));
        }

        public ActionResult CrearAnotacionFrontal()
        {
            ViewBag.idTipoAnotacion = new SelectList(db.TipoAnotacion, "idTipoAnotacion", "nombreTipoAnotacion");
            return View();
        }

        // POST: Anotacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearAnotacionFrontal([Bind(Include = "idAnotacion,nombreUsuario,datosCliente,observacion,imei,idTipoAnotacion")] Anotacion anotacion)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    anotacion.nombreUsuario = User.Identity.GetUserName();
                    db.Anotacion.Add(anotacion);
                    db.SaveChanges();
                    return RedirectToAction("IndexFrontalAnotacion");
                }
            }
            catch (DbUpdateException)
            {
                TempData["Message"] = "El IMEI " + anotacion.imei.ToString() + " ya registra una anotación";
            } 
            catch (Exception e)
            {
                TempData["Message"] = e.Message.ToString();
            }

            ViewBag.idTipoAnotacion = new SelectList(db.TipoAnotacion, "idTipoAnotacion", "nombreTipoAnotacion", anotacion.idTipoAnotacion);
            return View(anotacion);
        }

        [Authorize(Roles = "Frontal")]
        public ActionResult ConsultaAnotacionFrontal()
        {

            var consulta = db.Anotacion;
            return View((consulta.ToList().Where(c => c.imei == 0)));
        }

        [Authorize(Roles = "Frontal")]
        [HttpPost]
        public ActionResult ConsultaAnotacionFrontal(long? imei)
        {

            string usuario = User.Identity.GetUserName();
            var consulta = db.Anotacion;
            var resultado = consulta.ToList().Where(c => c.imei == imei);

            if (resultado.Count() == 0)
            {
                TempData["Message"] = "IMEI no registra una anotación";
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
                //    ViewBag.Message = "Imei no registra una anotación";
                //    resultado = consulta.ToList().Where(c => c.imei == imei && c.nombreUsuario == usuario);
                //}

            }

            return View(resultado);
        }


        // Supervisor


        [Authorize(Roles = "Supervisor")]
        public ActionResult ConsultaAnotacionSupervisor()
        {

            var consulta = db.Anotacion;
            return View((consulta.ToList().Where(c => c.imei == 0)));
        }

        [Authorize(Roles = "Supervisor")]
        [HttpPost]
        public ActionResult ConsultaAnotacionSupervisor(long? imei)
        {

            string usuario = User.Identity.GetUserName();
            var consulta = db.Anotacion;
            var resultado = consulta.ToList().Where(c => c.imei == imei);

            if (resultado.Count() == 0)
            {
                TempData["Message"] = "IMEI no registra una anotación";
            }

            return View(resultado);
        }

    }


}
