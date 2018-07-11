using System;
using System.IO;
using System.Text.RegularExpressions;

namespace RedBrick2 {
	public class Estimation {
		static string metalSTPPath = @"\\Amstore-svr-22\cad\STRIKER LASER PROGRAMS\STP";
		public static double GetLaserPartRuntime(string part) {
			double per_part_time_ = 0.0F;
			FileInfo f_ = new FileInfo(string.Format(@"{0}\{1}.txt", metalSTPPath, part));
			if (File.Exists(f_.FullName)) {
				string content_ = File.ReadAllText(f_.FullName);
				Regex qty_r_ = new Regex(@"Parts/Sheet:[ \t]*([0-9]*)");
				Regex time_r_ = new Regex(@"TOTAL TIME\ *:\ *([0-9.]*) minutes\ *([0-9.]*)");
				Match qty_match_ = qty_r_.Match(content_);
				Match time_match_ = time_r_.Match(content_);
				uint qty_ = Convert.ToUInt32(qty_match_.Groups[1].Value);
				if (qty_ != 0) {
					double minutes_ = Convert.ToDouble(time_match_.Groups[1].Value);
					double seconds_ = Convert.ToDouble(time_match_.Groups[2].Value);
					per_part_time_ = (minutes_ + (seconds_ / 60)) / qty_;
				}
			}
			return per_part_time_;
		}
	}
}
