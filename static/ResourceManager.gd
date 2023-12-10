extends Node

var resources: Dictionary

func _ready():
	resources = {
		"food": 500,
		"wood": 500,
		"mineral": 500
	}

func add(type: String, qty: float):
	if resources.has(type):
		resources[type] += qty
	else:
		resources[type] = qty
