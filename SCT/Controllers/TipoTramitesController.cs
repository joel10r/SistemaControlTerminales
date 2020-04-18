using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SCT.Models;
using PagedList;
using System.Data.Entity.Infrastructure;

namespace SCT.Controllers
{
    public class TipoTramitesController : Controller
    {
        private SCT_DBEntities db = new SCT_DBEntities();

        // GET: TipoTramites
        [Authorize(Roles = "Administrador")]
        public ActionResult Index(int? page)
        {
            return View(db.TipoTramite.ToList().OrderBy(t => t.nombreTipoTramite).ToPagedList(page ?? 1, 10));
        }

        // GET: TipoTramites/Details/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTramite tipoTramite = db.TipoTramite.Find(id);
            if (tipoTramite == null)
            {
                return HttpNotFound();
            }
            return View(tipoTramite);
        }

        // GET: TipoTramites/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoTramites/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create([Bind(Include = "idTipoTramite,nombreTipoTramite")] TipoTramite tipoTramite)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.TipoTramite.Add(tipoTramite);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                TempData["Message"] = "El tipo de tramite: " + tipoTramite.nombreTipoTramite.ToString() + " ya se encuentra registrado";
            }
            catch (Exception e)
            {
                TempData["Message"] = e.Message.ToString();
            }

            return View(tipoTramite);
        }

        // GET: TipoTramites/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTramite tipoTramite = db.TipoTramite.Find(id);
            if (tipoTramite == null)
            {
                return HttpNotFound();
            }
            return View(tipoTramite);
        }

        // POST: TipoTramites/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit([Bind(Include = "idTipoTramite,nombreTipoTramite")] TipoTramite tipoTramite)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tipoTramite).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["Message"] = e.Message.ToString();
            }
            return View(tipoTramite);
        }

        // GET: TipoTramites/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTramite tipoTramite = db.TipoTramite.Find(id);
            if (tipoTramite == null)
            {
                return HttpNotFound();
            }
            return View(tipoTramite);
        }

        // POST: TipoTramites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                TipoTramite tipoTramite = db.TipoTramite.Find(id);
                db.TipoTramite.Remove(tipoTramite);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                TempData["Message"] = e.Message.ToString();
            }
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
    }
}
