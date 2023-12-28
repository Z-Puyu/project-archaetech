class_name Deque extends RefCounted

var _set: Dictionary
var _head: int
var _tail: int

func _init():
	self._set = {}
	self._head = 0
	self._tail = 0
	
func is_empty() -> bool:
	return self._set.is_empty()
	
func size() -> int:
	return self._set.size()
	
func contains(i: Variant) -> bool:
	return self._set.values().has(i)
	
func value_at(i: int) -> Variant:
	if not self._set.has(self._head + i):
		return null
	return self._set.get(self._head + i)
	
func push_front(i: Variant):
	if self._set.has(self._head):
		self._head -= 1
	self._set[self._head] = i

func push_back(i: Variant):
	if self._set.has(self._tail):
		self._tail += 1
	self._set[self._tail] = i

func peek_front() -> Variant:
	if self._set.is_empty():
		return null
	return self._set.get(self._head)

func peek_back() -> Variant:
	if self._set.is_empty():
		return null
	return self._set.get(self._tail)
	
func pop_front() -> Variant:
	if self._set.is_empty():
		return null
	var front: Variant = self._set.get(self._head)
	self._set.erase(self._head)
	if not self._set.is_empty():
		self._head += 1
	return front
	
func pop_back() -> Variant:
	if self._set.is_empty():
		return null
	var back: Variant = self._set.get(self._tail)
	self._set.erase(self._tail)
	if not self._set.is_empty():
		self._tail -= 1
	return back
	
func swap(i: int, j: int):
	if i != j and self._set.has(self._head + i) and self._set.has(self._head + j):
		var temp: Variant = self._set.get(self._head + i)
		self._set[self._head + i] = self._set.get(self._head + j)
		self._set[self._head + j] = temp
