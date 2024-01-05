class_name SpeedUp extends Button

func _ready():
	self.pressed.connect(Global.GameManager.GameClock.SpeedUp)
