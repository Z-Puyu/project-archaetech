using Godot;

namespace ProjectArchaetech.interfaces {
    public interface IFunctionable {
        public delegate void ObjectUIDataUpdatedEventHandler(string key, Variant value);
        public event ObjectUIDataUpdatedEventHandler ObjectUIDataUpdatedEvent;
        public abstract void Execute();
    }
}