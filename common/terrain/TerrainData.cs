using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
    [GlobalClass, RegisteredType(nameof(Tech), "", nameof(Resource))]
    public partial class TerrainData : Resource {
        [Export]
        public TerrainType Type { set; get; }
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
    }
}