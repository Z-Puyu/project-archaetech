using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.resource {
	[RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
	public partial class TransportRouteSpecification : Resource {
		[Export]
		public string Name { set; get; }
		[Export]
		public string Desc { set; get; }
		[Export]
		public ResourceData MaintenanceType { set; get; }
		[Export]
		public Array<double> MaintenanceCost { set; get; }
		[Export]
		public Array<double> Capacity { set; get; }
	}
}
