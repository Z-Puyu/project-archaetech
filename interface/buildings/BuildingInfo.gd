class_name BuildingInfo extends Panel

var components: Dictionary

func _ready():
	self.components = {
		"name": $Name,
		"icon": $IconFrame/Icon,
		"output": $ProductionInfo,
		"local_storage": $LocalStorage
	}
	self.set_position(Vector2(7.5, 10))

func show_info(info: Dictionary):
	for key in info:
		components.get(key).apply(info.get(key))
	self.set_visible(true)
	
func close():
	self.set_visible(false)
