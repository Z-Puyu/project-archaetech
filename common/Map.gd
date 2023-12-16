extends TileMap

const MAIN_LAYER: int = 0;
const MAIN_ATLAS: int = 0;
enum TILE_TYPE {PLAIN, OCEAN, FOREST, DESERT, MOUNTAIN};

@onready var debugger: Label = $"../UILayer/ForDebug";
@onready var InGameUI = get_tree().root.get_node("World").get_node("InGameUI").get_node("UILayer").get_node("InfoPanel");
var grid: Dictionary = {};

signal tileSelected(pos);
var curr_selected: Vector2;

# Called when the node enters the scene tree for the first time.
func _ready():
	self.add_layer(1);
	for x in range(-1, 12):
		for y in range(-1, 9):
			var _pos: Vector2 = Vector2(x, y);
			grid[_pos] = CellData.new(_pos);
		

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _unhandled_input(event: InputEvent):
	if event is InputEventMouse:
		if event.is_pressed() and event.button_index == MOUSE_BUTTON_LEFT:
			var global_cursor_pos: Vector2 = self.make_input_local(event).position;
			var local_selection: Vector2 = self.local_to_map(global_cursor_pos);
			var atlas_coords: Vector2i = self.get_cell_atlas_coords(MAIN_LAYER, local_selection);
			var tile_type: int = atlas_coords[0];
			self.clear_layer(1);
			self.set_cell(1, local_selection, 0, Vector2i(5, 0));
			self.curr_selected = local_selection;
			emit_signal("tileSelected", grid[local_selection]);
			var f_str: String = "{pos} is ".format({"pos": local_selection});
			match tile_type:
				TILE_TYPE.PLAIN: 
					f_str += "a plain.";
				TILE_TYPE.OCEAN:
					f_str += "an ocean.";
				TILE_TYPE.FOREST:
					f_str += "a forest.";
				TILE_TYPE.DESERT:
					f_str += "a desert.";
				TILE_TYPE.MOUNTAIN:
					f_str += "a mountain.";
				_:
					f_str += "nothing.";
			print(f_str);

func new_unit():
	if (true):  #资源限制？
		var cell = self.grid[self.curr_selected]
		print(cell.units_count);
		cell.units.append(UnitData.new(self.curr_selected, cell.units_count[0] + 1));
		cell.units_count[0] += 1;
		emit_signal("tileSelected", grid[curr_selected]);

func _to_string():
	return str(grid);
