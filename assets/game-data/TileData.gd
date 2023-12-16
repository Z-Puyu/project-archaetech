extends Object
class_name CellData

var pos: Vector2;

var units: Array[UnitData] = [];

var buildings: Array[String] = ["none"];

var units_count: Array[int] = [0];

func _init(_pos: Vector2):
	pos = _pos;
	
func _to_string():
	return str("position: ", self.pos, "building: ", self.buildings, "units: ", self.units)
