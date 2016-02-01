using AIGames.Configuration;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace AIGames
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

		/// <summary>Gets the URL to the game log of a player.</summary>
		/// <param name="playerName">
		/// The name of the player
		/// </param>
		/// <param name="page">
		/// The page to visit.
		/// </param>
		public Uri GetGameListUrl(string playerName, int page)
		{
			var url = String.Format(CultureInfo.InvariantCulture,
				"http://theaigames.com/competitions/{0}/game-log/{1}/{2}",
				UrlKey,
				playerName,
				page);

			return new Uri(url);
		}

		/// <summary>Gets the URL to the dump of a game.</summary>
		/// <param name="gameid">
		/// The identifier of a game.
		/// </param>
		public Uri GetGameDumpUrl(string gameid)
		{
			var url = String.Format(CultureInfo.InvariantCulture,
				"http://theaigames.com/competitions/{0}/games/{1}/dump",
				UrlKey,
				gameid);

			return new Uri(url);
		}

		/// <summary>Gets the directory where the game dumbs are stored.</summary>
		public DirectoryInfo GetGameDumpDirectory()
		{
			return new DirectoryInfo(Path.Combine(AppConfig.Games_RootDir_Dump.FullName, UrlKey));
		}

		/// <summary>Gets the URL to the leader board for this competition.</summary>
		public Uri GetLeaderboard()
		{
			var url = String.Format(CultureInfo.InvariantCulture,
				"http://theaigames.com/competitions/{0}/leaderboard/global/a/",
				UrlKey);

			return new Uri(url);
		}

		/// <summary>returns true if the string matches the display name or URL key.</summary>
		internal bool IsMatch(string str)
		{
			if (String.IsNullOrEmpty(str)) { return false; }
			var match = str.Replace(" ", "").Trim().ToUpperInvariant();

			var dp = DisplayName.Replace(" ", "").Trim().ToUpperInvariant();
			var key = UrlKey.Replace(" ", "").Trim().ToUpperInvariant();

			return match == dp || match == key;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay { get { return String.Format("Name: {0}, Key: {1}", DisplayName, UrlKey); } }

		
	}
}
