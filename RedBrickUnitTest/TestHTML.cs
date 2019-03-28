using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedBrickUnitTest {
	[TestClass]
	public class TestHTML {
		[TestMethod]
		public void HTMLTest() {
			string url_ = @"http://10.10.76.50/doku.php?id=reference:programcolors";
			HtmlAgilityPack.HtmlWeb web_ = new HtmlAgilityPack.HtmlWeb();
			HtmlAgilityPack.HtmlDocument doc_ = web_.Load(url_);
			HtmlAgilityPack.HtmlNodeCollection nn_ = doc_.DocumentNode.SelectNodes(@"//*/div");
			System.Collections.Generic.List<string> _specs = new System.Collections.Generic.List<string>();
			System.Text.RegularExpressions.Regex r_ = new System.Text.RegularExpressions.Regex(@"^.*\:(.*)$");
			foreach (HtmlAgilityPack.HtmlNode node_ in nn_) {
				if (node_.Attributes[@"class"] != null && node_.Attributes[@"class"].Value == @"li" && node_.InnerText.Contains(@":")) {
					System.Text.RegularExpressions.MatchCollection mc_ = r_.Matches(node_.InnerText);
					if (mc_.Count > 0 && mc_[0].Groups.Count > 1) {
						string txt_ = mc_[0].Groups[1].Value.Trim();
						Console.Write(txt_);
					}
				}
			}
		}

		[TestMethod]
		public void PingTest() {
			string url_ = @"http://10.10.76.50/doku.php?id=reference:programcolors";
			System.Net.NetworkInformation.Ping p_ = new System.Net.NetworkInformation.Ping();
			System.Net.IPAddress ip_ = System.Net.IPAddress.Parse(url_.Split('/')[2]);
			System.Net.NetworkInformation.PingReply pr_ = p_.Send(ip_);
			Console.WriteLine(pr_.ToString());
		}
	}
}
