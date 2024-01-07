using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.common.util;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resource;

namespace ProjectArchaetech.common {
    [GlobalClass]
    public partial class LogisticBuilding : ManableBuilding {
        [Export]
        public Array<TransportRouteSpecification> Specs { set; get; }
        protected HashDictionary<LogisticBuilding, TransportRoute> inPaths;
        protected HashDictionary<LogisticBuilding, TransportRoute> outPaths;

        public LogisticBuilding() : base() {
            this.inPaths = new HashDictionary<LogisticBuilding, TransportRoute>();
            this.outPaths = new HashDictionary<LogisticBuilding, TransportRoute>();
        }

        public override void Work() {
            foreach (TransportRoute route in this.outPaths.Values) {
                route.Work();
            }
            base.Work();
        }

        public void LinkTo(LogisticBuilding to) {
            TransportRoute route = new TransportRoute(this, to, this.Specs[0]);
            this.outPaths.Add(to, route);
            to.inPaths.Add(this, route);
            this.GetNode<Global>("/root/Global").EmitSignal(Global.SignalName.TransportRouteAdded, route);
        }

        public Array<TransportRoute> GetOutwardRoutes() {
            return new Array<TransportRoute>(this.outPaths.Values.ToArray());
        }

        protected override void OnClick(Node viewport, InputEvent e, long shapeIdx) {
            if (e.IsActionPressed("left_click")) {
                if (Global.GameState == Global.GameMode.BuildRoute) {
                    if (Global.PickUp is not LogisticBuilding) {
                        GD.PushError("The source building does not support transport routes!" +
                            "(You should not be seeing this normally)");
                        return;
                    }
                    if (!object.ReferenceEquals(Global.PickUp, this)) {
                        ((LogisticBuilding) Global.PickUp).LinkTo(this);
                        Global.GameState = Global.GameMode.Normal;
                    } else {
                        GD.PushError("No valid source found!");
                    }
                } else {
                    base.OnClick(viewport, e, shapeIdx);
                }
            }
        }

        public override void Disable() {
            foreach (LogisticBuilding building in this.inPaths.Keys) {
                building.outPaths.Remove(this);
            }
            foreach (LogisticBuilding building in this.outPaths.Keys) {
                building.inPaths.Remove(this);
            }
            this.inPaths.Clear();
            this.outPaths.Clear();
            base.Disable();
        }

        public void CloseRoute(TransportRoute route) {
            this.inPaths.Remove(route.From);
            this.outPaths.Remove(route.To);
            Global.Grave.Enqueue(route);
        }
    }
}