using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Objektdatabas {
	[Table(Name = "Märken")]
	class Märke : Rad {
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
		private string _namn;
		[Column(Storage = "_namn", DbType = "NVARCHAR(50)")]
		public string namn {
			get {
				return this._namn;
			}
			set {
				this._namn = value;
			}
		}
		private int _stadid;
		[Column(Storage = "_stadid", DbType = "INT NOT NULL")]
		public int stadid {
			get {
				return this._stadid;
			}
			set {
				this._stadid = value;
			}
		}
	}
}
