using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Objektdatabas {
	[Table(Name = "Ord")]
	class Ord : Rad {
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
		private string _ord;
		[Column(Storage = "_ord", DbType = "NVARCHAR(100) NOT NULL")]
		public string ord {
			get {
				return this._ord;
			}
			set {
				this._ord = value;
			}
		}
		private int _språkid;
		[Column(Storage = "_språkid", DbType = "INT NOT NULL")]
		public int språkid {
			get {
				return this._språkid;
			}
			set {
				this._språkid = value;
			}
		}

		public Ord() {

		}
	}
}
