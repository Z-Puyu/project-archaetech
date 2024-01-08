using Godot;

namespace ProjectArchaetech.interfaces {
    public interface IFunctionable {
        public delegate void BuildingUIDataUpdatedEventHandler(string key, Variant value);
        public event BuildingUIDataUpdatedEventHandler BuildingUIDataUpdatedEvent;
        public abstract void Execute();
    }
}