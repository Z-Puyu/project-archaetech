extends Node

var pop_count: int:
	set(count):
		pop_count = count
		changed.emit()
	get:
		return pop_count
var growth_rate: float:
	set(rate):
		growth_rate = rate
	get:
		return growth_rate
var pop_growth: float:
	set(progress):
		pop_growth = progress
		pop_grown.emit()
	get:
		return pop_growth
var unemployed: int:
	set(count):
		unemployed = count
		changed.emit()
	get:
		return unemployed

signal changed
signal pop_grown

func _ready():
	pop_count = 25
	growth_rate = 0.05
	pop_growth = 0.0
	unemployed = 25

func update():
	var yearly_food: int = pop_count * 12
	var growth_modifier: float = clamp(ResourceManager.resources.get(ResourceManager.FOOD) as float / -\
		(yearly_food * 3) as float, 0, 1.5)
	pop_growth += growth_rate * growth_modifier
	var real_growth: int = floor(self.pop_growth)
	if real_growth != 0:
		self.pop_growth -= real_growth
		self.pop_count += real_growth
