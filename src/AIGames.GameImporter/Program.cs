﻿using AIGames.Configuration;
using AIGames.Data;
using System;
using System.Diagnostics;
using System.IO;

namespace AIGames.GameImporter
{
	class Program
	{
		static void Main(string[] args)
		{
			var file = new FileInfo(@"c:\temp\games.xml");
			AIGameResults games = AIGameResults.Load(file);

			using (var driver = WebDriverWrapper.GetChrome())
			{
				driver.SignIn();
				Console.WriteLine("Log in first and then hit any key");
				Console.ReadLine();

				var competition = AIGamesCompetitions.All.Default;

				var sw = Stopwatch.StartNew();

				foreach(var game in driver.GetGames(competition, AppConfig.Bot_Name))
				{
					if(games.ContainsId(game))
					{
						break;
					}
				}
				games.Save(file);
				Console.WriteLine("Saved {0} game results.", games.Count);

				var saved = 0;
				var skipped = 0;
				var total = games.Count;

				foreach (var game in games)
				{
					if (driver.SaveGameDump(competition, game))
					{
						saved++;
					}
					else
					{
						skipped++;
					}
					Console.Write("\r");
					Console.Write(@"{4:hh\:mm\:ss} Saved: {0}, skipped: {1}, {2} out {3}",
						saved, skipped, saved + skipped, total, sw.Elapsed);
				}
			}


			Console.WriteLine();
			Console.WriteLine("Finished");
			Console.ReadLine();
		}
	}
}
