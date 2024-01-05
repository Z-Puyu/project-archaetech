using Godot;

namespace ProjectArchaetech {
	public partial class GameManager : Node {
		private Timer gameClock;
		private int nDays;

		public Timer GameClock { get => gameClock; private set => gameClock = value; }
		public int NDays { get => nDays; private set => nDays = value; }

		[Signal]
		public delegate void NewMonthEventHandler();

		public override void _Ready() {
			this.GameClock = this.GetChild<Timer>(0);
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
