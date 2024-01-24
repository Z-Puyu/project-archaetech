using System.Collections.Generic;
using Godot;
using ProjectArchaetech.common.components;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resources;
using ProjectArchaetech.util.events;

namespace ProjectArchaetech.common {
    [GlobalClass]
	public abstract partial class Unit : Node2D, IRecyclable {
		[Export]
		public UnitData Data { set; get; }

		protected readonly Godot.Collections.Dictionary<string, Variant> updatedUIData;
		protected readonly MovementFunctionality movementFunctionality;
        protected bool isMoving;

		[Signal]
		public delegate void BuildingInfoUpdatedUIEventHandler(Building building, Godot.Collections.Dictionary<string, Variant> info);

		protected Unit() {
			this.movementFunctionality = new MovementFunctionality(
                cell => this.Translate(cell.LocalCoords),
                () => this.isMoving = false
            );
			this.updatedUIData = new Godot.Collections.Dictionary<string, Variant>();
		}

		public override void _Ready() {
			this.GetNode<Area2D>("Area2D").InputEvent += this.OnClick;
			this.updatedUIData["name"] = this.Data.Name;
			this.updatedUIData["icon"] = this.Data.Icon;
            Global.EventBus.Subscribe<NewDayEvent>((sender, e) => {
                if (this.isMoving) {
                    this.movementFunctionality.Execute();
                }
            });
		}

		protected virtual void OnClick(Node viewport, InputEvent e, long shapeIdx) {
			if (e.IsActionPressed("left_click")) {
				Global global = this.GetNode<Global>("/root/Global");
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