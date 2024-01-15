using System;
using Godot;
using ProjectArchaetech.common.util;

namespace ProjectArchaetech.util.events {
    public class ConstructionTaskCompletedEvent<T> : EventArgs where T : Node {
        private readonly ConstructibleTask<T> task;

        public ConstructionTaskCompletedEvent(ConstructibleTask<T> task) {
            this.task = task;
        }

        public ConstructibleTask<T> Task => task;
    }
}