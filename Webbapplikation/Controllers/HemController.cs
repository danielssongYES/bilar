using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.Entity;
using Webbapplikation.Models;

namespace Webbapplikation.Controllers
{
	public class HemController : Controller
	{
		// GET: Hem
		public ActionResult Index()
		{
			Bildatabas db = new Bildatabas(@"Data Source=.\SQLEXPRESS;
											Initial Catalog=GustavsObjektdatabas;
											Integrated Security=True");
			var bilQuery = (from modell in db.Modeller
						 join märke in db.Märken on modell.märkeid equals märke.id
						 join stad in db.Städer on märke.stadid equals stad.id
						 join land in db.Länder on stad.landid equals land.id
						 orderby db.Random()
						 select new {
							 modell = modell.namn, år = modell.startår,
							 märke = märke.namn, stad = stad.namn, land = land.namn,
							 beskrivning = modell.beskrivning
						 }).FirstOrDefault();
			string[] beskrivningSträngar = bilQuery.beskrivning.Split(',');
			string outputSträng = bilQuery.år + " " + bilQuery.märke + " "
				+ bilQuery.modell + " " + bilQuery.stad + " " + bilQuery.land;
			foreach(string s in beskrivningSträngar)
				outputSträng += " " + s;
			return Content(outputSträng);
			//return View();
		}
	}
}