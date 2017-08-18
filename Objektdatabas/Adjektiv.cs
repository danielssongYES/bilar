using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Threading.Tasks;

namespace Objektdatabas {
	[Table(Name = "Adjektiv")]
	class Adjektiv : Rad {
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
		[Column(Storage = "_ord", DbType = "NVARCHAR(50) NOT NULL")]
		public string ord {
			get {
				return this._ord;
			}
			set {
				this._ord = value;
			}
		}
	}
}
