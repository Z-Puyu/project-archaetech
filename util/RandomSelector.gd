class_name RandomSelector extends RefCounted

# In the future, this needs to be optimised using a BST
var _items: Array[Variant]
var _weights: Array[int]
var _total_weight: int
var _cumulative_weights: Array[float]
var _rng: RandomNumberGenerator

func _init(items: Array[Variant], weights: Array[int]):
	self._items = items
	self._weights = weights
	self._total_weight = 0
	for item in items.size():
		self._total_weight += weights[item]
		self._cumulative_weights.append(self._total_weight)
	# Cumulative, so always sorted
	self._rng = RandomNumberGenerator.new()
	self._rng.randomize()

func select() -> Variant:
	var rnd: float = self._rng.randf_range(0.0, self._total_weight)
	for i in self._items.size():
		if self._cumulative_weights[i] >= rnd:
			return self._items[i]
	return null

func _update_weights(new_items: Array[Variant], new_weights: Array[int], fewer_items: bool = false):
	if fewer_items:
		self._cumulative_weights = []
		self._total_weight = 0
		for item in self._items.size():
			self._total_weight += self._weights[item]
			self._cumulative_weights.append(self._total_weight)
	else:
		for item in new_items.size():
			self._total_weight += new_weights[item]
			self._normalised_cumulative_weights[self._items.size() + item] = self._total_weight
		
func add_items(items: Array[Variant], weights: Array[int]):
	self._items.append_array(items)
	self._weights.append_array(weights)
	self._update_weights(items, weights)
	
func remove_items(items: Array[Variant], weights: Array[int]):
	for item in items:
		self._items.erase(item)
	for weight in weights: 
		self._weights.erase(weight)
	self._update_weights(items, weights, true)
