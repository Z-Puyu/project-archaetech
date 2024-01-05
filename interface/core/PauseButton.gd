class_name PauseButton extends TextureButton

func _ready():
	self.toggled.connect(func(toggled: bool): Global.GameManager.GameClock.Paused = not toggled)
