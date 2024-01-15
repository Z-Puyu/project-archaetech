using System;
using Godot;

namespace ProjectArchaetech.events {
    public class BuildingUIDataUpdatedEvent : EventArgs {
        private readonly string key;
        private readonly Variant info;

        public BuildingUIDataUpdatedEvent(string key, Variant info) {
            this.key = key;
            this.info = info;
        }

        public string Key => key;

        public Variant Info => info;
    }
}