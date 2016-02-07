using Qowaiv;
using System;
using System.Globalization;

namespace AIGames.AllTimeRanking
{
	public class HistoricalMatch: IEquatable<HistoricalMatch>
	{
		public HistoricalBot Bot1 { get; set; }
		public HistoricalBot Bot2 { get; set; }

		public int Wins { get; set; }
		public int Draws { get; set; }
		public int Loses { get; set; }
		public int Count { get { return Wins + Draws + Loses; } }

		public Percentage Score { get { return Count == 0 ? 0.5 : (Wins + 0.5 * Draws) / Count; } }

		public override bool Equals(object obj) { return Equals(obj as HistoricalMatch); }

		public bool Equals(HistoricalMatch other)
		{
			return other != null &&
				Bot1.Equals(other.Bot1) &&
				Bot2.Equals(other.Bot2);
		}

		public string GetOther(HistoricalBot bot)
		{
			var selected = bot == Bot1 ? Bot2 : Bot1;

			return string.Format("{0} v{1:0000}{2}", selected.Name, selected.Revision, bot == Bot2 ? "*" : "");
		}

		public override int GetHashCode()
		{
			return Bot1.GetHashCode() ^ Bot2.GetHashCode();}

		public override string ToString()
		{
			return string.Format
			(
				CultureInfo.InvariantCulture,
				"{0}+ {1}= {2}- {3}# {4:0.00%} ({5} v{6} - {7} v{8})",
				Wins, Draws, Loses, Count, Score,
				Bot1.Name, Bot1.Revision,
				Bot2.Name, Bot2.Revision
			);
		}
	}
}
