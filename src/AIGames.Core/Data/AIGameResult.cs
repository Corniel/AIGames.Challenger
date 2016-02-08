using AIGames.Configuration;
using HtmlAgilityPack;
using Qowaiv;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace AIGames.Data
{
	[Serializable]
	[DebuggerDisplay("{DebuggerDisplay}")]
	public partial class AIGameResult : IComparable<AIGameResult>
	{
		public enum Score
		{
			Win,
			Draw,
			Lose,
		}

		/// <summary>Initializes a new AI Game.</summary>
		public AIGameResult()
		{
			Bot1 = new AIGameResult.Bot();
			Bot2 = new AIGameResult.Bot();
		}

		/// <summary>Game ID.</summary>
		[XmlAttribute("id")]
		public string Id { get; set; }

		/// <summary>Gets bot 1.</summary>
		public AIGameResult.Bot Bot1 { get; set; }

		/// <summary>Gets bot 2.</summary>
		public AIGameResult.Bot Bot2 { get; set; }

		public Score Outcome
		{
			get
			{
				if (Bot1.Score == 1) { return Score.Win; }
				if (Bot2.Score == 1) { return Score.Lose; }
				return Score.Draw;
			}
		}

		/// <summary>Date on which the game was played.</summary>
		public Date Date { get; set; }

		/// <summary>Number of rounds.</summary>
		public int Rounds { get; set; }

		public int CompareTo(AIGameResult other)
		{
			return other.Date.CompareTo(Date);
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get
			{
				var res = "1/2";
				/**/ if (Outcome == Score.Win) { res = "1-0"; }
				else if (Outcome == Score.Lose) { res = "0-1"; }
				return String.Format("{0:yyyy-MM-dd} {1} - {2} : {3} {{{4}}}", 
					Date, 
					Bot1.Name, 
					Bot2.Name,
					res,
					Id);
			}
		}

		internal static AIGameResult FromRow(HtmlNode row)
		{
			var game = new AIGameResult();

			var tds = row.Elements("td").ToArray();

			//Rounds
			string rounds = tds[3].InnerText.Trim();

			// No moves, so skip.
			if (rounds == "...") { return null; }

			game.Rounds = Int32.Parse(rounds.Split(' ')[0]);

			//Id
			var divNode = tds[4].Descendants("a").First();
			string url = divNode.Attributes["href"].Value;
			game.Id = url.Substring(url.Length - 24);

			//Date
			game.Date = ParseDate( tds[0].InnerText);

			//Bots
			var td1 = tds.ElementAt(1);
			var td2 = tds.ElementAt(2);
			game.Bot1.Name = td1.Descendants("div").ElementAt(0).InnerText.Trim();
			game.Bot1.Version = int.Parse(td1.Descendants("div").ElementAt(1).InnerText.Trim().Substring(1));
			game.Bot2.Name = td2.Descendants("div").ElementAt(0).InnerText.Trim();
			game.Bot2.Version = int.Parse(td2.Descendants("div").ElementAt(1).InnerText.Trim().Substring(1));

			game.Bot1.IsMyBot = game.Bot1.Name.Equals(AppConfig.Bot_Name, StringComparison.InvariantCultureIgnoreCase);
			game.Bot2.IsMyBot = game.Bot2.Name.Equals(AppConfig.Bot_Name, StringComparison.InvariantCultureIgnoreCase);

			// Result.
			if (td1.Attributes["class"].Value.Split(' ').Any(c => c.Equals("is-game-winner")))
			{
				game.Bot1.Score = 1f;
				game.Bot2.Score = 0f;
			}
			else if (td2.Attributes["class"].Value.Split(' ').Any(c => c.Equals("is-game-winner")))
			{
				game.Bot1.Score = 0f;
				game.Bot2.Score = 1f;
			}

			return game;
		}
		private static Date ParseDate(string str)
		{
			DateTime dt;
			if (DateTime.TryParseExact(str.Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
			{
				return (Date)dt;
			}
			return Date.Today;
		}
	}
}
