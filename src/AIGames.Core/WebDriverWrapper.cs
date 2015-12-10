using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace AIGames
{
	/// <summary>A small wrapper around the Selenium WebDriver.</summary>
	public class WebDriverWrapper
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

		/// <summary>Get all bots on the leaderboard for a given competition.</summary>
		public IEnumerable<AIGamesBot> GetLeaderboard(AIGamesCompetition competition)
		{
			if (competition == null) { throw new ArgumentNullException("competition"); }
			Driver.Url = competition.GetLeaderboard().ToString();

			var leaderTable = Driver.FindElement(By.Id("leaderboard-table"));
			if (leaderTable != null)
			{
				var rows = leaderTable.FindElements(By.ClassName("row-table"));
				foreach (var row in rows)
				{
					var id = row.GetAttribute("data-rowid");
					var divName = row.FindElement(By.ClassName("bot-name"));
					var divRevision = row.FindElement(By.ClassName("bot-revision"));
					var divUser = row.FindElement(By.ClassName("user-name"));
					var divElo = row.FindElement(By.ClassName("cell-table-square"));
					var bot = new AIGamesBot();

					if (divName != null) { bot.Name = divName.Text.Trim(); }
					if (divUser != null) { bot.Owner = divUser.Text.Trim(); }
					if (divElo != null) 
					{
						var str = divElo.Text.Trim();
						decimal elo;
						if (Decimal.TryParse(str, out elo))
						{
							bot.Elo = elo;
						}
					}
					if (divRevision != null)
					{
						var str = divRevision.Text.Trim();
						int rev;
						if (Int32.TryParse(str.Substring(1), out rev))
						{
							bot.Revision = rev;
						}
					}
					Guid guid;
					if (Guid.TryParse(id, out guid))
					{
						bot.Id = guid;
					}
					yield return bot;
				}
			}
		}

		/// <summary>Navigates to the sign in page.</summary>
		public void SignIn() { Driver.Url = "http://theaigames.com/sign-in/"; }

		/// <summary>Quits the driver.</summary>
		public void Quit() { Driver.Quit(); }

		/// <summary>Gets the driver for Google Chrome.</summary>
		public static WebDriverWrapper GetChrome()
		{
			return new WebDriverWrapper(new ChromeDriver());
		}
	}
}
