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
    public class ModeloesController : Controller
    {
        private SCT_DBEntities db = new SCT_DBEntities();

        // GET: Modeloes
        [Authorize(Roles = "Administrador")]
        public ActionResult Index(int? page)
        {
            var modelo = db.Modelo.Include(m => m.Marca);
            return View(modelo.ToList().OrderBy(m => m.Marca.nombreMarca).ToPagedList(page ?? 1, 10));
        }

        // GET: Modeloes/Details/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelo modelo = db.Modelo.Find(id);
            if (modelo == null)
            {
                return HttpNotFound();
            }
            return View(modelo);
        }

        // GET: Modeloes/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.idMarca = new SelectList(db.Marca, "idMarca", "nombreMarca");
            return View();
        }

        // POST: Modeloes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create([Bind(Include = "idModelo,nombreModelo,idMarca")] Modelo modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Modelo.Add(modelo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            } catch (DbUpdateException)
            {
                TempData["Message"] = "El modelo: " + modelo.nombreModelo.ToString() + " ya se encuentra registrada";
            }
            catch (Exception e)
            {
                TempData["Message"] = e.Message.ToString();
            }


            ViewBag.idMarca = new SelectList(db.Marca, "idMarca", "nombreMarca", modelo.idMarca);
            return View(modelo);
        }

        // GET: Modeloes/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelo modelo = db.Modelo.Find(id);
            if (modelo == null)
            {
                return HttpNotFound();
            }
            ViewBag.idMarca = new SelectList(db.Marca, "idMarca", "nombreMarca", modelo.idMarca);
            return View(modelo);
        }

        // POST: Modeloes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit([Bind(Include = "idModelo,nombreModelo,idMarca")] Modelo modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(modelo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["Message"] = e.Message.ToString();
            }
            ViewBag.idMarca = new SelectList(db.Marca, "idMarca", "nombreMarca", modelo.idMarca);
            return View(modelo);
        }

        // GET: Modeloes/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modelo modelo = db.Modelo.Find(id);
            if (modelo == null)
            {
                return HttpNotFound();
            }
            return View(modelo);
        }

        // POST: Modeloes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Modelo modelo = db.Modelo.Find(id);
                db.Modelo.Remove(modelo);
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
