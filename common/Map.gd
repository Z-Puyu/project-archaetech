extends TileMap

const MAIN_LAYER: int = 0;
const MAIN_ATLAS: int = 0;
enum TILE_TYPE {PLAIN, OCEAN, FOREST, DESERT, MOUNTAIN};

@onready var debugger: Label = $"../UILayer/ForDebug";

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _input(event: InputEvent):
	if event is InputEventMouse:
		if event.is_pressed() and event.button_index == MOUSE_BUTTON_LEFT:
			var global_cursor_pos: Vector2 = self.make_input_local(event).position;
			var local_selection: Vector2 = self.local_to_map(global_cursor_pos);
			var atlas_coords: Vector2i = self.get_cell_atlas_coords(MAIN_LAYER, local_selection);
			var tile_type: int = atlas_coords[0];
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
					f_str += "a plain.";
			print(f_str);
