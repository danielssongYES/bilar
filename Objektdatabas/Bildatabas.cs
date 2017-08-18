using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using UnidecodeSharpFork;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;

namespace Objektdatabas {
	class Bildatabas : DataContext {
		public Table<Märke> MärkenTabell;
		public Table<Bil> ModellerTabell;
		public Table<Land> LänderTabell;
		public Table<Ord> OrdTabell;
		public Table<Namn> NamnTabell;
		public Table<Stad> StäderTabell;
		public Table<Språk> SpråkTabell;
		public Table<Adjektiv> AdjektivTabell;

		public Bildatabas(string connection) : base(connection) { }

		public void FyllNamn(int antal, ProgressBar förlopp) {
			Random r = new Random();
			string kategoriTitel;
			List<int> färdigaSpråk = new List<int>();
			this.ExecuteCommand("TRUNCATE TABLE Namn");
			Namn namn;
			List<string> wikiNamn;
			IQueryable<Språk> språkQuery =
				from språk in this.SpråkTabell
				select språk;
			foreach(Språk språk in språkQuery) {
				förlopp.Value = Math.Round((Double)((färdigaSpråk.Count / språkQuery.Count()) * 100));
				if(färdigaSpråk.Contains(språk.id) == false) {
					kategoriTitel = "Category:" + språk.namn + "-language surnames";
					wikiNamn = new List<string>();
					string continueKod = "";
					int iterationer = 0;
					int max = 500;
					if(antal < max)
						max = antal;
					WikiXml wikiXml;
					do {
						wikiXml = new WikiXml("en.wikipedia.org", kategoriTitel, max, continueKod);
						wikiNamn.AddRange(wikiXml.innehållLista);
						continueKod = wikiXml.utContinueKod;
						//Fortsätt så länge det finns fler resultat.
					} while(continueKod != "");
					foreach(string namnString in wikiNamn) {
						iterationer++;
						if(r.Next(wikiNamn.Count) <= antal / this.LänderTabell.Count() &&
							namnString.StartsWith("Category:") == false &&
							namnString.StartsWith("List of") == false &&
							namnString.StartsWith("Template:") == false &&
							namnString.Contains("family") == false) {
							namn = new Namn();
							namn.namn = namnString;
							if(namn.namn.EndsWith(")"))
								namn.namn = namn.namn.Substring(0, namn.namn.IndexOf(" ("));
							if(namn.namn.Contains(" ") && char.IsLower(namn.namn[namn.namn.LastIndexOf(" ") + 1]))
								namn.namn = namn.namn.Substring(0, namn.namn.LastIndexOf(" "));
							namn.språkid = språk.id;
							Console.WriteLine(namn.namn);
							this.NamnTabell.InsertOnSubmit(namn);
						}
					}
					färdigaSpråk.Add(språk.id);
				}
			}
			this.SubmitChanges();
			förlopp.Value = 0;
		}

		public void FyllOrd(int antal, ProgressBar förlopp) {
			Random r = new Random();
			string kategoriTitel;
			List<int> färdigaSpråk = new List<int>();
			this.ExecuteCommand("TRUNCATE TABLE Ord");
			Ord ord;
			List<string> wikiOrd;
			IQueryable<Språk> språkQuery =
				from språk in this.SpråkTabell
				select språk;
			int antalMult;
			foreach(Språk språk in språkQuery) {
				förlopp.Value = Math.Round((Double)((färdigaSpråk.Count / språkQuery.Count()) * 100));
				if(färdigaSpråk.Contains(språk.id) == false) {
					kategoriTitel = "Category:" + språk.namn + " lemmas";
					wikiOrd = new List<string>();
					string continueKod = "";
					int max = 500;
					if(antal < max)
						max = antal;
					WikiXml wikiXml;
					do {
						wikiXml = new WikiXml("en.wiktionary.org", kategoriTitel, max, continueKod);
						wikiOrd.AddRange(wikiXml.innehållLista);
						continueKod = wikiXml.utContinueKod;
					} while(continueKod != "");
					antalMult = antal / this.LänderTabell.Count();
					foreach(string ordString in wikiOrd) {
						if(r.Next(wikiOrd.Count) <= antalMult &&
							ordString.StartsWith("Category:") == false) {
							Console.WriteLine(ordString);
							ord = new Ord();
							ord.ord = ordString;
							ord.språkid = språk.id;
							this.OrdTabell.InsertOnSubmit(ord);
						}
					}
					färdigaSpråk.Add(språk.id);
				}
			}
			this.SubmitChanges();
			förlopp.Value = 0;
		}

