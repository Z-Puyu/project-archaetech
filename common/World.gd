extends Node2D

@onready var map: TileMap = $Map
@onready var game_clock: Timer = GameManager.game_clock
@onready var info: Control = $UILayer/InGameUI/InfoPanel
@onready var building_panel: Panel = $UILayer/InGameUI/InfoPanel/BuildingInfo
@onready var new_unit = $UILayer/InGameUI/InfoPanel/NewUnit
@onready var new_building = $UILayer/InGameUI/InfoPanel/NewBuilding
@onready var unit_selector = $UILayer/InGameUI/UnitSelection/ScrollContainer/VBoxContainer
@onready var player_base: BaseBuilding = $BaseBuilding
@onready var resource_panel: ResourcePanel = $UILayer/InGameUI/ResourcePanel
const PAUSE_MENU = preload("res://interface/core/PauseMenu.tscn")
var days: int 
var pf: PathFinder;

func _ready():
	self.days = 0
	self.map.tile_selected.connect(self.info.showInfo)
	self.map.tile_selected.connect(self.unit_selector.create_entries)
	self.new_unit.pressed.connect(self.map.new_unit)
	self.new_building.pressed.connect(self.map.new_building)
	BuildingManager.spawn_building.connect(self.add_child)
	UnitManager.spawn_unit.connect(self.spawn_unit)
	ResourceManager.qty_updated.connect(self.resource_panel.update)
	self.player_base.main_storage_changed.connect(resource_panel.update)
	pf = PathFinder.new(map);
	UnitManager.new_unit(Vector2i(1,1), 0);
	
func _unhandled_input(event: InputEvent):
	if GameManager.game_is_paused:
		return
	if event.is_action_pressed("toggle_pause") or event.is_action_pressed("go_back"):
		GameManager.pause_game()
		var pause_menu: Node = self.PAUSE_MENU.instantiate()
		get_tree().root.add_child(pause_menu) 
		
func spawn_unit(unit: Node2D):
	unit.translate(map.map_to_local(map.curr_selected))
	self.add_child(unit)

