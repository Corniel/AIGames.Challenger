using NUnit.Framework;
using System;
using System.Linq;

namespace AIGames.UnitTests
{
	[TestFixture]
	public class WebDriverWrapperTest
	{
		[Test]
		public void GetLeaderboard_BlockBattle_MultipleElements()
		{
			var driver = WebDriverWrapper.GetChrome();
			var result = driver.GetLeaderboard(AIGamesCompetitions.All["FourInARow"]).ToList();

			Assert.IsTrue(result.Any());
		}
	}
}
