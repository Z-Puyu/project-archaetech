using Godot;
using ProjectArchaetech;
using ProjectArchaetech.events;
using ProjectArchaetech.ui;
using ProjectArchaetech.util.events;

public partial class EventPopups : CanvasLayer {
	private PackedScene eventWindow;
	public override void _Ready() {
		this.eventWindow = GD.Load<PackedScene>("res://interface/events/EventWindow.tscn");
		Global.EventBus.Subscribe<GameEventFiredEvent>(
			(sender, e) => this.ShowEvent((GameEvent) sender)
		);
	}

	public void ShowEvent(GameEvent e) {
		EventWindow eventWindow = this.eventWindow.Instantiate<EventWindow>();
		this.AddChild(eventWindow);
		eventWindow.AssociateWith(e);
	}
}
