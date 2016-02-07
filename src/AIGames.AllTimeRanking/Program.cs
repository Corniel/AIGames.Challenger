using AIGames.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AIGames.AllTimeRanking
{
	class Program
	{
		static void Main(string[] args)
		{
			bool skip = false;
			skip = args.Length > 0 && bool.TryParse(args[0], out skip) && skip;
			
			var sw = Stopwatch.StartNew();

			var collection = new Dictionary<AIGamesCompetition, AIGameResults>();

			foreach (var competition in AIGamesCompetitions.All)
			{
				collection[competition] = AIGameResults.Load(competition);
			}

			if (!skip) { Update(sw, collection); }

			foreach (var kvp in collection)
			{
				var competition = kvp.Key;
				var games = kvp.Value;

				var bots = new HistoricalBots(competition.IsSymmetric);
				bots.AddRange(games);
				bots.ApplyRatings();

				bots.Save(competition);
			}
		}

		private static void Update(Stopwatch sw, Dictionary<AIGamesCompetition, AIGameResults> collection)
		{
			using (var driver = WebDriverWrapper.GetChrome())
			{
				foreach (var kvp in collection)
				{
					var competition = kvp.Key;
					var games = kvp.Value;

					var count = games.Count;
					foreach (var game in driver.GetGames(competition))
					{
						if (games.ContainsId(game))
						{
							break;
						}
						else
						{
							games.Add(game);
						}
					}
					games.Save(competition);

					Console.WriteLine(@"{0:mm\:ss\:f}  Saved {1:#,##0} new game results for {2}. Total: {3:#,##0}",
						sw.Elapsed,
						games.Count - count,
						competition.DisplayName,
						games.Count);
				}
			}
		}
	}
}