using Godot;
using ProjectArchaetech.common.util;
using ProjectArchaetech.util.events;

namespace ProjectArchaetech {
	[GlobalClass]
	public partial class GameManager : Node {
		private GameClock gameClock;
		private Date date;
		private static readonly NewDayEvent newDayEvent = new NewDayEvent();
		private static readonly NewWeekEvent newWeekEvent = new NewWeekEvent();
		private static readonly NewFortnightEvent newFortnightEvent = new NewFortnightEvent();
		private static readonly NewMonthEvent newMonthEvent = new NewMonthEvent();
		private static readonly NewBiMonthEvent newBiMonthEvent = new NewBiMonthEvent();
		private static readonly NewQuarterEvent newQuarterEvent = new NewQuarterEvent();
		private static readonly NewHalfYearEvent newHalfYearEvent = new NewHalfYearEvent();
		private static readonly NewYearEvent newYearEvent = new NewYearEvent();




		public GameClock GameClock { get => gameClock; private set => gameClock = value; }
		public Date Date { 
			get => date; 
			private set {
				date = value;
				Global.EventBus.Publish(this, newDayEvent);
				if (this.Date % 7 == 1) {
					Global.EventBus.Publish(this, newWeekEvent);
				}
				if (this.Date % 14 == 1) {
					Global.EventBus.Publish(this, newFortnightEvent);
				}
				if (this.Date % 30 == 1) {
					Global.EventBus.Publish(this, newMonthEvent);
				}
				if (this.Date % 60 == 1) {
					Global.EventBus.Publish(this, newBiMonthEvent);
				}
				if (this.Date % 120 == 1) {
					Global.EventBus.Publish(this, newQuarterEvent);
				}
				if (this.Date % 180 == 1) {
					Global.EventBus.Publish(this, newHalfYearEvent);
				}
				if (this.Date % 360 == 1) {
					Global.EventBus.Publish(this, newYearEvent);
				}
			} 
		}

		public GameManager() {
			this.date = new Date(0, 0, 1);
		}

		public override void _Ready() {
			this.GameClock = (GameClock) this.GetChild<Timer>(0);
			this.GameClock.Timeout += this.OnWorldTimerTimeout;
		}

		private void OnWorldTimerTimeout() {
			this.Date += 1;
		}
	}
}
