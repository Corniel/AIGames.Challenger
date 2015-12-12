using AIGames.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace AIGames
{
	[Serializable, XmlRoot("AIGamesCompetitions")]
	public class AIGamesCompetitions : List<AIGamesCompetition>
	{
		/// <summary>Gets all competitions specified in the assembly.</summary>
		public static AIGamesCompetitions All
		{
			get
			{
				if (s_All == null)
				{
					s_All = Load(typeof(AIGamesCompetitions).Assembly.
					GetManifestResourceStream("AIGames.AIGamesCompetitions.xml"));
				}
				return s_All;
			}
		}
		private static AIGamesCompetitions s_All;

		/// <summary>Gets a competition based on a name.</summary>
		public AIGamesCompetition this[string name]
		{
			get
			{
				return this.FirstOrDefault(comp => comp.IsMatch(name));
			}
		}

		/// <summary>Gets the default AI-Games competition.</summary>
		public AIGamesCompetition Default
		{
			get
			{
				var def = this[AppConfig.Competition_Default];
				if (def == null) { def = this.LastOrDefault(); }
				return def;
			}
		}

		#region I/O

		/// <summary>Saves the AI-Games competitions as XML to a stream.</summary>
		public void Save(Stream stream)
		{
			if (stream == null) { throw new ArgumentNullException("stream"); }
			serializer.Serialize(stream, this);
		}

		/// <summary>Loads the AI-Games competitions from an XML stream.</summary>
		public static AIGamesCompetitions Load(Stream stream)
		{
			if (stream == null) { throw new ArgumentNullException("stream"); }
			return (AIGamesCompetitions)serializer.Deserialize(stream);
		}

		private static readonly XmlSerializer serializer = new XmlSerializer(typeof(AIGamesCompetitions));

		#endregion
	}
}
