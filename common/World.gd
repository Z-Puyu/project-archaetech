extends Node2D

@onready var map: TileMap = $Map;
@onready var game_clock: Timer = GameManager.game_clock;
@onready var info: Control = $InGameUI/UILayer/InfoPanel;
@onready var new_unit = $InGameUI/UILayer/InfoPanel/NewUnit;
@onready var unit_selector = $InGameUI/UILayer/UnitSelection/ScrollContainer/VBoxContainer;
var days: int 

func _ready():
	self.days = 0
	self.game_clock.timeout.connect(self._on_world_timer_timeout)
	map.connect("tileSelected", info.showInfo)
	map.connect("tileSelected", unit_selector.create_entries)
	new_unit.connect("pressed", map.new_unit)

func next_turn():
	var curr_pop_count: int = PopManager.pop_count
	ResourceManager.add("food", -curr_pop_count)
	print(ResourceManager.resources.get("food"))
	PopManager.update()
	BuildingManager.update()

func f():
	print("success");

func _on_world_timer_timeout():
	self.days += 1
	print(str("Day ", self.days))
	if self.days % 30 == 0:
		print("new month")
		self.next_turn()
