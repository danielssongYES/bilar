using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Webbapplikation.Models {
	public class Bildatabas : DataContext {
		public Table<Bil> Modeller;
		public Table<Stad> Städer;
		public Table<Land> Länder;
		public Table<Märke> Märken;

		public Bildatabas(string connection) : base(connection) { }

		//Slumpmetod för Linq To SQL.
		[Function(Name = "NEWID", IsComposable = true)]
		public Guid Random() {
			throw new NotImplementedException();
		}
	}
}