using System;
using Godot;
using ProjectArchaetech.events;
using static ProjectArchaetech.events.EventBus;

namespace ProjectArchaetech {
	[GlobalClass]
	public partial class GameManager : Node {
		private GameClock gameClock;
		private int nDays;
		private static readonly NewMonthEvent newMonthEvent = new NewMonthEvent();

		public GameClock GameClock { get => gameClock; private set => gameClock = value; }
		public int NDays { get => nDays; private set => nDays = value; }

		public override void _Ready() {
			this.GameClock = (GameClock) this.GetChild<Timer>(0);
			this.GameClock.Timeout += this.OnWorldTimerTimeout;
		}

		private void NextTurn() {
			Global.EventBus.Publish(this, newMonthEvent);
		}

		private void OnWorldTimerTimeout() {
			this.NDays += 1;
			if (this.NDays % 30 == 0) {
				this.NextTurn();
			}
		}
	}
}
