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
var _player_base: Dictionary

signal spawn_building(building: Building, pos: Vector2i)

func _ready():
	buildings = {}
	_player_base = {
		Vector2i(0, 0): null,
		Vector2i(1, 0): null,
		Vector2i(-1, 0): null,
		Vector2i(-1, -1): null,
		Vector2i(0, -1): null,
		Vector2i(-1, 1): null,
		Vector2i(0, 1): null
	}
	available_buildings[0] = load("res://assets/buildings/ForestryBuilding.tscn")
	construction_queue = ConstructionQueue.new(2)
	spawn_building.connect(on_building_complete)

func add_building(tile_data: TileData, pos: Vector2i, coords: Vector2, type: int) -> bool:
	# Note that pos is the local map coordinates
	curr_spawning_obj = type
	var obj: Building = available_buildings.get(curr_spawning_obj).instantiate()
	print(obj.data.required_terrains.size())
	if obj.can_be_built(tile_data) and not _player_base.has(pos):
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
	print("The tile is player base")
	return false
		
func delete_building(pos: Vector2i):
	var building: Building = buildings.get(pos)
	if building != null:
		buildings.erase(pos)
		remove_child(building)
		building.queue_free()
	else:
		print(str("Cell ", pos, " has no building!"))
				
func on_building_complete(building: Building, pos: Vector2i):
	buildings[pos] = building
