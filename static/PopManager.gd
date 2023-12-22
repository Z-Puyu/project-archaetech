extends Node

var pop_count: int
var growth_rate: float
var pop_growth: float
var unemployed: int

func _ready():
	pop_count = 25
	growth_rate = 0.05
	pop_growth = 0.0
	unemployed = 25

func update():
	var yearly_food: int = pop_count * 12
	var growth_modifier: float = clamp(ResourceManager.resources[ResourceManager.FOOD] as float / -\
		(yearly_food * 3) as float, 0, 1.5)
	pop_growth += growth_rate * growth_modifier
	var real_growth: int = floor(self.pop_growth)
	if real_growth != 0:
		self.pop_growth -= real_growth
		self.pop_count += real_growth
