extends Node

var game_clock: Timer
var speed_level: int
var config: Dictionary
var days: int = 0
var game_is_paused: bool
var game_clock_status

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
	game_is_paused = false
				
func next_turn():
	var curr_pop_count: int = PopManager.pop_count
	ResourceManager.add({ResourceManager.FOOD: -curr_pop_count})
	# print("Remaining food: %f" % ResourceManager.resources.get(ResourceManager.FOOD))
	PopManager.update()
	
func pause_game():
	game_clock_status = game_clock.paused
	game_clock.set_paused(true)
	game_is_paused = true
	
func resume_game():
	game_clock.set_paused(game_clock_status)
	game_is_paused = false

func _on_pause_button_toggled(toggled: bool):
	if not game_is_paused:
		game_clock.set_paused(not toggled)
	
func _on_speed_up():
	if not game_is_paused:
		speed_level = clamp(speed_level + 1, 1, 5)	
		game_clock.set_wait_time(config[str("time_elapse_", speed_level)]["value"])
		# print("Speed up to %s" % game_clock.wait_time)
	
func _on_speed_down():
	if not game_is_paused:
		speed_level = clamp(speed_level - 1, 1, 5)	
		game_clock.set_wait_time(config[str("time_elapse_", speed_level)]["value"])
		# print("Speed down")

func _on_world_timer_timeout():
	self.days += 1
	# print(str("Day ", self.days))
	if self.days % 30 == 0:
		# print("new month")
		next_turn()
		new_month.emit()
