using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Webbapplikation.Models;

namespace Webbapplikation.Controllers
{
    public class ModellController : Controller
    {
        private GustavsObjektdatabasEntities db = new GustavsObjektdatabasEntities();

        // GET: Modell
        public ActionResult Index()
        {
            var modeller = db.Modeller.Include(m => m.Märken).Include(m => m.Märken.Städer).Include(m => m.Märken.Städer.Länder);
            return View(modeller.ToList());
        }

        // GET: Modell/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modeller modeller = db.Modeller.Find(id);
            if (modeller == null)
            {
                return HttpNotFound();
            }
            return View(modeller);
        }

        // GET: Modell/Create
        public ActionResult Create()
        {
            ViewBag.märkeid = new SelectList(db.Märken, "id", "namn");
            return View();
        }

        // POST: Modell/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,namn,konfig,volym,kaross,cyl,startår,slutår,märkeid,beskrivning")] Modeller modeller)
        {
            if (ModelState.IsValid)
            {
                db.Modeller.Add(modeller);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.märkeid = new SelectList(db.Märken, "id", "namn", modeller.märkeid);
            return View(modeller);
        }

        // GET: Modell/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modeller modeller = db.Modeller.Find(id);
            if (modeller == null)
            {
                return HttpNotFound();
            }
            ViewBag.märkeid = new SelectList(db.Märken, "id", "namn", modeller.märkeid);
            return View(modeller);
        }

        // POST: Modell/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,namn,konfig,kaross,startår")] Modeller modeller)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modeller).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.märkeid = new SelectList(db.Märken, "id", "namn", modeller.märkeid);
            return View(modeller);
        }

        // GET: Modell/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modeller modeller = db.Modeller.Find(id);
            if (modeller == null)
            {
                return HttpNotFound();
            }
            return View(modeller);
        }

        // POST: Modell/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Modeller modeller = db.Modeller.Find(id);
            db.Modeller.Remove(modeller);
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
