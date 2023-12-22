class_name UnitData extends Object

var name: String
var id: Vector3
var pos: Vector2
var speed: int = 1
var production_cost: Array[int]

func _init(_pos: Vector2, _id: int):
	self.pos = _pos
	self.name = str("unit ", _id)
	self.id = Vector3(_pos.x, _pos.y, _id)

func find_path(destination: int):
	pass

func _to_string():
	return str("Name: ", self.name, ", ", "Position: ", self.pos, "\n")
