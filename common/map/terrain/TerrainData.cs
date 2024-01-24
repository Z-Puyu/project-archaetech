using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;
using ProjectArchaetech.resources;
using ProjectArchaetech.util.events;

namespace ProjectArchaetech.common {
    [GlobalClass, RegisteredType(nameof(Tech), "", nameof(Resource))]
    public partial class TerrainData : Resource {
        [Export]
        public bool IsWater { set; get; }
        [Export]
        public string Name { set; get; }
        [Export]
        public int TimeToTraverse { set; get; }
        [Export]
        public bool Harmful { set; get; }
        [Export]
        public int Damage { set; get; }
        [Export]
        public double ResourceProbability { set; get; }
        [Export]
        public Dictionary<ResourceData, int> DiscoverableResources { set; get; }

        public TerrainData() {
            this.IsWater = false;
            this.Name = "";
            this.TimeToTraverse = 1;
            this.Harmful = false;
            this.Damage = 0;
            this.ResourceProbability = 0;
            this.DiscoverableResources = new Dictionary<ResourceData, int>();
        }
    }
}