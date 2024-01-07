using Godot;
using ProjectArchaetech.resources;
using static ProjectArchaetech.events.EventBus;

namespace ProjectArchaetech.common {
	[GlobalClass]
	public partial class BaseBuilding : ProductiveBuilding {
		public override void _Ready() {
			base._Ready();
			this.warehouse = Global.ResManager;
			foreach (JobData job in this.EmploymentCap.Keys) {
				this.Workers[job] = 0;
				this.Recruit(job);
			}
			Global.EventBus.Subscribe<NewMonthEvent>((sender, e) => this.Work());
		}

        public override void Work() {
			// Clear the research point record as it is not cumulative.
			Global.ResManager.ClearRp();
			Global.EventBus.Publish(this, new ProcessingBuildingsEvent());
            base.Work();
        }
    }
}
