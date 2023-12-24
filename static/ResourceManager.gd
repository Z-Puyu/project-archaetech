extends Node

var warehouse: LocalStorage

const FOOD = preload("res://common/resources/basic/FoodResource.tres")
const WODD = preload("res://common/resources/basic/WoodResource.tres")
const MINERAL = preload("res://common/resources/basic/MineralResource.tres")

func _ready():
	warehouse = LocalStorage.new()
	warehouse.resources = {
		FOOD: 500,
		WODD: 500,
		MINERAL: 500
	}
	print(warehouse.resources)
	
func get_resources() -> Dictionary:
	return warehouse.resources

func add(affected_resources: Dictionary):
	warehouse.add(affected_resources)
	
func consume(affected_resources: Dictionary):
	warehouse.consume(affected_resources)
	
func take_away(resources: Dictionary) -> Dictionary:
	return warehouse.take_away(resources)

func supply(job: JobData, num_workers: int):
	warehouse.supply(job, num_workers)
		
func has_enough(type: ResourceData, benchmark: float) -> bool:
	return warehouse.has_enough(type, benchmark)
