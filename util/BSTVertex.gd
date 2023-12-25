class_name BSTVertex extends RefCounted

var value: Variant;
var parent: BSTVertex = null;
var left: BSTVertex = null;
var right: BSTVertex = null;
var height: int = 0;
var other: Variant;

func _init(v: Variant, other: Variant = null):
	self.value = v;
	self.other = other;

func _to_string():
	return str(value);
