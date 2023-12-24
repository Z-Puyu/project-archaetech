class_name LocalStorage

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

#const FOOD = preload("res://common/resources/basic/FoodResource.tres")
#const WODD = preload("res://common/resources/basic/WoodResource.tres")
#const MINERAL = preload("res://common/resources/basic/MineralResource.tres")
var resources: Dictionary
var storage_limit: Array[int]
var monthly_output: Dictionary

func _init():
	self.resources = {}
	self.storage_limit = []
	for resource_type in self.resource_types:
		self.storage_limit.append(200)
	print(self.storage_limit)

func add(affected_resources: Dictionary):
	for type in affected_resources:
		var amount: float = affected_resources.get(type)
		if not self.resources.has(type):
			amount = 0
			self.resources[type] = 0
		var net_amount: float = self.resources.get(type) + affected_resources.get(type)
		self.resources[type] = min(net_amount, self.storage_limit[type.type])
		print("%s has increased by %d" % [type.name, min(net_amount, self.storage_limit[type.type])])
	print(self.resources)
	
func consume(affected_resources: Dictionary):
	for type in affected_resources:
		if self.has_enough(type, affected_resources[type]):
			self.resources[type] -= affected_resources[type]
			print("%s has decreased by %d" % [type.name, affected_resources[type]])
		else:
			print("Not enough %s to consume" % type.name)
	print(self.resources)
	
func take_away(resources: Dictionary) -> Dictionary:
	var taken: Dictionary = {}
	for res in resources:
		var proportion: float = resources.get(res)
		var amount_taken = self.output[res] * proportion
		if taken.has(res):
			taken[res] += amount_taken
		else:
			taken[res] = amount_taken
	# This is safe because taken <= monthly_output <= resources
	self.consume(taken)
	return taken
		
func supply(job: JobData, num_workers: int):
	if num_workers <= 0:
		print("There is no workers in the job %s" % job)
		return
	var k = 1
	var input: Dictionary = job.input.duplicate(true)
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
		input[res] *= k
	for res in output.keys():
		if self.monthly_output.has(res):
			self.monthly_output[res] += output[res] * k * num_workers
		else:
			self.monthly_output[res] = output[res] * k * num_workers
	self.consume(input)
	self.add(self.monthly_output)
		
func has_enough(type: ResourceData, benchmark: float) -> bool:
	return self.resources[type] >= benchmark
