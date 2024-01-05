using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.common.util;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resource;

namespace ProjectArchaetech.common {
    [GlobalClass]
    public partial class LogisticBuilding : ManableBuilding, IFunctionable, IRecyclable {
        [Export]
        public static Array<TransportRouteSpecification> specs { set; get; }
        protected HashDictionary<LogisticBuilding, TransportRoute> inPaths;
        protected HashDictionary<LogisticBuilding, TransportRoute> outPaths;

        public LogisticBuilding() : base() {
            this.inPaths = new HashDictionary<LogisticBuilding, TransportRoute>();
            this.outPaths = new HashDictionary<LogisticBuilding, TransportRoute>();
        }

        public virtual void Work() {
            foreach (TransportRoute route in this.outPaths.Values) {
                route.Work();
            }
        }

        public void LinkTo(LogisticBuilding to) {
            TransportRoute route = new TransportRoute(this, to, specs[0]);
            this.outPaths.Add(to, route);
            to.inPaths.Add(this, route);
        }

        protected override void OnClick(Node viewport, InputEvent e, long shapeIdx) {
            if (e.IsActionPressed("left_click")) {
                if (Global.GameState == Global.GameMode.BuildRoute) {
                    if (Global.PickUp is LogisticBuilding && !object.ReferenceEquals(Global.PickUp, this)) {
                        ((LogisticBuilding) Global.PickUp).LinkTo(this);
                    } else {
                        GD.PushError("No valid source found!");
                    }
                } else {
                    Global.EventBus.EmitSignal(Events.SignalName.DisplayingBuildingUI, this);
                    Global.EventBus.EmitSignal(Events.SignalName.ModalToggled, "building");
                }
            }
        }

        public void Disable() {
            foreach (LogisticBuilding building in this.inPaths.Keys) {
                building.outPaths.Remove(this);
            }
            foreach (LogisticBuilding building in this.outPaths.Keys) {
                building.inPaths.Remove(this);
            }
            this.inPaths.Clear();
            this.outPaths.Clear();
            Global.Grave.Enqueue(this);
        }
    }
}