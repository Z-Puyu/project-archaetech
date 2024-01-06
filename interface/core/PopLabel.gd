class_name PopLabel extends HBoxContainer
const PopManager = preload("res://static/PopManager.cs")

@onready var label: Label = $DynamicLabel
@onready var pops: PopManager = get_node("/root/Global/PopManager")

func _ready():
	self.pops.PopCountChanged.connect(self._on_pop_change)
	self.label.set_text(str(pops.NUnemployed, " / ", pops.PopCount))

func _on_pop_change():
	var total: int = self.pops.PopCount
	var labour: int = self.pops.NUnemployed
	self.label.set_text(str(labour, " / ", total))
