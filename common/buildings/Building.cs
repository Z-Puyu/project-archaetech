using System;
using Godot;
using Godot.Collections;
using ProjectArchaetech.common.util;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public abstract partial class Building : Node2D, IRecyclable {
		[Export]
		public BuildingData data { set; get; }

		protected class DisplayingBuildingUIEvent : EventArgs { }

		public override void _Ready() {
			this.GetNode<Area2D>("Area2D").InputEvent += this.OnClick;
			Global.EventBus.Subscribe<DisplayingBuildingUIEvent>(this.NotifyUI);
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
					Global.EventBus.Publish(this, new DisplayingBuildingUIEvent());
				}
			}
		}

		private void NotifyUI(object sender, EventArgs e) {
			Global global = ((Building) sender).GetNode<Global>("/root/Global");
			global.EmitSignal(Global.SignalName.DisplayingBuildingUI, this);
			global.EmitSignal(Global.SignalName.ModalToggledUI, "building");
		}

		public virtual void Disable() {
			Global.EventBus.Unsubscribe<DisplayingBuildingUIEvent>(this.NotifyUI);
			Global.Grave.Enqueue(this);
		}
	}
}
