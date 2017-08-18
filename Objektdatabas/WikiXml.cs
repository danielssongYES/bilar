using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Xml;

namespace Objektdatabas {
	class WikiXml {
		private string _kategoriTitel;
		public string kategoriTitel {
			get {
				return this._kategoriTitel;
			}
		}
		private string innehåll;
		private string _utContinueKod;
		public string utContinueKod {
			get {
				return this._utContinueKod;
			}
		}
		private List<string> _innehållLista;
		public List<string> innehållLista {
			get {
				return this._innehållLista;
			}
		}
		public WikiXml(string wikiTitel, string kategoriTitel, int antal = 500, string inContinueKod = "") {
			this._kategoriTitel = kategoriTitel;
			this._innehållLista = new List<string>();
			string url = String.Format(
				"https://{0}/w/api.php?action=query&list=categorymembers&cmprop=title&cmtitle={1}&cmlimit={2}&cmsort=timestamp&format=xml&continue=",
				wikiTitel, kategoriTitel, antal);
			if(inContinueKod != "")
				url += "&cmcontinue=" + inContinueKod;
			WebRequest efterfrågan = WebRequest.Create(url);
			WebResponse svar = efterfrågan.GetResponse();
			Stream svarStröm = svar.GetResponseStream();
			StreamReader läsare = new StreamReader(svarStröm);
			this.innehåll = läsare.ReadToEnd();
			läsare.Close();
			svar.Close();
			XmlDocument xmlDokument = new XmlDocument();
			xmlDokument.LoadXml(this.innehåll);
			XmlNodeList elementLista = xmlDokument.GetElementsByTagName("cm");
			foreach(XmlNode element in elementLista) {
				this._innehållLista.Add(element.Attributes["title"].Value);
			}
			this._utContinueKod = xmlDokument.GetElementsByTagName("continue").Count != 0 ?
				xmlDokument.GetElementsByTagName("continue")[0].Attributes["cmcontinue"].Value :
				"";
		}
	}
}
