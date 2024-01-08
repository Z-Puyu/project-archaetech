using System;
using Godot;
using ProjectArchaetech.events;

namespace ProjectArchaetech.common.util {
	public partial class ConstructibleTask<T> : Node2D where T : Node {
		private int daysRemaining;
		private Cell location;
		private readonly T value;

		public T Value => value;
		public int DaysRemaining { get => daysRemaining; set => daysRemaining = value; }
		public Cell Location { get => location; }

		public ConstructibleTask(T value, Cell location, int daysRemaining) {
			this.value = value;
			this.daysRemaining = daysRemaining;
			this.location = location;
			Global.GameManager.GameClock.Timeout += this.Progress;
		}

		public void Start() {
			Global.GameManager.GameClock.Timeout += this.Progress;
		}

		public void Pause() {
			Global.GameManager.GameClock.Timeout -= this.Progress;
		}

		protected virtual void Progress() {
			this.DaysRemaining -= 1;
			if (this.DaysRemaining == 0) {
				if (this.Value is Building) {
					this.location.Building = (Building) (Node) this.Value;
					this.GetNode<Node2D>("/root/World").AddChild(this.Value);
				}
				Global.GameManager.GameClock.Timeout -= this.Progress;
				Global.EventBus.Publish(this, new ConstructionTaskCompletedEvent<T>(this));
			}
		}

		public override string ToString() {
			return "[" + this.Value.ToString() + "]";
		}
	}
}
