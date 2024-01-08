using Godot;
using Godot.Collections;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common.components {
    public class ProductionFunctionality : IFunctionable {
        private readonly Warehouse warehouse;
        private readonly Dictionary<JobData, int> workers;

        public ProductionFunctionality(Warehouse warehouse, Dictionary<JobData, int> workers) {
            this.warehouse = warehouse;
            this.workers = workers;
        }

        public void Execute(Dictionary<string, Variant> updatedUIData) {
            // Erase last month's product records.
            this.warehouse.Reset();
			// Process each job in the building sequentially
			foreach (JobData job in this.workers.Keys) {
				this.warehouse.Supply(job, this.workers[job]);
			}
            updatedUIData["output"] = this.warehouse.MonthlyOutput;
            updatedUIData["local_storage"] = this.warehouse.Resources;
        }
    }
}