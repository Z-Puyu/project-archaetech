class_name PopLabel extends HBoxContainer

@onready var label: Label = $DynamicLabel

func _ready():
	Global.PopManager.changed.connect(self._on_pop_change)
	self.label.set_text(str(PopManager.unemployed, " / ", PopManager.pop_count))

func _on_pop_change():
	var total: int = Global.PopManager.pop_count
	var labour: int = Global.PopManager.unemployed
	self.label.set_text(str(labour, " / ", total))
