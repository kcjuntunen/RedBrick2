using System.Collections.Generic;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace RedBrick2.Drawings {
	public class DrawingStaticFunctions {
		private static string templateName = @"G:\Solid Works\AMSTORE_SHEET_FORMATS\METAL_AM_ASSEMBLY.slddrt";
		private static string templateNameA4 = @"G:\Solid Works\AMSTORE_SHEET_FORMATS\METAL_AM_PART.slddrt";

		private static double[] A4Size = { 0.2794, 0.2159 };
		private static double[] BigSize = { 0.4318, 0.2794 };

		public static void SetMetalLayers(ModelDoc2 md) {
			LayerMgr lm = (LayerMgr)md.GetLayerManager();
			AdjustLayerVisibility(lm);
			DrawingDoc dd = md as DrawingDoc;
			SetupSheets(dd);
		}

		private static void SetupSheets(DrawingDoc dd) {
			string[] sheets = dd.GetSheetNames();
			string template = templateName;

			foreach (string sheetname in sheets) {
				Sheet sheet = dd.Sheet[sheetname];
				double[] props = sheet.GetProperties2();
				double[] size = IsA4(sheet) ? A4Size : BigSize;

				dd.SetupSheet6(
					sheetname,
					(int)swDwgPaperSizes_e.swDwgPapersUserDefined,
					(int)swDwgTemplates_e.swDwgTemplateCustom,
					props[2], props[3],
					true,
					IsA4(sheet) ? templateNameA4 : templateName,
					size[0], size[1],
					"Default",
					true,
					0, 0, 0, 0, 0, 0);
			}
		}

		private static bool IsA4(Sheet sheet) {
			double w = 0.0, h = 0.0;
			sheet.GetSize(ref w, ref h);
			return Redbrick.FloatEquals(w * h, A4Size[0] * A4Size[1]);
		}

		private static void AdjustLayerVisibility(LayerMgr lm) {
			string[] layers = (string[])lm.GetLayerList();
			List<string> goodlayers = new List<string> { "FORMAT", "AMSTORE", "AMS.1-5", "0", "TEXT", "CONFIDENTIAL" };
			List<string> badlayers = new List<string> { "KOHLS", "AMS.6-10", "AMS.11-15", "CUS.1-5", "CUS.6-10", "CUS.11-15", "CUS.16-20", "BORDER", "REV1_5", "REV6_10" };

			foreach (string layer in layers) {
				Layer l = lm.GetLayer(layer);
				if (goodlayers.Contains(layer)) {
					l.Visible = true;
				}

				if (badlayers.Contains(layer)) {
					l.Visible = false;
				}
			}
		}

	}
}
