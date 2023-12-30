extends Button

@onready var building_menu = $BuildingMenu



func _ready():
	building_menu.hide()

func _on_pressed():
	if (building_menu.visible):
		building_menu.hide()
	else:
		building_menu.show()
