class_name ResourceItem extends Node

var data: ResourceData
var amount: float

func _init(data: ResourceData, amount: float):
	self.data = data
	self.amount = amount

func _to_string() -> String:
	return "%f %s" % [self.amount, self.data._to_string()]
