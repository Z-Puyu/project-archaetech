using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech {
	[RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
	public partial class Tech : Resource {
		[Export]
		public string name { get; set; }
		[Export]
		public string desc { get; set; }
		[Export]
		public int type { get; set; }
		[Export]
		public Texture2D icon { get; set; }
		[Export]
		public Array<Tech> prerequisites { get; set; }
		[Export]
		public Array<Tech> children { get; set; }
		[Export]
		public int cost { get; set; }
		[Export]
		public int progress { get; set; }
		[Export]
		public int weight { get; set; }
		[Export]
		public Array<Variant> rewards { get; set; }

		public Tech() {
			this.name = "Test";
			this.desc = "";
			this.type = 0;
			this.icon = null;
			this.prerequisites = new Array<Tech>();
			this.children = new Array<Tech>();
			this.cost = 100;
			this.progress = 0;
			this.weight = 10;
			this.rewards = new Array<Variant>();
		}
	}
}
