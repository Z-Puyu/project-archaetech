class_name Cell extends Node

var pos: Vector2i;

var units: Array[UnitData] = [];
var building: Building;
var units_count: Array[int] = [0];

func _init(_pos: Vector2i):
	pos = _pos;
	
func set_building(building: Building):
	self.building = building

func get_building() -> Building:
	return self.building
	
func _to_string():
	return "position: %s\nbuidling: %s\nunits: %s\n" % [self.pos, self.building, self.units]
	#return str("position: ", self.pos, "building: ", self.building, "units: ", self.units)
