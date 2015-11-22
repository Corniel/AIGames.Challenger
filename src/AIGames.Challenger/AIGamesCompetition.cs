using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml.Serialization;

namespace AIGames.Challenger
{
	/// <summary>Represents AI-Games competition data.</summary>
	[Serializable, DebuggerDisplay("{DebuggerDisplay}")]
	public class AIGamesCompetition
	{
		/// <summary>The display name in the UI.</summary>
		[XmlAttribute("name")]
		public string DisplayName { get; set; }

		/// <summary>The key in the URL.</summary>
		[XmlAttribute("key")]
		public string UrlKey { get; set; }

		/// <summary>Gets the URL to challenge an opponent.</summary>
		/// <param name="playerName">The name of the player</param>
		public Uri GetChallengeUrl(string playerName)
		{
			if (String.IsNullOrEmpty(playerName)) { return new Uri("http://theaigames.com/competitions"); }

			var url = String.Format(CultureInfo.InvariantCulture,
				"http://theaigames.com/competitions/{0}/game/challenge/{1}/new",
				UrlKey,
				playerName.ToLowerInvariant());

			return new Uri(url);
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay { get { return String.Format("Name: {0}, Key: {1}", DisplayName, UrlKey); } }
	}
}
