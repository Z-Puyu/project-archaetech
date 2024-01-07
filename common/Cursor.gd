class_name Cursor extends Node2D

const Building = preload("res://common/buildings/Building.cs")

enum mode {
	NORMAL,
	CREATE_ROUTE,
	BUILD
}

signal click(cell: Cell)
signal moved(new_pos: Vector2i)

var curr_mode: int:
	set(mode):
		curr_mode = mode
	get:
		return curr_mode
var map: Map
var grid: Dictionary
var cell: Vector2i
var _selected_building: Building
var pick_up: Node

# When the cursor enters the scene tree, we snap its position to the centre of the cell and we
# initialise the timer with our ui_cooldown variable.
func _ready():
	self.curr_mode = self.mode.NORMAL
	self.cell = Vector2i.ZERO
	
func initialise():
	self._map = get_node("/root/World/Map")
	self.grid = self._map.grid
	self.set_position(self._map.map_to_local(self._cell))
	
func set_cell(cell: Vector2i):
	self.cell = cell
	
func get_cell() -> Vector2i:
	return self._cell

func _unhandled_input(event: InputEvent):
	if event is InputEventMouseMotion:
		if self.map != null:
			self._cell = self._map.local_to_map(event.position)
	#elif event.is_action_pressed("left_click"):
		# Signal the World to handle input.
		#self.click.emit(self._cell)
		
func on_select_building(building: Building):
	self._selected_building = building
	self._curr_mode = self.mode.CREATE_ROUTE
	
func get_selected_building() -> Building:
	return self._selected_building
	
func set_mode(mode: int):
	self._curr_mode = mode
	
func get_mode() -> int:
	return self._curr_mode

#func _draw():
#	draw_rect(Rect2(-grid.cell_size / 2, grid.cell_size), Color.aliceblue, false, 2.0)
