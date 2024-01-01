class_name Building extends Node2D

@export var data: BuildingData

signal show_building_info(info: Dictionary)
signal select(building: Building)

func can_be_built(tile_data: TileData, location: Cell) -> bool:
	if location.building != null and location.building.data.id == "building-player-base":
		return false
	var cost: Dictionary = self.data.cost
	if not (self.data.required_terrains.has(tile_data.get_custom_data("terrain"))):
		return false
	for resource in cost.keys():
		if not ResourceManager.has_enough(resource, cost.get(resource)):
			return false
	return true
