class_name Main extends Node2D

func _ready():
	%PlayButton.pressed.connect(self._on_play_pressed)
	%QuitButton.pressed.connect(self._on_quit_pressed)

func _on_play_pressed():
	get_tree().change_scene_to_file("res://common/map/World.tscn")
	
func _on_quit_pressed():
	get_tree().quit()
