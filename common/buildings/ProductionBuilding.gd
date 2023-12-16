extends Node2D

class_name ProductionBuilding

@export var data: BuildingData
var employment: Dictionary

func _ready():
	var jobs: Dictionary = self.data.activated_production_method.recipe
	for job in jobs.keys():
		self.employment[job] = jobs[job]
	
func produce():
	for job in employment.keys():
		var input: Dictionary = job.input
		var output: Dictionary = job.output
		for resource in input.keys():
			var cost: int = input[resource] * self.employement[job]
			ResourceManager.resources[resource] -= cost
			print(str("Consumed ", cost, " ", resource))
		for product in output.keys():
			var income: int = output[product] * self.employement[job]
			ResourceManager.resources[product] += income
			print(str("Produced ", income, " ", product))

func _to_string() -> String:
	return str(self.data.name, " with production method: ", self.data.activated_production_method.name)
