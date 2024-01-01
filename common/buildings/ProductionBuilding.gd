class_name ProductionBuilding extends Building

const Building = preload("res://common/buildings/Building.gd")
const LocalStorage = preload("res://common/buildings/LocalStorage.gd")

var employment: Dictionary:
	get:
		return employment if employment != null else {}
@onready var warehouse: LocalStorage = $Warehouse:
	get:
		return warehouse
var max_employment: Dictionary:
	get:
		return max_employment if max_employment != null else {}
var transport_network: Array[TransportRoute]:
	get:
		return transport_network if transport_network != null else []
var output: Dictionary:
	get: return output if output != null else {}

func _ready():
	var jobs: Dictionary = self.data.activated_production_method.recipe
	for job in jobs.keys():
		self.max_employment[job] = jobs.get(job)
	GameManager.new_month.connect(self.work)
	$Area2D.input_event.connect(self._on_click)

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
	Events.show_building_info.emit(self)
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
	
func _on_click(viewport: Viewport, event: InputEvent, shape_idx: int):
	if event.is_action_pressed("left_click"):
		Events.show_building_info.emit(self)

func _to_string() -> String:
	return str(self.data.name, " with production method ", self.data.activated_production_method.name)
