using ProjectArchaetech.events.@abstract;

namespace ProjectArchaetech.util.events {
    public class TechUnlockedEvent :GameEvent {
        public TechUnlockedEvent(Tech tech) : base(
            $"Discovered {tech.name}!", 
            $"We have discovered {tech.name}!"
        ) { }
    }
}