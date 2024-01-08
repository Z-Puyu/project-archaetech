using System.Collections.Generic;
using Godot;
using ProjectArchaetech.common.components;
using ProjectArchaetech.common.util;
using ProjectArchaetech.events;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resource;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public partial class Building : Node2D, IRecyclable {
		[Export]
		public BuildingData Data { set; get; }

		protected readonly Godot.Collections.Dictionary<string, Variant> updatedUIData;
		protected readonly List<IFunctionable> functionalities;
		protected static readonly BuildingProcessedEvent buildingProcessedEvent = new BuildingProcessedEvent();

		[Signal]
		public delegate void BuildingInfoUpdatedUIEventHandler(Building building, Godot.Collections.Dictionary<string, Variant> info);

		protected Building(params IFunctionable[] functionalities) {
			this.functionalities = new List<IFunctionable>();
			foreach (IFunctionable f in functionalities) {
				this.AddFunctionality(f);
			}
			this.updatedUIData = new Godot.Collections.Dictionary<string, Variant>();
		}

		public override void _Ready() {
			this.GetNode<Area2D>("Area2D").InputEvent += this.OnClick;
			this.updatedUIData["name"] = this.Data.Name;
			this.updatedUIData["icon"] = this.Data.Icon;

			Global.EventBus.Subscribe<ProductionStartedEvent>((sender, e) => this.Work());
		}

		public void AddFunctionality(IFunctionable f) {
			this.functionalities.Add(f);
			f.BuildingUIDataUpdatedEvent += (key, value) => this.updatedUIData[key] = value;
		}

		public bool CanBeBuilt(TileData tile, Cell location) {
			if (location.Building != null && location.Building is not BaseBuilding) {
				return false;
			}
			if (this.Data.RequiredTerrains.Count != 0 && !this.Data.RequiredTerrains.ContainsKey((Resource) tile.GetCustomData("terrain"))) {
				return false;
			}
			Godot.Collections.Dictionary<ResourceData, int> cost = this.Data.Cost;
			foreach (ResourceData res in cost.Keys) {
				if (!Global.ResManager.HasEnough(res, cost[res])) {
					return false;
				}
			}
			return true;
		}

		protected virtual void Work() {
			// Clear old UI data.
			this.updatedUIData.Clear();
			foreach (IFunctionable f in this.functionalities) {
				f.Execute();
			}
			this.UpdateUI();
			Global.EventBus.Publish(this, buildingProcessedEvent);
		}

		public Godot.Collections.Dictionary<ResourceData, double> GetOutput() {
			return this.GetType() == typeof(ProductiveBuilding) ? ((ProductiveBuilding)this).Warehouse.Resources : new Godot.Collections.Dictionary<ResourceData, double>();
		}

		protected virtual void OnClick(Node viewport, InputEvent e, long shapeIdx) {
			if (e.IsActionPressed("left_click")) {
				Global global = this.GetNode<Global>("/root/Global");
				switch (Global.GameState) {
					case Global.GameMode.BuildRoute:
						int fIdx = this.functionalities.FindIndex(f => f.GetType() == typeof(TransportFunctionality));
						if (fIdx >= 0) {
							TransportFunctionality network = (TransportFunctionality) this.functionalities[fIdx];
							network.Add(new TransportRoute((Building) Global.PickUp, this, new TransportRouteSpecification()));
						} else {
							GD.PushWarning("Building " + this + " does not support transport routes!");
						}
						break;
					default:
						Global.PickUp = this;
						global.EmitSignal(Global.SignalName.OpeningModalUI, "building", this.updatedUIData);
						break;
				}
			}
		}

		protected virtual void UpdateUI() {
			this.EmitSignal(SignalName.BuildingInfoUpdatedUI, this, this.updatedUIData);
		}

		public virtual void Disable() {
			Global.Grave.Enqueue(this);
		}

		public override string ToString() {
			return this.Data.Name;
		}
	}
}
