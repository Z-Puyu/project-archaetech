using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.util.events;

namespace ProjectArchaetech.common.components {
    public class MovementFunctionality : IFunctionable {
        private readonly Action<Cell> onMoveTo;
        private readonly Action onReached;
        private List<ValueTuple<Cell, int>> path;

        public List<ValueTuple<Cell, int>> Path { get => path; set => path = value; }
        private int next;

        public event IFunctionable.ObjectUIDataUpdatedEventHandler ObjectUIDataUpdatedEvent;

        public MovementFunctionality(Action<Cell> onMoveTo, Action onReached) {
            this.onMoveTo = onMoveTo;
            this.onReached = onReached;
        }

        public void Start() {
            if (this.Path.Count > 1) {
                this.next = 1;
            } else {
                GD.PushError("Attempting to move but there is no path!");
            }
        }

        public void Execute() {
            ValueTuple<Cell, int> next = this.Path[this.next];
            next.Item2 -= 1;
            if (next.Item2 == 0) {
                this.next += 1;
                this.onMoveTo(next.Item1);
                if (this.next == this.Path.Count) {
                    this.onReached();
                }
            }
        }
    }
}