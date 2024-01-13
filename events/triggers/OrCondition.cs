using System;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.events {
    [RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
    public partial class OrCondition : Resource {
        [Export]
        public Dictionary<string, Variant> Conditions { set; get; }

        public OrCondition() {
            this.Conditions = new Dictionary<string, Variant>();
        }

        public bool IsTrue() {
            foreach (string name in this.Conditions.Keys) {
                Variant args = this.Conditions[name];
                if (!((bool) Type.GetType("Triggers").GetMethod(name).Invoke(null, [args]))) {
                    return false;
                };
            }
            return true;
        }
    }
}