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
	
func _on_click(viewport: Viewport, event: InputEvent, shape_idx: int):
	if event.is_action_pressed("left_click"):
		if GameManager.game_mode == GameManager.GAME_MODES.BUILD_ROUTE:
			Events.add_route.emit(self)
		else:
			Events.show_building_info.emit(self)
			Events.toggle_modal.emit("building")
