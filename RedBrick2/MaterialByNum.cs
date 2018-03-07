using System;
using System.Windows.Forms;

namespace RedBrick2 {
	/// <summary>
	/// A quick popup for setting a cutlist material according to an M2M number.
	/// </summary>
	public partial class MaterialByNum : Form {
		/// <summary>
		/// The relevant table.
		/// </summary>
		public enum Table {
			/// <summary>
			/// Looking up an M2M number in the Cutlist Material tables.
			/// </summary>
			MATERIAL,
			/// <summary>
			/// Looking up an M2M number in the Cutlist Edging table.
			/// </summary>
			EDGING
		}

		private Table _t = Table.MATERIAL;

		/// <summary>
		/// A constructor.
		/// </summary>
		/// <param name="table">A value from the enum <see cref="Table"/>. This tells us what tables to look in.</param>
		public MaterialByNum(Table table) {
			_t = table;
			InitializeComponent();
		}

		private int get_material(string search_term_) {
			int res_ = 0;
			if (search_term_.Length > 0) {
				using (ENGINEERINGDataSetTableAdapters.CUT_MATERIAL_SIZESTableAdapter cms_ =
					new ENGINEERINGDataSetTableAdapters.CUT_MATERIAL_SIZESTableAdapter()) {
					res_ = Convert.ToInt32(cms_.MatIDbyPartnum(search_term_));
				}
			}
			return res_;
		}

		private int get_edging(string search_term_) {
			int res_ = 0;
			if (search_term_.Length > 0 && search_term_ != @"SEE JOB BOM") {
				using (ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter cet_ =
					new ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter()) {
					res_ = Convert.ToInt32(cet_.EdgeIDbyEdgenum(search_term_));
				}
			}
			return res_;
		}

		/// <summary>
		/// If we found a cutlist material, this event is triggered. The <see cref="MaterialByNumEventArgs"/>
		/// object carries the correct MATID, or EDGEID.
		/// </summary>
		public event EventHandler FoundMatID;
		/// <summary>
		/// Invoke the registered functions.
		/// </summary>
		/// <param name="mbp">A <see cref="MaterialByNumEventArgs"/> object,
		/// containing the MATID, or EDGEID we're looking for.</param>
		protected virtual void OnFoundMatID(MaterialByNumEventArgs mbp) {
			FoundMatID?.Invoke(this, mbp);
		}

		private void rawPartNo_tb_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter) {
				string search_term_ = (sender as TextBox).Text.Trim();
				if (_t == Table.MATERIAL) {
					OnFoundMatID(new MaterialByNumEventArgs(get_material(search_term_)));
				}
				if (_t == Table.EDGING) {
					OnFoundMatID(new MaterialByNumEventArgs(get_edging(search_term_)));
				}
				Close();
			}
		}
	}

	/// <summary>
	/// An <see cref="EventArgs"/> object to carry our found ID.
	/// </summary>
	public class MaterialByNumEventArgs : EventArgs {
		private int _id = 0;
		/// <summary>
		/// MATID or EDGEID.
		/// </summary>
		public int ID
		{
			get {
				return _id;
			}
			private set {
				_id = value;
			}
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="mat_id">MATID or EDGEID.</param>
		public MaterialByNumEventArgs(int mat_id) {
			_id = mat_id;
		}
	}
}

