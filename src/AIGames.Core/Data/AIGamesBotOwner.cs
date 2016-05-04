using System;
using System.Collections.Generic;
using System.Linq;
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

		/// <summary>The GUID of the owner.</summary>
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement("Bot")]
		public List<AIGamesBot> Bots { get; set; }

		public AIGamesBot AddOrUpdate(AIGamesBot bot)
		{
			var match = Bots.FirstOrDefault(b => 
				b.Name == bot.Name &&
				b.Revision == bot.Revision);

			if (match == null)
			{
				match = new AIGamesBot();
				match.Name = bot.Name;
				match.Revision = bot.Revision;
				Bots.Add(match);
			}
			if (string.IsNullOrEmpty(Id))
			{
				Id = bot.Id;
			}
			// Don't store double.
			match.Owner = null;
			match.Id = null;

			return match;
		}
	}
}
