class_name SpeedUp extends Button

func _ready():
	self.pressed.connect(GameManager._on_speed_up)
