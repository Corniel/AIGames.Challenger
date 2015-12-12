﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace AIGames.Data
{
	public partial class AIGameResult
	{
		[Serializable, DebuggerDisplay("{DebuggerDisplay}")]
		public class Bot
		{
			public Bot()
			{
				Score = 0.5f;
			}

			[XmlAttribute("sc")]
			public float Score { get; set; }

			[XmlAttribute("name")]
			public string Name { get; set; }

			[XmlAttribute("version")]
			public int Version { get; set; }

			[XmlAttribute("mybot")]
			public bool IsMyBot { get; set; }

			[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
			private string DebuggerDisplay { get { return String.Format("Bot: {0}{2} v{1}", Name, Version, IsMyBot ? "*" : ""); } }
		}
	}
}
