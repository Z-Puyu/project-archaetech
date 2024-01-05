using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.resources {
    [RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
    public partial class BuildingData : Resource {
        [Export]
        public string name { set; get; }
        [Export]
        public string id { set; get; }
        [Export]
        public Texture2D icon { set; get; }
        [Export]
        public int width { set; get; }
        [Export]
        public int height { set; get; }
        [Export]
        public Dictionary<ResourceData, int> cost { set; get; }
        [Export]
        public Dictionary<Resource, Variant> requiredTerrains { set; get; }
        [Export]
        public ResourceData requiredResource { set; get; }
        [Export]
        public int timeToBuild { set; get; }
        [Export]
        public Array<ProductionMethod> productionMethods { set; get; }
    }
}