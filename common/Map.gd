class_name Map extends TileMap

enum layers {
	UI,
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

var grid: Grid # [Vector2i, Cell]
var land_navigable: Dictionary # [Vector2i, Cell]
var water_navigable: Dictionary # [Vector2i, Cell]

signal tile_selected(cell: Cell)
signal cell_selected(data: TileData)
var curr_selected: Vector2i

func _ready():
	var base = get_node("/root/World/BaseBuilding")
	self.grid = Grid.new()
	self.grid.add({
		Vector2i(0, 0): GDCell.new(Vector2i(0, 0), self.map_to_local(Vector2i(0, 0))),
		Vector2i(1, 0): GDCell.new(Vector2i(1, 0), self.map_to_local(Vector2i(1, 0))),
		Vector2i(-1, 0): GDCell.new(Vector2i(-1, 0), self.map_to_local(Vector2i(-1, 0))),
		Vector2i(-1, -1): GDCell.new(Vector2i(-1, -1), self.map_to_local(Vector2i(-1, -1))),
		Vector2i(0, -1): GDCell.new(Vector2i(0, -1), self.map_to_local(Vector2i(0, -1))),
		Vector2i(-1, 1): GDCell.new(Vector2i(-1, 1), self.map_to_local(Vector2i(-1, 1))),
		Vector2i(0, 1): GDCell.new(Vector2i(0, 1), self.map_to_local(Vector2i(0, 1)))
	})
	self.land_navigable = self.grid.cells.duplicate(true);
	for coords in self.grid.cells.keys():
		self.grid.get_cell(coords).building = base.duplicate()
	var land_cells = self.get_used_cells(self.layers.LAND)
	var water_cells = self.get_used_cells(self.layers.WATER)
	for coords in land_cells:
		if self.grid.cells.has(coords):
			continue
		var cell: GDCell = GDCell.new(coords, self.map_to_local(coords))
		self.grid.add(cell)
		self.land_navigable[coords] = cell
	for coords in water_cells:
		if self.grid.cells.has(coords):
			continue
		var cell: GDCell = GDCell.new(coords, self.map_to_local(coords))
		self.grid.add(cell)
		self.land_navigable[coords] = cell
	#self.cell_selected.connect(func(cell, data): Events.cell_selected.emit(cell, data))

func _unhandled_input(event: InputEvent):
	if event is InputEventMouse:
		if event.is_action_pressed("left_click"):
			var global_cursor_pos: Vector2 = self.make_input_local(event).position
			self.curr_selected = self.local_to_map(global_cursor_pos)
			var tile_data: TileData = self.get_cell_tile_data(self.layers.LAND, self.curr_selected)
			var selected_cell: Cell = self.grid.get_cell(self.curr_selected)
			if tile_data != null:
				Global.EventBus.cell_selected.emit(selected_cell, tile_data)
				var terrain: TerrainData = tile_data.get_custom_data("terrain")
			self.clear_layer(self.layers.UI)
			self.set_cell(self.layers.UI, self.curr_selected, self.atlases.CELLS, Vector2i(5, 0))

func new_unit():
	if (true):  #资源限制？
		var cell = self.grid.get_cell(self.curr_selected)
		# print(cell.units_count);
		UnitManager.new_unit(self.curr_selected, 0);
		#self.tile_selected.emit(grid[self.curr_selected]);

func connect_signal(s: Signal, target: Callable):
	s.connect(target)

func _to_string():
	return str(grid);
