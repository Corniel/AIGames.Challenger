using System;
using System.Configuration;
using System.IO;

namespace AIGames.Configuration
{
	public static class AppConfig
	{
		public static String Competition_Default
		{
			get { return ConfigurationManager.AppSettings["Competition.Default"]; }
		}
		public static String Bot_Name
		{
			get { return ConfigurationManager.AppSettings["Bot.Name"]; }
		}

		public static DirectoryInfo Games_RootDir_Dump
		{
			get { return new DirectoryInfo(ConfigurationManager.AppSettings["Games.RootDir.Dump"]); }
		}
	}
}
