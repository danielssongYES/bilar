using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Objektdatabas {
	[Table(Name = "Modeller")]
	public class Bil : Rad {
		private int _id;
		[Column(Storage = "_id", DbType = "INT NOT NULL IDENTITY",
			IsPrimaryKey = true, IsDbGenerated = true)]
		public int id {
			get {
				return this._id;
			}
			set {
				this._id = value;
			}
		}
		private int _märkeid;
		[Column(Storage = "_märkeid", DbType = "INT NOT NULL FOREIGN KEY REFERENCES Märken(id)")]
		public int märkeid {
			get {
				return this._märkeid;
			}
			set {
				this._märkeid = value;
			}
		}
		private string _modell;
		[Column(Storage = "_modell", DbType = "NVARCHAR(50) NOT NULL")]
		public string namn {
			get {
				return this._modell;
			}
			set {
				this._modell = value;
			}
		}
		private string _konfig;
		[Column(Storage = "_konfig", DbType = "CHAR(2) NOT NULL")]
		public string konfig {
			get {
				return this._konfig;
			}
			set {
				this._konfig = value;
			}
		}
		private int _volym;
		[Column(Storage = "_volym", DbType = "INT NOT NULL")]
		public int volym {
			get {
				return this._volym;
			}
			set {
				this._volym = value;
			}
		}
		private string _kaross;
		[Column(Storage = "_kaross", DbType = "CHAR(10) NOT NULL")]
		public string kaross {
			get {
				return this._kaross;
			}
			set {
				this._kaross = value;
			}
		}
		private int _cyl;
		[Column(Storage = "_cyl", DbType = "INT NOT NULL")]
		public int cyl {
			get {
				return this._cyl;
			}
			set {
				this._cyl = value;
			}
		}
		private int _årStart;
		[Column(Storage = "_årStart", DbType = "INT NOT NULL")]
		public int startår {
			get {
				return this._årStart;
			}
			set {
				this._årStart = value;
			}
		}
		private int _årSlut;
		[Column(Storage = "_årSlut", DbType = "INT NULL")]
		public int slutår {
			get {
				return this._årSlut;
			}
			set {
				this._årSlut = value;
			}
		}
		private string _beskrivning;
		[Column(Storage = "_beskrivning", DbType = "NVARCHAR(100) NULL")]
		public string beskrivning {
			get {
				return this._beskrivning;
			}
			set {
				this._beskrivning = value;
			}
		}
		Random r = new Random();

		public Bil() {
			/*string[] konfigVal = {"FF", "FR", "F4", "MR", "M4", "RR", "R4"};
			string[] karossVal = {"Sedan", "Kombi", "Halvkombi",
									"Stadsjeep", "Kupé", "Cabriolet",
									"Minibuss"};
			//märke = 
			modell = GenModellnamn();
			
			konfig = konfigVal[r.Next(0, konfigVal.Length - 1)];
			kaross = karossVal[r.Next(0, karossVal.Length - 1)];*/
		}

		public Bil(string märkeId, string modell, string konfig, int volym,
			string kaross, int cyl, int årStart, int årSlut) {

			this._märkeid = märkeid;
			this._modell = modell;
			this._konfig = konfig;
			this._volym = volym;
			this._kaross = kaross;
			this._cyl = cyl;
			this._årStart = årStart;
			this._årSlut = årSlut;
		}

		public Bil(int märkeId, string modell, string konfig, int volym,
			string kaross, int cyl, int årStart) {

			this._märkeid = märkeid;
			this._modell = modell;
			this._konfig = konfig;
			this._volym = volym;
			this._kaross = kaross;
			this._cyl = cyl;
			this._årStart = årStart;
			this._årSlut = 0;
		}

		/*private string GenModellnamn(string språk) {

		}*/
	}
}
