using AIGames.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AIGames.Challenger
{
	public partial class ChallengerForm : Form
	{
		private const string oppentFilePath = "./opponents.txt";

		private WebDriverWrapper WebDriver;

		public ChallengerForm()
		{
			InitializeComponent();
		}

		private void ChallengerForm_Load(object sender, EventArgs e)
		{
			LoadOpponents();
			WebDriver = WebDriverWrapper.GetChrome();
			WebDriver.SignIn();

			competitionsComboBox.Items.AddRange(AIGamesCompetitions.All.Select(comp => ComboboxICompetitiontem.Create(comp)).ToArray());
			competitionsComboBox.Text = AppConfig.Competition_Default;
			if (String.IsNullOrEmpty(competitionsComboBox.Text))
			{
				competitionsComboBox.Text = AIGamesCompetitions.All.Default.DisplayName;
			}
		}

		/// <summary>
		/// Challenges the next opponent from the list, turns off timer if there are no more opponents in the list
		/// </summary>
		private void DoChallenge()
		{
			var  competition = ((ComboboxICompetitiontem)competitionsComboBox.SelectedItem).Value as AIGamesCompetition;
			var opponents = opponentsTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var newText = new StringBuilder();
			bool startedGame = false;

			foreach (string line in opponents)
			{
				SaveOpponents();
				if (startedGame)
				{
					newText.AppendLine(line);
				}
				else if (!line.EndsWith("\tDONE", StringComparison.InvariantCultureIgnoreCase))
				{
					string opponent = line;
					WebDriver.ChallengeOpponent(competition, opponent);
					newText.AppendLine(line + "\tDONE");
					startedGame = true;
				}
				else
				{
					newText.AppendLine(line);
				}
			}
			if (startedGame)
			{
				opponentsTextBox.Text = newText.ToString();
			}
			else if (opponents.Any())
			{
				//All done
				opponentsTextBox.Text = newText.ToString().Replace("\tDONE", "");
				DoChallenge();
			}
		}

		private void ChallengerForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			WebDriver.Quit();
		}

		private void btnChallenge_Click(object sender, EventArgs e)
		{
			StartEverything();
		}

		private int Interval
		{
			get
			{
				int interval;		
				if(!int.TryParse(intervalTextBox.Text, out interval) || interval < 0)
				{
					return 310; //5:10 minutes
				}
				return interval;
			}
		}

		/// <summary>Number of seconds until start of next challenge</summary>
		private int CountDownSeconds { get; set; }

		private void StartEverything()
		{
			CountDownSeconds = Interval;
			timer1.Interval = Interval * 1000;
			timer1.Enabled = true;
			timer2.Interval = 1000;
			timer2.Enabled = true;

			OriginalText = opponentsTextBox.Text.Replace("\tDONE", "");

			DoChallenge();
		}

		private string OriginalText = null;

		private void timer1_Tick(object sender, EventArgs e)
		{
			StartEverything();
		}

		private void btnPause_Click(object sender, EventArgs e)
		{
			if (timer1.Enabled)
			{
				//Pause
				btnPause.Text = "C&ontinue";
				timer1.Enabled = false;
				timer2.Enabled = false;
			}
			else
			{
				//Continue
				btnPause.Text = "&Pause";
				StartEverything();
				timer1.Enabled = true;
				timer2.Enabled = true;
			}
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
			CountDownSeconds--;
			var time = new TimeSpan(0, CountDownSeconds / 60, CountDownSeconds % 60);
			label1.Text = time.ToString();
		}

		private void SaveOpponents()
		{
			try
			{
				using (var writer = new StreamWriter(oppentFilePath))
				{
					writer.Write(opponentsTextBox.Text);
				}
			}
			catch { }
		}
		private void LoadOpponents()
		{
			try
			{
				using (var reader = new StreamReader(oppentFilePath))
				{
					opponentsTextBox.Text = reader.ReadToEnd();
				}
			}
			catch { }
		}
	}
}
