class_name Vertex extends RefCounted

var _value: Variant
var _next: Vertex
var _prev: Vertex

func _init(value: Variant):
	self._value = value
	self._next = null
	self._prev = null

func value() -> Variant:
	return self._value
	
func next() -> Vertex:
	return self._next
	
func prev() -> Vertex:
	return self._prev
	
func link_to(v: Vertex):
	self._next = v
	v._prev = self
	
func _to_string() -> String:
	return str("[", self._value, "]")
