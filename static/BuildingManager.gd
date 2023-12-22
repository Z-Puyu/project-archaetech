extends Node

enum BUILING_TYPES {
	FORESTRY
}
var buildings: Dictionary # [Vector2i, Node]
var available_buildings: Dictionary # [Integer, PackedScene]
var curr_spawning_obj: Node2D
var queue: LinkedList
var days_left: int

signal spawn_building(building: Node2D)

func _ready():
	curr_spawning_obj = null
	buildings = {}
	available_buildings[0] = load("res://assets/buildings/ForestryBuilding.tscn")
	queue = LinkedList.new()
	days_left = 0
	GameManager.game_clock.timeout.connect(next_day)

func add_building(pos: Vector2i, coords: Vector2, type: int):
	# Note that pos is the local map coordinates
	if buildings.has(pos):
		print(str("Cell ", pos, " already has a building!"))
	else: 
		if curr_spawning_obj == null:
			curr_spawning_obj = available_buildings[type].instantiate()
		var obj: Node2D = curr_spawning_obj.duplicate()
		buildings[pos] = obj
		obj.translate(coords)
		if queue.is_empty():
			days_left = obj.data.time_to_build
		queue.push_back(obj)
		
func delete_building(pos: Vector2i):
	var building: Node2D = buildings.get(pos)
	if building != null:
		buildings.erase(pos)
		remove_child(building)
		building.queue_free()
	else:
		print(str("Cell ", pos, " has no building!"))
		
func next_day():
	if not queue.is_empty():
		if days_left > 1:
			days_left -= 1
		else:
			var new_building: Node2D = queue.pop_front()
			if queue.is_empty():
				days_left = 0
			else:
				days_left = queue.peek_front().data.time_to_build
			spawn_building.emit(new_building)
