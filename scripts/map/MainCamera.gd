extends Camera2D

@export var speed: float = 500.0;
@export var zoom_speed: float = 10.0;
@export var zoom_margin: float = 0.1;
@export var zoom_min: float = 0.25;
@export var zoom_max: float = 3.0;
var v_x: float = 0.0;
var v_y: float = 0.0;
var cursor_pos: Vector2 = Vector2();
var zoom_factor: float = 1.0;

func _ready():
	pass

func _process(delta: float):
	var input_x: int = 0;
	var input_y: int = 0;
	
	if not Input.is_key_pressed(KEY_SHIFT) and Input.is_action_pressed("ui_arrow_key"): 
		input_x = int(Input.is_action_pressed("ui_right")) - \
				  int(Input.is_action_pressed("ui_left"));
		input_y = int(Input.is_action_pressed("ui_down")) - \
				  int(Input.is_action_pressed("ui_up"));
	else:
		var x_diff: int = (self.cursor_pos.x - self.position.x) as int;
		var y_diff: int = (self.cursor_pos.y - self.position.y) as int;
		input_x = (int(x_diff > 300) - int(x_diff < -300)) * (absf(x_diff) / 300.0);
		input_y = (int(y_diff > 200) - int(y_diff < -200)) * (absf(y_diff) / 300.0);
				
	self.v_x = lerp(self.v_x, input_x * self.speed, 0.025);
	self.v_y = lerp(self.v_y, input_y * self.speed, 0.025);
	self.position.x += self.v_x * delta;
	self.position.y += self.v_y * delta;
	
func _input(event: InputEvent):
	if event is InputEventMouseButton:
		if event.is_action_pressed("wheel_up"):
			self.zoom.x *= (1 + 0.01 * self.zoom_speed);
			self.zoom.y *= (1 + 0.01 * self.zoom_speed);
		elif event.is_action_pressed("wheel_down"):
			self.zoom.x *= (1 - 0.01 * self.zoom_speed);
			self.zoom.y *= (1 - 0.01 * self.zoom_speed);
		self.zoom.x = clamp(self.zoom.x, self.zoom_min, self.zoom_max);
		self.zoom.y = clamp(self.zoom.y, self.zoom_min, self.zoom_max);		
			
	if event is InputEventMouseMotion:
		self.cursor_pos = self.get_global_mouse_position();
	
