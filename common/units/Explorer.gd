class_name Explorer extends Node2D

@export var data: UnitData;
var id: String;

const unit_scene = preload("res://assets/units/Explorer.tscn")

static func create(id: String, pos: Vector2):
	var new = unit_scene.instantiate();
	new.id = id;
	return new;
	
func _to_string():
	return self.id;
