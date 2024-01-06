using Godot;

namespace ProjectArchaetech {
	[GlobalClass]
	public partial class GameManager : Node {
		private GameClock gameClock;
		private int nDays;

		public GameClock GameClock { get => gameClock; private set => gameClock = value; }
		public int NDays { get => nDays; private set => nDays = value; }

		[Signal]
		public delegate void NewMonthEventHandler();

		public override void _Ready() {
			this.GameClock = (GameClock) this.GetChild<Timer>(0);
			this.GameClock.Timeout += this.OnWorldTimerTimeout;
		}

		private void NextTurn() {
			Global.PopManager.Update();
		}

		private void OnWorldTimerTimeout() {
			this.NDays += 1;
			if (this.NDays % 30 == 0) {
				this.NextTurn();
				this.EmitSignal(SignalName.NewMonth);
			}
		}
	}
}
