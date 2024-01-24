using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
    [RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
    public partial class UnitData : Resource{
        [Export]
        public string Name { get; set; }
        [Export]
        public double Speed { get; set; }
        [Export]
        public Dictionary<ResourceData, int> Cost { get; set; }

        public UnitData() {
            this.Name = "";
            this.Speed = 1.0;
            this.Cost = new Dictionary<ResourceData, int>();
        }
    }
}