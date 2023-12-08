extends Node2D

@onready var map: TileMap = $Map;
@onready var calender: Label = $UILayer/Calendar;
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func next_turn() -> void:
	self.calender.next_day();

func _on_world_timer_timeout() -> void:
	self.next_turn();
