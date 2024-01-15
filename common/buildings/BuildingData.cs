using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.resources {
	[RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
	public partial class BuildingData : Resource {
		[Export]
		public string Name { set; get; }
		[Export]
		public string Desc { set; get; }
		[Export]
		public string Id { set; get; }
		[Export]
		public Texture2D Icon { set; get; }
		[Export]
		public int Width { set; get; }
		[Export]
		public int Height { set; get; }
		[Export]
		public Dictionary<ResourceData, int> Cost { set; get; }
		[Export]
		public Dictionary<Resource, Variant> RequiredTerrains { set; get; }
		[Export]
		public ResourceData RequiredResource { set; get; }
		[Export]
		public int TimeToBuild { set; get; }
		[Export]
		public Array<ProductionMethod> ProductionMethods { set; get; }

		public BuildingData() {
			this.Name = "";
			this.Desc = "";
			this.Id = "";
			this.Icon = null;
			this.Width = 1;
			this.Height = 1;
			this.Cost = new Dictionary<ResourceData, int>();
			this.RequiredTerrains = new Dictionary<Resource, Variant>();
			this.RequiredResource = null;
			this.TimeToBuild = 10;
			this.ProductionMethods = new Array<ProductionMethod>();
		}
	}
}
