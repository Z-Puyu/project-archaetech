using System;
using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.events {
    [RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
    public partial class XorCondition : Resource {
        [Export]
        public Dictionary<string, Variant> Conditions { set; get; }

        public XorCondition() {
            this.Conditions = new Dictionary<string, Variant>();
        }

        public bool IsTrue() {
            int count = 0;
            foreach (string name in this.Conditions.Keys) {
                Variant args = this.Conditions[name];
                if ((bool) Type.GetType("Triggers").GetMethod(name).Invoke(null, [args])) {
                    count += 1;
                };
            }
            return count == 1;
        }
    }
}