using Godot;
using ProjectArchaetech.common;
using ProjectArchaetech.resources;

namespace ProjectArchaetech {
	[GlobalClass]
	public partial class ResourceManager : Warehouse {
		[Export]
		public ResourceData rpResource { set; get; }

		public delegate void TechProgressEventHandler(int researchPoints);
		public event TechProgressEventHandler TechProgress;

		public override void Reset() {
			if (this.MonthlyOutput.Contains(this.rpResource)) {
				this.TechProgress.Invoke((int) this.MonthlyOutput[this.rpResource]);
			}
			this.MonthlyOutput.Clear();
		}
	}
}
