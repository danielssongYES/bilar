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
    public class MärkeController : Controller
    {
        private GustavsObjektdatabasEntities db = new GustavsObjektdatabasEntities();

        // GET: Märke
        public ActionResult Index()
        {
            var märken = db.Märken.Include(m => m.Städer);
            return View(märken.ToList());
        }

        // GET: Märke/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Märken märken = db.Märken.Find(id);
            if (märken == null)
            {
                return HttpNotFound();
            }
            return View(märken);
        }

        // GET: Märke/Create
        public ActionResult Create()
        {
            ViewBag.stadid = new SelectList(db.Städer, "id", "namn");
            return View();
        }

        // POST: Märke/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,namn,stadid")] Märken märken)
        {
            if (ModelState.IsValid)
            {
                db.Märken.Add(märken);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.stadid = new SelectList(db.Städer, "id", "namn", märken.stadid);
            return View(märken);
        }

        // GET: Märke/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Märken märken = db.Märken.Find(id);
            if (märken == null)
            {
                return HttpNotFound();
            }
            ViewBag.stadid = new SelectList(db.Städer, "id", "namn", märken.stadid);
            return View(märken);
        }

        // POST: Märke/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,namn,stadid")] Märken märken)
        {
            if (ModelState.IsValid)
            {
                db.Entry(märken).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.stadid = new SelectList(db.Städer, "id", "namn", märken.stadid);
            return View(märken);
        }

        // GET: Märke/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Märken märken = db.Märken.Find(id);
            if (märken == null)
            {
                return HttpNotFound();
            }
            return View(märken);
        }

        // POST: Märke/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Märken märken = db.Märken.Find(id);
            db.Märken.Remove(märken);
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
