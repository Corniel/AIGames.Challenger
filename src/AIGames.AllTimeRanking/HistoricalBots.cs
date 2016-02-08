using AIGames.Configuration;
using AIGames.Data;
using Qowaiv;
using Qowaiv.Statistics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AIGames.AllTimeRanking
{
	public class HistoricalBots : List<HistoricalBot>
	{
		public const int MinimumGameCount = 30;

		public HistoricalBots(bool symmetric)
		{
			IsSymmetric = symmetric;
		}
		public bool IsSymmetric { get; private set; }

		public void AddRange(IEnumerable<AIGameResult> results)
		{
			foreach (var result in results)
			{
				Add(result);
			}
		}

		public void Add(AIGameResult result)
		{
			HistoricalBot bot1 = Select(result.Bot1);
			HistoricalBot bot2 = Select(result.Bot2);

			bot1.Update(result, IsSymmetric);
			bot2.Update(result, IsSymmetric);
			
			var match = bot1.Matches.Select(bot1, bot2, IsSymmetric);
			var flip = match.Bot1 != match.Bot1;

			switch (result.Outcome)
			{
				case AIGameResult.Score.Win:
					if (flip)
					{
						match.Loses++;
					}
					else
					{
						match.Wins++;
					}
					break;
				case AIGameResult.Score.Draw:
					match.Draws++;
					break;
				case AIGameResult.Score.Lose:
					if (flip)
					{
						match.Wins++;
					}
					else
					{
						match.Loses++;
					}
					break;
			}
		}

		public void ApplyRatings()
		{
			for (var k = 30; k > 0; k--)
			{
				foreach (var bot in this)
				{
					foreach (var match in bot.Matches.Where(m => m.Bot1 == bot))
					{
						var z = Elo.GetZScore(match.Bot1.Rating, match.Bot2.Rating);
						var s = (double)match.Score;

						match.Bot1.Rating += (s - z) * k;
						match.Bot2.Rating += (z - s) * k;
					}
				}
			}
			Sort();
		}

		public HistoricalBot Select(AIGameResult.Bot bot)
		{
			var b = this.FirstOrDefault(item => item.Name == bot.Name && item.Revision == bot.Version);
			if (b == null)
			{
				b = HistoricalBot.Create(bot);
				Add(b);
			}
			return b;
		}

		public void Save(AIGamesCompetition competition)
		{
			var file = new FileInfo(Path.Combine(AppConfig.Games_RootDir_Dump.FullName, competition.UrlKey + ".txt"));

			using (var writer = new StreamWriter(new FileStream(file.FullName, FileMode.Create, FileAccess.Write)))
			{
				var done = new HashSet<string>();

				var pos = 1;
				foreach (var bot in this.Where(b =>
					b.MatchCounts >= MinimumGameCount &&
					b.LastGame >= Date.Today &&
					!done.Contains(b.Name)))
				{
					writer.WriteLine("{0:000}\t{1}", pos++, bot);
					done.Add(bot.Name);
				}

				writer.WriteLine();

				pos = 1;
				foreach (var bot in this.Where(b => b.MatchCounts >= MinimumGameCount))
				{
					writer.WriteLine("{0:000}\t{1}", pos++, bot);
				}

				writer.WriteLine();

				pos = 1;
				foreach (var bot in this.Where(b => b.MatchCounts >= MinimumGameCount))
				{
					writer.WriteLine("{0:000}\t{1}", pos++, bot);

					foreach (var match in bot.Matches.OrderBy(m => m.GetOther(bot)))
					{
						writer.Write('\t');
						writer.WriteLine(match);
					}
					writer.WriteLine();
				}
			}
		}
	}
}
