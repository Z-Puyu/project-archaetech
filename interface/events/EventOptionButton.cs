using Godot;
using ProjectArchaetech.events;

namespace ProjectArchaetech.ui {
	[GlobalClass]
	public partial class EventOptionButton : TextureButton {
		private Option option;
		private TextEdit tooltip;
		private Label name;

		public override void _Ready() {
			this.tooltip = this.GetNode<TextEdit>("Tooltip");
			this.name = this.GetNode<Label>("EventOptionText"); 
			this.MouseEntered += this.tooltip.Show;
			this.MouseExited += this.tooltip.Hide;
		}

		public void AssociateWith(Option o) {
			this.option = o;
			this.tooltip.Text = o.ToString();
			this.name.Text = o.Desc;
		}
	}
}
