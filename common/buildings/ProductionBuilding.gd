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
	self.output = {}
	var jobs: Dictionary = self.data.activated_production_method.recipe
	for job in jobs.keys():
		self.max_employment[job] = jobs.get(job)
		for res in job.output.keys():
			self.output[res] = 0
	GameManager.new_month.connect(self.work)
	Events.add_route.connect(func(to): self._new_route(to, 1))
	$Area2D.input_event.connect(self._on_click)

func work():
	# Process each job in the building sequentially
	for job in self.max_employment.keys():
		if not self.employment.has(job):
			self.employment[job] = 0
		self.warehouse.supply(job, employment.get(job))
		# If the building is under-employed, it will try to recruit
		if employment.get(job) < self.max_employment.get(job):
			self.employ(job)
	self.output.merge(self.warehouse.monthly_output, true)
	# Now, transport the output to other buildings
	for route in transport_network:
		route.transport()
	self.warehouse.reset()
	Events.show_building_info.emit(self)
			
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
	
func _new_route(to: Building, level: int):
	var route: TransportRoute = TransportRoute.new(to, level, self.output.keys())
	self.transport_network.append(route)
	self.add_child(route)
	GameManager.game_mode = GameManager.GAME_MODES.NORMAL
	Events.add_route_ui.emit(route)
	
func _delete_route(route: TransportRoute):
	if self.transport_network.has(route):
		self.transport_network.erase(route)
		route.queue_free()

func _to_string() -> String:
	return str(self.data.name, " with production method ", self.data.activated_production_method.name)
