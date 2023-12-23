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
	if curr_spawning_obj == null:
		curr_spawning_obj = available_buildings[type].instantiate()
	if curr_spawning_obj.can_be_built():
		if buildings.has(pos):
			print(str("Cell ", pos, " already has a building!"))
			var building: Node2D = buildings.get(pos)
			if queue.peek_front() == building:
				# User Validation panel needed here
				var returned_cost: Dictionary = building.data.cost
				ResourceManager.add(returned_cost)
			else:
				# User Validation panel needed here
				delete_building(pos) 
		var obj: Node2D = curr_spawning_obj.duplicate(8)
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
		var new_building: Node2D = queue.peek_front()
		if new_building.data.time_to_build > 1:
			new_building.data.time_to_build -= 1
		else:
			queue.pop_front()
			spawn_building.emit(new_building)
