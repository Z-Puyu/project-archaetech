extends Timer

var config: Dictionary = {}
var speed_level: int = 1
@onready var debugger: Label = $"../UILayer/ForDebug"

func _ready() -> void:
	var config = FileAccess.open("res://assets/game-data/GlobalTimerConfig.json", FileAccess.READ)
	self.config = JSON.parse_string(config.get_as_text())
	self.speed_level = 1
	self.start(self.config["time_elapse_1"]["value"])
	self.paused = true

func _process(delta):
	pass


