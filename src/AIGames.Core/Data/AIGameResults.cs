using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace AIGames.Data
{
	[Serializable, XmlRoot("Games")]
	public class AIGameResults : List<AIGameResult> 
	{
		/// <summary>Returns true if the game is already in de collection.</summary>
		public bool ContainsId(AIGameResult game)
		{
			return game != null && this.Any(g => g.Id == game.Id);
		}
		#region I/O

		/// <summary>Saves the AI-Games results as XML to a file.</summary>
		public void Save(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
			{
				Save(stream);
			}
		}

		/// <summary>Saves the AI-Games results as XML to a stream.</summary>
		public void Save(Stream stream)
		{
			if (stream == null) { throw new ArgumentNullException("stream"); }
			serializer.Serialize(stream, this);
		}

		/// <summary>Loads the AI-Games results from a file.</summary>
		public static AIGameResults Load(FileInfo file)
		{
			using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
			{
				return Load(stream);
			}
		}

		/// <summary>Loads the AI-Games results from an XML stream.</summary>
		public static AIGameResults Load(Stream stream)
		{
			if (stream == null) { throw new ArgumentNullException("stream"); }
			return (AIGameResults)serializer.Deserialize(stream);
		}

		private static readonly XmlSerializer serializer = new XmlSerializer(typeof(AIGameResults));

		#endregion
	}
}
