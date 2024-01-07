class_name PauseMenuOverlay extends Control

func is_to_be_blocked(event: InputEvent) -> bool:
	if event.is_action("go_back") or event.is_action("toggle_pause") or event.is_action("close"):
		return true
	return false

func _gui_input(event: InputEvent):
	if self.is_to_be_blocked(event):
		self.accept_event()
		
func _ready():
	%ResumeButton.pressed.connect(self.accept_event)
