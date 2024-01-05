using System;
using Godot;
using Godot.Collections;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
    public partial class TransportRoute : Node2D, IManable, IFunctionable {
        private LogisticBuilding from;
        private LogisticBuilding to;
        private int level;
        private int manpower;
        private TransportRouteSpecification spec;
        private Dictionary<ResourceData, double> resources;

        public Dictionary<ResourceData, double> Resources { get => resources; set => resources = value; }
        public LogisticBuilding From { get => from; set => from = value; }
        public LogisticBuilding To { get => to; set => to = value; }
        public int Level { get => level; set => level = value; }
        public int Manpower { get => manpower; set => manpower = value; }
        public TransportRouteSpecification Spec { get => spec; set => spec = value; }
        

        public TransportRoute(LogisticBuilding from, LogisticBuilding to, 
            TransportRouteSpecification spec) {
            this.from = from;
            this.to = to;
            this.spec = spec;
            this.level = 1;
            this.manpower = 0;
            this.resources = new Dictionary<ResourceData, double>();
        }

        public void Recruit(JobData job = null) {
            int shortage = this.Level - this.Manpower;
            int nRecruited = Math.Min(shortage, Global.PopManager.NUnemployed);
            if (nRecruited == 0) {
                this.Level = Math.Max(this.Level - 1, this.manpower);
            } else {
                this.manpower += nRecruited;
                Global.PopManager.NUnemployed -= nRecruited;
            }
            // More features in the future.
            foreach (ResourceData res in this.Resources.Keys) {
                this.Resources[res] = this.Spec.capacity[this.Manpower];
            }
        }

        public void Work() {
            double maintenanceCost = this.Spec.maintenanceCost[this.Manpower] * this.Manpower;
            if (Global.ResManager.HasEnough(this.Spec.maintenanceType, maintenanceCost)) {
                Global.ResManager.Consume(this.Spec.maintenanceType, maintenanceCost);
                this.to.Store(this.from.Take(Resources));
            }
            if (this.Manpower < this.level) {
                this.Recruit();
            }
        }
    }
}