using C5;
using Godot.Collections;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
    public partial class ProductiveBuilding : LogisticBuilding, IFunctionable {
        private ProductionMethod activePM;

        public override void _Ready() {
            base._Ready();
            this.activePM = this.data.productionMethods[0];
            Dictionary<JobData, int> jobs = this.activePM.recipe;
            foreach (JobData job in jobs.Keys) {
                this.employmentCap[job] = jobs[job];
            }
        }

        public override void Work() {
            // Process each job in the building sequentially
            foreach (KeyValuePair<JobData, int> job in this.employmentCap) {
                if (!this.workers.Contains(job.Key)) {
                    this.workers[job.Key] = 0;
                }
                this.warehouse.Supply(job.Key, this.workers[job.Key]);
                if (this.workers[job.Key] < job.Value) {
                    this.Recruit(job.Key);
                }
            }
            base.Work();
        }
    }
}