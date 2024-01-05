class_name PauseMenu extends CanvasLayer

func _ready():
	self.hide()

func _on_resume_button_pressed():
	self.hide()
	Global.ResumeGame()

func _on_exit_button_pressed():
	get_tree().quit()
