class_name Map extends TileMap

const MAIN_LAYER: int = 0
const MAIN_ATLAS: int = 0
enum terrains {
	PLAIN, 
	OCEAN, 
	FOREST, 
	DESERT, 
	MOUNTAIN
}

# @onready var debugger: Label = $"../UILayer/ForDebug";
@onready var InGameUI = get_tree().root.get_node("World/InGameUI/UILayer/InfoPanel")
var grid: Dictionary # [Vector2i, Cell]

signal tile_selected(cell: Cell)
var curr_selected: Vector2i

# Called when the node enters the scene tree for the first time.
func _ready():
	self.grid = {}
	self.add_layer(1)
	for x in range(-1, 12):
		for y in range(-1, 9):
			var _pos: Vector2i = Vector2i(x, y)
			grid[_pos] = Cell.new(_pos)
		

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _unhandled_input(event: InputEvent):
	if event is InputEventMouse:
		if event.is_pressed() and event.button_index == MOUSE_BUTTON_LEFT:
			var global_cursor_pos: Vector2 = self.make_input_local(event).position
			self.curr_selected = self.local_to_map(global_cursor_pos)
			var atlas_coords: Vector2i = self.get_cell_atlas_coords(MAIN_LAYER, self.curr_selected)
			var tile_data: TileData = self.get_cell_tile_data(MAIN_LAYER, self.curr_selected)
			var terrain: TerrainData = tile_data.get_custom_data("terrain")
			self.clear_layer(1)
			self.set_cell(1, self.curr_selected, 0, Vector2i(5, 0))
			self.tile_selected.emit(grid[self.curr_selected])
			var f_str: String = "{pos} is ".format({"pos": self.curr_selected})
			match terrain.type:
				terrains.PLAIN: 
					f_str += "a plain."
				terrains.OCEAN:
					f_str += "an ocean."
				terrains.FOREST:
					f_str += "a forest."
				terrains.DESERT:
					f_str += "a desert."
				terrains.MOUNTAIN:
					f_str += "a mountain."
				_:
					f_str += "nothing."
			print(f_str);

func new_unit():
	if (true):  #资源限制？
		var cell = self.grid[self.curr_selected]
		print(cell.units_count);
		UnitManager.new_unit(self.curr_selected, 0);
		self.tile_selected.emit(grid[self.curr_selected]);
		
func new_building():
	if (true): 
		var cell_pos: Vector2i = self.curr_selected
		var local_coords: Vector2 = self.map_to_local(cell_pos)
		var curr_cell: Cell = self.grid.get(cell_pos)
		var tile_data: TileData = self.get_cell_tile_data(MAIN_LAYER, self.curr_selected)
		if BuildingManager.add_building(tile_data, cell_pos, local_coords, 0):
			curr_cell.building = BuildingManager.buildings.get(cell_pos)
			self.tile_selected.emit(self.grid.get(cell_pos));

func _to_string():
	return str(grid);
