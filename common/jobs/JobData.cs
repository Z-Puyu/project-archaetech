using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.resources {
    [RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
    public partial class JobData : Resource {
        [Export]
        public string name { set; get; }
        [Export]
        public string desc { set; get; }
        [Export]
        public Texture2D icon { set; get; }
        [Export]
        public Dictionary<ResourceData, double> input { set; get; }
        [Export]
        public Dictionary<ResourceData, double> output { set; get; }

        public JobData() {
            this.name = "New Job";
            this.desc = "";
            this.icon = null;
            this.input = new Dictionary<ResourceData, double>();
            this.output = new Dictionary<ResourceData, double>();
        }

        public override string ToString() {
            return this.name;
        }
    }
}