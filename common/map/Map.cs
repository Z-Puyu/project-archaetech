using System;
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

		private enum Terrain {
			Plains,
			Ocean,
			Forest,
			Desert,
			Mountains
		}

		[Export]
		private Dictionary<Vector2I, Cell> grid;
		private readonly HashDictionary<Vector2I, Cell> navigableLand;
		private readonly HashDictionary<Vector2I, Cell> navigableWater;
		private Cell currSelection;

		public Cell CurrSelection { get => currSelection; set => currSelection = value; }

		class CellSelectedEvent : EventArgs {
			private readonly TileData data;

			public CellSelectedEvent(TileData data) {
				this.data = data;
			}

			public TileData Data => data;
		}

		public Map() {
			this.grid = new Dictionary<Vector2I, Cell>();
			this.navigableLand = new HashDictionary<Vector2I, Cell>();
			this.navigableWater = new HashDictionary<Vector2I, Cell>();
		}

		public override void _Ready() {
			Array<Vector2I> landCells = this.GetUsedCells((int) Layer.Land);
			foreach (Vector2I pt in landCells) {
				Cell cell = new Cell(pt, this.MapToLocal(pt));
				if (!this.grid.ContainsKey(pt)) {
					this.grid.Add(pt, cell);
				}
				this.navigableLand.Add(pt, cell);
			}
			Array<Vector2I> waterCells = this.GetUsedCells((int) Layer.Water);
			foreach (Vector2I pt in waterCells) {
				Cell cell = new Cell(pt, this.MapToLocal(pt));
				if (!this.grid.ContainsKey(pt)) {
					this.grid.Add(pt, cell);
				}
				this.navigableWater.Add(pt, cell);
			}
			Global.EventBus.Subscribe<CellSelectedEvent>(
				(sender, e) => ((Map) sender)
					.GetNode<Global>("/root/Global")
					.EmitSignal(Global.SignalName.CellSelected, ((CellSelectedEvent) e).Data)
			);
		}

		public void EmitCellEvent(TileData tileData) {
			
		}

		public override void _UnhandledInput(InputEvent e) {
			if (e is InputEventMouse) {
				if (e.IsActionPressed("left_click")) {
					this.SelectCell(e);
				}
			}
		}

		private void SelectCell(InputEvent e) {
			Vector2 mousePos = ((InputEventMouse) this.MakeInputLocal(e)).Position;
			Vector2I mapCoords = this.LocalToMap(mousePos);
			if (this.grid.ContainsKey(mapCoords)) {
				this.CurrSelection = this.grid[mapCoords];
				TileData tileData = this.GetCellTileData((int) Layer.Land, mapCoords)
					?? this.GetCellTileData((int) Layer.Water, mapCoords);
				if (tileData != null) {
					this.ClearLayer((int) Layer.UI);
					this.SetCell((int) Layer.UI, this.CurrSelection.Pos, 
						(int) Atlas.Cells, new Vector2I(5, 0));
					Global.EventBus.Publish(this, new CellSelectedEvent(tileData));
				}
			}
		}
	}
}
