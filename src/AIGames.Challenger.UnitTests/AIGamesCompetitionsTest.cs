using NUnit.Framework;
using System.IO;

namespace AIGames.Challenger.UnitTests
{
	[TestFixture]
	public class AIGamesCompetitionsTest
	{
		[Test]
		public void Save_Stream_SomeData()
		{
			// Arrange.
			var file = new FileInfo("competitions.xml");
			if (file.Exists) { file.Delete(); }

			var competions = new AIGamesCompetitions()
			{
				new AIGamesCompetition(){DisplayName = "Warlight", UrlKey = "warlight-ai-challenge" }
			};
			
			// Act.
			using (var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
			{
				competions.Save(stream);

				// Assert
				Assert.IsTrue(file.Exists, "The file should exist.");
			}
		}
	}
}
