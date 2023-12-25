class_name PauseButton extends TextureButton

func _ready():
	self.toggled.connect(GameManager._on_pause_button_toggled)
