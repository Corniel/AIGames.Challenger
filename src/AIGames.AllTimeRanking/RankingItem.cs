using AIGames.Data;
using Qowaiv.Statistics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AIGames.AllTimeRanking
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class RankingItem : IComparable<RankingItem>
	{
		public RankingItem()
		{
			Opponents = new List<BotResults>();
		}
		public AIGamesBot Bot { get; set; }

		public Elo Rating { get { return Bot.Rating; } set { Bot.Rating = value; } }

		public int Games { get { return Opponents.Sum(r => r.Count); } }

		public BotResults GetOrAdd(AIGamesBot opponent, bool isMirrored)
		{
			var results = Opponents.FirstOrDefault(r => r.Opponent == opponent && r.IsMirrored == isMirrored);
			if (results == null)
			{
				results = new BotResults()
				{
					Opponent = opponent,
					IsMirrored = isMirrored,
					
				};
				Opponents.Add(results);
			}
			return results;	
		}

		public List<BotResults> Opponents { get; private set; }

		public int CompareTo(RankingItem other)
		{
			return other.Rating.CompareTo(Rating);
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("{0:0000.0}  {1} v{2}, games: {3}",
					Rating,
					Bot.Name,
					Bot.Revision,
					Games);
			}
		}
	}
}