		public void FyllAdjektiv(int antal, ProgressBar förlopp) {
			int räknare = 0;
			Random r = new Random();
			string kategoriTitel;
			List<int> färdigaSpråk = new List<int>();
			this.ExecuteCommand("TRUNCATE TABLE Adjektiv");
			Adjektiv a;
			List<string> wikiAdjektiv;
			kategoriTitel = "Kategori:Svenska/Adjektiv";
			wikiAdjektiv = new List<string>();
			string continueKod = "";
			int max = 500;
			if(antal < max)
				max = antal;
			WikiXml wikiXml;
			do {
				wikiXml = new WikiXml("sv.wiktionary.org", kategoriTitel, max, continueKod);
				wikiAdjektiv.AddRange(wikiXml.innehållLista);
				continueKod = wikiXml.utContinueKod;
			} while(continueKod != "");
			foreach(string ordString in wikiAdjektiv) {
				if(r.Next(wikiAdjektiv.Count) <= antal &&
					ordString.StartsWith("Kategori:") == false &&
					//Olämpliga ändelser på adjektiv.
					ordString.EndsWith("des") == false &&
					ordString.EndsWith("t") == false) {
						räknare++;
						if(räknare > antal)
							förlopp.Value = 100;
						else
							förlopp.Value = Math.Round((Double)((räknare / antal) * 100));
						Console.WriteLine(ordString);
						a = new Adjektiv();
						a.ord = ordString;
						this.AdjektivTabell.InsertOnSubmit(a);
				}
			}
			this.SubmitChanges();
			förlopp.Value = 0;
		}

