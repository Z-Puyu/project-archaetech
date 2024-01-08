using System;
using Godot;
using Godot.Collections;
using ProjectArchaetech.common;
using ProjectArchaetech.common.util;
using ProjectArchaetech.events;
using static ProjectArchaetech.events.EventBus;

namespace ProjectArchaetech {
	[GlobalClass]
	public partial class BuildingManager : Node {
		[Export]
		public Dictionary<string, PackedScene> AvailableBuildings { set; get; }

		private string spawningObjId;
		private ConstructionQueue<Building> constructionQueue;
		private Cell selectedCell;
		private TileData tileData;
		private ResourceManager resManager;
		private static int nBuildings = 0;
		private static int nBuildingsProcessed = 0;
		private static readonly ProductionEndedEvent productionEndedEvent = new ProductionEndedEvent();

		public string SpawningObjId { get => spawningObjId; set => spawningObjId = value; }
		public ConstructionQueue<Building> ConstructionQueue { get => constructionQueue; set => constructionQueue = value; }
		public Cell SelectedCell { get => selectedCell; set => selectedCell = value; }
		public TileData TileData { get => tileData; set => tileData = value; }

		public BuildingManager() {
			this.AvailableBuildings = new Dictionary<string, PackedScene>();
		}

		public override void _Ready() {
			this.constructionQueue = new ConstructionQueue<Building>(2);
			this.AddChild(this.constructionQueue);
			Global.EventBus.Subscribe<ConstructionTaskCompletedEvent<Building>>(
				(sender, e) => nBuildings += 1
			);
			Global.EventBus.Subscribe<BuildingProcessedEvent>((sender, e) => {
				nBuildingsProcessed += 1;
				if (nBuildingsProcessed == nBuildings) {
					nBuildingsProcessed = 0;
					Global.EventBus.Publish(this, productionEndedEvent);
				}
			});
		}

		public void SetCell(CellSelectedEvent e) {
			this.SelectedCell = e.Cell;
			this.TileData = e.Data;
		}

		public void AddBuilding(string id) {
			this.SpawningObjId = id;
			Building obj = (Building) this.AvailableBuildings[this.SpawningObjId].Instantiate();
			if (obj.CanBeBuilt(this.TileData, this.SelectedCell)) {
				if (this.ConstructionQueue.RemoveAt(
					this.SelectedCell, out ConstructibleTask<Building> task
				)) {
					// There is a building under construction at the cell.
					this.resManager.Add(task.Value.Data.Cost);
					this.ConstructionQueue.Remove(task);
				}
				else if (this.SelectedCell.Building != null) {
					// No construction going on, so remove the existing one.
					DeleteBuilding(this.SelectedCell);
				}
				Global.ResManager.Consume(obj.Data.Cost);
				obj.Translate(this.SelectedCell.LocalCoords);
				obj.ZIndex = 1;
				ConstructibleTask<Building> newTask = new ConstructibleTask<Building>(
					obj, this.SelectedCell, obj.Data.TimeToBuild
				);
				newTask.Translate(this.SelectedCell.LocalCoords);
				this.ConstructionQueue.Enqueue(newTask);
			}
		}

		public static void DeleteBuilding(Cell location) {
			if (location.Building != null) {
				Global.Grave.Enqueue(location.Building);
				location.Building = null;
				nBuildings -= 1;
			}
		}
	}
}
