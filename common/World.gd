class_name World extends Node2D

@onready var map: Map = $Map
@onready var game_clock: Timer = Global.GameManager.game_clock
@onready var info: Control = $UILayer/InGameUI/InfoPanel
@onready var new_unit = $UILayer/InGameUI/InfoPanel/NewUnit
@onready var unit_selector = $UILayer/InGameUI/UnitSelection/ScrollContainer/VBoxContainer
var player_base
@onready var resource_panel: ResourcePanel = $UILayer/InGameUI/ResourcePanel
const PAUSE_MENU = preload("res://interface/core/PauseMenu.tscn")
var days: int 
var pf: PathFinder

func _ready():
	self.days = 0
	self.new_unit.pressed.connect(self.map.new_unit)
	Global.BuildManager.spawn_building.connect(self.add_child)
	UnitManager.spawn_unit.connect(self.spawn_unit)
	Global.ResManager.qty_updated.connect(self.resource_panel.update)
	self.player_base.main_storage_changed.connect(resource_panel.update)
	pf = PathFinder.new(map)
	UnitManager.new_unit(Vector2i(1,1), 0)
	self.player_base = get_node("BaseBuilding")
	
func _unhandled_input(event: InputEvent):
	if Global.GameState == Global.GameMode.Paused:
		return
	if event.is_action_pressed("toggle_pause") or event.is_action_pressed("go_back"):
		Global.PauseGame()
		var pause_menu: Node = self.PAUSE_MENU.instantiate()
		get_tree().root.add_child(pause_menu) 
		
func spawn_unit(unit: Node2D):
	unit.translate(map.map_to_local(map.curr_selected))
	self.add_child(unit)

