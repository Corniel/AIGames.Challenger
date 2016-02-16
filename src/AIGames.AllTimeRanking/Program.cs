using AIGames.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Qowaiv.Statistics;

namespace AIGames.AllTimeRanking
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			bool skip = false;
			skip = args.Length > 0 && bool.TryParse(args[0], out skip) && skip;
			
			var sw = Stopwatch.StartNew();

			var results = new Dictionary<AIGamesCompetition, AIGameResults>();
			var bots = new Dictionary<AIGamesCompetition, AIGamesBots>();

			foreach (var competition in AIGamesCompetitions.All)
			{
				results[competition] = AIGameResults.Load(competition);
				bots[competition] = AIGamesBots.Load(competition);
			}

			if (!skip) 
			{
				Update(sw, results, bots);
			}

			foreach (var comp in AIGamesCompetitions.All)
			{
				var ranking = Ranking.Create(bots[comp], results[comp], comp.IsSymmetric);

				var all = bots[comp].SelectMany(owner => owner.Bots).ToList();
				var avg_pre = all.Where(a => a.Active).Select(bot => bot.Rating).Avarage();
				ranking.Process();
				var avg_pos = all.Select(bot => bot.Rating).Avarage();

				var delta = avg_pos - avg_pre;

				foreach (var bot in all)
				{
					bot.Rating -= delta;
				}

				foreach (var owner in bots[comp])
				{
					owner.Bots.Sort();
				}
				ranking.Save(comp);

				bots[comp].Save(comp);
				results[comp].Save(comp);
			}
		}

		private static void Update(
			Stopwatch sw,
			Dictionary<AIGamesCompetition, AIGameResults> results,
			Dictionary<AIGamesCompetition, AIGamesBots> bots)
		{
			using (var driver = WebDriverWrapper.GetChrome())
			{
				foreach (var kvp in results)
				{
					var competition = kvp.Key;
					var games = kvp.Value;

					var bs = bots[competition];

					var gamesCount = games.Count;
					var botsCount = bs.BotCount;
					
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

					bs.DeactiveAll();

					var all = driver.GetHtmlLeaderboard(competition).ToList();
					foreach (var bot in all)
					{
						var bt = bs.AddOrUpdate(bot);
						bt.Active = true;
					}
					bs.Save(competition);

					Console.WriteLine(@"{0:mm\:ss\:f}  Saved {1:#,##0} new game results and {2:#,##0} bots for {3}. Results: {4:#,##0}, bots: {5:#,##0}",
						sw.Elapsed,
						games.Count - gamesCount,
						bs.BotCount - botsCount,
						competition.DisplayName,
						games.Count,
						bs.BotCount);
				}
			}
		}
	}
}