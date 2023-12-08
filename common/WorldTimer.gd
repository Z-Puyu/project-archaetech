extends Timer

var config: Dictionary = {};
var speed_level: int = 1;
@onready var debugger: Label = $"../UILayer/ForDebug";

func _ready() -> void:
	var config = FileAccess.open("res://assets/game-data/GlobalTimerConfig.json", FileAccess.READ);
	self.config = JSON.parse_string(config.get_as_text());
	self.speed_level = 1;
	self.start(self.config["time_elapse_1"]["value"]);

func _process(delta):
	pass

func _input(event) -> void:
	if event is InputEventKey:
		if Input.is_action_just_pressed("ui_select"):
			self.paused = not self.paused;
			debugger.t_speed = 0 if self.paused else self.speed_level;
			print("Paused" if self.paused else "Un-paused");
		if Input.is_key_pressed(KEY_SHIFT):
			if Input.is_action_pressed("ui_up"):
				self.speed_level = clamp(speed_level + 1, 1, 5);	
				self.wait_time = self.config[str("time_elapse_", self.speed_level)]["value"];
				debugger.t_speed = self.speed_level;
				print("Speed up to %s" % self.wait_time);
			if Input.is_action_pressed("ui_down"):
				self.speed_level = clamp(speed_level - 1, 1, 5);	
				self.wait_time = self.config[str("time_elapse_", self.speed_level)]["value"];
				debugger.t_speed = self.speed_level;
				print("Speed down");
