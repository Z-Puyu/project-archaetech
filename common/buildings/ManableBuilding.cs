using System;
using C5;
using Godot.Collections;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
    // Encapsulates a building which can recruit workers.
    public abstract partial class ManableBuilding : Building, IManable {
        protected HashDictionary<JobData, int> employmentCap;
        protected HashDictionary<JobData, int> workers;
        protected Warehouse warehouse;

        public ManableBuilding() {
            this.employmentCap = new HashDictionary<JobData, int>();
            this.workers = new HashDictionary<JobData, int>();
            this.warehouse = new Warehouse();
        }

        public void Store(Dictionary<ResourceData, double> resources) {
            this.warehouse.Add(resources);
        }

        public Dictionary<ResourceData, double> Take(Dictionary<ResourceData, double> resources) {
            return this.warehouse.TakeAway(resources);
        }

        public void Recruit(JobData job) {
            int shortage = this.employmentCap[job] - this.workers[job];
            int nRecruited = Math.Min(shortage, Global.PopManager.NUnemployed);
            this.workers[job] += nRecruited;
            Global.PopManager.NUnemployed -= nRecruited;
        }
    }
}