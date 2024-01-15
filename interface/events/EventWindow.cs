using Godot;
using ProjectArchaetech.events;

namespace ProjectArchaetech.ui {
	[GlobalClass]
	public partial class EventWindow : PanelContainer {
		private PackedScene optionButton;
		private VBoxContainer eventContents;
		private Label eventTitle;
		private TextureRect eventPicture;
		private TextEdit eventDesc;

		[Signal]
		public delegate void GameEventFiredEventHandler();

		public override void _Ready() {
			this.GetNode<Global>("/root/Global").ForcePause();
			this.optionButton = GD.Load<PackedScene>(
				"res://interface/events/EventOptionButton.tscn"
			);
			this.eventContents = this.GetNode<VBoxContainer>(
				"MarginContainer/VBoxContainer/HBoxContainer/EventContents"
			);
			this.eventTitle = this.GetNode<Label>("MarginContainer/VBoxContainer/EventTitle");
			this.eventPicture = this.GetNode<TextureRect>(
				"MarginContainer/VBoxContainer/HBoxContainer/EventPicture"
			);
			this.eventDesc = this.GetNode<TextEdit>(
				"MarginContainer/VBoxContainer/HBoxContainer/EventContents/EventDescription"
			);
		}

		public void AssociateWith(GameEvent e) {
			this.eventTitle.Text = e.Title;
			this.eventDesc.Text = e.Desc;
			foreach (Option o in e.Options) {
				EventOptionButton optionButton = this.optionButton.Instantiate<EventOptionButton>();
				this.eventContents.AddChild(optionButton);
				optionButton.AssociateWith(o);
				optionButton.Pressed += o.OnSelect;
				optionButton.Pressed += this.QueueFree;
				optionButton.Pressed += () => {
					if (Global.WasForcePaused()) {
						this.GetNode<Global>("/root/Global").ForceResume();
					}
				};
			}
		}
	}
}
