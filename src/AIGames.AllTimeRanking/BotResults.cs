using AIGames.Data;
using Qowaiv;
using System;
using System.Globalization;

namespace AIGames.AllTimeRanking
{
	public class BotResults: IComparable<BotResults>
	{
		public bool IsMirrored { get; set; }

		public int Wins { get; set; }
		public int Draws { get; set; }
		public int Loses { get; set; }
		public int Count { get { return Wins + Draws + Loses; } }

		public Percentage Score { get { return Count == 0 ? 0.5 : (Wins + 0.5 * Draws) / Count; } }

		public AIGamesBot Opponent { get; set; }

		public override string ToString()
		{
			return string.Format
			(
				CultureInfo.InvariantCulture,
				"{0,4}+ {1,4}= {2,4}- {3,4}# {4,7} {5} ({6} v{7})",
				Wins, Draws, Loses, Count, 
				
				Score.ToString("0.00%"),
				
				IsMirrored ? "*" : " ",
				
				Opponent.Name,
				Opponent.Revision
			);
		}

		public int CompareTo(BotResults other)
		{
			var compare = Opponent.Rating.CompareTo(other.Opponent.Rating);
			if (compare != 0) { return -compare; }

			compare = Opponent.Name.CompareTo(other.Opponent.Name);
			if (compare != 0) { return compare; }

			compare = Opponent.Revision.CompareTo(other.Opponent.Revision);
			if (compare != 0) { return -compare; }

			compare = IsMirrored.CompareTo(other.IsMirrored);
			return compare;
		}
	}
}
