extends Node2D

@onready var map: TileMap = $Map;
@onready var calender: Label = $InGameUI/UILayer/Calendar;
@onready var info: Control = $InGameUI/UILayer/InfoPanel;
@onready var new_unit = $InGameUI/UILayer/InfoPanel/NewUnit;
@onready var unit_selector = $InGameUI/UILayer/UnitSelection/ScrollContainer/VBoxContainer;

# Called when the node enters the scene tree for the first time.
func _ready():
	map.connect("tileSelected", info.showInfo)
	map.connect("tileSelected", unit_selector.create_entries)
	new_unit.connect("pressed", map.new_unit)
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func next_turn() -> void:
	pass

func _on_world_timer_timeout() -> void:
	self.next_turn();

func f():
	print("success");
