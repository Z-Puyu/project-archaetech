using System;
using System.Runtime.InteropServices;
using C5;
using Godot;
using ProjectArchaetech.common;

namespace ProjectArchaetech {
	public partial class Events : Node {
		private HashDictionary<Type, HashSet<EventHandler>> subscribers;

		public void Subscribe<T>(EventHandler eventHandler) where T : EventArgs {
			// First, we retrieve the event type.
			Type eventType = typeof(T);
			if (!this.subscribers.Contains(eventType)) {
				// There is currently no listeners.
				this.subscribers.Add(eventType, new HashSet<EventHandler>());
			}
			// Register the handler with the event.
			this.subscribers[eventType].Add(eventHandler);
		}

		public void Unsubscribe<T>(EventHandler eventHandler) where T : EventArgs {
			Type eventType = typeof(T);
			if (this.subscribers.Find(ref eventType, out HashSet<EventHandler> listeners)) {
				listeners.Remove(eventHandler);
			}
		}

		public void Publish(object sender, EventArgs e) {
			Type eventType = e.GetType();
			if (this.subscribers.Find(ref eventType, out HashSet<EventHandler> listeners)) {
				foreach (EventHandler handler in listeners) {
					handler(sender, e);
				}
			}
		}

		// Map-related
		[Signal]
		public delegate void CellSelectedEventHandler(Cell cell, TileData data);

		// Building-related
		public delegate void AddingRouteEventHandler(LogisticBuilding to);
		public event AddingRouteEventHandler AddingRoute;

		[Signal]
		public delegate void DisplayingBuildingUIEventHandler(Building building);
		[Signal]
		public delegate void AddingRouteUIEventHandler(Node route);

		// General UI
		[Signal]
		public delegate void ModalToggledEventHandler(string windowName);
	}
}
