using System.Reflection;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace RedBrick2 {
	public partial class AboutBox : Form {
		System.Windows.Controls.Canvas c = new System.Windows.Controls.Canvas();
		System.Windows.Media.Brush[] b = {
				System.Windows.Media.Brushes.DeepSkyBlue,
				System.Windows.Media.Brushes.DarkRed,
				System.Windows.Media.Brushes.DarkSeaGreen,
				System.Windows.Media.Brushes.DarkCyan,
				System.Windows.Media.Brushes.Yellow
			};
		System.Windows.Point lastPoint = new System.Windows.Point(0, 0);
		bool cleared = false;
		System.Random r_ = new System.Random();
		System.Windows.Media.Brush b_ = System.Windows.Media.Brushes.Red;
		public AboutBox() {
			InitializeComponent();
			string descr_ = AssemblyDescription;
			this.Text = string.Format("About {0}", AssemblyTitle);
			this.labelProductName.Text = AssemblyProduct;
			this.labelVersion.Text = string.Format("Version {0}", AssemblyVersion);
			this.labelCopyright.Text = AssemblyCopyright;
			this.textBoxDescription.Text = string.Format("New in version {3}:{0}" +
				"{2}{0}" +
				"Get a complete history at https://github.com/kcjuntunen/RedBrick2/commits/master{0}" +
				"{0}------------------------{0}" +
				"Crc32.cs Copyright (c) Damien Guard.  All rights reserved.{0}" +
				"Licensed under the Apache License, Version 2.0 (the \"License\"){0}" +
				"You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0",
				System.Environment.NewLine,
				descr_,
				GetVersionInfo(@"message"),
				AssemblyVersion);

			c.Background = System.Windows.Media.Brushes.Black;
			for (int i = 0; i < r_.Next(elementHost1.Width); i += 10) {
				for (int j = 0; j < r_.Next(elementHost1.Height); j += 10) {
					System.Windows.Point p = new System.Windows.Point(i, j);
					System.Windows.Shapes.Ellipse e = new System.Windows.Shapes.Ellipse();
					e.Stroke = b[r_.Next(b.Length)];
					e.TranslatePoint(p, c);
					e.Width = r_.Next(20, 70) * i;
					e.Height = r_.Next(20, 70) * j;
					c.Children.Add(e);
				}
			}
			elementHost1.Child = c;
			c.MouseMove += C_MouseMove;
		}

		private void C_MouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
			System.Windows.Shapes.Line l_ = new System.Windows.Shapes.Line();
			if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) {
				if (!cleared) {
					c.Children.Clear();
					cleared = true;
				}
				System.Windows.Point p_ = e.GetPosition(c);
				if (lastPoint.X == 0 && lastPoint.Y == 0) {
					lastPoint = p_;
				}
				l_.Stroke = b_;
				l_.StrokeThickness = 3;
				l_.TranslatePoint(p_, c);
				l_.X1 = lastPoint.X;
				l_.Y1 = lastPoint.Y;
				l_.X2 = p_.X;
				l_.Y2 = p_.Y;
				lastPoint = p_;
				c.Children.Add(l_);
			} else {
				b_ = b[r_.Next(b.Length)];
				lastPoint = new System.Windows.Point();
			}
		}

		private static string GetVersionInfo(string element) {
			FileInfo pi = new FileInfo(Properties.Settings.Default.InstallerNetworkPath);
			string fn_ = string.Format(@"{0}\version.xml", pi.DirectoryName);
			string elementName = string.Empty;
			using (XmlReader r_ = XmlReader.Create(fn_)) {
				while (r_.Read()) {
					if (r_.NodeType == XmlNodeType.Element) {
						elementName = r_.Name;
					} else if (r_.NodeType == XmlNodeType.Text && r_.HasValue && elementName == element) {
						return r_.Value.Replace("\n", System.Environment.NewLine);
					}
				}
			}
			return string.Empty;
		}

		#region Assembly Attribute Accessors

		public string AssemblyTitle {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0) {
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "") {
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion {
			get {
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string AssemblyDescription {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public string AssemblyProduct {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}
		#endregion
	}
}
