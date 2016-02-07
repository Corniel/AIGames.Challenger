using System.Collections.Generic;
using System.Linq;

namespace AIGames.AllTimeRanking
{
	public class HistoricalMatches : List<HistoricalMatch>
	{
		public HistoricalMatch Select(HistoricalBot bot1, HistoricalBot bot2, bool symmetric)
		{
			var match = this.FirstOrDefault(m =>
				(m.Bot1 == bot1 && m.Bot2 == bot2)  ||
				(symmetric && m.Bot1 == bot2 && m.Bot2 == bot1)
			);
			if (match == null)
			{
				match = new HistoricalMatch() { Bot1 = bot1, Bot2 = bot2 };
				bot1.Matches.Add(match);
				bot2.Matches.Add(match);
			}

			return match;
		}
	}
}
