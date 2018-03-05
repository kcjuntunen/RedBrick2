using System;
using System.Windows.Forms;

namespace RedBrick2 {
	public partial class MaterialByNum : Form {
		public enum Table {
			MATERIAL,
			EDGING
		}

		Table _t = Table.MATERIAL;

		public MaterialByNum(Table table) {
			_t = table;
			InitializeComponent();
		}

		private int get_material(string search_term_) {
			int res_ = 0;
			if (search_term_.Length > 0) {
				ENGINEERINGDataSetTableAdapters.CUT_MATERIAL_SIZESTableAdapter cms_ =
					new ENGINEERINGDataSetTableAdapters.CUT_MATERIAL_SIZESTableAdapter();
				res_ = Convert.ToInt32(cms_.MatIDbyPartnum(search_term_));
			}
			return res_;
		}

		private int get_edging(string search_term_) {
			int res_ = 0;
			if (search_term_.Length > 0 && search_term_ != @"SEE JOB BOM") {
				ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter cet_ =
					new ENGINEERINGDataSetTableAdapters.CUT_EDGESTableAdapter();
				res_ = Convert.ToInt32(cet_.EdgeIDbyEdgenum(search_term_));
			}
			return res_;
		}

		public event EventHandler FoundMatID;
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

	public class MaterialByNumEventArgs : EventArgs {
	private int _id = 0;
	public int ID
	{
		get
		{
			return _id;
		}
		private set
		{
			_id = value;
		}
	}
	public MaterialByNumEventArgs(int mat_id) {
		_id = mat_id;
	}
}
}

