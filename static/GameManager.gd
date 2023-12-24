extends Node

var game_clock: Timer
var speed_level: int
var config: Dictionary
var days: int = 0

signal new_month

func _ready():
	# Set up timer
	config = JSON.parse_string(FileAccess.open("res://assets/game-data/GlobalTimerConfig.json", 
		FileAccess.READ).get_as_text())
	game_clock = Timer.new()
	game_clock.set_paused(true)
	game_clock.set_autostart(true)
	GameManager.add_child(game_clock)
	speed_level = 1
	game_clock.timeout.connect(_on_world_timer_timeout)

func _input(event: InputEvent) -> void:
	if event is InputEventKey:
		if Input.is_action_just_pressed("ui_select"):
			game_clock.set_paused(not game_clock.paused)
			if game_clock.paused:
				get_tree().change_scene_to_file("res://interface/PauseMenu.tscn")
			print("Paused" if game_clock.paused else "Un-paused")
		if Input.is_key_pressed(KEY_SHIFT):
			if Input.is_action_pressed("ui_up"):
				speed_level = clamp(speed_level + 1, 1, 5)	
				game_clock.set_wait_time(config[str("time_elapse_", speed_level)]["value"])
				print("Speed up to %s" % game_clock.wait_time)
			if Input.is_action_pressed("ui_down"):
				speed_level = clamp(speed_level - 1, 1, 5)	
				game_clock.set_wait_time(config[str("time_elapse_", speed_level)]["value"])
				print("Speed down")
				
func next_turn():
	var curr_pop_count: int = PopManager.pop_count
	ResourceManager.add({ResourceManager.FOOD: -curr_pop_count})
	print("Remaining food: %f" % ResourceManager.get_resources().get(ResourceManager.FOOD))
	PopManager.update()

func _on_world_timer_timeout():
	self.days += 1
	print(str("Day ", self.days))
	if self.days % 30 == 0:
		print("new month")
		next_turn()
		new_month.emit()
