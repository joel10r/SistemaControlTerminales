using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using SCT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCT.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Roles
        public ActionResult Index(int? page)
        {
            return View(db.Roles.ToList().ToPagedList(page ?? 1, 10));
        }

        // GET: Roles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Roles.Add(role);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(role);
            }
            catch (DbEntityValidationException)
            {
                TempData["Message"] = "El rol: " + role.Name.ToString() + " ya se encuentra registrado";
            }
            catch (Exception e)
            {
                TempData["Message"] = e.Message.ToString();
            }

            return View();
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(string id)
        {
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }


            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(role);
            }
            catch (Exception e)
            {
                TempData["Message"] = e.Message.ToString();
                return View();
            }
        }
            // GET: Roles/Delete/5
            public ActionResult Delete(string id)
        {
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(IdentityRole deleteRol)
        {
            try
            {
                IdentityRole role = db.Roles.Find(deleteRol.Id);
                db.Roles.Remove(role);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                TempData["Message"] = e.Message.ToString();
            }
            return RedirectToAction("Index");
        }
    }
}
