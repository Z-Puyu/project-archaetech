class_name EventWindow extends PanelContainer
const GameEvent = preload("res://events/GameEvent.cs")
const OPTION = preload("res://interface/events/EventOptionButton.tscn")
const EventOptionButton = preload("res://interface/events/EventOptionButton.gd")

@onready var _event_contents: VBoxContainer = %EventContents

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func associate_with(e: GameEvent):
	%EventTitle.set_text(e.Data.Title)
	%EventDescription.set_text(e.Data.Desc)
	for o in e.Data.Options:
		var option_button: EventOptionButton = OPTION.instantiate()
		option_button.associate_with(o)
		self._event_contents.add_child(option_button)
	
