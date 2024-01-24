using C5;
using Godot;
using Godot.Collections;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public partial class Map : TileMap {
		private enum Layer {
			UI,
			MapObjects,
			Resources,
			Land,
			Water
		}

		private enum Atlas {
			Cells
		}

		private readonly Dictionary<Vector2I, Cell> grid;
		private readonly HashDictionary<Vector2I, Cell> navigableLand;
		private readonly HashDictionary<Vector2I, Cell> navigableWater;
		private Cell currSelection;

		public Cell CurrSelection { get => currSelection; set => currSelection = value; }

		public Dictionary<Vector2I, Cell> Grid => grid;

		public Map() {
			this.grid = new Dictionary<Vector2I, Cell>();
			this.navigableLand = new HashDictionary<Vector2I, Cell>();
			this.navigableWater = new HashDictionary<Vector2I, Cell>();
		}

		public override void _Ready() {
			Array<Vector2I> landCells = this.GetUsedCells((int) Layer.Land);
			foreach (Vector2I pt in landCells) {
				Cell cell = new Cell(pt, this.MapToLocal(pt));
				if (!this.Grid.ContainsKey(pt)) {
					this.Grid.Add(pt, cell);
				}
				this.navigableLand.Add(pt, cell);
			}
			Array<Vector2I> waterCells = this.GetUsedCells((int) Layer.Water);
			foreach (Vector2I pt in waterCells) {
				Cell cell = new Cell(pt, this.MapToLocal(pt));
				if (!this.Grid.ContainsKey(pt)) {
					this.Grid.Add(pt, cell);
				}
				this.navigableWater.Add(pt, cell);
			}
		}

		public override void _UnhandledInput(InputEvent e) {
			if (e is InputEventMouse) {
				if (e.IsActionPressed("left_click")) {
					if (Global.GameState != Global.GameMode.BuildRoute) {
						this.SelectCell(e);
					}
				}
			}
		}

		public TerrainData GetTerrain(Vector2I pos) {
			return (TerrainData) this.GetCellTileData((int) Layer.Land, pos)
				.GetCustomData("terrain");
		}

		public double GetDistance(Vector2I from, Vector2I to) {
			return this.MapToLocal(from).DistanceTo(this.MapToLocal(to));
		}

		private void SelectCell(InputEvent e) {
			Vector2 mousePos = ((InputEventMouse) this.MakeInputLocal(e)).Position;
			Vector2I mapCoords = this.LocalToMap(mousePos);
			if (this.Grid.ContainsKey(mapCoords)) {
				this.CurrSelection = this.Grid[mapCoords];
				TileData tileData = this.GetCellTileData((int) Layer.Land, mapCoords)
					?? this.GetCellTileData((int) Layer.Water, mapCoords);
				if (tileData != null) {
					this.ClearLayer((int) Layer.UI);
					this.SetCell((int) Layer.UI, this.CurrSelection.Pos, 
						(int) Atlas.Cells, new Vector2I(5, 0));
					Global.PickUp = this.CurrSelection;
					this.GetNode<Global>("/root/Global")
						.EmitSignal(Global.SignalName.CellSelected, this.CurrSelection, tileData);
				}
			}
		}
	}
}
