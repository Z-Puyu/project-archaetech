class_name World extends Node2D

@onready var map: Map = $Map
@onready var pause_menu: PauseMenu = $PauseMenu

func _ready():
	pass
	
func _unhandled_input(event: InputEvent):
	if Global.IsGamePaused():
		return
	if event.is_action_pressed("toggle_pause") or event.is_action_pressed("go_back"):
		Global.PauseGame()
		self.pause_menu.show()
		
func spawn_unit(unit: Node2D):
	unit.translate(map.map_to_local(map.curr_selected))
	self.add_child(unit)

