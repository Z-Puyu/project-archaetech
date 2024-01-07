using System;
using System.Collections.Generic;
using Godot;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public partial class Cell : Node {
		private Vector2I pos;
		private Vector2 localCoords;
		private List<Node> units;
		private Building building;

		public Cell(Vector2I pos, Vector2 localCoords)
		{
			this.Pos = pos; 
			this.LocalCoords = localCoords;
			this.Units = new List<Node>();
		}

		public Vector2I Pos { get => pos; set => pos = value; }
		public Vector2 LocalCoords { get => localCoords; set => localCoords = value; }
		public List<Node> Units { get => units; set => units = value; }
		public Building Building { get => building; set => building = value; }

		public override string ToString() {
			return String.Format(
				"Cell %s at %s with %d units and building %s", 
				this.Pos.ToString(), this.LocalCoords.ToString(), 
				this.Units.Count, this.Building.ToString()
			);
		}
	}
}
