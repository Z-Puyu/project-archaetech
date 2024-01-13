using System;

namespace ProjectArchaetech.common {
    public static class ScriptedEffects {
        public class TestEffect : Effect<int> {

            public TestEffect(string name, Action<int> action) : base(name, action) { }

            public override void Fire(in int arg) {
                this.action.Invoke(arg);
            }

            public new static string ToString(in int arg) {
                return "Test" + " with arg: " + arg;
            }
        }
    }
}