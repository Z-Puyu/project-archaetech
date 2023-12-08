extends Node

class_name InventoryManager

@export var resources: Dictionary = {}

func add(type: Resource, qty: float):
	if self.resources.has(type):
		self.resources[type] += qty
	else:
		self.resources[type] = qty
