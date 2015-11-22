using System;
using System.Windows.Forms;

namespace AIGames.Challenger
{
	public static class Program
	{
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new ChallengerForm());
		}
	}
}
