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

var resources: Dictionary
var storage_limit: Array[int]
var monthly_output: Dictionary

const FOOD = preload("res://common/resources/basic/FoodResource.tres")
const WODD = preload("res://common/resources/basic/WoodResource.tres")
const MINERAL = preload("res://common/resources/basic/MineralResource.tres")
const RESEARCH = preload("res://common/resources/research/ResearchPointResource.tres")

signal qty_updated(res: ResourceData, new_qty: float)
signal tech_progress(research_point: int)

func _ready():
	resources = {
		FOOD: 500,
		WODD: 500,
		MINERAL: 500,
		RESEARCH: 0
	}
	storage_limit.append(10000)
	storage_limit.append(5000)
	storage_limit.append(5000)
	storage_limit.append(10000)
	storage_limit.append(10000)
	storage_limit.append(100000)
	storage_limit.append(2000)
	storage_limit.append(1000)
	
func add(affected_resources: Dictionary):
	for type in affected_resources:
		if not resources.has(type):
			resources[type] = 0
		var net_amount: float = resources.get(type) + affected_resources.get(type)
		resources[type] = min(net_amount, storage_limit[type.type])
		# # print("%s has increased by %d" % [type.name, min(net_amount, storage_limit[type.type])])
		qty_updated.emit(type, resources.get(type))
	# # print(resources)
	
	
func consume(affected_resources: Dictionary):
	for type in affected_resources:
		if has_enough(type, affected_resources[type]):
			resources[type] -= affected_resources[type]
			# # print("%s has decreased by %d" % [type.name, affected_resources[type]])
			qty_updated.emit(type, resources.get(type))
		# else:
			# print("Not enough %s to consume" % type.name)
	# print(resources)
	
func take_away(resources: Dictionary) -> Dictionary:
	var taken: Dictionary = {}
	for res in resources:
		var proportion: float = resources.get(res)
		var amount_taken = monthly_output[res] * proportion
		if taken.has(res):
			taken[res] += amount_taken
		else:
			taken[res] = amount_taken
	# This is safe because taken <= monthly_output <= resources
	consume(taken)
	return taken
		
func supply(job: JobData, num_workers: int):
	if num_workers <= 0:
		# print("There is no workers in the job %s" % job)
		return
	var k = 1
	var input: Dictionary = job.input.duplicate(true)
	for res in input.keys():
		if resources[res] < 1:
			# We use < 1 to avoid (possible) float point precision issues
			# print("There is insufficient %s! %s cannot produce anything." % [res.name, job.name])
			return
		input[res] *= num_workers
		# Find the maximal possible proportion of production 
		# which can be achieved using the current available resources
		k = min(resources[res] / input[res], k)
	var output: Dictionary = job.output.duplicate()
	for res in input.keys():
		input[res] *= k
	for res in output.keys():
		if monthly_output.has(res):
			monthly_output[res] += output[res] * k * num_workers
		else:
			monthly_output[res] = output[res] * k * num_workers
	consume(input)
	add(monthly_output)
	if monthly_output.has(RESEARCH):
		tech_progress.emit(monthly_output.get(RESEARCH))
	
func reset():
	monthly_output = {}
		
func has_enough(type: ResourceData, benchmark: float) -> bool:
	return resources[type] >= benchmark
