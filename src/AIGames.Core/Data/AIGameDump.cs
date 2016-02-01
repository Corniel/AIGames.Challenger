using AIGames.Configuration;
using System;
using System.IO;
using System.Text;

namespace AIGames.Data
{
	public static class AIGameDump
	{
		public static Stream Load(AIGamesCompetition competition, AIGameResult game)
		{
			if (competition == null) { throw new ArgumentNullException("competition"); }
			if (game == null) { throw new ArgumentNullException("game"); }

			var location = GetLocation(competition, game);
			return new FileStream(location.FullName , FileMode.Open, FileAccess.Read);
		}
		
		public static void Save(Stream stream, string code)
		{
			if (stream == null) { throw new ArgumentNullException("stream"); }
			if (string.IsNullOrEmpty(code)) { throw new ArgumentNullException("code"); }

			var writer = new StreamWriter(stream);
			var buffer = new StringBuilder();
			var inTag = false;

			var tag = new StringBuilder();

			// don't write the HTML tags to the stream.
			foreach (var ch in code)
			{
				if (ch == '<')
				{
					inTag = true;
				}
				else if (ch == '>')
				{
					// for readability we have to add a new line after the
					// break tag.
					if (tag.ToString() == "br")
					{
						writer.WriteLine(buffer.ToString());
						buffer.Clear();
					}
					inTag = false;
					tag.Clear();
				}
				else if (inTag)
				{
					tag.Append(ch);
				}
				else
				{
					if (buffer.Length > 0 || !Char.IsWhiteSpace(ch))
					{
						buffer.Append(ch);
					}
				}
			}
			writer.WriteLine(buffer.ToString());
			writer.Flush();
		}

		private static FileInfo GetLocation(AIGamesCompetition competition, AIGameResult game)
		{
			return new FileInfo(Path.Combine(AppConfig.Games_RootDir_Dump.FullName, competition.UrlKey, string.Format("{0}.dat", game.Id)));
		}
	}
}
