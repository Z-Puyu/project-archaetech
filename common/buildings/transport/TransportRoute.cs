using System;
using Godot;
using Godot.Collections;
using ProjectArchaetech.common.util;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resource;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public partial class TransportRoute : Node2D, IRecyclable {
		private Building from;
		private Building to;
		private int level;
		private int manpower;
		private TransportRouteSpecification spec;
		private Dictionary<ResourceData, double> resources;

		public Dictionary<ResourceData, double> Resources { get => resources; set => resources = value; }
		public Building From { get => from; set => from = value; }
		public Building To { get => to; set => to = value; }
		public int Level { get => level; set => level = value; }
		public int Manpower { get => manpower; set => manpower = value; }
		public TransportRouteSpecification Spec { get => spec; set => spec = value; }

		public TransportRoute(Building from, Building to, 
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
				this.Resources[res] = this.Spec.Capacity[this.Manpower];
			}
		}

		public void Work() {
			// For debug purposes only!!!
			if (this.From.GetType() == typeof(Building) || this.To.GetType() == typeof(Building)) {
				return;
			}
			foreach (ResourceData res in this.from.GetOutput().Keys) {
				this.resources[res] = this.spec.Capacity[this.level];
			}
			double maintenanceCost = this.Spec.MaintenanceCost[this.Manpower] * this.Manpower;
			if (Global.ResManager.HasEnough(this.Spec.MaintenanceType, maintenanceCost)) {
				Global.ResManager.Consume(this.Spec.MaintenanceType, maintenanceCost);
				((ProductiveBuilding) this.to).Store(((ProductiveBuilding) this.From).Take(this.Resources));
			}
			if (this.Manpower < this.level) {
				this.Recruit();
			}
		}

		public void Disable() {
			this.level = 0;
			this.QueueFree();
			// this.From.CloseRoute(this);
		}
	}
}
