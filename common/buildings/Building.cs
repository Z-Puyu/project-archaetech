using System;
using Godot;
using Godot.Collections;
using ProjectArchaetech.common.util;
using ProjectArchaetech.events;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public abstract partial class Building : Node2D, IRecyclable {
		[Export]
		public BuildingData data { set; get; }

		[Signal]
		public delegate void BuildingInfoUpdatedUIEventHandler(Building building);

		public override void _Ready() {
			this.GetNode<Area2D>("Area2D").InputEvent += this.OnClick;
		}

		public bool CanBeBuilt(TileData tile, Cell location) {
			if (location.Building != null && location.Building is not BaseBuilding) {
				return false;
			}
			if (this.data.RequiredTerrains.Count != 0 && !this.data.RequiredTerrains.ContainsKey((Resource) tile.GetCustomData("terrain"))) {
				return false;
			}
			Dictionary<ResourceData, int> cost = this.data.Cost;
			foreach (ResourceData res in cost.Keys) {
				if (!Global.ResManager.HasEnough(res, cost[res])) {
					return false;
				}
			}
			return true;
		}

		protected virtual void OnClick(Node viewport, InputEvent e, long shapeIdx) {
			if (e.IsActionPressed("left_click")) {
				if (Global.GameState != Global.GameMode.BuildRoute) {
					Global.PickUp = this;
					Global global = this.GetNode<Global>("/root/Global");
					global.EmitSignal(Global.SignalName.OpeningModalUI, "building");
				}
			}
		}

		public virtual void Disable() {
			Global.Grave.Enqueue(this);
		}

		public override string ToString() {
			return this.data.Name;
		}
	}
}
