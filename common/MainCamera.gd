extends Camera2D

@export var speed: float = 1000.0
@export var zoom_speed: float = 10.0
@export var zoom_margin: float = 0.1
@export var zoom_min: float = 0.25
@export var zoom_max: float = 3.0
@export var x_margin: float = 0.9
@export var y_margin: float = 0.9
var v_x: float = 0.0
var v_y: float = 0.0
var cursor_pos: Vector2 = Vector2()
var zoom_factor: float = 1.0
var start_pos: Vector2 = Vector2()
var move: bool = false
var origin: Vector2 = self.position

var x_threshold: float
var y_threshold: float

func _process(delta: float):
	if GameManager.game_is_paused:
		return
	var input_x: int = 0
	var input_y: int = 0
	if not Input.is_mouse_button_pressed(MOUSE_BUTTON_RIGHT):
		if not Input.is_key_pressed(KEY_SHIFT) and Input.is_action_pressed("ui_arrow_key"): 
			input_x = int(Input.is_action_pressed("ui_right")) - \
					  int(Input.is_action_pressed("ui_left"))
			input_y = int(Input.is_action_pressed("ui_down")) - \
					  int(Input.is_action_pressed("ui_up"))
		else:
			x_threshold = get_viewport().size.x / 2 * x_margin * (zoom_factor)
			y_threshold = get_viewport().size.y / 2 * y_margin * (zoom_factor)
			var x_diff: float = (self.cursor_pos.x - self.position.x)
			var y_diff: float = (self.cursor_pos.y - self.position.y)
			input_x = (int(x_diff > x_threshold) - int(x_diff < -x_threshold)) * (absf(x_diff) / x_threshold)
			input_y = (int(y_diff > y_threshold) - int(y_diff < -y_threshold)) * (absf(y_diff) / y_threshold)
				
	self.v_x = lerp(self.v_x, input_x * self.speed * (zoom_factor), 0.025)
	self.v_y = lerp(self.v_y, input_y * self.speed * (zoom_factor), 0.025)
	self.cursor_pos.x += self.v_x * delta
	self.cursor_pos.y += self.v_y * delta
	self.position.x += self.v_x * delta
	self.position.y += self.v_y * delta
	
func _unhandled_input(event: InputEvent):
	if event is InputEventMouseButton:
		if event.is_action_pressed("wheel_up"):
			self.zoom.x *= (1 + 0.01 * self.zoom_speed)
			self.zoom.y *= (1 + 0.01 * self.zoom_speed)
			self.zoom_factor = 1 / zoom.x
		if event.is_action_pressed("wheel_down"):
			self.zoom.x *= (1 - 0.01 * self.zoom_speed)
			self.zoom.y *= (1 - 0.01 * self.zoom_speed)
			self.zoom_factor = 1 / zoom.x

		if event.button_index == MOUSE_BUTTON_MIDDLE:
			self.position = self.origin
			
		if event.button_index == MOUSE_BUTTON_RIGHT:
			if event.is_pressed():
				start_pos = event.position
				move = true
			else:
				move = false
				
		self.zoom.x = clamp(self.zoom.x, self.zoom_min, self.zoom_max)
		self.zoom.y = clamp(self.zoom.y, self.zoom_min, self.zoom_max)
	elif event is InputEventMouseMotion:
		self.cursor_pos = self.get_global_mouse_position()
		if move:
			position += (Vector2(1,1) / self.zoom) * (start_pos - event.position);
			start_pos = event.position
	
