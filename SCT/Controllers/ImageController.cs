using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SCT.Models;
using Image = SCT.Models.Image;

namespace SCT.Controllers
{
    public class ImageController : Controller
    {

        [HttpGet]
        // GET: Image
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Image imageModel)
        {

            string filename = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            filename = filename + DateTime.Now.ToString("yymmssffff") + extension;
            imageModel.imagePath = "~/Image/" + filename;
            filename = Path.Combine(Server.MapPath("~/Image/"), filename);
            imageModel.ImageFile.SaveAs(filename);
            using (SCT_DBEntities db = new SCT_DBEntities())
            {
                db.Image.Add(imageModel);
                db.SaveChanges();
            }
            ModelState.Clear();
                return View();

        }
        [HttpGet]

        public ActionResult View(int id)
        {
            Image ImageModel = new Image();
            using (SCT_DBEntities db = new SCT_DBEntities())
            {
                ImageModel = db.Image.Where(x => x.imageID == id).FirstOrDefault();
            }
            return View(ImageModel);
        }

    }
}