using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.common.util;
using ProjectArchaetech.events;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public partial class Building : Node2D, IRecyclable {
		[Export]
		public BuildingData Data { set; get; }

		protected readonly Dictionary<string, Variant> updatedUIData;
		private readonly LinkedList<IFunctionable> functionalities;
		private static readonly BuildingProcessedEvent buildingProcessedEvent = new BuildingProcessedEvent();

		[Signal]
		public delegate void BuildingInfoUpdatedUIEventHandler(Building building);

		protected Building(params IFunctionable[] functionalities) {
			this.functionalities = new LinkedList<IFunctionable>();
			foreach (IFunctionable f in functionalities) {
				this.functionalities.Enqueue(f);
			}
			this.updatedUIData = new Dictionary<string, Variant>();
		}

		public override void _Ready() {
			this.GetNode<Area2D>("Area2D").InputEvent += this.OnClick;
			this.updatedUIData["name"] = this.Data.Name;
			this.updatedUIData["icon"] = this.Data.Icon;
			if (this is not BaseBuilding) {
				Global.EventBus.Subscribe<ProductionStartedEvent>((sender, e) => this.Work());
			}
		}

		public void AddFunctionality(IFunctionable f) {
			this.functionalities.Enqueue(f);
		}

		public bool CanBeBuilt(TileData tile, Cell location) {
			if (location.Building != null && location.Building is not BaseBuilding) {
				return false;
			}
			if (this.Data.RequiredTerrains.Count != 0 && !this.Data.RequiredTerrains.ContainsKey((Resource) tile.GetCustomData("terrain"))) {
				return false;
			}
			Dictionary<ResourceData, int> cost = this.Data.Cost;
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
				f.Execute(updatedUIData);
			}
			this.UpdateUI();
			Global.EventBus.Publish(this, buildingProcessedEvent);
		}

		public Dictionary<ResourceData, double> GetOutput() {
			return this.GetType() == typeof(ProductiveBuilding) ? ((ProductiveBuilding)this).Warehouse.Resources : new Dictionary<ResourceData, double>();
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

		protected void UpdateUI() {
			this.EmitSignal(SignalName.BuildingInfoUpdatedUI, this.updatedUIData);
		}

		public virtual void Disable() {
			Global.Grave.Enqueue(this);
		}

		public override string ToString() {
			return this.Data.Name;
		}
	}
}
