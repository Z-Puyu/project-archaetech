using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.resources {
    [RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
    public partial class ProductionMethod : Resource {
        [Export]
        public string name { set; get; }
        [Export]
        public Dictionary<JobData, int> recipe { set; get; }

        public ProductionMethod() {
            this.name = "New Production Method";
            this.recipe = new Dictionary<JobData, int>();
        }

        public override string ToString() {
            string str = "- " + this.name + "\n";
            foreach (JobData job in this.recipe.Keys) {
                str += (" └ " + job.ToString() + ": " + this.recipe[job] + "\n");
            }
            return str;
        }
    }
}