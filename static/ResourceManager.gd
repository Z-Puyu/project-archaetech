extends Node

const ResearchPointResource = preload("res://common/resources/research/ResearchPointResource.tres")
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

@export var resources: Dictionary
@export var storage_limit: Array[int]
var monthly_output: Dictionary

signal qty_updated(res: ResourceData, new_qty: float)
signal tech_progress(research_point: int)

func _ready():
	tech_progress.connect(get_node("/root/TechManager").Research)
	
func add(affected_resources: Dictionary):
	for type in affected_resources:
		if not resources.has(type):
			resources[type] = 0
		if type == ResearchPointResource:
			resources[type] = affected_resources.get(type)
		else:
			var net_amount: float = resources.get(type) + affected_resources.get(type)
			resources[type] = min(net_amount, storage_limit[type.type])
		qty_updated.emit(type, resources.get(type))
	
func consume(affected_resources: Dictionary):
	for type in affected_resources:
		if has_enough(type, affected_resources[type]):
			resources[type] -= affected_resources[type]
			qty_updated.emit(type, resources.get(type))
	
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
	if monthly_output.has(ResearchPointResource):
		tech_progress.emit(monthly_output.get(ResearchPointResource))
	
func reset():
	monthly_output = {}
		
func has_enough(type: ResourceData, benchmark: float) -> bool:
	return resources.has(type) and resources.get(type) >= benchmark
