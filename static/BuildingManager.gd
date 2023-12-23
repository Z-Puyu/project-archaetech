extends Node

enum BUILING_TYPES {
	FORESTRY
}

var buildings: Dictionary # [Vector2i, Node]
var available_buildings: Dictionary # [Integer, PackedScene]
var curr_spawning_obj: int
# var queues: Array[LinkedList]
# var max_capacity: int
# var under_construction: Dictionary
var construction_queue: ConstructionQueue

signal spawn_building(building: Node2D, pos: Vector2i)

func _ready():
	buildings = {}
	available_buildings[0] = load("res://assets/buildings/ForestryBuilding.tscn")
	construction_queue = ConstructionQueue.new(2)
	spawn_building.connect(on_building_complete)

func add_building(pos: Vector2i, coords: Vector2, type: int) -> bool:
	# Note that pos is the local map coordinates
	curr_spawning_obj = type
	var obj: Node2D = available_buildings.get(curr_spawning_obj).instantiate()
	if obj.can_be_built():
		if buildings.has(pos):
			print(str("Cell ", pos, " already has a building!"))
			var building: Node = buildings.get(pos)
			if building is ConstructibleTask:
				# If the building is under construction, we return the cost
				# User Validation panel needed here
				ResourceManager.add(building.value().data.cost)
				buildings.erase(building)
			else:
				# User Validation panel needed here
				delete_building(pos) 
		ResourceManager.consume(obj.data.cost)
		obj.translate(coords)
		var task: ConstructibleTask = ConstructibleTask.new(obj, pos)
		buildings[pos] = task
		construction_queue.enqueue(task)
		# under_construction[obj] = obj.data.time_to_build
		# queues[under_construction.size() % max_capacity].push_back(obj)
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
				
func on_building_complete(building: Node2D, pos: Vector2i):
	buildings[pos] = building
