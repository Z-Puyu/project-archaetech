class_name PauseMenu extends CanvasLayer

func _ready():
	%ResumeButton.pressed.connect(self._on_resume_button_pressed)
	%ExitButton.pressed.connect(self._on_exit_button_pressed)

func _on_resume_button_pressed():
	get_tree().root.remove_child(self)
	Global.ResumeGame()

func _on_exit_button_pressed():
	get_tree().quit()
