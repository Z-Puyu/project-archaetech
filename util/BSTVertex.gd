class_name BSTVertex extends RefCounted

var value: Variant;
var parent: BSTVertex = null;
var left: BSTVertex = null;
var right: BSTVertex = null;
var height: int = 0;

func _init(v: Variant):
	self.value = v;
