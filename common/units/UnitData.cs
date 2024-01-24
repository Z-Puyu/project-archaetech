using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.resources {
    [RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
    public partial class UnitData : Resource{
        [Export]
        public string Name { get; set; }
        [Export]
        public Texture2D Icon { get; set; }
        [Export]
        public double Speed { get; set; }
        [Export]
        public int PopCost { get; set; }
        [Export]
        public Dictionary<ResourceData, int> Cost { get; set; }

        public UnitData() {
            this.Name = "";
            this.Speed = 1.0;
            this.Cost = new Dictionary<ResourceData, int>();
        }
    }
}