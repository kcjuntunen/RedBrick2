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
				if (!uint.TryParse(qty_match_.Groups[1].Value, out uint qty_)) {
					qty_ = 1;
				}
				if (qty_ != 0) {
					if (!double.TryParse(time_match_.Groups[1].Value, out double minutes_)) {
						minutes_ = 0.0f;
					}
					if (!double.TryParse(time_match_.Groups[2].Value, out double seconds_)) {
						seconds_ = 0.0f;
					}
					per_part_time_ = (minutes_ + (seconds_ / 60)) / qty_;
				}
			}
			return per_part_time_;
		}
	}
}
