class_name Building extends Node2D

@export var data: BuildingData

signal show_building_info(info: Dictionary)
signal select(building: Building)

func can_be_built(tile_data: TileData) -> bool:
	var cost: Dictionary = self.data.cost
	if not (self.data.required_terrains.has(tile_data.get_custom_data("terrain"))):
		return false
	for resource in cost.keys():
		if not ResourceManager.has_enough(resource, cost.get(resource)):
			# print("%s is insufficient! Currently has %f but needs %d!" % [resource, ResourceManager.resources.get(resource), cost.get(resource)])
			return false
	# print("%s can be built!" % self)
	return true
	
func _unhandled_input(event: InputEvent):
	if event.is_action_released("left_click"):
		var cursor: Cursor = get_node("../Cursor")
		var from: Building = cursor.get_selected_building()
		if cursor.get_mode() == 1 and from is ProductionBuilding:
			var output_resources: Array[ResourceData] = from.output.keys()
			from.new_route(self, 1, output_resources)
			cursor.set_mode(0)
