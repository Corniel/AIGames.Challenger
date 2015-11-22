using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AIGames.Challenger
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

		/// <summary>Navigates to the sign in page.</summary>
		public void SignIn() { Driver.Url = "http://theaigames.com/sign-in/"; }

		/// <summary>Quits the driver.</summary>
		public void Quit() { Driver.Quit(); }

		public static WebDriverWrapper GetChrome()
		{
			return new WebDriverWrapper(new ChromeDriver());
		}
	}
}
