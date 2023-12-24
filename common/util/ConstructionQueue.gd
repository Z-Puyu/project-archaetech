class_name ConstructionQueue extends Node

const ConstructibleTask = preload("res://common/util/ConstructibleTask.gd")

var _active: LinkedList
var _inactive: LinkedList
var max_active_size: int

func _init(max_active_size: int):
	self.max_active_size = max_active_size
	self._active = LinkedList.new()
	self._inactive = LinkedList.new()

func size() -> int:
	return self._active.size() + self._inactive.size()

func remove(task: ConstructibleTask):
	task.terminate.disconnect(self.remove)
	if self._active.contains(task):
		self._active.pop(task)
	else:
		self._inactive.pop(task)
	# remove_child(task)
	task.queue_free()
	print("removed task")
	self._refresh_active()
	
func enqueue(task: ConstructibleTask):
	task.terminate.connect(self.remove)
	if self._active.size() < self.max_active_size:
		self._active.offer_last(task)
		task.start()
	else:
		self._inactive.offer_last(task)
	
func _refresh_active():
	print(("Currently %d" + " active tasks remaining") % self._active.size())
	while self._active.size() > self.max_active_size:
		var task: ConstructibleTask = self._active.poll_last()
		task.pause()
		self._inactive.offer_first(task)
	while self._active.size() < self.max_active_size:
		if self._inactive.is_empty():
			break
		var task: ConstructibleTask = self._inactive.poll_first()
		self._active.offer_last(task)
		task.start()
		
func _shrink_capacity():
	self.max_active_size -= 1
	self._refresh_active()

func _expand_capacity():
	self.max_active_size += 1
	self._refresh_active()
