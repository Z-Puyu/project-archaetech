extends Node

var resources: Dictionary = {
	"food": 500,
	"wood": 500,
	"mineral": 500
}

func add(type: String, qty: float):
	if self.resources.has(type):
		self.resources[type] += qty
	else:
		self.resources[type] = qty
