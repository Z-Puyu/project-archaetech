class_name BuildingInfo extends Panel

@onready var components: Dictionary = {
	"name": $Name,
	"icon": $IconFrame/Icon,
	"output": $ProductionInfo,
	"local_storage": $LocalStorage
}

func _ready():
	Events.show_building_info.connect(self._show_info)

func _show_info(building: Building):
	print(building.data)
	var info: Dictionary = {
		"name": building.data.name,
		"icon": building.data.icon,
		"output": building.output,
		"local_storage": {} if building is BaseBuilding else building.warehouse.resources
	}
	print(info)
	for key in info:
		components.get(key).apply(info.get(key))
	self.show()
