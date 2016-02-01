using AIGames.Data;
using NUnit.Framework;
using System.IO;

namespace AIGames.Challenger.UnitTests.Data
{
	[TestFixture]
	public class AIGameDumpTest
	{
		[Test]
		public void Save_AIGameDump0_ToStream()
		{
			using (var stream = new MemoryStream())
			{
				AIGameDump.Save(stream, Files.GameDump0);
			}
		}
	}
}
