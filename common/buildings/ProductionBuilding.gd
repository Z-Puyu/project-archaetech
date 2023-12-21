class_name ProductionBuilding extends Node2D

@export var data: BuildingData
var employment: Dictionary

func _ready():
	var jobs: Dictionary = self.data.activated_production_method.recipe
	for job in jobs.keys():
		self.employment[job] = 0
	GameManager.new_month.connect(self.work)
	
func work():
	print(str(self._to_string(), " is working "))
	# Process each job in the building sequentially
	for job in employment.keys():
		ResourceManager.supply(job, employment.get(job))
		# If the building is under-employed, it will try to recruit
		if employment.get(job) < self.data.activated_production_method.recipe.get(job):
			employ(job)
	
func employ(job: JobData):
	var num_positions: int = self.data.activated_production_method.recipe.get(job)
	var demand = num_positions - employment.get(job)
	var recruitment = min(demand, PopManager.unemployed)
	employment[job] += recruitment
	PopManager.unemployed -= recruitment
	print("%s has employed %d new workers" % [self, recruitment])

func _to_string() -> String:
	return str(self.data.name, " with production method ", self.data.activated_production_method.name)
