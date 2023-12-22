extends Node

enum UNIT_TYPES{
	EXPLOERER
}

var units: Dictionary = {}; #<pos: Vector2, Dictionary<id:int, unit: Node2D>>
var available_units: Dictionary;  #<int, PackedScene>	
var curr_spawning_obj: Node2D = null;
var units_counts: Array[int] = [0];

signal spawn_unit(unit: Node2D);

func _ready():
	available_units[0] = load("res://assets/units/Explorer.tscn")
	
func new_unit(pos: Vector2i, type:int):
	if curr_spawning_obj == null:
		curr_spawning_obj = available_units[type].instantiate();
	var new: Node2D = curr_spawning_obj.duplicate();
	self.units[pos] = {units_counts[type]: new};
	units_counts[type] += 1;
	spawn_unit.emit(new);
	
func remove_unit(obj: Node2D):
	pass
	
