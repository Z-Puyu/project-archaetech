class_name BaseBuilding extends Building

const FOOD = preload("res://common/resources/basic/FoodResource.tres")
const WODD = preload("res://common/resources/basic/WoodResource.tres")
const MINERAL = preload("res://common/resources/basic/MineralResource.tres")

var employment: Dictionary
var max_employment: Dictionary
var output: Dictionary
var net_output: Dictionary
var transport_network: Array[TransportRoute]

signal main_storage_changed(res: ResourceData, new_qty: float)

func _ready():
	var jobs: Dictionary = self.data.activated_production_method.recipe
	for job in jobs.keys():
		self.max_employment[job] = jobs.get(job)
		self.employment[job] = 0
		self.employ(job)
	GameManager.new_month.connect(self.work)
	self.transport_network = []
	

func work():
	print(str(self._to_string(), " is working "))
	# Process each job in the building sequentially
	for job in self.max_employment.keys():
		if not self.employment.has(job):
			self.employment[job] = 0
		ResourceManager.supply(job, employment.get(job))
		# If the building is under-employed, it will try to recruit
		if employment.get(job) < self.max_employment.get(job):
			self.employ(job)
	for route in transport_network:
		route.transport()
	ResourceManager.reset()
			
func employ(job: JobData):
	var num_positions: int = self.max_employment.get(job)
	var demand = num_positions - self.employment.get(job)
	var recruitment = min(demand, PopManager.unemployed)
	employment[job] += recruitment
	PopManager.unemployed -= recruitment
	print("%s has employed %d new workers" % [self, recruitment])
	
func take_away(resources: Dictionary) -> Dictionary:
	return ResourceManager.take_away(resources)
	
func store(resources: Dictionary):
	ResourceManager.add(resources)
	
func new_route(to: Building, level: int, resources: Dictionary):
	self.add_child(TransportRoute.new(to, level, resources))
	
func update_resource_panel(res: ResourceData, new_qty: float):
	self.main_storage_changed.emit(res, new_qty)

func _to_string() -> String:
	return str(self.data.name, " with production method ", self.data.activated_production_method.name)

