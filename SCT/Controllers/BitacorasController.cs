using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.AspNet.Identity;
using PagedList;
using SCT.Models;

namespace SCT.Controllers
{
    public class BitacorasController : Controller
    {
        private SCT_DBEntities db = new SCT_DBEntities();

        // GET: Bitacoras
        public ActionResult Index(int? page)
        {
            return View(db.Bitacora.ToList().OrderByDescending(b => b.fecha).ToPagedList(page ?? 1, 10));
        }


        public ActionResult ReporteGeneral()
        {
            string usuario = User.Identity.GetUserName().ToString();


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "BitacoraGeneral.rpt"));
            

            rd.SetDataSource(db.Bitacora.Select(s => new
            {
                idBitacora = s.idBitacora,
                usuario = s.usuario,
                accion = s.accion,
                fecha = s.fecha,

            }).OrderByDescending(s => s.idBitacora).ToList());


            rd.SetParameterValue("usuario", usuario);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Bitacora General " + ".pdf");
            }
            catch
            {
                throw;
            }
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
