extends Button

@onready var unit_menu = $UnitMenu


func _ready():
	unit_menu.hide()

func _on_pressed():
	if (unit_menu.visible):
		unit_menu.hide()
	else:
		unit_menu.show()
