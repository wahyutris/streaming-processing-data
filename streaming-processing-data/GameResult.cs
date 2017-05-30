using System;
namespace streamingprocessingdata
{
	public enum HomeOrAway
	{
		Home,
		Away
	}

    public class GameResult
    {
        public DateTime GameDate { get; set; }
        public String TeamName { get; set; }
        public HomeOrAway HomeOrAway {get; set;} //create own data type
        public int Goals { get; set; }
        public int GoalAttempts { get; set; }
        public int ShotsOnGoal { get; set; }
        public int ShotsOffGoal { get; set; }
        public Double PossessionPercent { get; set; }
		public Double ConversionRate
		{
			get
			{
				return (double)Goals / (double)GoalAttempts;
			}
		}
    }
}
