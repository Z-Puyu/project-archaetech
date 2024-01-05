class_name SpeedDown extends Button

func _ready():
	self.pressed.connect(Global.GameManager.GameClock.SlowDown)
