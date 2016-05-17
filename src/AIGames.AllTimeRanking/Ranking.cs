using AIGames.Configuration;
using AIGames.Data;
using Qowaiv.Statistics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AIGames.AllTimeRanking
{
	public class Ranking : List<RankingItem>
	{
		public Ranking(bool isSymmetric)
		{
			IsSymmetric = isSymmetric;
		}

		public bool IsSymmetric { get; set; }

		public List<AIGamesBot> Bots
		{
			get
			{
				return this
					.Select(item => item.Bot)
					.OrderByDescending(bot => bot.Revision)
					.ToList();
			}
		}

		public RankingItem GetOrAdd(AIGamesBot bot)
		{
			var item = this.FirstOrDefault(i => i.Bot == bot);
			if (item == null)
			{
				item = new RankingItem()
				{
					Bot =bot,
				};
				Add(item);
			}
			return item;
		}

		public void Process()
		{
			for (var k = 64; k >= 0.5; k /= 2)
			{
				foreach (var item in this)
				{
					var rs = item.Opponents.ToList();
					
					var bots = Bots
						.Where(b => b.Name == item.Bot.Name && b.Revision < item.Bot.Revision)
						.OrderByDescending(b => b.Revision)
						.ToList();
					foreach (var pre in bots)
					{
						var total = rs.Sum(r => r.Count);
						var wins = rs.Sum(r => r.Wins);
						var loss = rs.Sum(r => r.Loses);
						if (total > 50 && wins > 0 & loss > 0)
						{
							break;
						}
						var oppos = this.FirstOrDefault(i => i.Bot == pre).Opponents;
						rs.AddRange(oppos);
					}

					foreach (var results in rs)
					{
						var z = Elo.GetZScore(item.Rating, results.Opponent.Rating);
						var s = (double)results.Score;

						item.Rating += (s - z) * k;
						results.Opponent.Rating += (z - s) * k;
					}
					item.Opponents.Sort();
				}
			}
			Sort();
		}

		public void Save(AIGamesCompetition competition)
		{
			var file = new FileInfo(Path.Combine(AppConfig.Games_RootDir_Dump.FullName, competition.UrlKey + ".ranking.txt"));

			var pos = 1;

			using (var writer = new StreamWriter(new FileStream(file.FullName, FileMode.Create, FileAccess.Write)))
			{
				foreach (var item in this.Where(i=> i.Bot.Active))
				{
					writer.WriteLine("{0,4}  {1,4}  {2} v{3} ({4})", pos++, item.Rating.ToString("0"), item.Bot.Name, item.Bot.Revision, item.Games);
				}
				writer.WriteLine();

				foreach (var item in this)
				{
					writer.WriteLine("Opponents: {0} v{1} ({2:0})", item.Bot.Name, item.Bot.Revision, item.Rating);
					foreach (var oppo in item.Opponents)
					{
						writer.WriteLine("  {0}", oppo);
					}
					writer.WriteLine();
				}
			}
		}

		public static Ranking Create(AIGamesBots bots, AIGameResults results, bool isSymmetric)
		{
			var ranking = new Ranking(isSymmetric);

			results.Sort();

			var lookup = new Dictionary<AIGameResult.Bot, AIGamesBot>();

			foreach (var result in results)
			{
				AIGamesBot bot1 = null;
				AIGamesBot bot2 = null;

				if(!lookup.TryGetValue(result.Bot1, out bot1))
				{
					bot1 = bots.GetOrAdd(result.Bot1);
					lookup[result.Bot1] = bot1;
				}
				if (!lookup.TryGetValue(result.Bot2, out bot2))
				{
					bot2 = bots.GetOrAdd(result.Bot2);
					lookup[result.Bot2] = bot2;
				}
				
				var item1 = ranking.GetOrAdd(bot1);
				var item2 = ranking.GetOrAdd(bot2);

				var results1 = item1.GetOrAdd(bot2, false);
				var results2 = item2.GetOrAdd(bot1, !isSymmetric);

				switch (result.Outcome)
				{
					case AIGameResult.Score.Win:
						results1.Wins++;
						results2.Loses++;
						break;
					case AIGameResult.Score.Draw:
						results1.Draws++;
						results2.Draws++;
						break;
					case AIGameResult.Score.Lose:
						results1.Loses++;
						results2.Wins++;
						break;
				}

			}

			return ranking;
		}
	}
}
