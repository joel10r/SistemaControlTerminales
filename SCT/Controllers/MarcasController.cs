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
    public class MarcasController : Controller
    {
        private SCT_DBEntities db = new SCT_DBEntities();

        // GET: Marcas
        [Authorize(Roles = "Administrador")]
        public ActionResult Index(int? page)
        {
            return View(db.Marca.ToList().OrderBy(m => m.nombreMarca).ToPagedList(page ?? 1, 10));
        }

        // GET: Marcas/Details/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marca.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // GET: Marcas/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Marcas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create([Bind(Include = "idMarca,nombreMarca")] Marca marca)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Marca.Add(marca);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                TempData["Message"] = "La marca: " + marca.nombreMarca.ToString() + " ya se encuentra registrada";
            } 
            catch (Exception e)
            {
                TempData["Message"] = e.Message.ToString();
            }

            return View(marca);
        }

        // GET: Marcas/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marca.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit([Bind(Include = "idMarca,nombreMarca")] Marca marca)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(marca).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            } catch (Exception e)
            {
                TempData["Message"] = e.Message.ToString();
            }
            return View(marca);
        }

        // GET: Marcas/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marca marca = db.Marca.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Marca marca = db.Marca.Find(id);
                db.Marca.Remove(marca);
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
