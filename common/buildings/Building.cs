using Godot;
using Godot.Collections;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public abstract partial class Building : Node2D {
		[Export]
		public BuildingData data { set; get; }

		[Signal]
		public delegate void showBuildingInfoEventHandler(Dictionary<string, Building> info);
		[Signal]
		public delegate void selectedEventHandler(Building building);

		public override void _Ready() {
			this.GetNode<Area2D>("Area2D").InputEvent += this.OnClick;
		}

		public bool CanBeBuilt(TileData tile, Cell location) {
			if (location.Building != null && location.Building.data.id.Equals("building-player-base")) {
				return false;
			}
			if (!this.data.requiredTerrains.ContainsKey((Resource) tile.GetCustomData("terrain"))) {
				return false;
			}
			Dictionary<ResourceData, int> cost = this.data.cost;
			ResourceManager resManager = this.GetNode<ResourceManager>("/root/ResourceManager");
			foreach (ResourceData res in cost.Keys) {
				if (!resManager.HasEnough(res, cost[res])) {
					return false;
				}
			}
			return true;
		}

		protected virtual void OnClick(Node viewport, InputEvent e, long shapeIdx) {
			if (e.IsActionPressed("left_click")) {
				if (Global.GameState != Global.GameMode.BuildRoute) {
					Global.EventBus.EmitSignal(Events.SignalName.DisplayingBuildingUI, this);
					Global.EventBus.EmitSignal(Events.SignalName.ModalToggled, "building");
				}
			}
		}
	}
}
