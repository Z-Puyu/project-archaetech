class_name DynamicResourceLabel extends HBoxContainer

@onready var _icon: TextureRect = $Icon
@onready var _amount: Label = $DynamicLabel

func update(icon: Texture2D, amount: float):
	self._icon.set_texture(icon)
	self._amount.update_info(amount)

func set_text(amount: String):
	self._amount.set_text(amount)
