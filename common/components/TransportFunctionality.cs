using System.Linq;
using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.events;
using ProjectArchaetech.interfaces;

namespace ProjectArchaetech.common.components {
	public class TransportFunctionality : IFunctionable {
		private readonly HashDictionary<Building, TransportRoute> outPaths;
		private readonly Array<TransportRoute> allRoutes;
		private int maxNOut;

		public TransportFunctionality(HashDictionary<Building, TransportRoute> outPaths, 
			int maxNOut) {
			this.outPaths = outPaths;
			TransportRoute[] allRoutes = this.outPaths.Values.ToArray();
			this.allRoutes = new Array<TransportRoute>(allRoutes);
			this.maxNOut = maxNOut;
			Global.EventBus.Subscribe<TransportRouteRemovedEvent>((sender, e) => {
				TransportRoute route = ((TransportRouteRemovedEvent) e).Route;
				if (this.allRoutes.Remove(route)) {
					this.outPaths.Remove(route.To);
				}
			});
		}

		public int MaxNOut { get => maxNOut; set => maxNOut = value; }

		public event IFunctionable.BuildingUIDataUpdatedEventHandler BuildingUIDataUpdatedEvent;

		public void Execute() {
			foreach (TransportRoute route in this.outPaths.Values) {
				route.Work();
			}
		}

		public void Add(TransportRoute route) {
			if (this.outPaths.Contains(route.To)) {
				// Transport network should be a simple graph :O
				GD.PushWarning("Cannot establish duplicated routes between two buildings!");
				return;
			}
			if (this.maxNOut <= this.outPaths.Count) {
				GD.PushWarning("Cannot support more transport routes. Please upgrade the building.");
				return;
			}
			this.outPaths.Add(route.To, route);
			this.allRoutes.Add(route);
			Global.EventBus.Publish(this, new TransportRouteAddedEvent(route));
		}
	}
}
