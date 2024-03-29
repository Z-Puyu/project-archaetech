using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.common.components;
using ProjectArchaetech.util.events;
using ProjectArchaetech.interfaces;
using ProjectArchaetech.resources;
using ProjectArchaetech.util;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public partial class BaseBuilding : Building {
		private ProductionMethod activePM;
		private static readonly ProcessingPopsEvent processingPopsEvent = new ProcessingPopsEvent();

		public override void _Ready() {
			base._Ready();

			this.activePM = this.Data.ProductionMethods[0];
			Dictionary<JobData, Dictionary<int, Array<Pop>>> workers = new Dictionary<JobData, Dictionary<int, Array<Pop>>>();
			Dictionary<JobData, int> employmentCap = this.activePM.Recipe.Duplicate(true);
			foreach (JobData job in this.activePM.Recipe.Keys) {
				Dictionary<int, Array<Pop>> workersList = new Dictionary<int, Array<Pop>>();
				for (int i = 0; i < 3; i += 1) {
					workersList[i] = new Array<Pop>();
				}
				workers[job] = workersList;
			}

			this.AddFunctionality(new ProductionFunctionality(Global.ResManager, workers));
			RecruitmentFunctionality recruitmentFunctionality = new RecruitmentFunctionality(employmentCap, workers);
			this.AddFunctionality(recruitmentFunctionality);
			this.AddFunctionality(new TransportFunctionality(
			new HashDictionary<Building, TransportRoute>(), 6));
			recruitmentFunctionality.Execute();

			this.updatedUIData["employment"] = new Pair(workers.Duplicate(true), employmentCap.Duplicate(true));
			this.updatedUIData["output"] = new Dictionary<ResourceData, double>();
			this.updatedUIData["local_storage"] = new Dictionary<ResourceData, double>();
			Global.EventBus.Subscribe<ProductionEndedEvent>((sender, e) => this.FinishWork());
		}

		protected override void Work() {
			// Clear old UI data.
			this.updatedUIData.Clear();
			Global.EventBus.Publish(this, buildingProcessedEvent);
		}

		private void FinishWork() {
			foreach (IFunctionable f in this.functionalities) {
				f.Execute();
			}
			this.UpdateUI();
			Global.EventBus.Publish(this, processingPopsEvent);
			Global.EventBus.Publish(this, new TechProgressedEvent(Global.ResManager.GetRP()));
		}

		protected override void UpdateUI() {
			this.EmitSignal(SignalName.BuildingInfoUpdatedUI, this, this.updatedUIData);
		}
	}
}
