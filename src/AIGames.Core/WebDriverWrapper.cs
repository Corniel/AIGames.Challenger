using AIGames.Configuration;
using AIGames.Data;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AIGames
{
	/// <summary>A small wrapper around the Selenium WebDriver.</summary>
	public class WebDriverWrapper : IDisposable
	{
		/// <summary>Creates a new wrapper.</summary>
		public WebDriverWrapper(IWebDriver driver)
		{
			if (driver == null) { throw new ArgumentNullException("driver"); }
			Driver = driver;
		}

		/// <summary>Gets the underlying Selenium web driver.</summary>
		protected IWebDriver Driver { get; private set; }

		/// <summary>Challenges an opponent.</summary>
		public void ChallengeOpponent(AIGamesCompetition competition, string playerName)
		{
			if (competition == null) { throw new ArgumentNullException("competition"); }
			Driver.Url = competition.GetChallengeUrl(playerName).ToString();
		}

		/// <summary>Gets the game list as <see cref="HtmlAgilityPack.HtmlNode"/>.</summary>
		/// <param name="competition"></param>
		/// <param name="playerName"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		public HtmlNode GetGameListHtml(AIGamesCompetition competition, string playerName, int page)
		{
			if (competition == null) { throw new ArgumentNullException("competition"); }
			if (String.IsNullOrEmpty(playerName)) { throw new ArgumentNullException("playerName"); }
			Driver.Url = competition.GetGameListUrl(playerName, page).ToString();
			 
			var document = new HtmlDocument();
			document.LoadHtml(Driver.PageSource);
			return document.DocumentNode.SelectSingleNode("//body");
		}

		/// <summary>Gets the game list as <see cref="HtmlAgilityPack.HtmlNode"/>.</summary>
		public IEnumerable<AIGameResult> GetGames(AIGamesCompetition competition, string playerName)
		{
			if (competition == null) { throw new ArgumentNullException("competition"); }
			if (String.IsNullOrEmpty(playerName)) { throw new ArgumentNullException("playerName"); }

			var page = 0;
			var count = 0;
			var last = 0;

			while (true)
			{
				Driver.Url = competition.GetGameListUrl(playerName, ++page).ToString();

				var html = new HtmlDocument();
				html.LoadHtml(Driver.PageSource);

				var rows =html.DocumentNode.SelectNodes("//table[@class='table table-gameLog']/tbody/tr");
				if (rows == null) { yield break; }
				foreach(var row in rows)
				{
					var game = AIGameResult.FromRow(row);
					if (game != null)
					{
						count++;
						yield return game;
					}
				}
				// No new games for the last page, So break.
				if (last == count) { yield break; }
				last = count;
			}
		}
		
		/// <summary>Get all bots on the leader board for a given competition.</summary>
		public IEnumerable<AIGamesBot> GetLeaderboard(AIGamesCompetition competition)
		{
			if (competition == null) { throw new ArgumentNullException("competition"); }
			Driver.Url = competition.GetLeaderboard().ToString();

			var html = new HtmlDocument();
			html.LoadHtml(Driver.PageSource);
			var table = html.DocumentNode.SelectSingleNode("//table[@id='leaderboard-table']");

			if (table == null) { yield break; }

			foreach (var row in table.SelectNodes("//tr[@class='row-table']"))
			{
				var bot = AIGamesBot.FromRow(row);
				yield return bot;
			}
		}

		/// <summary>Saves the game dump for a specified game.</summary>
		public bool SaveGameDump(AIGamesCompetition competition, AIGameResult game)
		{
			if (competition == null) { throw new ArgumentNullException("competition"); }
			if (game == null) { throw new ArgumentNullException("game"); }

			var dir = new DirectoryInfo(Path.Combine(AppConfig.Games_RootDir_Dump.FullName, competition.UrlKey));
			if (!dir.Exists) { dir.Create(); }

			var dump = new FileInfo(Path.Combine(dir.FullName, String.Format("{0}.log", game.Id)));
			if (dump.Exists) { return false; }

			Driver.Url = competition.GetGameDumpUrl(game.Id).ToString();

			var html = new HtmlDocument();
			html.LoadHtml(Driver.PageSource);

			var code = html.DocumentNode.SelectNodes("//code").ElementAt(1).InnerHtml;

			if (String.IsNullOrEmpty(code)) { return false; }

			using (var stream = dump.OpenWrite())
			{
				AIGameDump.Save(code, stream);
			}
			return true;
		}

		/// <summary>Navigates to the sign in page.</summary>
		public void SignIn() { Driver.Url = "http://theaigames.com/sign-in/"; }

		/// <summary>Quits the driver.</summary>
		public void Quit() { Driver.Quit(); }

		#region IDispose

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !isDisposed)
			{
				isDisposed = true;
				if (Driver != null)
				{
					Driver.Dispose();
				}
				Driver = null;
			}
		}
		private bool isDisposed;

		#endregion

		/// <summary>Gets the driver for Google Chrome.</summary>
		public static WebDriverWrapper GetChrome()
		{
			return new WebDriverWrapper(new ChromeDriver());
		}
	}
}
