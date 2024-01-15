using ProjectArchaetech.events.@abstract;

namespace ProjectArchaetech.events {
    public class TechUnlockedEvent : @abstract.GameEvent {
        public TechUnlockedEvent(Tech tech) : base(
            $"Discovered {tech.name}!", 
            $"We have discovered {tech.name}!"
        ) { }
    }
}