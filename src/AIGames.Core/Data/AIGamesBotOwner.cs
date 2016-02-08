using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AIGames.Data
{
	[Serializable]
	public class AIGamesBotOwner
	{
		public AIGamesBotOwner()
		{
			Bots = new List<AIGamesBot>();
		}

		[XmlAttribute("Name")]
		public string Name { get; set; }

		[XmlElement("Bot")]
		public List<AIGamesBot> Bots { get; set; }

		public AIGamesBot AddOrUpdate(AIGamesBot bot)
		{
			var match = Bots.FirstOrDefault(b => b.Name == bot.Name && b.Revision == b.Revision);
			if (match == null)
			{
				match = new AIGamesBot();
				match.Name = bot.Name;
				match.Revision = bot.Revision;
				Bots.Add(match);
			}
			match.Id = bot.Id;
			match.Rating = bot.Rating;
			// Don't store double.
			match.Owner = null;

			return match;
		}
	}
}
