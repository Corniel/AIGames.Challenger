using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AIGames
{
	[Serializable, DebuggerDisplay("{DebuggerDisplay}")]
	public class AIGamesBot
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Revision { get; set; }
		public string Owner { get; set; }
		public decimal Elo { get; set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get
			{
				return String.Format("{3:0000} {0} v{1} ({2}) {3:b}", Name, Revision, Owner, Elo, Id);
			}
		}
	}
}
