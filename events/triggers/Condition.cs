using Godot;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.events {
    [RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
    public partial class Condition : Resource {
        [Export]
        public OrCondition OR { set; get; }
        [Export]
        public XorCondition XOR { set; get; }
        [Export]
        public NotCondition NOT { set; get; }

        public Condition() {
            this.OR = new OrCondition();
            this.XOR = new XorCondition();
            this.NOT = new NotCondition();
        }

        public bool IsTrue() {
            return this.OR.IsTrue() && this.XOR.IsTrue() && this.NOT.IsTrue();
        }
    }
}