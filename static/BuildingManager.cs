using Godot;
using Godot.Collections;
using ProjectArchaetech.common;
using ProjectArchaetech.common.util;

namespace ProjectArchaetech {
	public partial class BuildingManager : Node {
		[Export]
		private Dictionary<string, PackedScene> availableBuildings { set; get; }

		private string spawningObjId;
		private ConstructionQueue<Building> constructionQueue;
		private Cell selectedCell;
		private TileData tileData;
		private ResourceManager resManager;

		public string SpawningObjId { get => spawningObjId; set => spawningObjId = value; }
		public ConstructionQueue<Building> ConstructionQueue { get => constructionQueue; set => constructionQueue = value; }
		public Cell SelectedCell { get => selectedCell; set => selectedCell = value; }
		public TileData TileData { get => tileData; set => tileData = value; }

		public override void _Ready() {
			this.constructionQueue = new ConstructionQueue<Building>(2);
		}

		public void SetCell(Cell cell, TileData data) {
			this.SelectedCell = cell;
			this.TileData = data;
		}

		public void AddBuilding(string id) {
			this.SpawningObjId = id;
			Building obj = (Building) this.availableBuildings[this.SpawningObjId].Instantiate();
			if (obj.CanBeBuilt(this.TileData, this.SelectedCell)) {
				if (this.ConstructionQueue.RemoveAt(
					this.SelectedCell, out ConstructibleTask<Building> task
				)) {
					// There is a building under construction at the cell.
					this.resManager.Add(task.Value.data.cost);
					this.ConstructionQueue.Remove(task);
				}
				else {
					// No construction going on, so remove the existing one.
					DeleteBuilding(this.SelectedCell);
				}
				this.resManager.Consume(obj.data.cost);
				obj.Translate(this.SelectedCell.LocalCoords);
				obj.ZIndex = 1;
				ConstructibleTask<Building> newTask = new ConstructibleTask<Building>(
					obj, this.SelectedCell, obj.data.timeToBuild
				);
				this.ConstructionQueue.Enqueue(newTask);
			}
		}

		public static void DeleteBuilding(Cell location) {
			if (location.Building != null) {
				Global.Grave.Enqueue(location.Building);
				location.Building = null;
			}
		}
	}
}
