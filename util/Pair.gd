class_name pair extends RefCounted

var _first: Variant
var _second: Variant

func _init(first: Variant, second: Variant):
	self._first = first
	self._second = second

func first() -> Variant:
	return self._first
	
func second() -> Variant:
	return self._second
	
func sum() -> Variant:
	if self._first is int or self._first is float:
		if self._second is int or self._second is float:
			return self._first + self._second
		return null
	return null

func _to_string() -> String:
	return "[{first}, {second}]".format({"first": self._first, "second": self._second})
