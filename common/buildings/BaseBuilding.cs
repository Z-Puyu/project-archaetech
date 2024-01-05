namespace ProjectArchaetech.common {
	public partial class BaseBuilding : ProductiveBuilding {
		public override void _Ready() {
			base._Ready();
			this.warehouse = Global.ResManager;
		}
	}
}
