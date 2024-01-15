using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

namespace ProjectArchaetech.resources {
    [RegisteredType(nameof(Tech), "", nameof(Resource)), GlobalClass]
    public partial class ProductionMethod : Resource {
        [Export]
        public string Name { set; get; }
        [Export]
        public Dictionary<JobData, int> Recipe { set; get; }

        public ProductionMethod() {
            this.Name = "New Production Method";
            this.Recipe = new Dictionary<JobData, int>();
        }

        public override string ToString() {
            string str = "- " + this.Name + "\n";
            foreach (JobData job in this.Recipe.Keys) {
                str += (" └ " + job.ToString() + ": " + this.Recipe[job] + "\n");
            }
            return str;
        }
    }
}