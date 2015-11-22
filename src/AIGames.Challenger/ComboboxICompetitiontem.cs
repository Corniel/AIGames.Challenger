using System;

namespace AIGames.Challenger
{
	public class ComboboxICompetitiontem
	{
		public string Text { get; set; }
		public object Value { get; set; }

		public override string ToString() { return Text ?? String.Empty; }

		public static ComboboxICompetitiontem Create(AIGamesCompetition competition)
		{
			if (competition == null) { throw new ArgumentNullException("competition"); }

			return new ComboboxICompetitiontem()
			{
				Text = competition.DisplayName,
				Value = competition,
			};
		}
	}
}
