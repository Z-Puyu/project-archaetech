using C5;
using Godot;
using Godot.Collections;
using ProjectArchaetech.common.components;
using ProjectArchaetech.events;
using ProjectArchaetech.resources;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public partial class BaseBuilding : Building {
		private ProductionMethod activePM;
		private static readonly ProcessingPopsEvent processingPopsEvent = new ProcessingPopsEvent();

		public BaseBuilding() : base(new TransportFunctionality(
			new HashDictionary<Building, TransportRoute>(), 1)) { }

		public override void _Ready() {
			base._Ready();
			this.activePM = this.Data.ProductionMethods[0];
			Dictionary<JobData, int> workers = new Dictionary<JobData, int>();
			Dictionary<JobData, int> employmentCap = this.activePM.Recipe.Duplicate(true);
			foreach (JobData job in this.activePM.Recipe.Keys) {
				workers[job] = 0;
			}
			this.AddFunctionality(new ProductionFunctionality(Global.ResManager, workers));
			RecruitmentFunctionality recruitmentFunctionality = new RecruitmentFunctionality(employmentCap, workers);
			this.AddFunctionality(recruitmentFunctionality);
			recruitmentFunctionality.Execute(this.updatedUIData);
			Global.EventBus.Subscribe<ProductionEndedEvent>((sender, e) => this.Work());
		}

		protected override void Work() {
			base.Work();
			Global.EventBus.Publish(this, processingPopsEvent);
			Global.EventBus.Publish(this, new TechProgressedEvent(Global.ResManager.GetRP()));
		}
	}
}
