extends Node

enum BUILING_TYPES {
	FORESTRY
}
var buildings: Dictionary # [Vector2i, Node]
var available_buildings: Dictionary # [Integer, PackedScene]
var curr_spawning_obj: Node2D
var queues: Array[LinkedList]
var max_capacity: int
var under_construction: Dictionary

signal spawn_building(building: Node2D)

func _ready():
	curr_spawning_obj = null
	buildings = {}
	available_buildings[0] = load("res://assets/buildings/ForestryBuilding.tscn")
	queues = []   
	max_capacity = 5
	for i in max_capacity:
		queues.append(LinkedList.new())
	GameManager.game_clock.timeout.connect(next_day)

func add_building(pos: Vector2i, coords: Vector2, type: int) -> bool:
	# Note that pos is the local map coordinates
	if curr_spawning_obj == null:
		curr_spawning_obj = available_buildings[type].instantiate()
	if curr_spawning_obj.can_be_built():
		if buildings.has(pos):
			print(str("Cell ", pos, " already has a building!"))
			var building: Node2D = buildings.get(pos)
			if under_construction.has(building):
				# If the building is under construction, we return the cost
				# User Validation panel needed here
				ResourceManager.add(building.data.cost)
				under_construction.erase(building)
			else:
				# User Validation panel needed here
				delete_building(pos) 

		var obj: Node2D = curr_spawning_obj.duplicate()
		ResourceManager.consume(obj.data.cost)
		buildings[pos] = obj
		obj.translate(coords)
		under_construction[obj] = obj.data.time_to_build
		queues[under_construction.size() % max_capacity].push_back(obj)
		return true
	return false
		
func delete_building(pos: Vector2i):
	var building: Node2D = buildings.get(pos)
	if building != null:
		buildings.erase(pos)
		remove_child(building)
		building.queue_free()
	else:
		print(str("Cell ", pos, " has no building!"))
		
func next_day():
	for q in queues:
		if not q.is_empty():
			var new_building: Node2D = q.peek_front()
			under_construction[new_building] -= 1
			if under_construction[new_building] == 0:
				q.pop_front()
				under_construction.erase(new_building)
				spawn_building.emit(new_building)
