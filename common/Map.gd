class_name Map extends TileMap

enum layers {
	UI,
	BUILDINGS,
	MAP_OBJECTS,
	RESOURCES,
	LAND,
	WATER
}

enum atlases {
	CELLS,
	BUILDINGS
}

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

func _ready():
	const BASE = preload("res://assets/buildings/BaseBuilding.tscn")
	var base: BaseBuilding = BASE.instantiate()
	self.grid = {
		Vector2i(0, 0): Cell.new(Vector2i(0, 0)),
		Vector2i(1, 0): Cell.new(Vector2i(1, 0)),
		Vector2i(-1, 0): Cell.new(Vector2i(-1, 0)),
		Vector2i(-1, -1): Cell.new(Vector2i(-1, -1)),
		Vector2i(0, -1): Cell.new(Vector2i(0, -1)),
		Vector2i(-1, 1): Cell.new(Vector2i(-1, 1)),
		Vector2i(0, 1): Cell.new(Vector2i(0, 1))
	}
	for coords in self.grid:
		self.grid.get(coords).building = base.duplicate()
	var land_cells = self.get_used_cells(self.layers.LAND)
	var water_cells = self.get_used_cells(self.layers.WATER)
	for coords in land_cells:
		if self.grid.has(coords):
			continue
		self.grid[coords] = Cell.new(coords)
	for coords in water_cells:
		if self.grid.has(coords):
			continue
		self.grid[coords] = Cell.new(coords)
	BuildingManager.spawn_building.connect(self.spawn_building)
		

func _unhandled_input(event: InputEvent):
	if event is InputEventMouse:
		if event.is_pressed() and event.button_index == MOUSE_BUTTON_LEFT:
			var global_cursor_pos: Vector2 = self.make_input_local(event).position
			self.curr_selected = self.local_to_map(global_cursor_pos)
			var tile_data: TileData = self.get_cell_tile_data(self.layers.LAND, self.curr_selected)
			if tile_data != null:
				var f_str: String = "{pos} is ".format({"pos": self.curr_selected})
				var terrain: TerrainData = tile_data.get_custom_data("terrain")
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
			self.clear_layer(self.layers.UI)
			self.set_cell(self.layers.UI, self.curr_selected, self.atlases.CELLS, Vector2i(5, 0))
			self.tile_selected.emit(grid[self.curr_selected])
			

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
		var tile_data: TileData = self.get_cell_tile_data(self.layers.LAND, self.curr_selected)
		if BuildingManager.add_building(tile_data, cell_pos, local_coords, 0):
			curr_cell.building = BuildingManager.buildings.get(cell_pos)
			self.tile_selected.emit(self.grid.get(cell_pos));
			
func spawn_building(building: Building, pos: Vector2i):
	self.set_cell(self.layers.BUILDINGS, pos, self.atlases.BUILDINGS, building.data.map_object)

func _to_string():
	return str(grid);
