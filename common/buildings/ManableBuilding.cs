using System;
using Godot;
using Godot.Collections;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resources;
using ProjectArchaetech.util;
using static ProjectArchaetech.events.EventBus;

namespace ProjectArchaetech.common {
    [GlobalClass]
    // Encapsulates a building which can recruit workers.
    public abstract partial class ManableBuilding : Building, IManable, IFunctionable {
        private readonly Dictionary<JobData, int> employmentCap;
        private readonly Dictionary<JobData, int> workers;
        protected Warehouse warehouse;

        protected Dictionary<JobData, int> EmploymentCap => employmentCap;

        protected Dictionary<JobData, int> Workers => workers;

        public ManableBuilding() {
            this.employmentCap = new Dictionary<JobData, int>();
            this.workers = new Dictionary<JobData, int>();
            this.warehouse = new Warehouse();
        }

        public override void _Ready() {
            base._Ready();
            if (this is not BaseBuilding) {
                Global.EventBus.Subscribe<ProcessingBuildingsEvent>((sender, e) => this.Work());
            }   
        }

        public Pair GetEmploymentData() {
            return new Pair(this.Workers, this.EmploymentCap);
        }

        public Dictionary<ResourceData, double> GetOutput() {
            return this.warehouse.MonthlyOutput;
        }

        public void Store(Dictionary<ResourceData, double> resources) {
            this.warehouse.Add(resources);
        }

        public Dictionary<ResourceData, double> Take(Dictionary<ResourceData, double> resources) {
            return this.warehouse.TakeAway(resources);
        }

        public void Recruit(JobData job) {
            if (!this.Workers.ContainsKey(job)) {
                this.Workers[job] = 0;
            } else if (this.Workers[job] < this.EmploymentCap[job]) {
                int shortage = this.EmploymentCap[job] - this.Workers[job];
                int nRecruited = Math.Min(shortage, Global.PopManager.NUnemployed);
                this.Workers[job] += nRecruited;
                Global.PopManager.NUnemployed -= nRecruited;
            }
        }

        public virtual void Work() {
            foreach (JobData job in this.EmploymentCap.Keys) {
                this.Recruit(job);
            }
        }

        public override void Disable(){
            Global.EventBus.Unsubscribe<NewMonthEvent>((sender, e) => this.Work());
            base.Disable();
        }
    }
}