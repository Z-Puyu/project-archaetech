extends Node

enum UNIT_TYPES{
	EXPLOERER
}

const uuid = preload("res://util/uuid.gd");

var units: Dictionary = {}; #<pos: Vector2, Dictionary<id:int, unit: Node2D>>
var available_units: Dictionary;  #<int, PackedScene>	
var curr_spawning_obj: Node2D = null;
var units_count: int = 0;

signal spawn_unit(unit: Node2D);

func _ready():
	available_units[0] = load("res://common/units/Explorer.gd")
	
func new_unit(pos: Vector2, type:int):
	var new = Explorer.create(uuid.v4(), pos);
	if not units.keys().has(pos):
		self.units[pos] = {units_count : new};
	else: 
		self.units[pos][units_count] = new
	units_count += 1;
	spawn_unit.emit(new);
	
func remove_unit(obj: Node2D):
	pass
	
