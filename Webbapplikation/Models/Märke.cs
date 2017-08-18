using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using Webbapplikation.Models;

namespace Webbapplikation.Models {
	[Table(Name = "Märken")]
	public class Märke {
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
		[Column(Storage = "_namn", DbType = "NVARCHAR(50) NOT NULL")]
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