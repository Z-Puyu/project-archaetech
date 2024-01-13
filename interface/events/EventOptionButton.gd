class_name EventOptionButton extends TextureButton

var _option: Option

func _ready():
	self.pressed.connect(self._option.OnSelect)
	self.mouse_entered.connect(%Tooltip.show)
	self.mouse_exited.connect(%Tooltip.hide)

func associate_with(o: Option):
	self._option = o
	%EventOptionText.set_text(o.Desc);
	%Tooltip.set_text(self._option.ToString())
