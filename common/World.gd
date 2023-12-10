extends Node2D

@onready var map: TileMap = $Map
@onready var game_clock: Timer = GameManager.game_clock

var days: int
# Called when the node enters the scene tree for the first time.
func _ready():
	self.days = 0;
	self.game_clock.timeout.connect(self._on_world_timer_timeout)

func next_turn():
	var curr_pop_count: int = PopManager.pop_count
	ResourceManager.add("food", -curr_pop_count)
	print(ResourceManager.resources.get("food"))
	PopManager.update()

func _on_world_timer_timeout():
	self.days += 1
	print(str("Day ", self.days))
	if self.days % 30 == 0:
		print("new month")
		self.next_turn()
