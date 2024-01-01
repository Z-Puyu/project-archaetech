class_name Cell extends Node

var pos: Vector2i:
	get:
		return pos
var local_coords: Vector2:
	get:
		return local_coords
var units: Array:
	get: 
		return units
var building: Building:
	set(new_building):
		building = new_building
	get:
		return building

func _init(pos: Vector2i, local_coords: Vector2):
	self.pos = pos
	self.local_coords = local_coords
	self.units = []
	
func _to_string():
	return "position: %s\nbuidling: %s\nunits: %s\n" % [self.pos, self.building, self.units]
	#return str("position: ", self.pos, "building: ", self.building, "units: ", self.units)
