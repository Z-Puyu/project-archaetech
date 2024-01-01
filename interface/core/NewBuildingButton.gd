class_name NewBuildingButton extends TextureButton

signal toggle(window_name: String)

func _ready():
	self.pressed.connect(self._on_pressed)

func _on_pressed():
	print("button pressed")
	self.toggle.emit("building")
