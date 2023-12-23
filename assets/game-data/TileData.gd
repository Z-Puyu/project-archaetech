class_name CellData extends Object


var pos: Vector2;

var units: Array[UnitData] = [];

var building: Node2D = null;

var units_count: Array[int] = [0];

func _init(_pos: Vector2):
	pos = _pos;
	
func _to_string():
	return str("position: ", self.pos, "building: ", self.buildings, "units: ", self.units)
