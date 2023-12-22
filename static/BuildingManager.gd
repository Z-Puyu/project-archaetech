extends Node

enum BUILING_TYPES {
	FORESTRY
}
var buildings: Dictionary # [Vector2i, Node]
var available_buildings: Dictionary # [Integer, PackedScene]
var curr_spawning_obj: Node2D

signal spawn_building(building: Node2D)

func _ready():
	curr_spawning_obj = null
	buildings = {}
	available_buildings[0] = load("res://assets/buildings/ForestryBuilding.tscn")

func add_building(pos: Vector2i, type: int):
	# Note that pos is the local map coordinates
	if buildings.has(pos):
		print(str("Cell ", pos, " already has a building!"))
	else: 
		if curr_spawning_obj == null:
			curr_spawning_obj = available_buildings[type].instantiate()
		var obj: Node2D = curr_spawning_obj.duplicate()
		buildings[pos] = obj
		spawn_building.emit(obj)
		
func delete_building(pos: Vector2i):
	var building: Node2D = buildings.get(pos)
	if building != null:
		buildings.erase(pos)
		remove_child(building)
		building.queue_free()
	else:
		print(str("Cell ", pos, " has no building!"))
