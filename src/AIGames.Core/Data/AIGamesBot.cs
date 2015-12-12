using HtmlAgilityPack;
using Qowaiv.Statistics;
using System.Linq;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Qowaiv;

namespace AIGames.Data
{
	/// <summary>Represents an AI Games bot.</summary>
	[Serializable, DebuggerDisplay("{DebuggerDisplay}")]
	public class AIGamesBot: IComparable<AIGamesBot>
	{
		/// <summary>Initializes a new instance of an AI Games bot.</summary>
		public AIGamesBot()
		{
			Rating = 1400;
		}

		/// <summary>The GUID of the bot.</summary>
		public String Id { get; set; }
		
		/// <summary>The name of the bot.</summary>
		/// <remarks>
		/// The name may change over time.
		/// </remarks>
		public String Name { get; set; }

		/// <summary>The current revision of the bot.</summary>
		public int Revision { get; set; }

		/// <summary>The owner of the bot.</summary>
		public String Owner { get; set; }

		/// <summary>The rating of the bot.</summary>
		/// <remarks>
		/// The initial rating is 1400.
		/// </remarks>
		public Elo Rating { get; set; }

		/// <summary>Orders the bots by rating descending.</summary>
		public int CompareTo(AIGamesBot other)
		{
			return other.Rating.CompareTo(Rating);
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get
			{
				return String.Format("{3:0000} {0} v{1} ({2}) {3}", Name, Revision, Owner, Rating, Id);
			}
		}

		internal static AIGamesBot FromRow(HtmlNode row)
		{
			var bot = new AIGamesBot();

			var tds = row.Elements("td").ToArray();

			bot.Id = row.GetAttributeValue("data-rowid", String.Empty);
			bot.Name = tds[2].FirstChild.InnerText.Trim();
			bot.Owner = tds[3].InnerText.Trim();
			bot.Rating = Elo.TryParse(tds[4].InnerText.Trim());

			int reversion;
			if (Int32.TryParse(tds[2].ChildNodes[1].InnerText.Trim().Replace("v", ""), out reversion))
			{
				bot.Revision = reversion;
			}
			return bot;
		}
	}
}
