class_name PopLabel extends HBoxContainer
const PopManager = preload("res://static/PopManager.cs")

@onready var label: Label = $DynamicLabel

func _ready():
	var pops: PopManager = get_node("/root/Global/PopManager")
	pops.PopCountUpdated.connect(self._on_pop_change)
	self.label.set_text(str(pops.CountUnemployed(), " / ", pops.PopCount()))

func _on_pop_change(total: int, labour: int):
	self.label.set_text(str(labour, " / ", total))
