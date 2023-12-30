class_name ProductionBuilding extends Building

const Building = preload("res://common/buildings/Building.gd")
const LocalStorage = preload("res://common/buildings/LocalStorage.gd")

var employment: Dictionary
@onready var warehouse: LocalStorage = $Warehouse
var max_employment: Dictionary
var transport_network: Array[TransportRoute]
var output: Dictionary

func _ready():
	var jobs: Dictionary = self.data.activated_production_method.recipe
	for job in jobs.keys():
		self.max_employment[job] = jobs.get(job)
	GameManager.new_month.connect(self.work)
	self.transport_network = []
	var building_panel: Panel = get_node("/root/World/UILayer/InGameUI/InfoPanel/BuildingInfo")
	self.show_building_info.connect(building_panel.show_info)
	self.select.connect(get_node("../Cursor").on_select_building)
	# print(self.employment)

func work():
	# print(str(self._to_string(), " is working "))
	# Process each job in the building sequentially
	for job in self.max_employment.keys():
		if not self.employment.has(job):
			self.employment[job] = 0
		self.warehouse.supply(job, employment.get(job))
		# If the building is under-employed, it will try to recruit
		if employment.get(job) < self.max_employment.get(job):
			self.employ(job)
	self.output = self.warehouse.monthly_output
	for route in transport_network:
		route.transport()
	self.warehouse.reset()
	self.show_building_info.emit({
		"output": self.output,
		"local_storage": self.warehouse.resources
	})
	# THIS IS FOR DEBUG PURPOSE ONLY!!!
	if self.transport_network.is_empty():
		if not self.output.keys().is_empty():
			for key in self.output.keys():
				if not key is ResourceData:
					print(key.name + " is not ResourceData")
			self.new_route(get_node("../BaseBuilding"), 1, self.output.keys())
			
func employ(job: JobData):
	var num_positions: int = self.max_employment.get(job)
	var demand = num_positions - employment.get(job)
	var recruitment = min(demand, PopManager.unemployed)
	employment[job] += recruitment
	PopManager.unemployed -= recruitment
	# print("%s has employed %d new workers" % [self, recruitment])
	
func take_away(resources: Dictionary) -> Dictionary:
	return self.warehouse.take_away(resources)
	
func store(resources: Dictionary):
	self.warehouse.add(resources)
	
func new_route(to: Building, level: int, resources: Array):
	var route: TransportRoute = TransportRoute.new(to, level, resources)
	self.transport_network.append(route)
	self.add_child(route)
	
func show_info():
	self.show_building_info.emit({
		"name": self.data.name,
		"icon": self.data.icon,
		"output": self.output,
		"local_storage": self.warehouse.resources
	})

func _to_string() -> String:
	return str(self.data.name, " with production method ", self.data.activated_production_method.name)
