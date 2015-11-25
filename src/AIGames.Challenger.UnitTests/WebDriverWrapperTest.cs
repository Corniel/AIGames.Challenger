using NUnit.Framework;
using System.Linq;

namespace AIGames.Challenger.UnitTests
{
	[TestFixture]
	public class WebDriverWrapperTest
	{
		[Test]
		public void GetLeaderboard_BlockBattle_MultipleElements()
		{
			var driver = WebDriverWrapper.GetChrome();
			var result = driver.GetLeaderboard(AIGamesCompetitions.All[4]).ToList();

			Assert.IsTrue(result.Any());
		}
	}
}
