class_name TransportRoute extends Node2D

@onready var _from: Building = self.get_parent()
var _to: Building
var _manpower: int
var _curr_manpower: int
var _maintenance: Array[float]
var _max_capacity: Array[float]
var target_resources: Dictionary # [Resource, percentage transported]

func _init(to: Building, manpower: int, target_resources: Array):
	self._to = to
	self._manpower = manpower
	self._curr_manpower = 0
	self._maintenance = [0, 0.25, 0.3, 0.5, 0.75, 1]
	self._max_capacity = [0, 0.3, 0.35, 0.4, 0.45, 0.5]
	for res in target_resources:
		if res is ResourceData:
			self.target_resources[res] = self._max_capacity[manpower]

func _ready():
	print(self.get_parent())
	self.employ()

func employ():
	var demand: int = self._manpower - self._curr_manpower
	var recruitment: int = min(demand, PopManager.unemployed)
	self._curr_manpower += recruitment
	PopManager.unemployed -= recruitment
	# print("%s has employed %d new workers" % [self, recruitment])
	
func transport():
	var maintenance: float = self._maintenance[self._curr_manpower] * self._curr_manpower
	if ResourceManager.has_enough(ResourceManager.FOOD, maintenance):
		ResourceManager.consume({ResourceManager.FOOD: maintenance})
		print("taking away " + str(self.target_resources))
		var resources: Dictionary = self._from.take_away(self.target_resources)
		self._to.store(resources)
	
	
func _to_string() -> String:
	return "Transport route from " + self._from._to_string() + " to " + self._to._to_string()
