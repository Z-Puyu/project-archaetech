using System;
using Godot.Collections;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common.components {
    public class ProductionFunctionality : IFunctionable {
        const int NOVICE_BASE_XP = 5;
        private readonly Warehouse warehouse;
        private readonly Dictionary<JobData, Dictionary<int, Array<Pop>>> workers;
        private readonly Random rand;

        public ProductionFunctionality(Warehouse warehouse, Dictionary<JobData, Dictionary<int, Array<Pop>>> workers) {
            this.warehouse = warehouse;
            this.workers = workers;
            this.rand = new Random(Guid.NewGuid().GetHashCode());
        }

        public event IFunctionable.ObjectUIDataUpdatedEventHandler ObjectUIDataUpdatedEvent;

        public void Execute() {
            // Erase last month's product records.
            this.warehouse.Reset();
			// Process each job in the building sequentially
			foreach (JobData job in this.workers.Keys) {
                for (Competency competency = Competency.Novice; competency <= Competency.Expert; 
                    competency += 1) {
                    this.warehouse.Supply(job, this.workers[job][(int) competency].Count, 
                        competency);
                }
			}
            this.CollectXp();
            this.ObjectUIDataUpdatedEvent.Invoke("local_storage", this.warehouse.Resources);
            this.ObjectUIDataUpdatedEvent.Invoke("output", this.warehouse.MonthlyOutput);
        }

        private void CollectXp() {
            foreach (JobData job in this.workers.Keys) {
                foreach (Pop pop in this.workers[job][(int) Competency.Novice]) {
                    // Easy to upgrade from Novice to Regular
                    pop.GainXp(job, rand.NextDouble() * NOVICE_BASE_XP);
                }
                foreach (Pop pop in this.workers[job][(int) Competency.Regular]) {
                    // Hard to become expert
                    pop.GainXp(job, rand.NextDouble() * rand.Next(0, 3));
                }
            }
        }
    }
}