		public void FyllMärken(int antal, ProgressBar förlopp) {
			//Denna metod tömmer märkestabellen och befolkar den med nya data.
			//Parametern 'antal' avgör antalet rader.
			List<string> upptagnaNamn = new List<string>();
			Random r = new Random();
			int läge;
			Märke märke;
			//För att kunna tillämpa Unidecode på icke-latinska textsträngar.
			TextInfo textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
			for(int i = 0; i < antal; i++) {
				förlopp.Value = Math.Round((Double)((i / antal) * 100));
				märke = new Märke();
				//Välj en slumpmässig hemstad för det nya märket.
				var stadQuery = (
					from stad in this.StäderTabell
					join land in this.LänderTabell on stad.landid equals land.id
					orderby this.Random()
					select new {stadid = stad.id, landid = land.id}).Take(1).First();
				märke.stadid = stadQuery.stadid;
				läge = r.Next(1, 8);
				switch(läge) {
					case 1:
						//Namnet är en förkortning.
						char[] namnAkro = new char[3];
						for(int j = 0; j < 3; j++)
							namnAkro[j] = (char)('a' + r.Next(26));
						märke.namn = new string(namnAkro).ToUpper();
						break;
					case 2:
						//Namnet är ett ord på latin.
						var ordQuery = (
							from ord in this.OrdTabell
							join språk in this.SpråkTabell on ord.språkid equals språk.id
							where språk.namn == "Latin"
							orderby this.Random()
							select ord.ord).FirstOrDefault().ToString();
						märke.namn = textInfo.ToTitleCase(ordQuery.Unidecode());
						break;
					default:
						//Namnet är ett inhemskt efternamn.
						string namnString = (
							from stad in this.StäderTabell
							join land in this.LänderTabell on stad.landid equals land.id
							join språk in this.SpråkTabell on land.namnspråkid equals språk.id
							join namn in this.NamnTabell on språk.id equals namn.språkid
							where stad.id == märke.stadid
							orderby this.Random()
							select namn.namn).First();
						märke.namn = textInfo.ToTitleCase(namnString.Unidecode());
						break;
				}
				string stadNamn = (
					from stad in this.StäderTabell
					where stad.id == märke.stadid
					select stad.namn).First();
				//Kontrollera att märkets namn inte är upptaget innan bekräftelse.
				if((upptagnaNamn.Contains(märke.namn) == false) && märke.namn.Length <= 50) {
					this.MärkenTabell.InsertOnSubmit(märke);
					upptagnaNamn.Add(märke.namn);
				}
			}
			this.SubmitChanges();
			förlopp.Value = 0;
		}
		public void FyllModeller(int antal, ProgressBar förlopp) {
			//Denna metod tömmer modelltabellen och befolkar den med nya data.
			//Parametern 'antal' avgör antalet nya modeller.
			List<string> upptagnaNamn = new List<string>();
			Random r = new Random();
			//För att kunna tillämpa Unidecode på icke-Latinska textsträngar.
			TextInfo textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
			Bil bil;
			int språkId;
			int t;
			//Möjliga data för modellens konfiguration.
			string[] karossVal = {
				"Sedan", "Kombi", "Halvkombi",
				"Stadsjeep", "Kupé", "Cabriolet",
				"Minibuss"
			};
			string[] konfigVal = { "FF", "FR", "F4", "MR", "M4", "RR", "R4" };
			int[] ickeLatin = {6, 26, 15, 5, 14, 24, 9, 17};

			for(int i = 0; i < antal; i++) {
				förlopp.Value = Math.Round((Double)((i / antal) * 100));
				bil = new Bil();
				//Välj ett slumpmässigt märke som den nya modellen skall tillhöra.
				var bilQuery = (
					from märke in this.MärkenTabell
					orderby this.Random()
					select new {märkeid = märke.id, stadid = märke.stadid}).Take(1).First();
				bil.märkeid = bilQuery.märkeid;
				//Välj ett antal beskrivande adjektiv.
				string beskrivningSträng = "";
				for(int j = 0; j <= r.Next(1, 4); j++) {
					if(beskrivningSträng.Length != 0)
						beskrivningSträng += ", ";
					beskrivningSträng += (from adjektiv in this.AdjektivTabell
										 orderby this.Random()
										 select adjektiv.ord).First();
				}
				if(beskrivningSträng == "")
					beskrivningSträng = null;
				else {
					int sistaKommaIndex = beskrivningSträng.LastIndexOf(',');
					beskrivningSträng = beskrivningSträng.Remove(sistaKommaIndex, 1).Insert(sistaKommaIndex, " och");
					beskrivningSträng = beskrivningSträng.Length > 100 ? beskrivningSträng.Substring(0, 100) : beskrivningSträng;
				}
				bil.beskrivning = beskrivningSträng;
				if(r.Next(6) > 1) {
					//Modellnamnet är ett ord.
					if(r.Next(3) == 0)
						//Modellnamnet är ett ord på Latin.
						språkId = 23;
					else
						//Modellnamnet är ett inhemskt ord.
						språkId = (
							from land in this.LänderTabell
							join stad in this.StäderTabell on land.id equals stad.landid
							join märke in this.MärkenTabell on stad.id equals märke.stadid
							where märke.id == bil.märkeid
							select land.språkid
						).First();
					string ordQuery = (
						from ord in this.OrdTabell
						where ord.språkid == språkId
						orderby this.Random()
						select ord.ord).Take(1).First();
					if(ickeLatin.Contains(språkId))
						bil.namn = textInfo.ToTitleCase(ordQuery.Unidecode());
					else
						bil.namn = textInfo.ToTitleCase(ordQuery);
				}
				else {
					//Modellnamnet är ett nummer.
					t = r.Next(4) * 1000;
					if(r.Next(2) < 1)
						t += r.Next(999);
					bil.namn = t.ToString();
				}

				//Slumpa motorkonfigurationen.
				bil.cyl = 4;
				while(r.Next(4) < 1)
					bil.cyl += 2;

				bil.volym = bil.cyl * (400 + r.Next(-100, 200)) + (bil.cyl - 4) * r.Next(150, 550);

				//Slumpa tillverkningsår.
				bil.startår = r.Next(DateTime.Now.Year - 40, DateTime.Now.Year);
				bil.slutår = r.Next(bil.startår + 1, DateTime.Now.Year);
				bil.kaross = karossVal[r.Next(karossVal.Length)];
				bil.konfig = konfigVal[r.Next(konfigVal.Length)];
				if(r.Next(3) < 2) //'FF' är den vanligaste drivlinekonfigurationen.
					bil.konfig = "FF";
				//Kontrollera att modellens namn inte är upptaget innan bekräftelse.
				if((upptagnaNamn.Contains(bil.namn) == false) && bil.namn.Length <= 50) {
					this.ModellerTabell.InsertOnSubmit(bil);
					Console.WriteLine(bil.namn + " - " + bil.beskrivning);
					upptagnaNamn.Add(bil.namn);
				}
			}
			this.SubmitChanges();
			förlopp.Value = 0;
		}

		public void TömTabell(string tabell) {
			string kommando = String.Format("DELETE FROM {0} DBCC CHECKIDENT ({0}, RESEED, 0)", tabell);
			this.ExecuteCommand(kommando);
		}

		//Slumpmetod för LINQ to SQL.
		[Function(Name = "NEWID", IsComposable = true)]
		public Guid Random() {
			throw new NotImplementedException();
		}
	}
}
