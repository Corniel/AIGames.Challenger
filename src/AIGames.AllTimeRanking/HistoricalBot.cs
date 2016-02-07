using AIGames.Data;
using Qowaiv;
using Qowaiv.Statistics;
using System;
using System.Diagnostics;
using System.Linq;

namespace AIGames.AllTimeRanking
{
	public class HistoricalBot : IEquatable<HistoricalBot>, IComparable<HistoricalBot>
	{
		public HistoricalBot()
		{
			FirstGame = Date.MaxValue;
			LastGame = Date.MinValue;
			Rating = 1600;
			Matches = new HistoricalMatches();
		}

		public string Name { get; set; }
		public int Revision { get; set; }

		public Elo Rating { get; set; }

		public Date FirstGame { get; set; }
		public Date LastGame { get; set; }

		public HistoricalMatches Matches { get; private set; }

		public int MatchCounts { get { return Matches.Sum(m => m.Count); } }

		public void Update(AIGameResult result, bool symmetric)
		{
			if (result.Date < FirstGame)
			{
				FirstGame = result.Date;
			}
			if (result.Date > LastGame)
			{
				LastGame = result.Date;
			}
		}

		public override int GetHashCode()
		{
			return Revision ^ (Name ?? String.Empty).GetHashCode();
		}

		public override bool Equals(object obj) { return Equals(obj as HistoricalBot); }

		public bool Equals(HistoricalBot other)
		{
			return other != null &&
				Revision == other.Revision &&
				Name == other.Name;
		}

		public int CompareTo(HistoricalBot other)
		{
			return other.Rating.CompareTo(Rating);
		}

		public override string ToString()
		{
			return string.Format("{0:0000.0}  {1} v{2} ({3:yyyy-MM-dd}-{4:yyyy-MM-dd}), games: {5}",
				Rating,
				Name,
				Revision,
				FirstGame,
				LastGame,
				MatchCounts);
		}

		public static HistoricalBot Create(AIGameResult.Bot bot)
		{
			return new HistoricalBot()
			{
				Name = bot.Name,
				Revision = bot.Version,
			};
		}
	}
}
