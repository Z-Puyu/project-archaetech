extends Node

var pop_count: int = 25
var growth_rate: float = 0.05
var pop_growth: float = 0.0

func update():
	self.pop_growth += self.growth_rate
	var real_growth: int = floor(self.pop_growth)
	if real_growth != 0:
		self.pop_growth -= real_growth
		self.pop_count += real_growth
