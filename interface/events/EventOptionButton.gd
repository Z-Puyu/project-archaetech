class_name EventOptionButton extends TextureButton
const Option = preload("res://events/Option.cs")

var _option: Option

func _ready():
	self.mouse_entered.connect(%Tooltip.show)
	self.mouse_exited.connect(%Tooltip.hide)

func associate_with(o: Option):
	self._option = o
	self.pressed.connect(self._option.OnSelect)
	%EventOptionText.set_text(o.Desc);
	%Tooltip.set_text(self._option.ToString())
