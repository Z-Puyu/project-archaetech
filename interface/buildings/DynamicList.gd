class_name DynamicList extends VBoxContainer
const TransportRoute = preload("res://common/buildings/transport/TransportRoute.cs")

var _listables: Dictionary = {
	typeof(TransportRoute): preload("res://interface/buildings/TransportRouteInfo.tscn")
}

var _cached_ui_elems: Dictionary
var _loaded_ui_elems: Dictionary

func _ready():
	self._cached_ui_elems = {}
	self._loaded_ui_elems = {}
	Global.TransportRouteAdded.connect(self._load)
	
func _load(item: TransportRoute):
	# Create a new UI card if the route has not been cached
	if not self._cached_ui_elems.has(item) or self._cached_ui_elems.get(item) == null:
		var new_ui_elem: Control = self._listables.get(typeof(item)).instantiate()
		new_ui_elem.initialise(item)
		new_ui_elem.tree_exiting.connect(func(): self._cached_ui_elems.erase(new_ui_elem))
		self._cached_ui_elems[item] = new_ui_elem
	var ui: TransportRouteInfo = self._cached_ui_elems.get(item)
	if not self._loaded_ui_elems.has(ui):
		self.add_child(ui)
		self.move_child(ui, self._loaded_ui_elems.size())
		self._loaded_ui_elems[ui] = null
		ui.ui_deleted.connect(self._remove)

func update_info(items: Array):	
	for item in items:
		self._load(item)
		
func _remove(ui_elem: TransportRouteInfo):
	self.remove_child(ui_elem)
	self._loaded_ui_elems.erase(ui_elem.route)

func clear():
	for child in self._loaded_ui_elems.values():
		self._remove(child)
