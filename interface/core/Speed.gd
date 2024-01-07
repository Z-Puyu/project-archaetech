class_name Speed extends Panel

@onready var speed_up_button: Button = $SpeedUp
@onready var speed_down_button: Button = $SpeedDown
@onready var speed_label: Label = $CurrentSpeed
var _speed: int

func _ready():
	self._speed = get_node("/root/Global/GameManager/GameClock").SpeedLevel
	self.speed_up_button.pressed.connect(self._on_speed_up)
	self.speed_down_button.pressed.connect(self._on_speed_down)

func _on_speed_up():
	Global.SpeedUpGame();
	self._speed = min(self._speed + 1, 5)
	self.speed_label.text = str(self._speed, "x")
	
func _on_speed_down():
	Global.SlowDownGame()
	self._speed = max(self._speed - 1, 1)
	self.speed_label.text = str(self._speed, "x")
