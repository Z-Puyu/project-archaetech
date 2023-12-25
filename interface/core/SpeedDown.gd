class_name SpeedDown extends Button

func _ready():
	self.pressed.connect(GameManager._on_speed_down)
