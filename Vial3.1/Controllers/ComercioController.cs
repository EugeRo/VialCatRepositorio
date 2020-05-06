using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vial3._1.Models;

namespace Vial3._1.Controllers
{
    public class ComercioController : Controller
    {
        private vialcatEntities db = new vialcatEntities();



        


        // GET: Comercio
        public ActionResult Index()
        {
            return View(db.vial_comercio.ToList());
        }

        // GET: Comercio/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vial_comercio vial_comercio = db.vial_comercio.Find(id);
            if (vial_comercio == null)
            {
                return HttpNotFound();
            }
            return View(vial_comercio);
        }

        // GET: Comercio/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comercio/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_comercio,razonsocial,domicilio,telefono,cuit")] vial_comercio vial_comercio)
        {
            if (ModelState.IsValid)
            {
                db.vial_comercio.Add(vial_comercio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vial_comercio);
        }

        // GET: Comercio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vial_comercio vial_comercio = db.vial_comercio.Find(id);
            if (vial_comercio == null)
            {
                return HttpNotFound();
            }
            return View(vial_comercio);
        }

        // POST: Comercio/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_comercio,razonsocial,domicilio,telefono,cuit")] vial_comercio vial_comercio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vial_comercio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vial_comercio);
        }

        // GET: Comercio/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vial_comercio vial_comercio = db.vial_comercio.Find(id);
            if (vial_comercio == null)
            {
                return HttpNotFound();
            }
            return View(vial_comercio);
        }

        // POST: Comercio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            vial_comercio vial_comercio = db.vial_comercio.Find(id);
            db.vial_comercio.Remove(vial_comercio);
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
    }
}
