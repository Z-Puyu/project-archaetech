using System.Collections.Generic;
using Godot;

namespace ProjectArchaetech.events.types {
	[GlobalClass]
	public partial class GameEvent : GodotObject, IEvent {
		private readonly EventData data;

		public GameEvent(EventData data, List<Option> options) {
			this.data = data;
		}

		public EventData Data => data;

		public void Fire() {
			throw new System.NotImplementedException();
		}
	}
}
