using Godot;
using Godot.Collections;

namespace ProjectArchaetech.interfaces {
    public interface IFunctionable {
        public abstract void Execute(Dictionary<string, Variant> updatedUIData);
    }
}