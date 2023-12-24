class_name PauseMenu extends CanvasLayer

func _on_resume_button_pressed():
	get_tree().root.remove_child(self)
	GameManager.resume_game()

func _on_exit_button_pressed():
	get_tree().quit()
