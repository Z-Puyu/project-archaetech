using Godot;

namespace ProjectArchaetech.common.util {
    public partial class ConstructibleTask<T> : Node where T : Node {
        private int daysRemaining;
        private Cell location;
        private readonly T value;

        public T Value { get => Value; }
        public int DaysRemaining { get => daysRemaining; set => daysRemaining = value; }
        public Cell Location { get => location; }

        public delegate void TerminateEventHandler(ConstructibleTask<T> task);
        public event TerminateEventHandler TerminateEvent;

        public ConstructibleTask(T value, Cell location, int daysRemaining) {
            this.value = value;
            this.daysRemaining = daysRemaining;
            this.location = location;
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
                TerminateEvent?.Invoke(this);
            }
        }     
    }
}