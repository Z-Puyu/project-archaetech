extends Node

var buildings: Dictionary

func _ready():
	buildings = {
		"production": []
	}

func add_building(building: Node2D):
	if building is ProductionBuilding:
		buildings["production"].append(building)

func update():
	var producers: Array[ProductionBuilding] = buildings["production"]
	for building in producers:
		building.produce() 
