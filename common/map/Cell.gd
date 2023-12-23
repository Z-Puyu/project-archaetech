class_name Cell extends Resource

var pos: Vector2i;

var units: Array[UnitData] = [];
var building: Building;
var units_count: Array[int] = [0];

func _init(_pos: Vector2i):
	pos = _pos;
	
func _to_string():
	return "position: %s\nbuidling: %s\nunits: %s\n" % [self.pos, self.building, self.units]
	#return str("position: ", self.pos, "building: ", self.building, "units: ", self.units)
