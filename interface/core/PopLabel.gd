class_name PopLabel extends HBoxContainer

@onready var label: Label = $DynamicLabel

func _ready():
	PopManager.changed.connect(self._on_pop_change)
	self.label.set_text(str(PopManager.unemployed, " / ", PopManager.pop_count))

func _on_pop_change():
	var total: int = PopManager.pop_count
	var labour: int = PopManager.unemployed
	self.label.set_text(str(labour, " / ", total))
