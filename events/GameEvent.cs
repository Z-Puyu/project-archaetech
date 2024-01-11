using System.Collections.Generic;

namespace ProjectArchaetech.events.types {
    public class GameEvent : IEvent {
        private readonly EventData data;
        private readonly List<Option> options;

        public void Fire() {
            throw new System.NotImplementedException();
        }
    }
}