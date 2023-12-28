class_name LinkedList extends RefCounted

const Vertex = preload("res://util/Vertex.gd")

var _head: Vertex
var _tail: Vertex
var _size: int
var _values: Dictionary

func _init():
	self._head = Vertex.new(null)
	self._tail = Vertex.new(null)
	self._head.link_to(self._tail)
	self._size = 0
	self._values = {}
	
func contains(v: Vertex) -> bool:
	return self._values.has(v)
	
func is_empty() -> bool:
	return self._size == 0
	
func offer_first(v: Vertex):
	var old_head: Vertex = self._head.next()
	self._head.link_to(v)
	v.link_to(old_head)
	self._size += 1
	if self._values.has(v):
		self._values[v] += 1
	else: 
		self._values[v] = 1
		
func offer_last(v: Vertex):
	var old_tail: Vertex = self._tail.prev()
	old_tail.link_to(v)
	v.link_to(self._tail)
	self._size += 1
	if self._values.has(v):
		self._values[v] += 1
	else: 
		self._values[v] = 1
	
func push_front(i: Variant):
	var old_head: Vertex = self._head.next()
	var new_head: Vertex = Vertex.new(i)
	self._head.link_to(new_head)
	new_head.link_to(old_head)
	self._size += 1
	if self._values.has(new_head):
		self._values[new_head] += 1
	else: 
		self._values[new_head] = 1

func push_back(i: Variant):
	var old_tail: Vertex = self._tail.prev()
	var new_tail: Vertex = Vertex.new(i)
	new_tail.link_to(self._tail)
	old_tail.link_to(new_tail)
	self._size += 1
	if self._values.has(new_tail):
		self._values[new_tail] += 1
	else: 
		self._values[new_tail] = 1
		
func poll_first() -> Vertex:
	if self.is_empty():
		return null
	var head: Vertex = self._head.next()
	self._head.link_to(head.next())
	self._size -= 1
	if self._values.get(head) == 1:
		self._values.erase(head)
	else:
		self._values[head] -= 1
	return head
	
func poll_last() -> Vertex:
	if self.is_empty():
		return null
	var tail: Vertex = self._tail.prev()
	tail.prev().link_to(self._tail)
	self._size -= 1
	if self._values.get(tail) == 1:
		self._values.erase(tail)
	else:
		self._values[tail] -= 1
	return tail
	
func pop_front() -> Variant:
	if self.is_empty():
		return null
	var head: Vertex = self._head.next()
	self._head.link_to(head.next())
	self._size -= 1
	if self._values.get(head) == 1:
		self._values.erase(head)
	else:
		self._values[head] -= 1
	return head.value()

func pop_back() -> Variant:
	if self.is_empty():
		return null
	var tail: Vertex = self._tail.prev()
	tail.prev().link_to(self._tail)
	self._size -= 1
	if self._values.get(tail) == 1:
		self._values.erase(tail)
	else:
		self._values[tail] -= 1
	return tail.value()
	
func peek_front() -> Variant:
	if self.is_empty():
		return null
	return self._head.next().value()
	
func peek_back() -> Variant:
	if self.is_empty():
		return null
	return self._tail.prev().value()
	
func size() -> int:
	return self._size
	
func link_to(l: LinkedList):
	var this_tail: Vertex = self._tail.prev()
	var other_head: Vertex = l._head.next()
	this_tail.link_to(other_head)
	self._tail = l._tail
	self._size += l.size()
	for v in l._values.keys():
		if self._values.has(v):
			self._values[v] += l._values.get(v)
		else:
			self._values[v] = l._values.get(v)

func pop(v: Vertex) -> Variant:
	if self._values.has(v):
		if self._values.get(v) == 1:
			self._values.erase(v)
		else:
			self._values[v] -= 1
		v.prev().link_to(v.next())
		self._size -= 1
		return v.value()
	return null
		
func swap(v: Vertex, u: Vertex) -> bool:
	if v.next() == u:
		var prev: Vertex = v.prev()
		var next: Vertex = u.next()
		prev.link_to(u)
		u.link_to(v)
		v.link_to(next)
		return true
	elif u.next() == v:
		return self.swap(u, v)
	return false

func _to_string() -> String:
	var curr: Vertex = self._head.next()
	var str: String = curr._to_string()
	curr = curr.next()
	while curr != self._tail:
		str += (" -> " + curr._to_string())
		curr = curr.next()
	return str
