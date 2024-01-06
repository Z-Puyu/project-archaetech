using Godot;
using Godot.Collections;
using System;

namespace ProjectArchaetech {
	[GlobalClass]
	public partial class GameClock : Timer {
		[Export]
		public Dictionary<int, double> config { set; get; }
		private int speedLevel;

		public int SpeedLevel { get => speedLevel; set => speedLevel = value; }
		
		public override void _Ready() {
			this.Paused = true;
			this.SpeedLevel = 1;
		}

		public void SpeedUp() {
			this.SpeedLevel = Math.Clamp(this.SpeedLevel + 1, 1, 5);
			this.WaitTime = this.config[this.SpeedLevel];
		}

		public void SlowDown() {
			this.SpeedLevel = Math.Clamp(this.SpeedLevel - 1, 1, 5);
			this.WaitTime = this.config[this.SpeedLevel];
		}
	}
}
