using System;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.events {
    [RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
    public partial class NotCondition : Resource {
        [Export]
        public Dictionary<string, Variant> Conditions { set; get; }

        public NotCondition() {
            this.Conditions = new Dictionary<string, Variant>();
        }

        public bool IsTrue() {
            foreach (string name in this.Conditions.Keys) {
                Variant args = this.Conditions[name];
                if ((bool) Type.GetType("Triggers").GetMethod(name).Invoke(null, [args])) {
                    return false;
                };
            }
            return true;
        }
    }
}