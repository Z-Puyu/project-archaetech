extends Node

var available_buildings: Dictionary:
	get:
		return available_buildings
var curr_spawning_obj: String:
	set(id):
		curr_spawning_obj = id
	get:
		return curr_spawning_obj
var under_construction: Dictionary:
	get:
		return under_construction
var construction_queue: ConstructionQueue:
	get:
		return construction_queue
var selected_cell: Cell:
	set(cell):
		selected_cell = cell
	get:
		return selected_cell
var curr_cell_data: TileData:
	set(data):
		curr_cell_data = data
	get:
		return curr_cell_data

signal spawn_building(building: Building)

func _ready():
	available_buildings["building-forestry-1"] = load("res://assets/buildings/ForestryBuilding.tscn")
	construction_queue = ConstructionQueue.new(2)
	Events.cell_selected.connect(func(cell, data): 
		selected_cell = cell
		curr_cell_data = data
	)

func add_building(id: String):
	# Note that pos is the local map coordinates
	curr_spawning_obj = id
	var obj: Building = available_buildings.get(curr_spawning_obj).instantiate()
	if obj.can_be_built(curr_cell_data, selected_cell):
		if under_construction.has(selected_cell.pos):
			var task: ConstructibleTask = under_construction.get(selected_cell.pos)
			# If the building is under construction, we return the cost
			# User Validation panel needed here
			ResourceManager.add(task.value().data.cost)
			under_construction.erase(task)
			construction_queue.remove(task)
		elif selected_cell.building != null:
			# User Validation panel needed here
			delete_building(selected_cell) 
		ResourceManager.consume(obj.data.cost)
		obj.translate(selected_cell.local_coords)
		obj.set_z_index(1)
		var new_task: ConstructibleTask = ConstructibleTask.new(obj, selected_cell)
		construction_queue.enqueue(new_task)
		
func delete_building(location: Cell):
	var building: Building = location.building
	if building != null:
		building.queue_free()
		location.building = null
