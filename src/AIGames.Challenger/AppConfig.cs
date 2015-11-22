using System;
using System.Configuration;

namespace AIGames.Challenger
{
	public static class AppConfig
	{
		private const string DefaultCompetitionKey = "Competition.Default";

		public static String DefaultCompetition { get { return ConfigurationManager.AppSettings[DefaultCompetitionKey]; } }
	}
}
