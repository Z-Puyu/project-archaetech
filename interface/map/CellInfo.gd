class_name CellInfo extends Panel
const Cell = preload("res://common/map/Cell.cs")

@onready var components: Dictionary = {
	"terrain": $Terrain
}:
	get:
		return components

func show_info(cell: Cell, data: TileData):
	self.components.get("terrain").set_text(data.get_custom_data("terrain").Name)
