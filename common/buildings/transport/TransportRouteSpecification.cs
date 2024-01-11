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

		public TransportRouteSpecification() {
			this.Name = "Manual";
			this.Desc = "";
			this.MaintenanceType = GD.Load<ResourceData>("res://common/resources/basic/FoodResource.tres");
			this.MaintenanceCost = [0, 0.25, 0.5, 1, 1.5, 2];
			this.Capacity = [0, 0.2, 0.3, 0.4, 0.5, 0.75];
		}
	}
}
