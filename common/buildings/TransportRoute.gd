class_name TransportRoute extends Node2D

@onready var from: Building:
	get: 
		return from
var to: Building:
	get:
		return to
var manpower: int:
	set(amount):
		manpower = amount
	get:
		return manpower
var curr_manpower: int:
	set(amount):
		curr_manpower = min(self.manpower, amount)
	get:
		return curr_manpower
var maintenance: Array[float]:
	get:
		return maintenance
var max_capacity: Array[float]:
	get:
		return max_capacity
var target_resources: Dictionary:
	get:
		return target_resources # [Resource, percentage transported]
var transported_resources: Dictionary:
	set(resources):
		transported_resources = resources
	get:
		return transported_resources

func _init(to: Building, manpower: int, target_resources: Array):
	self.to = to
	self.manpower = manpower
	self.curr_manpower = 0
	self.maintenance = [0, 0.25, 0.3, 0.5, 0.75, 1]
	self.max_capacity = [0, 0.3, 0.35, 0.4, 0.45, 0.5]
	for res in target_resources:
		if res is ResourceData:
			self.target_resources[res] = 0
	self.transported_resources = {}

func _ready():
	self.from = self.get_parent()
	self.employ()

func employ():
	var demand: int = self.manpower - self.curr_manpower
	var recruitment: int = min(demand, PopManager.unemployed)
	self.curr_manpower += recruitment
	PopManager.unemployed -= recruitment
	for res in target_resources:
		if res is ResourceData:
			self.target_resources[res] = self.max_capacity[curr_manpower]
	
func transport():
	var maintenance: float = self.maintenance[self.curr_manpower] * self.curr_manpower
	if ResourceManager.has_enough(ResourceManager.FOOD, maintenance):
		ResourceManager.consume({ResourceManager.FOOD: maintenance})
		self.transported_resources = self.from.take_away(self.target_resources)
		self.to.store(transported_resources)
		print(transported_resources)
	if self.curr_manpower < self.manpower:
		self.employ()
	
	
func _to_string() -> String:
	return "Transport route from " + self.from._to_string() + " to " + self.to._to_string()
