using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.resource {
    [RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
    public partial class TransportRouteSpecification : Resource {
        [Export]
        public string name { set; get; }
        [Export]
        public string desc { set; get; }
        [Export]
        public ResourceData maintenanceType { set; get; }
        [Export]
        public Array<double> maintenanceCost { set; get; }
        [Export]
        public Array<double> capacity { set; get; }
    }
}