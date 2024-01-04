extends Node

enum GAME_MODES {
	NORMAL,
	PAUSED,
	BUILD,
	BUILD_ROUTE
}

var game_clock: Timer
var speed_level: int
@export var config: Dictionary
var days: int = 0
var game_mode: int:
	set(mode):
		game_mode = mode
	get:
		return game_mode
var pick_up: Node:
	set(obj):
		pick_up = obj
	get:
		return pick_up

signal new_month

func _ready():
	# Set up timer
	game_clock = Timer.new()
	game_clock.set_paused(true)
	game_clock.set_autostart(true)
	GameManager.add_child(game_clock)
	speed_level = 1
	game_clock.timeout.connect(_on_world_timer_timeout)
	game_mode = GAME_MODES.NORMAL
				
func next_turn():
	var curr_pop_count: int = PopManager.pop_count
	PopManager.update()
	
func pause_game():
	game_mode = GAME_MODES.PAUSED
	game_clock.set_paused(true)
	
func resume_game():
	game_mode = GAME_MODES.NORMAL
	game_clock.set_paused(false)
	
func game_is_paused():
	return game_mode != GAME_MODES.NORMAL

func _on_pause_button_toggled(toggled: bool):
	game_clock.set_paused(not toggled)
	
func _on_speed_up():
	if not game_is_paused():
		speed_level = clamp(speed_level + 1, 1, 5)	
		game_clock.set_wait_time(config.get(speed_level))
		# print("Speed up to %s" % game_clock.wait_time)
	
func _on_speed_down():
	if not game_is_paused():
		speed_level = clamp(speed_level - 1, 1, 5)	
		game_clock.set_wait_time(config.get(speed_level))
		# print("Speed down")

func _on_world_timer_timeout():
	self.days += 1
	# print(str("Day ", self.days))
	if self.days % 30 == 0:
		# print("new month")
		next_turn()
		new_month.emit()
