using System;
using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public partial class ProductiveBuilding : LogisticBuilding, IFunctionable {
		private ProductionMethod activePM;

		public override void _Ready() {
			base._Ready();
			this.activePM = this.data.ProductionMethods[0];
			Dictionary<JobData, int> jobs = this.activePM.recipe;
			foreach (JobData job in jobs.Keys) {
				this.EmploymentCap[job] = jobs[job];
			}
		}

		public override void Work() {
			this.warehouse.Reset();
			// Process each job in the building sequentially
			foreach (JobData job in this.Workers.Keys) {
				this.warehouse.Supply(job, this.Workers[job]);
			}
			base.Work();
		}
	}
}
