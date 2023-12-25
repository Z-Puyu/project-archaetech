class_name Building extends Node2D

@export var data: BuildingData

func can_be_built(tile_data: TileData) -> bool:
	var cost: Dictionary = self.data.cost
	if not (self.data.required_terrains.has(tile_data.get_custom_data("terrain"))):
		return false
	for resource in cost.keys():
		if not ResourceManager.has_enough(resource, cost.get(resource)):
			print("%s is insufficient! Currently has %f but needs %d!" % [resource, ResourceManager.resources.get(resource), cost.get(resource)])
			return false
	print("%s can be built!" % self)
	return true
