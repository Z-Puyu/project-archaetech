class_name World extends Node2D
const Map = preload("res://common/map/Map.cs")

@onready var map: Map = $Map
@onready var pause_menu: PauseMenu = load("res://interface/core/PauseMenu.tscn").instantiate()
	
func _unhandled_input(event: InputEvent):
	if Global.IsGamePaused():
		return
	if event.is_action_pressed("toggle_pause") or event.is_action_pressed("go_back"):
		Global.PauseGame()
		get_tree().root.add_child(self.pause_menu)
		
func spawn_unit(unit: Node2D):
	unit.translate(map.map_to_local(map.curr_selected))
	self.add_child(unit)

