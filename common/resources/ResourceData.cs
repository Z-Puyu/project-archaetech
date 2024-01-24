using Godot;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.resources {
	[GlobalClass, RegisteredType(nameof(Tech), "", nameof(Resource))]
	public partial class ResourceData : Resource {
		[Export]
		public string name { set; get; }
		[Export]
		public string desc { set; get; }
		[Export]
		public Texture2D icon { set; get; }
		[Export]
		public ResourceType type { set; get; }

		public ResourceData() {
			this.name = "New Resource";
			this.desc = "";
			this.icon = null;
			this.type = ResourceType.CommonRaw;
		}

		public void Use(double amount) {
			Global.ResManager.Consume(this, amount);
		}
		
		public override string ToString() {
			return this.name;
		}
	}
}
