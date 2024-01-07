using System;
using C5;
using Godot;

namespace ProjectArchaetech.events {
	[GlobalClass]
	public partial class EventBus : Node {
		private readonly HashDictionary<Type, EventHandler> subscribers;

		public class NewMonthEvent : EventArgs { }
		public class TechProgressEvent : EventArgs {
			private readonly int availablePoints;

			public int AvailablePoints => availablePoints;

			public TechProgressEvent(int availablePoints) {
				this.availablePoints = availablePoints;
			}
		}
		public class ProcessingBuildingsEvent : EventArgs { }

		public EventBus() {
			this.subscribers = new HashDictionary<Type, EventHandler>();
		}

		public override void _Ready() {
			base._Ready();
		}

		public void Subscribe<T>(EventHandler eventHandler) where T : EventArgs {
			// First, we retrieve the event type.
			Type eventType = typeof(T);
			if (!this.subscribers.Contains(eventType)) {
				// There is currently no listeners.
				this.subscribers.Add(eventType, eventHandler);
			} else {
				// Register the handler with the event.
				this.subscribers[eventType] += eventHandler;
			}
		}

		public void Unsubscribe<T>(EventHandler eventHandler) where T : EventArgs {
			Type eventType = typeof(T);
			if (this.subscribers.Contains(eventType)) {
				this.subscribers[eventType] -= eventHandler;
			}
		}

		public void Publish(object sender, EventArgs e) {
			Type eventType = e.GetType();
			if (this.subscribers.Find(ref eventType, out EventHandler listeners)) {
				listeners(sender, e);
			}
		}
	}
}
