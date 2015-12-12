using AIGames.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				AIGameDump.Save(Files.GameDump0, stream);
			}
		}
	}
}
