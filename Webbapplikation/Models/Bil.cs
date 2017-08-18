using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Webbapplikation.Models {
	[Table(Name = "Modeller")]
	public class Bil {
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
		private string _namn;
		[Column(Storage = "_namn", DbType = "NVARCHAR(50) NOT NULL")]
		public string namn {
			get {
				return this._namn;
			}
			set {
				this._namn = value;
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
		private int _startår;
		[Column(Storage = "_startår", DbType = "INT NOT NULL")]
		public int startår {
			get {
				return this._startår;
			}
			set {
				this._startår = value;
			}
		}
		private int _slutår;
		[Column(Storage = "_slutår", DbType = "INT NULL")]
		public int slutår {
			get {
				return this._slutår;
			}
			set {
				this._slutår = value;
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

		public Bil() {

		}
	}
}