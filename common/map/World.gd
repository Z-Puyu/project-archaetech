class_name World extends Node2D
const Map = preload("res://common/map/Map.cs")
const PauseMenu = preload("res://interface/core/PauseMenu.gd")

@onready var map: Map = $Map
@onready var pause_menu: PauseMenu = load("res://interface/core/PauseMenu.tscn").instantiate()
@export var default_player_base: Array[Vector2i]

func _ready():
	var base_building: BaseBuilding = $BaseBuilding
	for pt in self.default_player_base:
		self.map.Grid[pt].Building = base_building
	
func _unhandled_input(event: InputEvent):
	if Global.IsGamePaused():
		return
	if event.is_action_pressed("toggle_pause") or event.is_action_pressed("go_back"):
		Global.PauseGame()
		get_tree().root.add_child(self.pause_menu)
		
func spawn_unit(unit: Node2D):
	unit.translate(map.map_to_local(map.curr_selected))
	self.add_child(unit)

