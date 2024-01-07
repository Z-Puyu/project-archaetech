class_name CellInfo extends Panel

@onready var components: Dictionary = {
	"terrain": $Terrain
}:
	get:
		return components

func show_info(data: TileData):
	self.components.get("terrain").set_text(data.get_custom_data("terrain").name)
