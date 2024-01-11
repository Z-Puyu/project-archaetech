using System.Runtime.CompilerServices;

namespace ProjectArchaetech.events {
    public interface IEvent {
        public abstract void Fire();

        // public abstract void CoolDown();
    }
}