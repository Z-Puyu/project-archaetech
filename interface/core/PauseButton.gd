class_name PauseButton extends TextureButton

func _ready():
	self.toggled.connect(func(toggled: bool): 
		Global.ResumeTime() if toggled else Global.PauseTime()
	)
