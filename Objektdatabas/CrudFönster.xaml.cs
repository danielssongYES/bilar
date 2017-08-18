using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Objektdatabas {
	/// <summary>
	/// Interaction logic for CrudFönster.xaml
	/// </summary>
	public partial class CrudFönster:Window {
		Bildatabas db;
		List<Control> kontroller;
		public CrudFönster() {
			InitializeComponent();
			kontroller = new List<Control> {
				antalBox, hämtaAdjektivButton, hämtaNamnButton,
				hämtaOrdButton, genereraModellerButton, genereraMärkenButton
			};
			Table<Bil> Modeller;
			Table<Märke> Märken;
			Table<Stad> Städer;
			Table<Land> Länder;
			Table<Ord> Ord;
			Table<Namn> Namn;
			//db.FyllNamn(6000);
			//db.FyllOrd(30000);
			//db.FyllAdjektiv(8000);
			//db.TömTabell("Modeller");
			//db.TömTabell("Märken");
			//db.FyllMärken(1000);
			//db.FyllModeller(3000);
		}

		private void FönsterInläst(object sender, RoutedEventArgs e) {
			db = new Bildatabas(@"Data Source=.\SQLEXPRESS;Initial Catalog=GustavsObjektdatabas;Integrated Security=True");
			//List<Märke> märkeLista = db.MärkenTabell.ToList();
			//List<Bil> modellLista = db.ModellerTabell.ToList();
			var bilLista = (
				from modell in db.ModellerTabell
				join märke in db.MärkenTabell on modell.märkeid equals märke.id
				join stad in db.StäderTabell on märke.stadid equals stad.id
				join land in db.LänderTabell on stad.landid equals land.id
				select new {
					Årsmodell = modell.startår, Märke = märke.namn,
					Modell = modell.namn, Kaross = modell.kaross,
					Beskrivning = modell.beskrivning,
					Tillverkningsort = stad.namn, Ursprungsland = land.namn
				}
			);
			

			märkenDataGrid.ItemsSource = db.MärkenTabell;
			modellerDataGrid.ItemsSource = db.ModellerTabell;
			detaljDataGrid.ItemsSource = bilLista;
		}

		private void dataKlicka(object sender, RoutedEventArgs e) {
			List<string> kontrollNamn = new List<string>();
			foreach(Control kontroll in kontroller) {
				kontroll.IsEnabled = false;
			}
			int antal;
			Control avsändare = (Control)sender;
			if(int.TryParse(antalBox.Text, out antal)) {
				switch(avsändare.Name) {
					case "hämtaAdjektivButton":
						db.TömTabell("Adjektiv");
						db.FyllAdjektiv(antal, förlopp);
						break;
					case "hämtaOrdButton":
						db.TömTabell("Ord");
						db.FyllOrd(antal, förlopp);
						break;
					case "hämtaNamnButton":
						db.TömTabell("Namn");
						db.FyllNamn(antal, förlopp);
						break;
					case "genereraMärkenButton":
						db.TömTabell("Märken");
						db.FyllMärken(antal, förlopp);
						break;
					case "genereraModellerButton":
						db.TömTabell("Modeller");
						db.FyllModeller(antal, förlopp);
						break;
				}
			}
			foreach(Control kontroll in kontroller) {
				kontroll.IsEnabled = true;
			}
		}

	}
}
