extends Control

func _on_resume_button_pressed():
	get_tree().change_scene_to_file("res://common/world.tscn")

func _on_exit_button_pressed():
	get_tree().change_scene_to_file("res://common/main.tscn")
