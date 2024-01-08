using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.common.components;
using ProjectArchaetech.resources;
using ProjectArchaetech.util;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public partial class ProductiveBuilding : Building {
		private Warehouse warehouse;
		private ProductionMethod activePM;

		public Warehouse Warehouse { get => warehouse; set => warehouse = value; }

		public override void _Ready() {
			base._Ready();

			this.warehouse = this.GetNode<Warehouse>("Warehouse");
			this.activePM = this.Data.ProductionMethods[0];
			Dictionary<JobData, int> workers = new Dictionary<JobData, int>();
			Dictionary<JobData, int> employmentCap = this.activePM.Recipe.Duplicate(true);
			foreach (JobData job in this.activePM.Recipe.Keys) {
				workers[job] = 0;
			}

			this.AddFunctionality(new ProductionFunctionality(this.warehouse, workers));
			this.AddFunctionality(new RecruitmentFunctionality(employmentCap, workers));
			this.AddFunctionality(new TransportFunctionality(
				new HashDictionary<Building, TransportRoute>(), 1
			));
			
			this.updatedUIData["employment"] = new Pair(workers.Duplicate(true), employmentCap.Duplicate(true));
			this.updatedUIData["output"] = new Dictionary<ResourceData, double>();
			this.updatedUIData["local_storage"] = new Dictionary<ResourceData, double>();
		}

		public void Store(Dictionary<ResourceData, double> resources) {
			this.warehouse.Add(resources);
		}

		public Dictionary<ResourceData, double> Take(Dictionary<ResourceData, double> resources) {
			return this.warehouse.TakeAway(resources);
		}
	}
}
