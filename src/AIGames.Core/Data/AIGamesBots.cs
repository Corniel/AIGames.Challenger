using AIGames.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace AIGames.Data
{
	[Serializable, XmlRoot("Bots")]
	public class AIGamesBots : List<AIGamesBotOwner>
	{
		public int BotCount { get { return this.Sum(owner => owner.Bots.Count); } }

		public AIGamesBot AddOrUpdate(AIGamesBot bot)
		{
			var owner = this.FirstOrDefault(o => o.Name == bot.Owner);
			if (owner == null)
			{
				owner = new AIGamesBotOwner() { Name = bot.Owner };
				Add(owner);
			}
			return owner.AddOrUpdate(bot);
		}

		public AIGamesBot GetOrAdd(AIGameResult.Bot bot)
		{
			var owner = this.FirstOrDefault(o => o.Bots.Any(b => b.Name == bot.Name));
			if (owner != null)
			{
				var result = owner.Bots.FirstOrDefault(b => b.Name == bot.Name && b.Revision == bot.Version);
				if (result == null)
				{
					result = new AIGamesBot()
					{
						Name = bot.Name,
						Revision = bot.Version,
					};
					owner.Bots.Add(result);
					return result;
				}
				return result;	
			}
			return new AIGamesBot()
			{
				Id = "?",
				Owner = "?",
				Name = bot.Name,
				Revision = bot.Version,
			};
		}

		public void DeactiveAll()
		{
			foreach (var bot in this.SelectMany(owner => owner.Bots))
			{
				bot.Active = false;
			}
		}
		
		#region I/O

		/// <summary>Saves the AI-Games bots as XML for a competition.</summary>
		public void Save(AIGamesCompetition competition)
		{
			Save(ToFile(competition));
		}

		/// <summary>Saves the AI-Games bots as XML to a file.</summary>
		public void Save(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
			{
				Save(stream);
			}
		}

		/// <summary>Saves the AI-Games bots as XML to a stream.</summary>
		public void Save(Stream stream)
		{
			if (stream == null) { throw new ArgumentNullException("stream"); }
			serializer.Serialize(stream, this);
		}

		/// <summary>Loads the AI-Games bots for a competition.</summary>
		public static AIGamesBots Load(AIGamesCompetition competition)
		{
			return Load(ToFile(competition));
		}
		/// <summary>Loads the AI-Games bots from a file.</summary>
		public static AIGamesBots Load(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.OpenOrCreate, FileAccess.Read))
			{
				return Load(stream);
			}
		}

		/// <summary>Loads the AI-Games bots from an XML stream.</summary>
		public static AIGamesBots Load(Stream stream)
		{
			if (stream == null) { throw new ArgumentNullException("stream"); }
			if (stream.Length == 0) { return new AIGamesBots(); }
			return (AIGamesBots)serializer.Deserialize(stream);
		}

		private static FileInfo ToFile(AIGamesCompetition competition)
		{
			var path = Path.Combine(AppConfig.Games_RootDir_Dump.FullName, String.Format("{0}.bots.xml", competition.UrlKey));
			return new FileInfo(path);
		}

		private static readonly XmlSerializer serializer = new XmlSerializer(typeof(AIGamesBots));

		#endregion
	}
}
