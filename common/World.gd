extends Node2D

@onready var map: TileMap = $Map
@onready var game_clock: Timer = GameManager.game_clock
@onready var info: Control = $InGameUI/UILayer/InfoPanel
@onready var new_unit = $InGameUI/UILayer/InfoPanel/NewUnit
@onready var new_building = $InGameUI/UILayer/InfoPanel/NewBuilding
@onready var unit_selector = $InGameUI/UILayer/UnitSelection/ScrollContainer/VBoxContainer
var days: int 

func _ready():
	self.days = 0
	self.map.tile_selected.connect(self.info.showInfo)
	self.map.tile_selected.connect(self.unit_selector.create_entries)
	self.new_unit.pressed.connect(self.map.new_unit)
	self.new_building.pressed.connect(self.map.new_building)
	BuildingManager.spawn_building.connect(self.spawn_building)

func spawn_building(building: Node2D):
	building.translate(map.map_to_local(map.curr_selected))
	self.add_child(building)
