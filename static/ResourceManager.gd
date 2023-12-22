extends Node

enum resource_types {
	COMMON_RAW,
	RARE_RAW,
	ENERGY,
	CONSUMER,
	INDUSTRIAL,
	RESEARCH,
	LUXUARY,
	HIGH_END
}

const FOOD = preload("res://common/resources/basic/FoodResource.tres")
const WODD = preload("res://common/resources/basic/WoodResource.tres")
const MINERAL = preload("res://common/resources/basic/MineralResource.tres")
var resources: Dictionary

func _ready():
	resources = {
		FOOD: 500,
		WODD: 500,
		MINERAL: 500
	}

func add(affected_resources: Dictionary):
	for type in affected_resources:
		if resources.has(type):
			resources[type] += affected_resources[type]
			print("%s has increased by %d" % [type.name, affected_resources[type]])
		else:
			resources[type] = affected_resources[type]
	print(resources)
	
func consume(affected_resources: Dictionary):
	for type in affected_resources:
		if resources.has(type):
			resources[type] -= affected_resources[type]
			print("%s has decreased by %d" % [type.name, affected_resources[type]])
		else:
			resources[type] = affected_resources[type]
	print(resources)
		
func supply(job: JobData, num_workers: int):
	if num_workers <= 0:
		print("There is no workers in the job %s" % job)
		return
	var k = 1
	var input: Dictionary = job.input.duplicate()
	for res in input.keys():
		if self.resources[res] < 1:
			# We use < 1 to avoid (possible) float point precision issues
			print("There is insufficient %s! %s cannot produce anything." % [res.name, job.name])
			return
		input[res] *= num_workers
		# Find the maximal possible proportion of production 
		# which can be achieved using the current available resources
		k = min(self.resources[res] / input[res], k)
	var output: Dictionary = job.output.duplicate()
	for res in input.keys():
		input[res] *= (-k)
	for res in output.keys():
		output[res] *= (k * num_workers)
	add(input)
	add(output)
		
func has_enough(type: ResourceData, benchmark: float) -> bool:
	return self.resources[type] >= benchmark
