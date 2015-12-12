using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.Data
{
	public class AIGameDump
	{
		public static void Save(AIGamesCompetition competition, AIGameResult game, DirectoryInfo root)
		{
			if (competition == null) { throw new ArgumentNullException("competition"); }
			if (game == null) { throw new ArgumentNullException("game"); }
			if (root == null) { throw new ArgumentNullException("root"); }

			var location = Path.Combine(root.FullName, competition.UrlKey, String.Format("{0}.dat", game.Id));

			

		}

		public static void Save(string code, Stream stream)
		{
			if (stream == null) { throw new ArgumentNullException("stream"); }
			if (String.IsNullOrEmpty(code)) { throw new ArgumentNullException("code"); }

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
	}
}
